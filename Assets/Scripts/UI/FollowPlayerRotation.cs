using UnityEngine;

namespace UI
{
    public class FollowPlayerRotation : MonoBehaviour
    {
        public Transform playerTransform;
        public float rotationSpeed = 5f;
        public float rotationDelay = 0.5f;

        private Quaternion targetRotation;

        void Start()
        {
            if (playerTransform == null)
            {
                Debug.LogError("Asign transform.");
            }
        }

        void Update()
        {
            Vector3 lookAtPlayer = playerTransform.position - transform.position;
            targetRotation = Quaternion.LookRotation(lookAtPlayer, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}