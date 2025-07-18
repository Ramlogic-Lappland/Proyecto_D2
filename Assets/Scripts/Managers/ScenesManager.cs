using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Instance { get; private set; }
    
    [Header("Scene Management")]
    public GameObject persistentObjects;
    private List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();
    private Scene currentLevelScene;

    // Scenes in build settings
    public enum Scenes
    {
        MainMenu,
        Credits,
        GameScene, 
        // Additive scenes
        Tutorial,
        Level_1,
        Level_2
    }

    // Additive Level scenes
    public enum GameLevels
    {
        Tutorial,
        Level_1,
        Level_2
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            if (persistentObjects != null) Destroy(persistentObjects);
            return;
        }
        
        Instance = this;
        if (persistentObjects == null)
        {
            persistentObjects = new GameObject("PersistentObjects");
            persistentObjects.transform.SetParent(transform);
            Debug.Log("Created PersistentObjects parent", this);
        }
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(persistentObjects);
    }

    public void StartGame()
    {
        Debug.Log("Starting game...");

        scenesToLoad.Clear();

        scenesToLoad.Add(SceneManager.LoadSceneAsync(Scenes.GameScene.ToString(), LoadSceneMode.Single));

        scenesToLoad.Add(SceneManager.LoadSceneAsync(GameLevels.Tutorial.ToString(), LoadSceneMode.Additive));

        StartCoroutine(TrackLevelScene(GameLevels.Tutorial));
    }

    private IEnumerator TrackLevelScene(GameLevels level)
    {
        string levelName = level.ToString();
        Debug.Log($"Waiting for {levelName} to load...");
        
        // Wait until scene is fully loaded
        while (!SceneManager.GetSceneByName(levelName).isLoaded)
        {
            yield return null;
        }
        
        currentLevelScene = SceneManager.GetSceneByName(levelName);
        SceneManager.SetActiveScene(currentLevelScene);
        Debug.Log($"Set active scene to {currentLevelScene.name}");
    }

    public void LoadLevel(GameLevels level)
    {
        Debug.Log($"Loading level: {level}");
        
        // Unload current level if exists
        if (currentLevelScene.IsValid())
        {
            SceneManager.UnloadSceneAsync(currentLevelScene);
        }
        
        // Load new level additively
        SceneManager.LoadSceneAsync(level.ToString(), LoadSceneMode.Additive).completed += (AsyncOperation op) =>
        {
            currentLevelScene = SceneManager.GetSceneByName(level.ToString());
            SceneManager.SetActiveScene(currentLevelScene);
            Debug.Log($"Level {level} loaded and set as active");
        };
    }
    public void LoadConnectedLevel(string levelName)
    {
        if (System.Enum.TryParse(levelName, out GameLevels level))
        {
            LoadLevel(level);
        }
        else
        {
            Debug.LogError($"Invalid level name: {levelName}");
        }
    }
    public void LoadMainMenu()
    {
        Debug.Log("Loading main menu...");
        SceneManager.LoadScene(Scenes.MainMenu.ToString());
    }

    public void LoadCreditsScene()
    {
        Debug.Log("Loading credits...");
        SceneManager.LoadScene(Scenes.Credits.ToString());
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}