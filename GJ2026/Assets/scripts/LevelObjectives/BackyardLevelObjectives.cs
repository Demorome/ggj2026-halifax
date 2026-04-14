using Components;
using UnityEngine;
using UnityEngine.UIElements;

namespace LevelObjectives
{
    // TODO: Update the UI whenever an objective is progressing!

    // Sets the script to be executed later than all default scripts
    // This is helpful for UI, since other things may need to be initialized before setting the UI
    [DefaultExecutionOrder(1000)]
    public class BackyardLevelObjectives : MonoBehaviour
    {
        // TODO: Create objective array, store current objective index.
        // TODO: This will let us query + increment current objective progress.
        //private int currentObjective = ????;

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
            string objectiveEmojiIcon = null;

            // Only used if EmojiIcon isn't null.
            int progressRequired = -99999;
            int currentProgress = -99999;

            if (actorType == ActorType.PlayerMaskWithoutHost)
            {
                objectiveMessage = "Return home without killing any host!";
            }
            else if (actorType == ActorType.Mouse)
            {
                objectiveMessage = "Collect cheese!";
                objectiveEmojiIcon = "🧀";
                progressRequired = requiredCollectedCheese;
                currentProgress = currentCollectedCheese;
            }
            else if (actorType == ActorType.Cat)
            {
                objectiveMessage = "Steal the dog's bone!";
                objectiveEmojiIcon = "🦴";
                progressRequired = requiredCollectedDogBones;
                currentProgress = currentCollectedDogBones;

            }
            else if (actorType == ActorType.Dog)
            {
                progressRequired = requiredIntrudersRepelled;
                currentProgress = currentIntrudersRepelled;

                if (currentProgress >= requiredIntrudersRepelled)
                {
                    objectiveEmojiIcon = "🏡";
                    currentProgress = 0;
                    progressRequired = 0;
                    objectiveMessage = "Enter the house!";
                }
                else
                {
                    objectiveEmojiIcon = "🐭";
                    objectiveMessage = "Repel intruders from the house!";
                }
            }
            else
            {
                objectiveMessage = "ERROR: Unknown objective!";
            }

            string progressMessage = null;
            if (objectiveEmojiIcon != null)
            {
                if (progressRequired != 0)
                {
                    progressMessage = $"{currentProgress}/{progressRequired}";
                }
                else
                {
                    progressMessage = string.Empty;
                }
            }

            var uiColor = ActorTypeComponent.ColorForActorType(actorType);

            GameManager.Instance.ChangeObjective(
                objectiveMessage,
                uiColor,
                objectiveEmojiIcon,
                progressMessage
            );
        }

        public void IncrementCheeseProgress()
        {

        }
        public void IncrementBoneProgress()
        {

        }
        public void IncrementIntrudersProgress()
        {

        }

        private void Start()
        {
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