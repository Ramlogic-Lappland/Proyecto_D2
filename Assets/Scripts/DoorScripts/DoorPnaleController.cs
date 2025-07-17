using System;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class DoorPanelController : MonoBehaviour
{
    [SerializeField] private int doorID;
    [SerializeField] private Color lockedColor = Color.red;
    [SerializeField] private Color unlockedColor = Color.green;
    [SerializeField] private float emissionIntensity = 2f;
    
    private Material _panelMaterial;
    private Renderer _panelRenderer;

    private void Awake()
    {
        _panelRenderer = GetComponent<Renderer>();
        _panelMaterial = _panelRenderer.material;
        _panelMaterial.EnableKeyword("_EMISSION");
        SetPanelColor(lockedColor);
    }

    private void Start()
    {
        if (GameEventsManager.Instance != null)
        {
            SubscribeToEvents();
            HandleDoorEvent(doorID); 
        }
        else
        {
            Debug.LogError("GameEventsManager not found in scene!", this);
        }
    }
    private void SubscribeToEvents()
    {
        GameEventsManager.Instance.OnOpenDoorEventTrigger += HandleDoorEvent;
        GameEventsManager.Instance.OnCloseDoorEventTrigger += HandleDoorEvent;
    }
    
    private void OnEnable()
    {
        if (GameEventsManager.Instance != null)
        {
            GameEventsManager.Instance.OnOpenDoorEventTrigger += HandleDoorEvent;
            GameEventsManager.Instance.OnCloseDoorEventTrigger += HandleDoorEvent;
        }
        else
        {
            Debug.LogWarning("GameEventsManager instance not available yet", this);
        }
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.OnOpenDoorEventTrigger -= HandleDoorEvent;
        GameEventsManager.Instance.OnCloseDoorEventTrigger -= HandleDoorEvent;
    }

    private void HandleDoorEvent(int triggeredDoorID)
    {
        if (triggeredDoorID != doorID) return;
        
        bool shouldOpen = GameEventsManager.Instance.IsDoorOpen(doorID);
        SetPanelColor(shouldOpen ? unlockedColor : lockedColor);
    }

    private void SetPanelColor(Color color)
    {
        _panelMaterial.SetColor("_EmissionColor", color * emissionIntensity);
        DynamicGI.UpdateEnvironment();
    }
}