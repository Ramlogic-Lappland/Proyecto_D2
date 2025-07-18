using System;using UnityEngine;
using UnityEngine.SceneManagement;

public enum CheckMethod
{
    Distance,
    Trigger
}
public class ScenePartLoader : MonoBehaviour
{
    public Transform player;
    public CheckMethod checkMethod;
    public float loadRange;

    private bool _isLoaded;
    private bool _shouldLoad;

    private void Start()
    {
        if (SceneManager.sceneCount > 0) //veryfy if scene is open this prevents of loading the same scene 2 times
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
              Scene scene = SceneManager.GetSceneAt(i);
              if (scene.name == gameObject.name)
              {
                  _isLoaded = true;
              }
            }
        }
    }

    private void Update()
    {
        if (checkMethod == CheckMethod.Distance)
        {
            DistanceCheck();
        }
        else if (checkMethod == CheckMethod.Trigger)
        {
            TriggerCheck();
        }
    }
    
    private void DistanceCheck()
    {
        if (Vector3.Distance(player.position, transform.position) <= loadRange)
        {
            LoadScene();
        }
        else
        {
            UnLoadScene();
        }
    
    }
    
    private void TriggerCheck()
    {
        if (_shouldLoad)
        {
            LoadScene();
        }
        else
        {
            UnLoadScene();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _shouldLoad = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _shouldLoad = false;
        }
    }

    void LoadScene()
    {
        if (!_isLoaded)
        {
            SceneManager.LoadSceneAsync(gameObject.name, LoadSceneMode.Additive);
            _isLoaded = true;
        }
    }

    void UnLoadScene()
    {
        if (_isLoaded)
        {
            SceneManager.UnloadSceneAsync(gameObject.name);
            _isLoaded = false;
        }
    }

}
