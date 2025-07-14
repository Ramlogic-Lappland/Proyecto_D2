using UnityEngine;

public class KeyScript : MonoBehaviour
{
    [SerializeField] private GameObject keyObject;
        public int doorID;
        /// <summary>
        /// Interacts with player hitting E to grab the key
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            //PlayerInventory.Current.AddKey(doorID);

            GameEventsManager.Current.OpenDoorEventTrigger(doorID);
            
            Destroy(keyObject);
        }
}
