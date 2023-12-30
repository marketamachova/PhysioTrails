using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using PathCreation.Examples;
using Player;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace Interactions.WireLoop
{
    public class WireLoopSceneManager : InteractionSceneManagerBase
    {
        [SerializeField] private WireLoopCollider wireLoopPathCollider;
        
        [Header("Torus")]
        [SerializeField] private List<Rigidbody> torusGrabbableRigidbodies;
        [SerializeField] private List<GameObject> torusSizes;
        [SerializeField] private GameObject torusGhost;
        [SerializeField] private GameObject torusGhostRightHand;
        [SerializeField] private GameObject torusGhostLeftHand;
        [SerializeField] private PathFollower torusPathFollower;
        [SerializeField] private List<WireLoopVisualiser> wireLoopVisualisers;
        [SerializeField] private List<InteractableUnityEventWrapper> torusGrabEventsWrappers;

        public UnityEvent onTorusGrabStarted = new UnityEvent();
        public UnityEvent onTorusGrabEnded = new UnityEvent();

        private WireLoopController _wireLoopController;
        private InteractionManager _interactionManager;
        private Coroutine _resetTorusRbPositionCoroutine;
        
        private PlayerPositionHandler _playerPositionHandler;
        
        private void Awake()
        {
            torusGrabEventsWrappers.ForEach(wrapper => wrapper.WhenSelect.AddListener(OnTorusGrabStart));
            torusGrabEventsWrappers.ForEach(wrapper => wrapper.WhenUnselect.AddListener(OnTorusGrabEnd));
        }
        
        private void Start()
        {
            _playerPositionHandler = FindObjectOfType<PlayerPositionHandler>();

            _wireLoopController = FindObjectOfType<WireLoopController>();
            _wireLoopController.WireLoopSceneManager = this;
            _wireLoopController.OnSceneLoaded();
            
            var difficulty = _wireLoopController.Difficulty;
            EnableTorusBasedOnDifficulty(difficulty);
            
            var handType = _wireLoopController.HandType;
            EnableTorusGhostHand(handType);
            PositionPlayerBasedOnHandType(handType);
            
            torusGhost.SetActive(false);
            torusPathFollower.enabled = false;
        }
        
        private void EnableTorusBasedOnDifficulty(InteractionConfigurator.DifficultyType difficulty)
        {
            torusSizes.ForEach(torusSize => torusSize.SetActive(false));

            switch (difficulty)
            {
                case InteractionConfigurator.DifficultyType.Easy:
                    torusSizes[0].SetActive(true);
                    break;
                case InteractionConfigurator.DifficultyType.Medium:
                    torusSizes[1].SetActive(true);
                    break;
                case InteractionConfigurator.DifficultyType.Hard:
                    torusSizes[2].SetActive(true);
                    break;
            }
        }

        private void EnableTorusGhostHand(InteractionConfigurator.HandType handType)
        {
            torusGhostRightHand.SetActive(handType == InteractionConfigurator.HandType.Right);
            torusGhostLeftHand.SetActive(handType == InteractionConfigurator.HandType.Left);
        }
        
        private void PositionPlayerBasedOnHandType(InteractionConfigurator.HandType handType)
        {
            Debug.Log("Kuk position player based on hand type");
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
            torusPathFollower.enabled = enable;
        }

        public void EnableTorusMovement(bool enable = true)
        {
            torusPathFollower.enabled = enable;
        }
        
        private void RecenterAllTorus()
        {
            torusGrabbableRigidbodies.ForEach(rb => RecenterTorus(rb.transform));
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
            // EnablePlayerMovement();
            onTorusGrabStarted.Invoke();
            torusGrabbableRigidbodies.ForEach(rb => rb.isKinematic = true);
            
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
            wireLoopVisualisers.ForEach(visualiser => visualiser.OnCollisionStart());
            _wireLoopController.OnMiss();

            if (isTrigger)
            {
                torusGhost.SetActive(true);
            }
        }

        private void OnTorusCollisionEnd(bool isTrigger)
        {
            wireLoopVisualisers.ForEach(visualiser => visualiser.OnCollisionEnd());
            
            // Start trail ghost?
            torusGhost.SetActive(false);
        }
        
        public void SetSpeed(int speed)
        {
            torusPathFollower.speed = speed;
        }
    }
}