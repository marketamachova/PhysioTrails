using System;
using UnityEngine;

namespace Interactions.AvoidObstacles
{
    public class AvoidableObstacle : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private AvoidObstaclesController avoidObstaclesController;
        [SerializeField] private AvoidableObstacleCollider avoidLeftCollider;
        [SerializeField] private AvoidableObstacleCollider avoidRightCollider;
        [SerializeField] private AvoidableObstacleCollider hitCollider;
        
        [Header("Settings")]
        [SerializeField] private AvoidableObstacleCollider.ObstacleColliderType _expectedColliderType;

        private void Awake()
        {
            avoidLeftCollider.onHit.AddListener(OnHit);
            avoidRightCollider.onHit.AddListener(OnHit);
            hitCollider.onHit.AddListener(OnHit);
        }

        private void OnHit(AvoidableObstacleCollider.ObstacleColliderType obstacleColliderType)
        {
            if (obstacleColliderType == AvoidableObstacleCollider.ObstacleColliderType.Hit)
            {
                avoidObstaclesController.OnHit();
                // TODO play sound, update visuals
            }
            else if (_expectedColliderType == obstacleColliderType)
            {
                avoidObstaclesController.OnAvoidCorrect();
                // TODO play sound, update visuals
            }
            else // Avoided the wrong way
            {
                avoidObstaclesController.OnAvoidIncorrect();
                // TODO play sound, update visuals
            }
        }

        public AvoidObstaclesController AvoidObstaclesController
        {
            get => avoidObstaclesController;
            set => avoidObstaclesController = value;
        }

        public AvoidableObstacleCollider.ObstacleColliderType ExpectedColliderType
        {
            get => _expectedColliderType;
            set => _expectedColliderType = value;
        }
    }
}
