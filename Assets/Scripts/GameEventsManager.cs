using System;
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
  public static GameEventsManager Current;

  private void Awake()
  {
    Current = this;
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
