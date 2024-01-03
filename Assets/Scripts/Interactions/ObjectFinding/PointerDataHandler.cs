using UnityEngine;

namespace Interactions.ObjectFinding
{
    public class PointerDataHandler : MonoBehaviour
    {
        [SerializeField] private Transform leftHandPointerTransform;
        [SerializeField] private Transform rightHandPointerTransform;
        [SerializeField] private Transform leftHandNetworkPointerTransform;
        [SerializeField] private Transform rightHandNetworkPointerTransform;

        public Transform LeftHandPointerTransform => leftHandPointerTransform;

        public Transform RightHandPointerTransform => rightHandPointerTransform;

        public Transform LeftHandNetworkPointerTransform => leftHandNetworkPointerTransform;

        public Transform RightHandNetworkPointerTransform => rightHandNetworkPointerTransform;
    }
}
