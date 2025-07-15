using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;


public class GameManager : MonoBehaviour
{ 
    public static GameManager GameManagerInstance { get; private set; }
    
    public UnitHealth PlayerHealth = new UnitHealth(100, 100);
    private void Awake()
    {
        if (GameManagerInstance == null) 
        {
            GameManagerInstance = this;
            DontDestroyOnLoad(gameObject); // Keeps object alive across scenes
        }
        else
        {
            Destroy(gameObject); // Prevents duplicates
        }
    }
}
