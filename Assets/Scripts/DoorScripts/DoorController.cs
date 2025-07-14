using System;
using UnityEngine;
using UnityEngine.AI;
public class DoorController : MonoBehaviour
{
    [SerializeField] private float movementInX;
    [SerializeField] private float movementInY;
    [SerializeField] private float movementInZ;
    [SerializeField] private float movementTime;

    private float _initialPositionX;
    private float _initialPositionY;
    private float _initialPositionZ;
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
        _initialPositionX = gameObject.transform.position.x;
        _initialPositionY = gameObject.transform.position.y;
        _initialPositionZ = gameObject.transform.position.z;
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
        GameEventsManager.Current.OnOpenDoorEventTrigger += OpenDoor;
        GameEventsManager.Current.OnCloseDoorEventTrigger += CloseDoor;
    }
    
    /// <summary>
    /// Opens the Door (moves to starting point + movement distance)
    /// </summary>
    private void OpenDoor(int id)
    {
        if (id == this.id)
        {
            ToggleDoor();
            LeanTween.moveLocalX(gameObject, gameObject.transform.position.x + movementInX, movementTime).setEaseOutQuad();
            LeanTween.moveLocalY(gameObject, gameObject.transform.position.y + movementInY, movementTime).setEaseOutQuad();
            LeanTween.moveLocalZ(gameObject, gameObject.transform.position.z +movementInZ, movementTime).setEaseOutQuad();
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
            LeanTween.moveLocalX(gameObject, _initialPositionX, movementTime).setEaseOutQuad();
            LeanTween.moveLocalY(gameObject, _initialPositionY, movementTime).setEaseOutQuad();
            LeanTween.moveLocalZ(gameObject, _initialPositionZ, movementTime).setEaseOutQuad();
        }
    }

    /// <summary>
    /// DeSubscribes from event list
    /// </summary>
    private void OnDestroy()
    {
        GameEventsManager.Current.OnOpenDoorEventTrigger  -= OpenDoor;
        GameEventsManager.Current.OnCloseDoorEventTrigger -= CloseDoor;
    }
}
