using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    
    private void Update()
    {
        transform.position = target.position; 
    }
}
