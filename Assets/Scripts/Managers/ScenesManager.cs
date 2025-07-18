using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Instance { get; private set; }
    
    [Header("Loading Screen")]
    [SerializeField] private GameObject loadingInterface;
    [SerializeField] private Image loadingProgressBar;
    
    [Header("Scene Management")]
    public GameObject persistentObjects;
    private List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();
    private Scene currentLevelScene;

    /// <summary>
    /// Scenes in build settings
    /// </summary>
    public enum Scenes
    {
        MainMenu,
        GameScene, 
        Credits,
        // Additive scenes
        Tutorial,
        Level01,
        Level02
    }

    /// <summary>
    ///  additive Level scenes
    /// </summary>
    public enum GameLevels
    {
        Tutorial,
        Level01,
        Level02
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            if (persistentObjects == null)
            {
                persistentObjects = new GameObject("PersistentObjects");
                persistentObjects.transform.SetParent(transform);
                Debug.LogWarning("Automatically created PersistentObjects parent", this);
            }
        
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(persistentObjects);
        }
        else
        {
            Destroy(gameObject);
            if (persistentObjects != null && persistentObjects.name == "PersistentObjects")
                Destroy(persistentObjects);
        }
    }

    // MENU NAVIGATION
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(Scenes.MainMenu.ToString());
    }

    public void LoadCreditsScene()
    {
        SceneManager.LoadScene(Scenes.Credits.ToString());
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    // GAME LOADING SYSTEM 
    public void StartGame()
    {
        HideMenuUI();
        ShowLoadingScreen();
        
        scenesToLoad.Add(SceneManager.LoadSceneAsync(Scenes.GameScene.ToString()));
        
        scenesToLoad.Add(SceneManager.LoadSceneAsync(GameLevels.Tutorial.ToString(), LoadSceneMode.Additive));
        
        StartCoroutine(LoadingScreen());
        StartCoroutine(TrackLevelScene(GameLevels.Tutorial));
    }

    private IEnumerator LoadingScreen()
    {
        float totalProgress = 0;
        var completed = false;
        
        while (!completed)
        {
            totalProgress = 0;
            completed = true;
            
            foreach (AsyncOperation operation in scenesToLoad)
            {
                totalProgress += operation.progress;
                if (!operation.isDone) completed = false;
            }
            
            loadingProgressBar.fillAmount = totalProgress / scenesToLoad.Count;
            yield return null;
        }
        
        HideLoadingScreen();
    }

    public void LoadLevel(GameLevels level)
    {
        // Prepare player for transition
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            var playerComp = player.GetComponent<Player>();
            if (playerComp != null) playerComp.PrepareForSceneTransition();
        }
    
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
            MovePlayerToSpawn();
        };
    }

    private IEnumerator TrackLevelScene(GameLevels level)
    {
        while (!SceneManager.GetSceneByName(level.ToString()).isLoaded)
        {
            yield return null;
        }
        
        currentLevelScene = SceneManager.GetSceneByName(level.ToString());
        SceneManager.SetActiveScene(currentLevelScene);
        
       MovePlayerToSpawn();
    }


    private void MovePlayerToSpawn()
    {
        // Find spawn point in the new scene
        SpawnPoint spawn = FindObjectOfType<SpawnPoint>();
        if (spawn != null)
        {
            // Find player using the Player tag
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                Player player = playerObj.GetComponent<Player>();
            
                // Reset player position and rotation
                playerObj.transform.position = spawn.transform.position;
                playerObj.transform.rotation = spawn.transform.rotation;
            
                // Reset physics state
                Rigidbody rb = playerObj.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
            
                // Reset player input state using your existing system
                if (player != null)
                {
                    // Re-enable input actions if they were disabled
                    player.EnableInputActions();
                    player.ResetPlayerState();
                }
            }
            else
            {
                Debug.LogError("Player not found! Ensure your player has the 'Player' tag.");
            }
        }
        else
        {
            Debug.LogError("SpawnPoint not found in the loaded level!");
        }
    }

    //SCENE LOADER 
    public void LoadConnectedLevel(string levelName)
    {
        if (System.Enum.TryParse(levelName, out GameLevels level))
        {
            LoadLevel(level);
        }
    }

    // UI SETTINGS
    private void ShowLoadingScreen()
    {
        if (loadingInterface != null)
            loadingInterface.SetActive(true);
    }

    private void HideLoadingScreen()
    {
        if (loadingInterface != null)
            loadingInterface.SetActive(false);
    }

    private void HideMenuUI()
    {
        
    }
}