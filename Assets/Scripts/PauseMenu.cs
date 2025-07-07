using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private InputActionReference pauseAction;
    public GameObject pauseMenu;
    public static bool IsPaused;


    private void Start()
    {
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
    
    public void GoToMainMenu(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    
    public void QuitGame()
    {
        Application.Quit();
        Debug.unityLogger.Log("Game has been Closed");
    }
}
