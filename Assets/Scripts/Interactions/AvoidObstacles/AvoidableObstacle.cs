using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactions.AvoidObstacles
{
    public class AvoidableObstacle : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private AvoidableObstacleCollider avoidLeftCollider;
        [SerializeField] private AvoidableObstacleCollider avoidRightCollider;
        [SerializeField] private AvoidableObstacleCollider hitCollider;
        [SerializeField] private Renderer meshRenderer;
        [SerializeField] private Animator balloonAnimator;
        [SerializeField] private Animation balloonAnimation;
        
        [Header("Settings")]
        [SerializeField] private AvoidableObstacleCollider.ObstacleColliderType _expectedColliderType;
        [SerializeField] private Material materialWithArrows;
        [SerializeField] private Material materialWithoutArrows;
        [SerializeField] private string hitAnimationName = "Deflate";
        [SerializeField] private int deflateAnimationSpeedFactor = 2;
        
        private AvoidObstaclesController _avoidObstaclesController;
        private bool useArrows = true;

        private void Awake()
        {
            avoidLeftCollider.onHit.AddListener(OnHit);
            avoidRightCollider.onHit.AddListener(OnHit);
            hitCollider.onHit.AddListener(OnHit);
        }
        
        private void Start()
        {
            _avoidObstaclesController = FindObjectOfType<AvoidObstaclesController>();
            if (_avoidObstaclesController != null)
            {
                useArrows = _avoidObstaclesController.DisplayArrows;
                _avoidObstaclesController.onDisplayArrowsChanged.AddListener(SetMaterial);
            }
            SetMaterial(useArrows);
        }

        private void OnHit(AvoidableObstacleCollider.ObstacleColliderType obstacleColliderType)
        {
            if (_avoidObstaclesController == null)
            {
                Debug.LogError("AvoidObstaclesController is null");
                return;
            }
            
            if (obstacleColliderType == AvoidableObstacleCollider.ObstacleColliderType.Hit)
            {
                _avoidObstaclesController.OnHit();
                EnableColliders(false);
                DeflateBalloon();
            }
            else if (_expectedColliderType == obstacleColliderType)
            {
                _avoidObstaclesController.OnAvoidCorrect();
                // TODO play sound, update visuals
            }
            else // Avoided the wrong way
            {
                _avoidObstaclesController.OnAvoidIncorrect();
                // TODO play sound, update visuals
            }
        }

        private void SetMaterial(bool displayArrows)
        {
            meshRenderer.material = displayArrows ? materialWithArrows : materialWithoutArrows;
        }

        [ContextMenu("Test Deflate")]
        public void TestDeflate()
        {
            DeflateBalloon();
        }

        private void EnableColliders(bool enable)
        {
            avoidLeftCollider.enabled = enable;
            avoidRightCollider.enabled = enable;
            hitCollider.enabled = enable;
        }
        
        private void DeflateBalloon()
        {
            balloonAnimator.Play(hitAnimationName);
            // TODO play sounds
            StartCoroutine(DestroyAfterAnimation());
        }
        
        private IEnumerator DestroyAfterAnimation()
        {
            yield return new WaitForSeconds(balloonAnimation.clip.length / deflateAnimationSpeedFactor);

            Destroy(gameObject);
        }

        public AvoidObstaclesController AvoidObstaclesController
        {
            get => _avoidObstaclesController;
            set => _avoidObstaclesController = value;
        }

        public AvoidableObstacleCollider.ObstacleColliderType ExpectedColliderType
        {
            get => _expectedColliderType;
            set => _expectedColliderType = value;
        }

        public bool UseArrows
        {
            get => useArrows;
            set
            {
                useArrows = value;
                SetMaterial(useArrows);
            }
        }
    }
}
