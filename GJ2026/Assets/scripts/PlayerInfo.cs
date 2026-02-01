using UnityEngine;


/// <summary>
/// Unlike PlayerController, this stores player info that is persistent,
/// even after changing between multiple hosts.
///
/// TODO: Merge this with "maskHolder.cs"!
/// </summary>
public class PlayerInfo : MonoBehaviour
{
    // The player's final health reserves when outside of a host.
    public float MaxMaskHealth = 10f;
    private float CurrentMaskHealth;

    private GameManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameManager.Instance;
        CurrentMaskHealth = MaxMaskHealth;
    }

    // TODO: Count down MaskHealth when no masks are equipped.
    void Update()
    {

    }

    public void CountDownMaskLife()
    {
        if (0 >= CurrentMaskHealth)
        {
            Debug.Log("Lost final health; game over!");
            gameManager.ChangePlayingState(GameManager.PlayingState.GameOver);
        }
        else
        {
            CurrentMaskHealth -= Time.deltaTime;
        }
    }
}
