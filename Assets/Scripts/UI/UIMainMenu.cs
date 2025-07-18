using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button quitButton;

    /// <summary>
    /// checks if assigned and adds listeners
    /// </summary>
    private void Start()
    {
        if (startGameButton == null) Debug.LogError("Start button not assigned!", this);
        if (creditsButton == null) Debug.LogError("Credits button not assigned!", this);
        if (quitButton == null) Debug.LogError("Quit button not assigned!", this);
        
        startGameButton.onClick.AddListener(StartGame);
        creditsButton.onClick.AddListener(LoadCredits);
        quitButton.onClick.AddListener(QuitGame);
        
        Debug.Log("UI Main Menu initialized");
    }

    private void StartGame()
    {
        Debug.Log("Start Game button clicked");
        
        if (ScenesManager.Instance != null)
        {
            ScenesManager.Instance.StartGame();
        }
        else
        {
            Debug.LogError("ScenesManager instance not found!");
        }
    }
/// <summary>
/// loads credits 
/// </summary>
    private void LoadCredits()
    {
        Debug.Log("Credits button clicked");
        ScenesManager.Instance.LoadCreditsScene();
    }
/// <summary>
/// quits game
/// </summary>
    private void QuitGame()
    {
        Debug.Log("Quit button clicked");
        ScenesManager.Instance.QuitGame();
    }
}