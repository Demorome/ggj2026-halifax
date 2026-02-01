using UnityEngine;

public abstract class EnemyEntity : MonoBehaviour
{
    public bool IsAlerted { get; private set; } = false;
    public EnemyFieldOfView EnemyFieldOfView;

    // WARNING: You'll need to call this from the inherited class!
    protected void Start()
    {
        
    }

    // WARNING: You'll need to call this from the inherited class!
    protected void Update()
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
