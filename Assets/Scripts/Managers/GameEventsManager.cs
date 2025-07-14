using System;
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
  public static GameEventsManager Instance;

  private void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
      DontDestroyOnLoad(gameObject); // Keeps object alive across scenes
    }
    else
    {
      Destroy(gameObject); // Prevents duplicates
    }
  }
  
  public event Action<int> OnOpenDoorEventTrigger;
  public void OpenDoorEventTrigger(int id)
  {
    OnOpenDoorEventTrigger?.Invoke(id);
  }
  public event Action<int> OnCloseDoorEventTrigger;
  public void CloseDoorEventTrigger(int id)
  {
    OnCloseDoorEventTrigger?.Invoke(id);
  }
  
}
