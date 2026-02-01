using UnityEngine;

public abstract class EnemyEntity : MonoBehaviour
{
    public bool IsAlerted { get; private set; } = false;
    public EnemyFieldOfView EnemyFieldOfView;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var gameManager = GameManager.Instance;
        if (gameManager.CurrentPlayingState != GameManager.PlayingState.Normal
            || gameManager.IsLoading)
        {
            return;
        }

        if (EnemyFieldOfView.HasSeenPlayer)
        {
            Debug.Log("Player has been seen, game over!");
            GameManager.Instance.ChangePlayingState(GameManager.PlayingState.GameOver);
        }
    }

    public abstract void MaskControl();
}
