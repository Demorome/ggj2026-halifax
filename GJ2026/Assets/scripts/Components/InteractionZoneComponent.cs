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
        private Color lastColorForClosest;

        private void FixedUpdate()
        {
            if (nearbyInteractables.Count == 0)
            {
                return;
            }

            float closestDistanceSquared = float.MaxValue;
            GameObject closestInteractable = null;

            // Backwards loop, for safe mid-loop removal.
            for (int it = nearbyInteractables.Count - 1; it >= 0; --it)
            {
                var interactable = nearbyInteractables[it];

                // Remove now-disabled interactables (probably extremely rare).
                if (!interactable.activeInHierarchy)
                {
                    nearbyInteractables.RemoveAt(it);
                }

                var distanceSquared = (interactable.transform.position - transform.position).sqrMagnitude;
                if (distanceSquared < closestDistanceSquared)
                {
                    closestInteractable = interactable;
                    closestDistanceSquared = distanceSquared;
                }
            }

            if (!closestInteractable)
            {
                return;
            }

            if (closestInteractable != lastClosestInteractable)
            {
                if (lastClosestInteractable)
                {
                    lastClosestInteractable.GetComponent<Renderer>().material.color = lastColorForClosest;
                }
                lastClosestInteractable = closestInteractable;

                var closestInteractableRenderer =  closestInteractable.GetComponent<Renderer>();
                lastColorForClosest = closestInteractableRenderer.material.color;
                closestInteractableRenderer.material.color = Color.yellow;

                Debug.Log("Found new closest interactable: " + closestInteractable);
            }

            // Only proceed further if player presses the Interact button.
            if (!Input.GetButtonDown("Interact"))
            {
                return;
            }

            // Actually interact with the closest object,
            // by checking for components for interaction logic.

            var teleport = closestInteractable.GetComponent<LinkedTeleportComponent>();
            if (teleport != null)
            {
                teleport.Teleport(gameObject);
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("InteractableObject"))
            {
                nearbyInteractables.Add(other.gameObject);
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("InteractableObject"))
            {
                nearbyInteractables.Remove(other.gameObject);
            }
        }

        public void Reset()
        {
            if (lastClosestInteractable)
            {
                lastClosestInteractable.GetComponent<Renderer>().material.color = lastColorForClosest;
            }
            lastClosestInteractable = null;

            // TODO: Might need to force a re-calculation for OnTriggerEnter, to re-populate this?
            nearbyInteractables.Clear();
        }
    }
}