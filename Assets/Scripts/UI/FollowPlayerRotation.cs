using UnityEngine;

namespace UI
{
    public class FollowPlayerRotation : MonoBehaviour
    {
        [SerializeField] private string playerTag = "MainCamera";
        [SerializeField] private float rotationSpeed = 5f;
        [SerializeField] private float rotationDelay = 0.5f;

        private Quaternion _targetRotation;
        private Transform _playerTransform;

        void Start()
        {
            if (_playerTransform == null)
            {
                _playerTransform = GameObject.FindWithTag(playerTag).transform;
            }
        }

        void Update()
        {
            Vector3 lookAtPlayer = _playerTransform.position - transform.position;
            _targetRotation = Quaternion.LookRotation(lookAtPlayer, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}