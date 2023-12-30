using UnityEngine;

namespace Player
{
    public class PlayerPositionHandler : MonoBehaviour
    {
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Transform playerTransformLeft;
        [SerializeField] private Transform playerTransformRight;
        [SerializeField] private Transform playerTransformCenter;
        
        public void MovePlayerToCenter()
        {
            playerTransform.position = playerTransformCenter.position;
        }
        
        public void MovePlayerToLeft()
        {
            playerTransform.position = playerTransformLeft.position;
        }
        
        public void MovePlayerToRight()
        {
            playerTransform.position = playerTransformRight.position;
        }
    }
}
