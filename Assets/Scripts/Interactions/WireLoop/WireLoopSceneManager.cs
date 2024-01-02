using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using PathCreation.Examples;
using Player;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Utils;

namespace Interactions.WireLoop
{
    public class WireLoopSceneManager : InteractionSceneManagerBase
    {
        [SerializeField] private WireLoopCollider wireLoopPathCollider;
        [SerializeField] private TorusDataHolder torusDataHolder;

        public UnityEvent onTorusGrabStarted = new UnityEvent();
        public UnityEvent onTorusGrabEnded = new UnityEvent();

        private WireLoopController _wireLoopController;
        private InteractionManager _interactionManager;
        private Coroutine _resetTorusRbPositionCoroutine;
        
        private PlayerPositionHandler _playerPositionHandler;
        
        
        private void Start()
        {
            _playerPositionHandler = FindObjectOfType<PlayerPositionHandler>();

            _wireLoopController = FindObjectOfType<WireLoopController>();
            _wireLoopController.WireLoopSceneManager = this;
            _wireLoopController.OnSceneLoaded();
            
            torusDataHolder.TorusGrabEventsWrappers.ForEach(wrapper => wrapper.WhenSelect.AddListener(OnTorusGrabStart));
            torusDataHolder.TorusGrabEventsWrappers.ForEach(wrapper => wrapper.WhenUnselect.AddListener(OnTorusGrabEnd));
            
            var difficulty = _wireLoopController.Difficulty;
            EnableTorusBasedOnDifficulty(difficulty);
            
            var handType = _wireLoopController.HandType;
            EnableTorusGhostHand(handType);
            PositionPlayerBasedOnHandType(handType);
            
            torusDataHolder.TorusGhost.SetActive(false);
            torusDataHolder.TorusPathFollower.enabled = false;
        }
        
        private void EnableTorusBasedOnDifficulty(InteractionConfigurator.DifficultyType difficulty)
        {
            torusDataHolder.TorusSizes.ForEach(torusSize => torusSize.SetActive(false));

            switch (difficulty)
            {
                case InteractionConfigurator.DifficultyType.Easy:
                    torusDataHolder.TorusSizes[0].SetActive(true);
                    break;
                case InteractionConfigurator.DifficultyType.Medium:
                    torusDataHolder.TorusSizes[1].SetActive(true);
                    break;
                case InteractionConfigurator.DifficultyType.Hard:
                    torusDataHolder.TorusSizes[2].SetActive(true);
                    break;
            }
        }

        private void EnableTorusGhostHand(InteractionConfigurator.HandType handType)
        {
            torusDataHolder.TorusGhostRightHand.SetActive(handType == InteractionConfigurator.HandType.Right);
            torusDataHolder.TorusGhostLeftHand.SetActive(handType == InteractionConfigurator.HandType.Left);
        }
        
        private void PositionPlayerBasedOnHandType(InteractionConfigurator.HandType handType)
        {
            if (handType == InteractionConfigurator.HandType.Right)
            {
                _playerPositionHandler.MovePlayerToLeft();
            }
            else
            {
                _playerPositionHandler.MovePlayerToRight();
            }
        }
        
        public void StartTorusMovement(bool enable = true)
        {
            wireLoopPathCollider.collisionStart.AddListener(OnTorusCollisionStart);
            wireLoopPathCollider.collisionEnd.AddListener(OnTorusCollisionEnd);
            
            wireLoopPathCollider.EnableCollisionDetection(enable);
            torusDataHolder.TorusPathFollower.enabled = enable;
        }

        public void EnableTorusMovement(bool enable = true)
        {
            torusDataHolder.TorusPathFollower.enabled = enable;
        }
        
        private void RecenterAllTorus()
        {
            torusDataHolder.TorusGrabbableRigidbodies.ForEach(rb => RecenterTorus(rb.transform));
        }
        
        private void RecenterTorus(Transform transform1)
        {
            transform1.localPosition = Vector3.zero;
            transform1.localRotation = Quaternion.identity;
        }

        private void OnTorusGrabStart()
        {
            Debug.Log("Kuk grab start");
            StartTorusMovement();
            onTorusGrabStarted.Invoke();
            torusDataHolder.TorusGrabbableRigidbodies.ForEach(rb => rb.isKinematic = true);
            
            if (_resetTorusRbPositionCoroutine != null)
            {
                StopCoroutine(_resetTorusRbPositionCoroutine);
                _resetTorusRbPositionCoroutine = null;
            }
        }

        private void OnTorusGrabEnd()
        {
            Debug.Log("Kuk grab end");
            RecenterAllTorus();
            StartTorusMovement();
            onTorusGrabEnded.Invoke();

            _resetTorusRbPositionCoroutine = StartCoroutine(ResetTorusRbPosition());
        }
        
        private IEnumerator ResetTorusRbPosition()
        {
            while (true)
            {
                RecenterAllTorus();
                yield return null;
            }
        }

        private void OnTorusCollisionStart(bool isTrigger)
        {
            torusDataHolder.WireLoopVisualisers.Find(visualiser => visualiser.isActiveAndEnabled).OnCollisionStart();
            _wireLoopController.OnMiss();

            if (isTrigger)
            {
                torusDataHolder.TorusGhost.SetActive(true);
            }
        }

        private void OnTorusCollisionEnd(bool isTrigger)
        {
            torusDataHolder.WireLoopVisualisers.ForEach(visualiser => visualiser.OnCollisionEnd());
            
            // Start trail ghost?
            torusDataHolder.TorusGhost.SetActive(false);
        }
        
        public void SetSpeed(int speed)
        {
            torusDataHolder.TorusPathFollower.speed = speed;
        }
    }
}