using UnityEngine;

namespace Components.Actors
{
    /// <summary>
    /// The form where the player is just the mask-alien, with no host.
    /// </summary>
    [RequireComponent(typeof(ActorComponent))]
    [RequireComponent(typeof(ControlledByPlayerComponent))]
    [RequireComponent(typeof(PlayerOnFinalHealthChangeComponent))]
    [RequireComponent(typeof(DrainHealthOverTimeComponent))]
    [RequireComponent(typeof(CameraTargetComponent))]
    [RequireComponent(typeof(CanPossess))]
    public class PlayerDefaultFormComponent : MonoBehaviour
    {

    }
}