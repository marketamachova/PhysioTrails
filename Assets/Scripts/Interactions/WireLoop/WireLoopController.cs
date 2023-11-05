using System.Collections.Generic;
using Oculus.Interaction.HandGrab;
using UnityEngine;
using UnityEngine.Serialization;

namespace Interactions.WireLoop
{
    public class WireLoopController : MonoBehaviour
    {
        [SerializeField] private bool startCollisionDetectionOnGrab;
        [FormerlySerializedAs("pathCollider")] [SerializeField] private WireLoopCollider wireLoopCollider;
        [SerializeField] private WireLoopVisualiser wireLoopVisualiser;
        [SerializeField] private ScoreController scoreController;
        [SerializeField] private List<HandGrabInteractor> handGrabInteractors;
        
        private bool _enableCollisions = false;


        private void Awake()
        {
            wireLoopCollider.collisionStart.AddListener(OnTorusCollisionStart);
            wireLoopCollider.collisionEnd.AddListener(OnTorusCollisionEnd);
        }

        private void Start()
        {
            if (!startCollisionDetectionOnGrab)
            {
                wireLoopCollider.EnableCollisionDetection(true);
            }
        
            // TODO implement listening for ring grab by hands
        }

        private void OnTorusCollisionStart(bool isTrigger)
        {
            Debug.Log("ON COLLISION START");
            wireLoopVisualiser.OnCollisionStart();
            scoreController.OnMiss();

            if (isTrigger)
            {
                // Force disable grab from controllers
                handGrabInteractors.ForEach(interactor => interactor.Unselect());
            }
        }

        private void OnTorusCollisionEnd(bool isTrigger)
        {
            wireLoopVisualiser.OnCollisionEnd();
            // Start trail ghost?
        }

    }
}
