using System;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ScenesManager : MonoBehaviour //Scene[S] prevents from calling SceneManager which is from Unity (just in case you wonder) 
{
    public static ScenesManager Instance { get; private set; }
    

    /// <summary>
    /// declares that the instance of ScenesManager is this one
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// the Scenes are in order of how they appear in the build settings
    /// </summary>
    public enum Scenes
    {
        MainMenu,
        Tutorial,
        Credits,
        Level01,
        Level02
    }

    /// <summary>
    /// loads Scene
    /// </summary>
    /// <param name="scene"></param>
    public void LoadScene(Scenes scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }
    /// <summary>
    /// loads main menu
    /// </summary>
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(Scenes.MainMenu.ToString());
    }
    /// <summary>
    /// load new game
    /// </summary>
    public void LoadNewGame()
    {
        SceneManager.LoadScene(Scenes.Tutorial.ToString());
    }
    /// <summary>
    /// Loads Credits
    /// </summary>
    public void LoadCreditsScene()
    {
        SceneManager.LoadScene(Scenes.Credits.ToString());
    }
    /// <summary>
    /// loads next scene level
    /// </summary>
    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    /// <summary>
    /// quits game
    /// </summary>
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
