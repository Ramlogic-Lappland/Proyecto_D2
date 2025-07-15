using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class UILoadCredits : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button quitGameButton;
    private void Start()
    {
        mainMenuButton.onClick.AddListener(LoadCredits);
        quitGameButton.onClick.AddListener(QuitGame);
    }

    private void LoadCredits()
    {
        ScenesManager.Instance.LoadMainMenu();
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
