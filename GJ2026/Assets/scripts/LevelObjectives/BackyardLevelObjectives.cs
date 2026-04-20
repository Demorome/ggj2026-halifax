using System.Collections.Generic;
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

        private GameManager _gameManager;

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
            ObjectiveType objectiveType;
            switch (actorType)
            {
                case ActorType.PlayerMaskWithoutHost:
                    objectiveType = ObjectiveType.DefaultPlayerObjective;
                    break;
                case ActorType.Mouse:
                    objectiveType = ObjectiveType.CollectCheese;
                    break;
                case ActorType.Cat:
                    objectiveType = ObjectiveType.StealBone;
                    break;
                case ActorType.Dog:
                    objectiveType = ObjectiveType.RepelIntruders;
                    break;
                default:
                    Debug.LogError($"Unknown ActorType: {actorType}");
                    return;
            }

            var objectiveInfo = objectives[objectiveType];
            Debug.Assert(_gameManager != null);
            _gameManager.ChangeCurrentObjective(objectiveType);

            var uiColor = ActorTypeComponent.ColorForActorType(actorType);

            _gameManager.ChangeObjectiveUI(
                objectiveInfo.GetCurrentMessage(),
                uiColor,
                objectiveInfo.GetCurrentEmoji(),
                objectiveInfo.GetProgressText()
            );
        }

        private readonly Dictionary<ObjectiveType, LevelObjective> objectives = new()
        {
            { ObjectiveType.DefaultPlayerObjective,
                new("Return home without killing any host!", null,
                    null, null,
                    0)
            },
            { ObjectiveType.CollectCheese,
                new("Collect cheese!", "🧀",
                    "Trick the cat!", "😾",
                    5)
            },
            { ObjectiveType.StealBone,
                new("Steal the dog's bone!", "🦴",
                    "Trick the dog!", "🐶",
                    1)
            },
            { ObjectiveType.RepelIntruders,
                new("Repel intruders!", "🐭",
                    "Enter the house!", "🏡",
                    3)
            },
        };

        private void OnIncrementChallengeSignal(ObjectiveType objectiveType)
        {
            if (objectives.TryGetValue(objectiveType, out var objectiveInfo))
            {
                objectiveInfo.IncrementProgress();
                _gameManager.UpdateObjectiveProgress(objectiveType, objectiveInfo.GetProgressText());
            }
            else
            {
                Debug.LogError("Unknown objective type: " + objectiveType);
            }
        }

        private void Start()
        {
            _gameManager = GameManager.Instance;
            _gameManager.OnIncrementObjectiveProgressSignal += OnIncrementChallengeSignal;

            CanPossessComponent.OnPlayerEnterHost += OnPlayerEnterHost;
            CanPossessComponent.OnPlayerExitHost += OnPlayerExitHost;

            currentActorType = CanPossessComponent.GetCurrentPlayerActorType();

        }

        private void OnDestroy()
        {
            _gameManager.OnIncrementObjectiveProgressSignal -= OnIncrementChallengeSignal;

            CanPossessComponent.OnPlayerEnterHost -= OnPlayerEnterHost;
            CanPossessComponent.OnPlayerExitHost -= OnPlayerExitHost;
        }
    }
}