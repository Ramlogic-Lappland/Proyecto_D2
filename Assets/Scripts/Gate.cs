using System;
using UnityEngine;
using UnityEngine.AI;
public class Gate : MonoBehaviour
{
    [SerializeField] private NavMeshObstacle obstacle;
    private bool _isOpen;

    private void Awake()
    {
        _isOpen = false;
    }

    public void ToggleDoor()
    {
        _isOpen = !_isOpen;
        obstacle.enabled = !_isOpen;
    }
}