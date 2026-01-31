using UnityEngine;
using UnityEngine.SceneManagement;

// Credits to this for the baseline:
// https://uhiyama-lab.com/en/notes/unity/unity-gameloop-gamemanager-pattern-guide/
public class GameManager : MonoBehaviour
{
    // Static field holding the singleton instance
    public static GameManager Instance { get; private set; }

    // Enum defining game states
    public enum GameState { MainMenu, Playing }
    public GameState CurrentGameState { get; private set; }

    public enum PlayingState { Normal, Paused, LevelComplete, GameOver }
    public PlayingState CurrentPlayingState { get; private set; }

    public GameObject GameOverScreenPrefab;
    private GameObject GameOverScreen;

    public GameObject PauseScreenPrefab;
    private GameObject PauseScreen;

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
        CurrentGameState = GameState.MainMenu;
    }

    // Method to add score (callable from anywhere)
    /*public void RegisterNewHighScoreForLevel(int amount, ?? Level)
    {
        if (CurrentState != GameState.Playing) return;
        Score += amount;
        // UIManager.Instance.UpdateScoreUI(Score); // Request UI update?
    }*/

    // TODO: Map levels (name, music, high-score, etc. info) to SceneIDs!

    // Method to change game state
    public void ChangeGameState(GameState newState)
    {
        if (CurrentGameState == newState) return;

        Debug.Log("Exiting Game state: " +  CurrentGameState);

        Debug.Log("Entering new Game state: " +  newState);

        // Execute state-specific processing
        switch (newState)
        {
            case GameState.MainMenu:
                // Prepare title screen
                break;
            case GameState.Playing:
                // Prepare for gameplay
                CurrentPlayingState = PlayingState.Normal;

                // TODO: Use level selection menu info instead, to load
                // a specific scene.
                LoadNextScene();

                break;
        }

        CurrentGameState = newState;
    }

    public void ChangePlayingState(PlayingState newState)
    {
        if (CurrentPlayingState == newState) return;

        Debug.Log("Exiting Playing state: " +  CurrentPlayingState);

        // Handle exiting-state logic
        switch (CurrentPlayingState)
        {
            case PlayingState.Normal:
                // Handle leaving normal gameplay mode.
                break;
            case PlayingState.LevelComplete:
                // Handle leaving LevelComplete
                // TODO: Show a level complete menu instead
                LoadNextScene();
                break;

            case PlayingState.Paused:
                // Handle leaving Pause menu
                Time.timeScale = 1f; // Resume time
                Destroy(PauseScreen);
                break;
            case PlayingState.GameOver:
                // Handle leaving the game over screen
                Destroy(GameOverScreen);
                GameOverScreen = null;
                break;
        }

        Debug.Log("Entering new Playing state: " +  newState);

        // Handle entering-state logic
        switch (newState)
        {
            case PlayingState.Normal:
                break;
            case PlayingState.LevelComplete:
                break;

            case PlayingState.Paused:
                Time.timeScale = 0f; // Stop time
                PauseScreen = Instantiate(PauseScreenPrefab);
                break;
            case PlayingState.GameOver:
                GameOverScreen = Instantiate(GameOverScreenPrefab);
                break;
        }

        CurrentPlayingState = newState;
    }

    public void LoadNextScene()
    {
        //sets scene to the current scene restarting
        var currentScene = SceneManager.GetActiveScene();
        var nextID = currentScene.buildIndex + 1;

        SceneManager.LoadScene(nextID);
    }

    public void ReloadCurrentScene()
    {
        //sets scene to the current scene restarting
        SceneManager.SetActiveScene(SceneManager.GetActiveScene());
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ShowLevelCompleteScreen()
    {
        //SceneManager.SetActiveScene();
    }

    // Example calls from other scripts:
    // GameManager.Instance.AddScore(100);
    // GameManager.Instance.ChangeState(GameManager.GameState.GameOver);
}