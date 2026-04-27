using UnityEngine;

namespace Components
{
    public static class Utils
    {
        public static void ToggleCanMoveState(GameObject target, bool canMove)
        {
            var moveSpeedComponent = target.GetComponent<MoveSpeedComponent>();
            var rotateSpeedComponent = target.GetComponent<RotateSpeedComponent>();
            if (!moveSpeedComponent || !rotateSpeedComponent)
            {
                Debug.LogError("Can't toggle movement on non-moving entity!");
                return;
            }

            moveSpeedComponent.canMove = canMove;
            rotateSpeedComponent.canMove = canMove;
        }

        public static void ToggleCanInteractState(GameObject target, bool canInteract)
        {
            var interactionComponent = target.GetComponentInChildren<InteractionZoneComponent>(true);
            if (!interactionComponent)
            {
                Debug.LogError("Can't toggle interaction on non-interacting entity!");
                return;
            }

            interactionComponent.Reset();
            interactionComponent.gameObject.SetActive(canInteract);
        }
    }
}