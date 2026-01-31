using UnityEngine;
using UnityEngine.SceneManagement;

// Credits to this for the baseline:
// https://uhiyama-lab.com/en/notes/unity/unity-gameloop-gamemanager-pattern-guide/
public class GameManager : MonoBehaviour
{
    // Static field holding the singleton instance
    public static GameManager Instance { get; private set; }

    // Enum defining game states
    public enum GameState { Title, Playing, Paused, GameOver }
    public GameState CurrentState { get; private set; }

    // Global game data
    // TODO: Completed levels? High-scores for each level?

    private void Awake()
    {
        // Singleton pattern implementation
        // If no instance exists, set this as the singleton
        if (Instance == null)
        {
            Instance = this;
            // Persist this object across scene loads
            DontDestroyOnLoad(gameObject);
        }
        // If instance already exists, destroy this duplicate
        else
        {
            Destroy(gameObject);
            return;
        }

        // Set initial state
        CurrentState = GameState.Title;
    }

    // Method to add score (callable from anywhere)
    /*public void RegisterNewHighScoreForLevel(int amount, ?? Level)
    {
        if (CurrentState != GameState.Playing) return;
        Score += amount;
        // UIManager.Instance.UpdateScoreUI(Score); // Request UI update?
    }*/

    // TODO: Map levels (name, music, high-score, etc. info) to SceneIDs!

    // TODO: Store current levelID, or -1 if we're not in Playing state!

    // Method to change game state
    public void ChangeState(GameState newState)
    {
        if (CurrentState == newState) return;

        CurrentState = newState;

        // Execute state-specific processing
        switch (newState)
        {
            case GameState.Title:
                // Prepare title screen
                break;
            case GameState.Playing:
                // Prepare for gameplay
                Time.timeScale = 1f; // Resume time
                break;
            case GameState.Paused:
                // Handle pause
                Time.timeScale = 0f; // Stop time
                break;
            case GameState.GameOver:
                // Handle game over
                break;
        }
    }

    // Example calls from other scripts:
    // GameManager.Instance.AddScore(100);
    // GameManager.Instance.ChangeState(GameManager.GameState.GameOver);
}