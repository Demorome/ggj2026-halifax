using Components.Actors;
using UnityEngine;

namespace Components
{
    /// <summary>
    /// Determines if an object can ever be possessed,
    /// and tracks if it is currently ready/vulnerable.
    /// </summary>
    [RequireComponent(typeof(HealthComponent))]
    public class CanBePossessedComponent : MonoBehaviour
    {
        // TODO: Toggle this component as a state in specific situations!
        // Should this always be based off Alert state,
        // or should we allow it to be set by triggering
        // certain states too???

        private void Start()
        {
            var healthComponent = GetComponent<HealthComponent>();
            if (healthComponent)
            {
                healthComponent.OnDeath += DestroyThisOnDeath;
            }
        }

        private void OnDestroy()
        {
            var healthComponent = GetComponent<HealthComponent>();
            if (healthComponent)
            {
                healthComponent.OnDeath -= DestroyThisOnDeath;
            }
        }

        /// <summary>
        /// Ensure we can't possess a dead host by removing this component on death.
        /// </summary>
        private void DestroyThisOnDeath()
        {
            Destroy(this);
        }
    }
}