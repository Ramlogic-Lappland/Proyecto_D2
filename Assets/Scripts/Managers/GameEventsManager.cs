using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
  public static GameEventsManager Instance;

  /// <summary>
  /// Declares a  singleton of this class - This lets us just have 1 eventManager in the entire game and also we will be able to call it from any scene since is not destryoed on load
  /// </summary>
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
  
  /// <summary>
  /// subscribes a trigger event 
  /// </summary>
  public event Action<int> OnOpenDoorEventTrigger;
  private readonly HashSet<int> _openDoors = new HashSet<int>(); 
  public bool IsDoorOpen(int doorID) => _openDoors.Contains(doorID);

  public void OpenDoorEventTrigger(int id)
  {
    _openDoors.Add(id);
    OnOpenDoorEventTrigger?.Invoke(id);
  }
  /// <summary>
  /// subscribes a trigger event 
  /// </summary>
  public event Action<int> OnCloseDoorEventTrigger;
  public void CloseDoorEventTrigger(int id)
  {
    _openDoors.Remove(id);
    OnCloseDoorEventTrigger?.Invoke(id);
  }
}
