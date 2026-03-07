using UnityEngine;

namespace Components.Actors
{
    [RequireComponent(typeof(ActorComponent))]
    [RequireComponent(typeof(FieldOfViewComponent))]
    [RequireComponent(typeof(AlertStateComponent))]
    public class EnemyActorComponent : MonoBehaviour
    {
        private FieldOfViewComponent fieldOfViewComponent;
        private AlertStateComponent alertStateComponent;

        private void Start()
        {
            fieldOfViewComponent = GetComponent<FieldOfViewComponent>();
            fieldOfViewComponent.OnActorDetection += OnActorDetection;

            alertStateComponent = GetComponent<AlertStateComponent>();
        }

        private void Update()
        {
            var gameManager = GameManager.Instance;
            if (gameManager.CurrentPlayingState != GameManager.PlayingState.Normal
                || gameManager.IsLoading)
            {
                return;
            }
        }

        private void OnActorDetection(GameObject gameObj)
        {
            if (gameObj.GetComponent<ControlledByPlayerComponent>())
            {
                Debug.Log("Enemy can see player!");
                alertStateComponent.state = AlertState.SpottedPlayer;
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.GetComponent<ControlledByPlayerComponent>())
            {
                Debug.Log("Enemy was touched by player!");
                alertStateComponent.state = AlertState.SpottedPlayer;
            }
        }
    }
}