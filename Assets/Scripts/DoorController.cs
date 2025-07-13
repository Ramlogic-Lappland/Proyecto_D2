using System;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public int id;
    private void Start()
    {
        GameEventsManager.Current.OnOpenDoorEventTrigger += OpenDoor;
        GameEventsManager.Current.OnCloseDoorEventTrigger += CloseDoor;
    }

    private void OpenDoor(int id)
    {
        if (id == this.id)
        {
            LeanTween.moveLocalY(gameObject, 9.16f, 2f).setEaseOutQuad();
        }
    }
    private void CloseDoor(int id)
    {
        if (id == this.id)
        {
            LeanTween.moveLocalY(gameObject, 4, 2f).setEaseOutQuad();
        }
    }
}
