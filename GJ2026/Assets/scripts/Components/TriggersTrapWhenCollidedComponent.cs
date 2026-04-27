using UnityEngine;

namespace Components
{
    public class TriggersTrapWhenCollidedComponent : MonoBehaviour
    {
        private bool triggered = false;


        private void OnTriggerEnter(Collider other)
        {
            if (triggered)
            {
                return;
            }

            if (other.gameObject.layer != LayerMask.NameToLayer("Actor"))
            {
                return;
            }

            // Trap-actor collision has occured!
            Debug.Log($"{other.gameObject} triggered the trap {gameObject.name}!");
            triggered = true;

            // TODO: Play SFX, play model animation

            // It's game over if the player walked over this in default mask form.
            // Otherwise, their host (likely a mouse) may be trapped,
            // but the mask can still escape.
            if (other.gameObject.GetComponent<ControlledByPlayerComponent>() != null)
            {
                if (CanPossessComponent.CurrentPlayerForm != other.gameObject)
                {
                    Debug.LogError("Global player variable is wrong: " + CanPossessComponent.CurrentPlayerForm);
                }

                if (CanPossessComponent.GetCurrentPlayerActorType() == ActorType.PlayerMaskWithoutHost)
                {
                    GameManager.Instance.ChangePlayingState(GameManager.PlayingState.GameOver);
                }
            }

            // Prevent movement for the trapped actor.
            // If the player exits a trapped host, the condition should be left on the host,
            // thus letting the player escape freely.
            Utils.ToggleCanMoveState(other.gameObject, false);
            Utils.ToggleCanInteractState(other.gameObject, false);
        }
    }
}