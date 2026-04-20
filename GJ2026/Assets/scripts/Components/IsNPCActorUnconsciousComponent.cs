using Components.Actors;
using UnityEngine;

namespace Components
{
    /// <summary>
    /// Represents a KO'd (unconscious) entity, which may still have health left. <br/>
    /// Can be possessed by the player and still remain "unconscious". <br/>
    /// Disables alert state, FOV, EnemyActorComponent, etc. while attached to an object/entity. <br/>
    /// WARNING: Currently assumes this will only be used against non-player actors.
    /// </summary>
    public class IsNPCActorUnconsciousComponent : MonoBehaviour
    {
        private void ToggleComponents(bool enable)
        {
            {
                var component = GetComponent<FieldOfViewComponent>();
                if (component)
                {
                    component.enabled = enable;
                }
            }
            {
                var component = GetComponent<MeshRenderer>();
                if (component)
                {
                    component.enabled = enable;
                }
            }
            {
                var component = GetComponent<EnemyActorComponent>();
                if (component)
                {
                    component.enabled = enable;
                }
            }
            {
                var component = GetComponent<AlertStateComponent>();
                if (component)
                {
                    component.enabled = enable;
                }
            }

        }

        private bool disabledActorCollisions = false;

        private void ToggleActorCollisions(bool enable)
        {
            // Prevent colliding with other actors while unconscious,
            // BUT only when the player isn't controlling them (via possession).
            // ReSharper disable once LocalVariableHidesMember
            var collider = GetComponent<Collider>();
            if (collider)
            {
                if (!enable)
                {
                    disabledActorCollisions = true;
                    collider.excludeLayers |= LayerMask.GetMask("Actor");
                }
                else
                {
                    disabledActorCollisions = false;
                    collider.excludeLayers &= ~LayerMask.GetMask("Actor");
                }
            }
        }

        private void Start()
        {
            // Render the entity unconscious by disabling certain components.
            ToggleComponents(false);
        }

        private void Update()
        {
            var isPlayerControlled = GetComponent<ControlledByPlayerComponent>();

            if (disabledActorCollisions)
            {
                if (isPlayerControlled)
                {
                    ToggleActorCollisions(true);
                }
            }
            else if (!isPlayerControlled)
            {
                ToggleActorCollisions(false);
            }
        }

        private void OnDestroy()
        {
            // With this removed, entity is likely no longer unconscious.
            // We'll double-check the Health state to verify it's not dead.
            // If not, then re-enable disabled components.
            {
                var healthComponent = GetComponent<HealthComponent>();
                if (healthComponent && healthComponent.IsDead)
                {
                    Debug.Log("Entity is dead, so no point in restoring disabled components.");
                    return;
                }
            }

            ToggleComponents(true);
        }
    }
}