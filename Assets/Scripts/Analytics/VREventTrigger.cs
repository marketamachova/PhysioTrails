using System;
using Player;
using UnityEngine;
using UnityEngine.Events;

namespace Analytics
{
    public class VREventTrigger : MonoBehaviour
    {
        [SerializeField] private string eventName;
        [SerializeField] private string tagToCompare = GameConstants.NetworkCamera;
        
        public UnityEvent<string> onTriggerEnter = new UnityEvent<string>();

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(tagToCompare))
            {
                onTriggerEnter.Invoke(eventName);
            }
        }
    }
}
