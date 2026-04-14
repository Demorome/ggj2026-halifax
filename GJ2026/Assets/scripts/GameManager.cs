using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using Components;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

// Credits to this for the baseline:
// https://uhiyama-lab.com/en/notes/unity/unity-gameloop-gamemanager-pattern-guide/
public class GameManager : MonoBehaviour
{
    // Static field holding the singleton instance
    public static GameManager Instance { get; private set; }

    // Enum defining game states
    public enum GameState { MainMenu, Playing }

    public GameState CurrentGameState { get; private set; }
        = GameState.MainMenu;

    public enum PlayingState { None, Normal, Paused, LevelComplete, GameOver }

    public PlayingState CurrentPlayingState { get; private set; }
        = PlayingState.None;

    public GameObject GameOverScreenPrefab;
    private GameObject GameOverScreen;

    public GameObject PauseScreenPrefab;
    private GameObject PauseScreen;

    public GameObject InteractionTextCanvasPrefab;
    private static GameObject InteractionTextCanvas;
    private static TMP_Text InteractionText;

    public UIDocument healthUIDoc;
    private VisualElement playerHostHealthMeter;
    private VisualElement playerFinalHealthMeter;
    private VisualElement playerHostHealthBarFill;

    public UIDocument objectivesUIDoc;
    private VisualElement objectiveProgressContainer;
    private TextElement objectiveMessageElement;
    private TextElement objectiveProgressElement;
    private TextElement objectiveEmojiIconElement;

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

            // Set event handlers
            SceneManager.sceneLoaded += OnSceneLoaded;

            var currentSceneID = SceneManager.GetActiveScene().buildIndex;

            // Set initial state
            if (currentSceneID != 0)
            {
                CurrentGameState = GameState.Playing;
                CurrentPlayingState = PlayingState.Normal;
            }

            playerHostHealthMeter = healthUIDoc.rootVisualElement.Q<VisualElement>("HealthBarMask");
            playerFinalHealthMeter = healthUIDoc.rootVisualElement.Q<VisualElement>("FinalHealthBarMask");
            playerHostHealthBarFill = healthUIDoc.rootVisualElement.Q<VisualElement>("HealthBarFill");
            Debug.Assert(playerHostHealthMeter != null && playerFinalHealthMeter != null
                                                       && playerHostHealthBarFill != null);

            objectiveMessageElement = objectivesUIDoc.rootVisualElement.Q<TextElement>("Objective");
            objectiveProgressContainer = objectivesUIDoc.rootVisualElement.Q<VisualElement>("Progress");
            objectiveProgressElement = objectivesUIDoc.rootVisualElement.Q<TextElement>("ProgressText");
            objectiveEmojiIconElement = objectivesUIDoc.rootVisualElement.Q<TextElement>("ObjectiveEmoji");
            Debug.Assert(objectiveMessageElement != null && objectiveProgressElement != null
                                                         && objectiveEmojiIconElement != null);

            CanPossessComponent.OnPlayerEnterHost += OnPlayerEnterHost;

            InteractionTextCanvas = Instantiate(InteractionTextCanvasPrefab);
            DontDestroyOnLoad(InteractionTextCanvas);
            var textChild = InteractionTextCanvas
                .transform
                .GetChild(0);
            Debug.Assert(textChild);
            InteractionText = textChild.gameObject.GetComponent<TMP_Text>(); 
            Debug.Assert(InteractionText);

            InteractionTextCanvas.SetActive(false);
        }
        // If instance already exists, destroy this duplicate
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public bool IsObjectiveUIHidden()
    {
        return objectivesUIDoc.rootVisualElement.style.display == DisplayStyle.None;
    }

    public void HideObjectiveUI()
    {
        objectivesUIDoc.rootVisualElement.style.display = DisplayStyle.None;
    }

    public void ShowObjectiveUI()
    {
        objectivesUIDoc.rootVisualElement.style.display = DisplayStyle.Flex;
    }

    public void ChangeObjective(string newMessage, Color color, string newEmojiIcon, string newProgress)
    {
        if (IsObjectiveUIHidden())
        {
            Debug.Log("ChangeObjective: Un-hid the objective UI.");
            ShowObjectiveUI();
        }

        objectiveMessageElement.text = newMessage;
        objectiveMessageElement.style.color = color;

        if (newEmojiIcon != null)
        {
            objectiveProgressContainer.visible = true;
            objectiveEmojiIconElement.text = newEmojiIcon;
            UpdateObjectiveProgress(newProgress);
            objectiveProgressElement.style.color = color;
        }
        else
        {
            objectiveProgressContainer.visible = false;
        }

    }

    public void UpdateObjectiveProgress(string newProgress)
    {
        if (!objectiveProgressContainer.visible)
        {
            Debug.LogError("Objective progress not visible; why update it?");
        }

        objectiveProgressElement.text = newProgress;
    }

    public GameObject GetInteractionCanvas()
    {
        return InteractionTextCanvas;
    }

    public void ChangeInteractionText(string newText)
    {
        InteractionText.text = newText;
    }
    public void ChangeInteractionTextColor(Color newColor)
    {
        InteractionText.color = newColor;
    }

    private void OnPlayerEnterHost()
    {
        var actorType = CanPossessComponent.GetCurrentPlayerActorType();
        var color = ActorTypeComponent.ColorForActorType(actorType);
        playerHostHealthBarFill.style.unityBackgroundImageTintColor = color;
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
        if (CurrentGameState == newState || IsLoading)
        {
            return;
        }

        if (IsLoading)
        {
            Debug.Log("Tried to change game state while loading");
            return;
        }

        Debug.Log("Exiting Game state: " +  CurrentGameState);

        Debug.Log("Entering new Game state: " +  newState);

        // Execute state-specific processing
        switch (newState)
        {
            case GameState.MainMenu:
                // Prepare title screen
                LoadMainMenuScene();
                break;
            case GameState.Playing:
                // Prepare for gameplay
                ChangePlayingState(PlayingState.Normal);

                // TODO: Use level selection menu info instead, to load
                // a specific scene.
                LoadNextScene();

                break;
        }

        CurrentGameState = newState;
    }

    public void ChangePlayingState(PlayingState newState)
    {
        if (CurrentPlayingState == newState)
        {
            return;
        }

        if (IsLoading)
        {
            Debug.Log("Tried to change playing state while loading");
            return;
        }

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

    private void LoadMainMenuScene()
    {
        //sets scene to the current scene restarting
        IsWaitingForSceneLoad = true;
        LoadScene(0);
    }

    public void LoadNextScene()
    {
        //sets scene to the current scene restarting
        IsWaitingForSceneLoad = true;
        var currentScene = SceneManager.GetActiveScene();
        var nextID = currentScene.buildIndex + 1;
        LoadScene(nextID);
    }

    public void ReloadCurrentScene()
    {
        //sets scene to the current scene restarting
        //SceneManager.SetActiveScene(SceneManager.GetActiveScene());
        IsWaitingForSceneLoad = true;
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        LoadScene(currentSceneIndex);
    }

    private CancellationTokenSource cts;
    public bool IsLoading => IsWaitingForSceneLoad || cts != null;
    private bool IsWaitingForSceneLoad;

    // Try Catch async task
    // Source: https://gist.github.com/VinayKashyap06/f536f68d769030101d93430b683e695c
    private async void LoadScene(int sceneIndex)
    {
        if (cts == null)
        {
            cts = new CancellationTokenSource();
            try
            {
                await PerformSceneLoading(cts.Token, sceneIndex);
            }
            catch (OperationCanceledException ex)
            {
                if (ex.CancellationToken == cts.Token)
                {
                    // Perform operation after cancelling
                    Debug.Log("LoadScene: Task cancelled!");
                }
            }
            finally
            {
                cts.Cancel();
                cts = null;
            }
        }
        else
        {
            // Cancel Previous token
            cts.Cancel();
            cts = null;
        }
    }

    // Actual Scene loading
    private async Task PerformSceneLoading(CancellationToken token, int sceneID)
    {
        token.ThrowIfCancellationRequested();
        if (token.IsCancellationRequested)
            return;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneID);
        asyncOperation.allowSceneActivation = false;
        while (true)
        {
            token.ThrowIfCancellationRequested();
            if (token.IsCancellationRequested)
                return;
            if (asyncOperation.progress >= 0.9f)
                break;
        }
        asyncOperation.allowSceneActivation = true;
        cts.Cancel();
        token.ThrowIfCancellationRequested();

        //added this as a failsafe unnecessary
        if (token.IsCancellationRequested)
            return;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene loaded: " + scene.name);
        IsWaitingForSceneLoad = false;
    }

    public void ShowLevelCompleteScreen()
    {
        //SceneManager.SetActiveScene();
    }

    // Example calls from other scripts:
    // GameManager.Instance.AddScore(100);
    // GameManager.Instance.ChangeState(GameManager.GameState.GameOver);

    public void UpdatePlayerFinalHealthUI(float currentHealth, float maxHealth)
    {
        // Credits to https://learn.unity.com/tutorial/make-health-bar-with-UItoolkit
        float healthRatio = currentHealth / maxHealth;
        float healthPercent = Mathf.Lerp(8, 88, healthRatio);
        playerFinalHealthMeter.style.width = Length.Percent(healthPercent);
        //Debug.Log("HostHealth UI width: " + playerHealthMeter.style.width);
    }

    // NOTE: Host health is different from the Player's own final health!
    public void UpdatePlayerHostHealthUI(float currentHealth, float maxHealth)
    {
        // Credits to https://learn.unity.com/tutorial/make-health-bar-with-UItoolkit
        float healthRatio = currentHealth / maxHealth;
        float healthPercent = Mathf.Lerp(8, 88, healthRatio);
        playerHostHealthMeter.style.width = Length.Percent(healthPercent);
        //Debug.Log("HostHealth UI width: " + hostHealthMeter.style.width);
    }
}