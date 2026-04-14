using UnityEngine;

namespace Components
{
    public class CanBeInteractedComponent : MonoBehaviour
    {
        public string interactionMessage;
        public GameObject meshObjectToHighlight;

        private void Awake()
        {
            if (meshObjectToHighlight == null)
            {
                Debug.LogError("Can't find mesh object to highlight");
            }

            if (interactionMessage == null)
            {
                Debug.LogError("Null interaction name");
                interactionMessage = "ERROR";
            }
        }
    }
}