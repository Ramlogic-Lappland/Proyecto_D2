using System;
using UnityEngine;
using UnityEngine.AI;
public class DoorController : MonoBehaviour
{
    [SerializeField] private float movementInX;
    [SerializeField] private float movementInY;
    [SerializeField] private float movementInZ;
    [SerializeField] private float movementTime;

    private Vector3 _initialPosition;
    private NavMeshObstacle _obstacle; 
    private bool _isOpen; 
    public int id;
    
    /// <summary>
    /// awake method sets the basic starting values
    /// </summary>
    private void Awake() 
    { 
        _obstacle = GetComponent<NavMeshObstacle>(); 
        _isOpen = false;
        _initialPosition = transform.localPosition;

    }
     
    /// <summary>
    /// toggles if the obstacle navmesh is on or off / if there's no navmesh sends console msg
    /// </summary>
    public void ToggleDoor()
    {
        _isOpen = !_isOpen;
        if (_obstacle != null) 
        { 
            _obstacle.enabled = !_isOpen;
        }
        else 
        { 
            Debug.LogWarning("No NavMeshObstacle found on this door. Skip obstacle NavMesh.", this);
        }
    }
         
    /// <summary>
    /// this method subscribes to the list of events new ones
    /// </summary>
    private void Start()
    {
        GameEventsManager.Instance.OnOpenDoorEventTrigger += OpenDoor;
        GameEventsManager.Instance.OnCloseDoorEventTrigger += CloseDoor;
    }
    
    /// <summary>
    /// Opens the Door (moves to starting point + movement distance)
    /// </summary>
    private void OpenDoor(int id)
    {
        if (id == this.id)
        {
            ToggleDoor();
            Vector3 targetPos = _initialPosition + new Vector3(movementInX, movementInY, movementInZ);
            LeanTween.moveLocal(gameObject, targetPos, movementTime).setEaseOutQuad();
        }
    }
    
    /// <summary>
    /// Closes the Door (moves back to starting point)
    /// </summary>
    private void CloseDoor(int id)
    {
        if (id == this.id)
        {
            ToggleDoor();
            LeanTween.moveLocal(gameObject, _initialPosition, movementTime).setEaseOutQuad();
        }
    }

    /// <summary>
    /// DeSubscribes from event list
    /// </summary>
    private void OnDestroy()
    {
        GameEventsManager.Instance.OnOpenDoorEventTrigger  -= OpenDoor;
        GameEventsManager.Instance.OnCloseDoorEventTrigger -= CloseDoor;
    }
    public bool isOpen => _isOpen;
}
