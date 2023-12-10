using System.Collections.Generic;
using Oculus.Interaction.HandGrab;
using PathCreation;
using PathCreation.Examples;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

namespace Interactions.WireLoop
{
    public class WireLoopController : InteractionControllerBase
    {
        [SerializeField] private bool startCollisionDetectionOnGrab;
        [SerializeField] private ScoreController scoreController;
        [SerializeField] private List<HandGrabInteractor> handGrabInteractors;

        [SerializeField] private Transform playerWrapperTransform;

        private WireLoopSceneManager _wireLoopSceneManager;
        
        private bool _enableCollisions = false;
        private bool _isIdle = true;

        private int _customSpeed;

        public void OnSceneLoaded()
        {
            _wireLoopSceneManager.onTorusGrabStarted.AddListener(OnTorusGrabStart);
            _wireLoopSceneManager.SetSpeed(_customSpeed);
        }

        public void OnMiss()
        {
            scoreController.OnMiss();
        }
        
        [ContextMenu("Generate Path")]
        public void GeneratePath()
        {
            // pathController.GeneratePath();
        }

        public WireLoopSceneManager WireLoopSceneManager
        {
            get => _wireLoopSceneManager;
            set => _wireLoopSceneManager = value;
        }

        private void OnTorusGrabStart()
        {
            InvokeInteractionReady();
            // _wireLoopSceneManager.EnableTorusMovement();
        }
        

        protected override void InvokeInteractionReady()
        {
            onInteractionReady.Invoke();
        }

        public override void SetSpeed(int speed)
        {
            _customSpeed = speed;
            if (_wireLoopSceneManager != null)
            {
                _wireLoopSceneManager.SetSpeed(speed);
            }
        }
    }
}