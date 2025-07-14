using UnityEditor.PackageManager;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// allows the interactor to invoke behaviours of different interactable items
/// </summary>
interface IInteractable
{
    public void Interact();
}
public class Interactor : MonoBehaviour
{
    public Transform interactorSource;
    public float interactRange;
    
    /// <summary>
    /// Checks if key pressed (should adapt to new input system) and if the key pressed casts a  raycast to player and if facing the object then works
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            var r = new Ray(interactorSource.position, interactorSource.forward);
            if (Physics.Raycast(r, out RaycastHit hit, interactRange))
            {
                if (hit.collider.gameObject.TryGetComponent(out IInteractable interactObj))
                {
                    interactObj.Interact();
                }
            }
        }
    }
}
