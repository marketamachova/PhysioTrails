using System.Collections;
using Oculus.Interaction;
using PathCreation.Examples;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace Interactions.WireLoop
{
    public class WireLoopSceneManager : InteractionSceneManagerBase
    {
        [SerializeField] private WireLoopCollider wireLoopPathCollider;
        
        [Header("Torus")]
        [SerializeField] private Rigidbody torusGrabbableRigidbody;
        [SerializeField] private GameObject torusGhost;
        [SerializeField] private PathFollower torusPathFollower;
        [SerializeField] private WireLoopVisualiser wireLoopVisualiser;
        [SerializeField] private InteractableUnityEventWrapper torusGrabEventsWrapper;
        
        [Header("Testing")] 
        [SerializeField] private PathFollower playerPathFollower;

        public UnityEvent onTorusGrabStarted = new UnityEvent();
        public UnityEvent onTorusGrabEnded = new UnityEvent();

        private WireLoopController _wireLoopController;
        private InteractionManager _interactionManager;
        private Coroutine _resetTorusRbPositionCoroutine;
        
        private void Awake()
        {
            torusGrabEventsWrapper.WhenSelect.AddListener(OnTorusGrabStart);
            torusGrabEventsWrapper.WhenUnselect.AddListener(OnTorusGrabEnd);
        }
        
        private void Start()
        {
            _wireLoopController = FindObjectOfType<WireLoopController>();
            _wireLoopController.WireLoopSceneManager = this;
            _wireLoopController.OnSceneLoaded();
            
            torusGhost.SetActive(false);
            torusPathFollower.enabled = false;
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
        
        private void RecenterTorus()
        {
            var transform1 = torusGrabbableRigidbody.transform;
            transform1.localPosition = Vector3.zero;
            transform1.localRotation = Quaternion.identity;
        }
        
        private void EnablePlayerMovement(bool enable = true)
        {
            playerPathFollower.enabled = enable;
        }
        
        private void OnTorusGrabStart()
        {
            StartTorusMovement();
            EnablePlayerMovement();
            onTorusGrabStarted.Invoke();
            torusGrabbableRigidbody.isKinematic = true;
            
            if (_resetTorusRbPositionCoroutine != null)
            {
                StopCoroutine(_resetTorusRbPositionCoroutine);
                _resetTorusRbPositionCoroutine = null;
            }
        }

        private void OnTorusGrabEnd()
        {
            RecenterTorus();
            StartTorusMovement();
            onTorusGrabEnded.Invoke();

            _resetTorusRbPositionCoroutine = StartCoroutine(ResetTorusRbPosition());
        }
        
        private IEnumerator ResetTorusRbPosition()
        {
            while (true)
            {
                RecenterTorus();
                yield return null;
            }
        }

        private void OnTorusCollisionStart(bool isTrigger)
        {
            wireLoopVisualiser.OnCollisionStart();
            _wireLoopController.OnMiss();

            if (isTrigger)
            {
                torusGhost.SetActive(true);
            }
        }

        private void OnTorusCollisionEnd(bool isTrigger)
        {
            wireLoopVisualiser.OnCollisionEnd();
            
            // Start trail ghost?
            torusGhost.SetActive(false);
        }
        
        public void SetSpeed(int speed)
        {
            torusPathFollower.speed = speed;
        }
    }
}