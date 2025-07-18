using UnityEngine;
public class LevelTransition : MonoBehaviour
{
    [SerializeField] private string nextLevelName;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ScenesManager.Instance.LoadConnectedLevel(nextLevelName);
        }
    }
}