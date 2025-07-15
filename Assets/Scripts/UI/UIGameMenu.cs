using UnityEngine;
using UnityEngine.UI;

public class UIGameMenu : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;

    /// <summary>
    /// Adds a listener to the menu event manager
    /// </summary>
    private void Start()
    {
        mainMenuButton.onClick.AddListener(LoadMainMenu); 
    }

    /// <summary>
    /// goes to main menu
    /// </summary>
    private void LoadMainMenu()
    {
        ScenesManager.Instance.LoadMainMenu();
    }
}
