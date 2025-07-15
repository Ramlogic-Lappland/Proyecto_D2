using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private InputActionReference pauseAction;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button quitButton;
    public GameObject pauseMenu;
    public static bool IsPaused;
    
    /// <summary>
    /// adds a  listener to menu manager of the methods in this class
    /// </summary>
    private void Start()
    {
        resumeButton.onClick.AddListener(ResumeGame);
        mainMenuButton.onClick.AddListener(LoadMainMenu);
        quitButton.onClick.AddListener(QuitGame);
        pauseMenu.SetActive(false);
    }

    private void OnEnable()
    {
        pauseAction.action.started += PauseCalled;
        pauseAction.action.Enable();
    }

    private void OnDisable()
    {
        pauseAction.action.started -= PauseCalled;
        pauseAction.action.Disable();
    }

    private void PauseCalled(InputAction.CallbackContext context)
    {
        if (SceneManager.GetActiveScene().name == "MainMenu") return;
        if (IsPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        IsPaused = true;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LoadMainMenu()
    {
        Time.timeScale = 1f; // Ensure timescale is reset
        IsPaused = false;
        ScenesManager.Instance.LoadMainMenu(); // Use your existing SceneManager
    }

    private void QuitGame()
    {
        ScenesManager.Instance.QuitGame(); // Use your existing SceneManager
    }
}