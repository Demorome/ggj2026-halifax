using UnityEngine;

namespace Components
{
    public class EndTriggerComponent : MonoBehaviour
    {
        private void OnTriggerEnter()
        {
            GameManager.Instance.LoadNextScene();
            Debug.Log("Level Complete!");
        }
    }
}
