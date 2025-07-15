using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private InputActionReference pauseAction;
    public GameObject pauseMenu;
    public static bool IsPaused;


    /// <summary>
    /// sets that menu by default is off once scene loads
    /// </summary>
    private void Start()
    {
        pauseMenu.SetActive(false);
    }

    /// <summary>
    /// Enables the new input system pause button call
    /// </summary>
    private void OnEnable()
    {
        pauseAction.action.started += PauseCalled;
        pauseAction.action.Enable();
    }
/// <summary>
/// Disables the new input system pause button call
/// </summary>
    private void OnDisable()
    {
        pauseAction.action.started -= PauseCalled;
        pauseAction.action.Disable();
    }
    
/// <summary>
/// Depending on if Pause was called when was true or false, decides to set the boolean the oposite of its current state
/// </summary>
/// <param name="context"></param>
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

/// <summary>
/// sets the pause menu as active and stops game
/// </summary>
    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        IsPaused = true;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
/// <summary>
/// disables pause menu and resumes game
/// </summary>
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    /// <summary>
    /// gos to main menu from button
    /// </summary>
    /// <param name="sceneName"></param>
    public void GoToMainMenu(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    /// <summary>
    /// quits game (in case of editor sends a  log msg)
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
        Debug.unityLogger.Log("Game has been Closed");
    }
}
