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
        if (EnemyFieldOfView.HasSeenPlayer)
        {
            GameManager.Instance.ChangePlayingState(GameManager.PlayingState.GameOver);
        }
    }

    public abstract void MaskControl();
}
