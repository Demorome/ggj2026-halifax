using UnityEngine;

namespace Components.Actors
{
    /// <summary>
    /// When the player possesses an EnemyEntity, that entity gets disabled.
    /// It is replaced with a look-alike Host entity, controlled by the player.<br/>
    ///
    /// When the player leaves a Host, the Host gets disabled
    /// and replaced with its corresponding EnemyEntity.
    /// </summary>
    [RequireComponent(typeof(ActorTypeComponent))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(RotateSpeedComponent))]
    [RequireComponent(typeof(MoveSpeedComponent))]
    [RequireComponent(typeof(HealthComponent))]
    [RequireComponent(typeof(MassComponent))]
    public class ActorComponent : MonoBehaviour
    {
    }
}