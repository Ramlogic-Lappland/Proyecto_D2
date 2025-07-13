using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public int id;

    private void OnTriggerEnter(Collider other)
    {
        GameEventsManager.Current.OpenDoorEventTrigger(id);
    }
    private void OnTriggerExit(Collider other)
    {
        GameEventsManager.Current.CloseDoorEventTrigger(id);
    }
}
