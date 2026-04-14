using UnityEngine;

namespace Components.Actors
{
    [RequireComponent(typeof(ActorTypeComponent))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(RotateSpeedComponent))]
    [RequireComponent(typeof(MoveSpeedComponent))]
    [RequireComponent(typeof(HealthComponent))]
    [RequireComponent(typeof(MassComponent))]
    public class ActorComponent : MonoBehaviour
    {
        private void Awake()
        {
            // Prevent physics from rotating actors.
            var rigidBody = GetComponent<Rigidbody>();
            rigidBody.freezeRotation = true;
        }
    }
}