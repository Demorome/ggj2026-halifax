using System;
using UnityEngine;

namespace Components
{
    public enum AlertState
    {
        Unaware,
        Suspecting,
        SpottedPlayer // game over
    }

    public class AlertStateComponent : MonoBehaviour
    {
        [SerializeField] private AlertState _state;
        public AlertState state
        {
            get => _state;
            set
            {
                if (_state == value)
                {
                    return;
                }
                _state = value;
                OnAlertStateChange?.Invoke(value);

                // TODO: SFX, visuals!!!
                if (_state == AlertState.SpottedPlayer)
                {
                    var audioSource = GetComponent<AudioSource>();
                    audioSource.clip = SoundManager.Instance.alert;
                    audioSource.Play();

                    // TODO: Trigger delayed game-over!! (freeze time too)
                }
            }
        }

        public event Action<AlertState> OnAlertStateChange;
    }
}