using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    public GameManager gameManager;
    private void OnTriggerEnter()
    {
        GameManager.Instance.LoadNextScene();
        Debug.Log("Level Complete!");
    }
}
