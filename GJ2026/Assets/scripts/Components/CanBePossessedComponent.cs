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
    }
}