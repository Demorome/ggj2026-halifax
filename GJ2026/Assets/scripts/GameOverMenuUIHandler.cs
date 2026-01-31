using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

// Sets the script to be executed later than all default scripts
// This is helpful for UI, since other things may need to be initialized before setting the UI
[DefaultExecutionOrder(1000)]
public class GameOverMenuUIHandler : MonoBehaviour
{
    private GameManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickRestartButton()
    {
        gameManager.ReloadCurrentScene();
        gameManager.ChangePlayingState(GameManager.PlayingState.Normal);
    }

    public void OnClickExitToMainMenuButton()
    {
        gameManager.ChangeGameState(GameManager.GameState.MainMenu);
    }
}
