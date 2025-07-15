using System;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button quitButton;

    /// <summary>
    /// adds a  listener to menu manager of the methos in this class
    /// </summary>
    private void Start()
    {
        creditsButton.onClick.AddListener(LoadCredits);
        startGameButton.onClick.AddListener(StartNewGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    /// <summary>
    /// loads first game scene (tutorial) through Scene Manager
    /// </summary>
    private void StartNewGame()
    {
        ScenesManager.Instance.LoadNewGame();
    }
    /// <summary>
    /// load Credits through Scene Manager
    /// </summary>
    private void LoadCredits()
    {
        ScenesManager.Instance.LoadCreditsScene();
    }
/// <summary>
/// quits game through Scene Manager
/// </summary>
    private void QuitGame()
    {
        ScenesManager.Instance.QuitGame();
    }
}
