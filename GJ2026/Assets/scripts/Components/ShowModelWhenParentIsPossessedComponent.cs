using Components.Actors;
using UnityEngine;

namespace Components
{
    public class ShowModelWhenParentIsPossessedComponent : MonoBehaviour
    {
        private void Start()
        {
            CanPossessComponent.OnPlayerEnterHost += OnPlayerEnterHost;
            CanPossessComponent.OnPlayerExitHost += OnPlayerExitHost;

            var canPossessComponent = FindAnyObjectByType<CanPossessComponent>();
            if (canPossessComponent)
            {
                if (canPossessComponent.IsPossessingHost)
                {
                    OnPlayerEnterHost();
                }
                else
                {
                    OnPlayerExitHost();
                }
            }
            else
            {
                Debug.LogError("Unable to find object with CanPossessComponent!");
            }
        }

        private void OnDestroy()
        {
            CanPossessComponent.OnPlayerEnterHost -= OnPlayerEnterHost;
            CanPossessComponent.OnPlayerExitHost -= OnPlayerExitHost;
        }

        private void OnPlayerEnterHost()
        {
            Debug.Log("ShowModelWhenParentIsPossessedComponent: Enabling model.");
            this.gameObject.SetActive(true);
        }

        private void OnPlayerExitHost()
        {
            Debug.Log("ShowModelWhenParentIsPossessedComponent: Disabling model.");
            this.gameObject.SetActive(false);
        }
    }
}