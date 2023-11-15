using System.Collections.Generic;
using Oculus.Interaction.HandGrab;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

namespace Interactions.WireLoop
{
    public class WireLoopController : MonoBehaviour
    {
        [SerializeField] private bool startCollisionDetectionOnGrab;
        [FormerlySerializedAs("pathCollider")] [SerializeField] private WireLoopCollider wireLoopCollider;
        [SerializeField] private WireLoopVisualiser wireLoopVisualiser;
        [SerializeField] private PathController pathController;
        [SerializeField] private ScoreController scoreController;
        [SerializeField] private List<HandGrabInteractor> handGrabInteractors;
        [SerializeField] private GameObject torus;
        [SerializeField] private GameObject torusGhost;
        
        private bool _enableCollisions = false;
        private bool _isIdle = true;


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
            
            pathController.GeneratePath();
            torusGhost.SetActive(false);

            // TODO implement listening for ring grab by hands
        }
        
        [ContextMenu("Generate Path")]
        public void GeneratePath()
        {
            pathController.GeneratePath();
        }

        private void OnTorusCollisionStart(bool isTrigger)
        {
            Debug.Log("ON COLLISION START");
            wireLoopVisualiser.OnCollisionStart();
            scoreController.OnMiss();

            if (isTrigger)
            {
                // Force disable grab from controllers
                // handGrabInteractors.ForEach(interactor => interactor.Unselect());
                torus.transform.localPosition = Vector3.zero;
                torusGhost.SetActive(true);
            }
        }

        private void OnTorusCollisionEnd(bool isTrigger)
        {
            wireLoopVisualiser.OnCollisionEnd();
            // Start trail ghost?
            torusGhost.SetActive(false);
        }

        // private void OnTorusCollisionStay(bool isTrigger)
        // {
        //     if (isTrigger)
        //     {
        //         torus.transform.localPosition = Vector3.zero;
        //     }
        // }

    }
}
