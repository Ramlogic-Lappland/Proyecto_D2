using UnityEngine;
using UnityEngine.UI;

public class UIGameMenu : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;

    private void Start()
    {
        mainMenuButton.onClick.AddListener(LoadMainMenu);
    }

    private void LoadMainMenu()
    {
        ScenesManager.Instance.LoadMainMenu();
    }
}
