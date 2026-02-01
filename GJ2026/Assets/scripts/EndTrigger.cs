using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    private void OnTriggerEnter()
    {
        GameManager.Instance.LoadNextScene();
        Debug.Log("Level Complete!");
    }
}
