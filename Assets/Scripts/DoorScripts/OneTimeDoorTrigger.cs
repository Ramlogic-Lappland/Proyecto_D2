using UnityEngine;

public class OneTimeDoorTrigger : MonoBehaviour
{
    public int id;
    private bool _hasTriggered = false;

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !_hasTriggered)
        {
            _hasTriggered = true;
            GameEventsManager.Instance.CloseDoorEventTrigger(id);
            GetComponent<Collider>().enabled = false;
        }
    }
}