using UnityEngine;

namespace Components.Actors
{
    public class CanPossessComponent : MonoBehaviour
    {
        /// <summary>
        /// Determines how close the player needs to be to possess someone. <br/>
        /// TODO: Make this the same as general interaction range?
        /// </summary>
        public float possessionRange = 25f;

        /// <summary>
        /// The form to revert to once no longer possessing anyone. <br/>
        /// This object will be disabled when the player enters a host. <br/>
        /// Will be re-enabled when leaving the host,
        /// unless switching immediately to another host.
        /// </summary>
        private PlayerDefaultFormComponent defaultPlayerForm;

        private bool IsPossessingHost => defaultPlayerForm != null
                                        && !defaultPlayerForm.isActiveAndEnabled;

        private bool registeredOnDeathEvent = false;

        private void Start()
        {
            if (!defaultPlayerForm)
            {
                defaultPlayerForm = FindAnyObjectByType<PlayerDefaultFormComponent>();
            }
            if (!defaultPlayerForm)
            {
                Debug.LogError("Can't find object with PlayerDefaultForm Component!");
            }

            if (!IsPossessingHost)
            {
                Debug.Log("Player either spawned without a host equipped, or just left their host.");
                GameManager.Instance.UpdatePlayerHostHealthUI(0, 100);
            }
            else
            {
                // Register on-death handler to auto-exit the host.
                var healthComponent = GetComponent<HealthComponent>();
                if (healthComponent)
                {
                    registeredOnDeathEvent = true;
                    healthComponent.OnDeath += TryLeaveCurrentHost;
                }
            }
        }

        private void OnDestroy()
        {
            // Un-register on-death handler.
            var healthComponent = GetComponent<HealthComponent>();
            if (healthComponent && registeredOnDeathEvent)
            {
                healthComponent.OnDeath -= TryLeaveCurrentHost;
            }
        }

        private void Update()
        {
            if (GameManager.Instance.CurrentPlayingState != GameManager.PlayingState.Normal
                || GameManager.Instance.IsLoading)
            {
                return;
            }

            // Handle possession/host-switching logic.
            // Host-specific gameplay logic should be handled in separate components.
            // TODO: Highlight nearby hosts that can be possessed!
            if (IsPossessingHost)
            {
                if (Input.GetKeyDown(KeyCode.X))
                {
                    TryLeaveCurrentHost();
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.X))
                {
                    var maybeNearbyHost = GetClosestHostInRange();
                    if (maybeNearbyHost)
                    {
                        TryPossessHost(maybeNearbyHost.gameObject);
                    }
                    else
                    {
                        Debug.Log("No nearby hosts to possess.");
                    }
                }
            }
        }

        /// <summary>
        /// Won't work if the player already has a host equipped.
        /// </summary>
        private void TryPossessHost(GameObject targetToPossess)
        {
            if (IsPossessingHost)
            {
                Debug.Log("TryPossessHost: Can't equip host; we already have one. Leave the current one first!");
                return;
            }
            if (!targetToPossess.activeInHierarchy)
            {
                Debug.LogError("TryPossessHost: Target should be active/enabled!");
                return;
            }
            if (!targetToPossess.GetComponent(typeof(CanBePossessedComponent)))
            {
                Debug.LogError("TryPossessHost: Can't equip host; target can't be possessed!");
                return;
            }
            {
                var healthComponent = (HealthComponent)GetComponent(typeof(HealthComponent));
                if (!healthComponent || healthComponent.IsDead)
                {
                    Debug.LogError("TryPossessHost: Target should be alive!");
                    return;
                }
            }

            Debug.Log("TryPossessHost: Now trying to equip host.");

            GameObject previousForm = this.gameObject;

            // Disable the player default form, if that was the previous form.
            if (previousForm == defaultPlayerForm.gameObject)
            {
                defaultPlayerForm.gameObject.SetActive(false);
            }

            AddPlayerRelatedComponentsToNewForm(targetToPossess);
            KnockEnemyUnconscious(targetToPossess);

            TransferThisComponentToTarget(targetToPossess);
        }

        /// <summary>
        /// Won't work if no host is possessed/equipped.
        /// </summary>
        private void TryLeaveCurrentHost()
        {
            if (!IsPossessingHost)
            {
                Debug.Log("Can't unequip host; none is possessed/equipped.");
                return;
            }

            GameObject previousHost = this.gameObject;
            GameObject newForm = defaultPlayerForm.gameObject;

            newForm.SetActive(true);

            // TODO: Add position offset, so player doesn't spawn inside/on top of old host?
            // TODO: Or, disable collision for unconscious actors.
            newForm.transform.position = previousHost.transform.position;

            AddPlayerRelatedComponentsToNewForm(newForm);
            RemovePlayerRelatedComponentsFromOldHost(previousHost);
            KnockEnemyUnconscious(previousHost);

            TransferThisComponentToTarget(newForm);
        }

        private void AddPlayerRelatedComponentsToNewForm(GameObject newForm)
        {
            // Default player form will only be disabled/re-enabled,
            // so it won't need these components to be added.
            if (newForm != defaultPlayerForm.gameObject)
            {
                newForm.AddComponent(typeof(CameraTargetComponent));
                newForm.AddComponent(typeof(ControlledByPlayerComponent));
                newForm.AddComponent(typeof(OnHostLifeChangeComponent));
                newForm.AddComponent(typeof(DrainHealthOverTimeComponent));
            }
        }
        private void RemovePlayerRelatedComponentsFromOldHost(GameObject oldHost)
        {
            Destroy(oldHost.GetComponent(typeof(CameraTargetComponent)));
            Destroy(oldHost.GetComponent(typeof(ControlledByPlayerComponent)));
            Destroy(oldHost.GetComponent(typeof(OnHostLifeChangeComponent)));
            Destroy(oldHost.GetComponent(typeof(DrainHealthOverTimeComponent)));
        }

        private void KnockEnemyUnconscious(GameObject enemy)
        {
            // Set the current host unconscious,
            // so it won't hurt us after we leave it.
            if (!enemy.GetComponent<IsHostBodyUnconsciousComponent>())
            {
                enemy.AddComponent(typeof(IsHostBodyUnconsciousComponent));
            }
        }

        private void TransferThisComponentToTarget(GameObject target)
        {
            if (target == defaultPlayerForm.gameObject)
            {
                target.GetComponent<CanPossessComponent>().enabled = true;
            }
            else
            {
                var transferredComponent = (CanPossessComponent)target
                    .AddComponent(typeof(CanPossessComponent));

                transferredComponent.defaultPlayerForm = defaultPlayerForm;
                transferredComponent.possessionRange = possessionRange;
            }

            if (this.gameObject == defaultPlayerForm.gameObject)
            {
                this.enabled = false;
            }
            else
            {
                Destroy(this);
            }
        }

        private CanBePossessedComponent GetClosestHostInRange()
        {
            CanBePossessedComponent closestTarget = null;
            float closestDistance = float.MaxValue;
            Vector3 currentPosition = transform.position;

            // TODO: Optimize, if needed.
            foreach (var host
                     in FindObjectsByType<CanBePossessedComponent>(
                         FindObjectsInactive.Exclude,
                         FindObjectsSortMode.None
                     )
                    )
            {
                // Credits to u/sakaraa:
                // https://www.reddit.com/r/Unity3D/comments/10vrb73/how_to_find_closest_object_without_costing_too/
                Vector3 differenceToTarget = host.transform.position - currentPosition;
                float distanceSquaredToTarget = differenceToTarget.sqrMagnitude;

                if (distanceSquaredToTarget > (possessionRange * possessionRange))
                {
                    continue;
                }

                if (distanceSquaredToTarget < closestDistance)
                {
                    closestDistance = distanceSquaredToTarget;
                    closestTarget = host;
                }
            }

            return closestTarget;
        }
    }
}