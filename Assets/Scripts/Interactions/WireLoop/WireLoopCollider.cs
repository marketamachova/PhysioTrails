using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Interactions.WireLoop
{
    public class WireLoopCollider : MonoBehaviour  
    {
        [SerializeField] private string torusTagName;
        [SerializeField] private int waitSecondsBetweenCollisions;
        [SerializeField] private List<Collider> colliders;

        // The bool signifies if the collision is trigger
        public UnityEvent<bool> collisionStart = new UnityEvent<bool>();
        public UnityEvent<bool> collisionEnd = new UnityEvent<bool>();
        
        public void EnableCollisionDetection(bool enable)
        {
            //colliders.ForEach(col => col.enabled = enable);
        }

        private void OnCollisionEnter(Collision other)
        {
            Debug.Log("Path collided with " + other.gameObject.name);

            if (other.gameObject.CompareTag(torusTagName))
            {
                collisionStart.Invoke(false);
            }
        }

        private void OnCollisionExit(Collision other)
        {
            Debug.Log("Path ended collision with " + other.gameObject.name);
            if (other.gameObject.CompareTag(torusTagName))
            {
                collisionEnd.Invoke(false);
            }
        }

        // Should account for collisions happening when holding the torus by user (torus' rigidbody is kinematic)
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Path started trigger with " + other.gameObject.name);
            if (other.gameObject.CompareTag(torusTagName))
            {
                collisionStart.Invoke(true);
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            Debug.Log("Path ended trigger with " + other.gameObject.name);
            if (other.gameObject.CompareTag(torusTagName))
            {
                collisionEnd.Invoke(true);
            }
        }
    }
}
