using System;
using Oculus.Interaction;
using UnityEngine;
using UnityEngine.Events;

namespace Interactions.ObjectFinding
{
    public class PointingGestureHandler : MonoBehaviour
    {
        [SerializeField] private SelectorUnityEventWrapper selectorUnityEventWrapperRightHand;

        public UnityEvent onPointGestureStarted = new UnityEvent();
        public UnityEvent onPointGestureEnded = new UnityEvent();

        private void Awake()
        {
            selectorUnityEventWrapperRightHand.WhenSelected.AddListener(OnRightHandPointGestureStart);
            selectorUnityEventWrapperRightHand.WhenUnselected.AddListener(OnRightHandPointGestureEnd);
        }

        private void OnRightHandPointGestureStart()
        {
            onPointGestureStarted.Invoke();
        }
        
        private void OnRightHandPointGestureEnd()
        {
            onPointGestureEnded.Invoke();
        }
    }
}
