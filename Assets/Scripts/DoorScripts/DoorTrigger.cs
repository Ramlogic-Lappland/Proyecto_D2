using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public int id;

    /// <summary>
    /// player hits the collider calling the door open trigger 
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        GameEventsManager.Current.OpenDoorEventTrigger(id);
    }
    /// <summary>
    /// player exists the collider calling the door close trigger
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        GameEventsManager.Current.CloseDoorEventTrigger(id);
    }
}
