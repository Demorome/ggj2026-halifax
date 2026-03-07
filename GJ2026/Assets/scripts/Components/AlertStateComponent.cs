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
                _state = value;
                OnAlertStateChange?.Invoke(value);
            }
        }

        public event Action<AlertState> OnAlertStateChange;
    }
}