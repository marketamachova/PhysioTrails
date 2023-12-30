using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Interactions.AvoidObstacles
{
    [RequireComponent(typeof(Collider))]
    public class AvoidableObstacleCollider : MonoBehaviour
    {
        [SerializeField] private string playerHeadColliderTag = "PlayerHead";
        public enum ObstacleColliderType
        {
            Left,
            Right,
            Hit
        }
        
        [SerializeField] private ObstacleColliderType obstacleColliderType;
        
        public UnityEvent<ObstacleColliderType> onHit = new UnityEvent<ObstacleColliderType>();
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(playerHeadColliderTag)) // TODO update the tags
            {
                Debug.Log("Head entered collider " + obstacleColliderType);
                onHit.Invoke(obstacleColliderType);
            }
        }
    }
}
