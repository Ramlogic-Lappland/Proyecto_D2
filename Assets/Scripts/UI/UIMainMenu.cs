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
        quitButton.onClick.AddListener(QuitGame);
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
