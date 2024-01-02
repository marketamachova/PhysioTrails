using System.Collections.Generic;
using Oculus.Interaction;
using PathCreation.Examples;
using UnityEngine;

namespace Interactions.WireLoop
{
    public class TorusDataHolder : MonoBehaviour
    {
        [SerializeField] private List<GameObject> torusGrabbables;
        [SerializeField] private List<Rigidbody> torusGrabbableRigidbodies;
        [SerializeField] private List<GameObject> torusSizes;
        [SerializeField] private GameObject torusGhost;
        [SerializeField] private GameObject torusGhostRightHand;
        [SerializeField] private GameObject torusGhostLeftHand;
        [SerializeField] private PathFollower torusPathFollower;
        [SerializeField] private List<WireLoopVisualiser> wireLoopVisualisers;
        [SerializeField] private List<InteractableUnityEventWrapper> torusGrabEventsWrappers;
        [SerializeField] private bool hasRigidbody = true;
        
        public List<GameObject> TorusGrabbables => torusGrabbables;
        
        public List<Rigidbody> TorusGrabbableRigidbodies => torusGrabbableRigidbodies;
        
        public List<GameObject> TorusSizes => torusSizes;

        public GameObject TorusGhost => torusGhost;

        public GameObject TorusGhostRightHand => torusGhostRightHand;

        public GameObject TorusGhostLeftHand => torusGhostLeftHand;

        public PathFollower TorusPathFollower => torusPathFollower;

        public List<WireLoopVisualiser> WireLoopVisualisers => wireLoopVisualisers;

        public List<InteractableUnityEventWrapper> TorusGrabEventsWrappers => torusGrabEventsWrappers;
    }
}
