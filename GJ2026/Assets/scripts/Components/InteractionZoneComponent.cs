using System.Collections.Generic;
using UnityEngine;

namespace Components
{
    /// <summary>
    /// Indicates a parent object can interact with interactable objects,
    /// provided that they have a child InteractionDetector object
    /// with this component to provide the interaction zone collision detection.
    /// <br/>
    /// WARNING: That child must be the bottom-most child of the object,
    /// for proper transferal w/ CanPossessComponent possession logic.
    /// </summary>
    public class InteractionZoneComponent : MonoBehaviour
    {
        private readonly List<GameObject> nearbyInteractables = new();
        private GameObject lastClosestInteractable;

        private void FixedUpdate()
        {
            if (GameManager.Instance.CurrentPlayingState == GameManager.PlayingState.GameOver)
            {
                Reset();
                return;
            }

            if (nearbyInteractables.Count == 0)
            {
                return;
            }

            if (GameManager.Instance.CurrentPlayingState != GameManager.PlayingState.Normal
                || GameManager.Instance.IsLoading)
            {
                return;
            }

            //Debug.Log($"List Contents:\n{string.Join("\n", nearbyInteractables)}");

            float closestDistanceSquared = float.MaxValue;
            GameObject closestInteractable = null;

            foreach (var interactable in nearbyInteractables)
            {
                // If we can't actually interact with this object, ignore it.
                if (!TryInteraction(interactable, false))
                {
                    continue;
                }

                var distanceSquared = (interactable.transform.position - transform.position).sqrMagnitude;
                if (distanceSquared < closestDistanceSquared)
                {
                    closestInteractable = interactable;
                    closestDistanceSquared = distanceSquared;
                }
            }

            if (closestInteractable != lastClosestInteractable)
            {
                if (lastClosestInteractable)
                {
                    lastClosestInteractable
                        .GetComponent<CanBeInteractedComponent>().meshObjectToHighlight
                        .GetComponent<MeshOutline>().enabled = false;
                }
                lastClosestInteractable = closestInteractable;

                if (closestInteractable)
                {
                    var canBeInteracted = closestInteractable.GetComponent<CanBeInteractedComponent>();

                    var outline = canBeInteracted.meshObjectToHighlight.GetComponent<MeshOutline>();
                    if (!outline)
                    {
                        outline = canBeInteracted.meshObjectToHighlight.AddComponent<MeshOutline>();
                        outline.OutlineMode = MeshOutline.Mode.OutlineVisible;
                        outline.OutlineColor = Color.yellow;
                        outline.OutlineWidth = 5f;
                    }
                    else
                    {
                        outline.enabled = true;
                    }

                    Debug.Log("Found new closest interactable: " + closestInteractable);
                }
                else
                {
                    Debug.Log("No interactables are in range anymore");
                }
            }

            if (closestInteractable && Input.GetButtonDown("Interact"))
            {
                TryInteraction(closestInteractable, true);
            }
        }

        /// <summary>
        /// Handles interaction logic, based on components for the source and target.
        /// </summary>
        /// <param name="actuallyInteract">If false, interaction logic is disabled.
        /// Useful for testing if an interaction could be made, i.e. for highlighting.</param>
        /// <returns>True for success, false for failure.</returns>
        private bool TryInteraction(GameObject toInteractWith, bool actuallyInteract)
        {
            // Ignore now-disabled interactables (probably extremely rare).
            if (!toInteractWith.activeInHierarchy)
            {
                return false;
            }

            var canBeInteracted = toInteractWith.GetComponent<CanBeInteractedComponent>();
            if (!canBeInteracted)
            {
                return false;
            }

            // The source of the interaction.
            var parentObject = transform.parent.gameObject;

            var holeTeleport = toInteractWith.GetComponent<HoleTeleportComponent>();
            if (holeTeleport && parentObject.GetComponent<CanEnterHolesComponent>())
            {
                var teleport = toInteractWith.GetComponent<LinkedTeleportComponent>();
                if (actuallyInteract)
                {
                    teleport.Teleport(parentObject);
                }
                return true;
            }

            var canPossess = parentObject.GetComponent<CanPossessComponent>();
            if (canPossess && !canPossess.IsPossessingHost)
            {
                // Handle possession/host-switching logic.
                // Host-specific gameplay logic should be handled in separate components.
                if (toInteractWith.GetComponent<CanBePossessedComponent>())
                {
                    if (actuallyInteract)
                    {
                        canPossess.TryPossessHost(toInteractWith);
                    }
                    return true;
                }
            }

            return false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<CanBeInteractedComponent>())
            {
                nearbyInteractables.Add(other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (nearbyInteractables.Contains(other.gameObject))
            {
                nearbyInteractables.Remove(other.gameObject);

                if (other.gameObject == lastClosestInteractable)
                {
                    ResetLastClosestInteractable();

                    //Debug.Log("Left interaction distance");
                }
            }
        }

        private void ResetLastClosestInteractable()
        {
            if (lastClosestInteractable)
            {
                lastClosestInteractable
                    .GetComponent<CanBeInteractedComponent>().meshObjectToHighlight
                    .GetComponent<MeshOutline>().enabled = false;

                lastClosestInteractable = null;
            }
        }

        public void Reset()
        {
            ResetLastClosestInteractable();

            // TODO: Might need to force a re-calculation for OnTriggerEnter, to re-populate this?
            nearbyInteractables.Clear();
        }
    }
}