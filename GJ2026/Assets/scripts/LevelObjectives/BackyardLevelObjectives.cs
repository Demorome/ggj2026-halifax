using Components;
using UnityEngine;
using UnityEngine.UIElements;

namespace LevelObjectives
{
    // Sets the script to be executed later than all default scripts
    // This is helpful for UI, since other things may need to be initialized before setting the UI
    [DefaultExecutionOrder(1000)]
    public class BackyardLevelObjectives : MonoBehaviour
    {
        private UIDocument uiDocument;
        private TextElement uiText;

        /// <summary>
        /// Can be 0, to represent a boolean objective
        /// which doesn't require point accumulation.
        /// </summary>
        private int progressRequired;
        private int currentProgress;

        private int currentCollectedCheese = 0;
        [SerializeField]
        private int requiredCollectedCheese = 3;

        private int currentCollectedDogBones = 0;
        [SerializeField]
        private int requiredCollectedDogBones = 1;

        private int currentIntrudersRepelled = 0;
        [SerializeField]
        private int requiredIntrudersRepelled = 3;


        private ActorType _currentActorType = ActorType.Unknown;
        private ActorType currentActorType
        {
            get => _currentActorType;
            set
            {
                if (_currentActorType != value)
                {
                    _currentActorType = value;
                    OnActorTypeChanged(value);
                }
            }
        }

        private void OnPlayerEnterHost()
        {
            currentActorType = CanPossessComponent.GetCurrentPlayerActorType();
        }

        private void OnPlayerExitHost()
        {
            currentActorType = ActorType.PlayerMaskWithoutHost;
        }

        private void OnActorTypeChanged(ActorType actorType)
        {
            string objectiveMessage;
            if (actorType == ActorType.PlayerMaskWithoutHost)
            {
                objectiveMessage = "Return home without killing any host!";
            }
            else if (actorType == ActorType.Mouse)
            {
                objectiveMessage = "Collect cheese!";
                progressRequired = requiredCollectedCheese;
                currentProgress = currentCollectedCheese;
            }
            else if (actorType == ActorType.Cat)
            {
                objectiveMessage = "Steal the dog's bone!";
                progressRequired = requiredCollectedDogBones;
                currentProgress = currentCollectedDogBones;

            }
            else if (actorType == ActorType.Dog)
            {
                progressRequired = requiredIntrudersRepelled;
                currentProgress = currentIntrudersRepelled;

                if (currentProgress >= requiredIntrudersRepelled)
                {
                    objectiveMessage = "Enter the house!";
                }
                else
                {
                    objectiveMessage = "Repel intruders from the house!";
                }
            }
            else
            {
                objectiveMessage = "ERROR: Unknown objective!";
            }

            UpdateObjectiveUIString(objectiveMessage);
        }

        // TODO: Add objective counter + image for item to collect UI!
        private void UpdateObjectiveUIString(string objectiveMsg)
        {
            uiText.text = objectiveMsg;
        }

        private void Start()
        {
            uiDocument = GetComponent<UIDocument>();
            uiText = uiDocument.rootVisualElement.Q<TextElement>("Objective");
            if (uiText == null)
            {
                Debug.LogError("Objective UI Text not found!");
                return;
            }

            currentActorType = CanPossessComponent.GetCurrentPlayerActorType();

            CanPossessComponent.OnPlayerEnterHost += OnPlayerEnterHost;
            CanPossessComponent.OnPlayerExitHost += OnPlayerExitHost;
        }

        private void OnDestroy()
        {
            CanPossessComponent.OnPlayerEnterHost -= OnPlayerEnterHost;
            CanPossessComponent.OnPlayerExitHost -= OnPlayerExitHost;
        }
    }
}