using UnityEngine;

namespace Interactions.ObjectFinding
{
    public class ObjectFindingNetworkHelper : MonoBehaviour
    {
        [SerializeField] private ObjectFindingSceneManager objectFindingSceneManager;

        private Transform _networkPointerTransform;
        private Transform _currentPointerTransform;
        private RaycastObjectPointer _currentRaycastObjectPointer;
        private ObjectFindingController _objectFindingController;
        void Start()
        {
            _objectFindingController = objectFindingSceneManager.ObjectFindingController;
            _currentPointerTransform = _objectFindingController.CurrentPointer.transform;
            _networkPointerTransform = _objectFindingController.NetworkPointerTransform;
            
            Debug.Log("current pointer: " + _currentPointerTransform.name);
            Debug.Log("network pointer: " + _networkPointerTransform.name);
            
            var handType = _objectFindingController.HandType;
        }   

        void Update()
        {
            SyncPointerPositionAndRotation();
        }
        
        private void SyncPointerPositionAndRotation()
        {
            var pointerPosition = _currentPointerTransform.position;
            var pointerRotation = _currentPointerTransform.rotation;

            _networkPointerTransform.position = pointerPosition;
            _networkPointerTransform.rotation = pointerRotation;
        }
        
    }
}
