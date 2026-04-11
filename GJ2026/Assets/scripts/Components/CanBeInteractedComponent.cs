using UnityEngine;

namespace Components
{
    public class CanBeInteractedComponent : MonoBehaviour
    {
        public GameObject meshObjectToHighlight;

        private void Awake()
        {
            if (meshObjectToHighlight == null)
            {
                Debug.LogError("Can't find mesh object to highlight");
            }
        }
    }
}