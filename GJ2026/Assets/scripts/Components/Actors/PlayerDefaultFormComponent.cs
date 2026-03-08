using UnityEngine;

namespace Components.Actors
{
    /// <summary>
    /// The form where the player is just the mask-alien, with no host.
    /// </summary>
    [RequireComponent(typeof(ActorComponent))]
    [RequireComponent(typeof(ControlledByPlayerComponent))]
    [RequireComponent(typeof(OnPlayerFinalHealthChangeComponent))]
    [RequireComponent(typeof(DrainHealthOverTimeComponent))]
    [RequireComponent(typeof(CameraTargetComponent))]
    [RequireComponent(typeof(CanPossessComponent))]
    public class PlayerDefaultFormComponent : MonoBehaviour
    {

    }
}