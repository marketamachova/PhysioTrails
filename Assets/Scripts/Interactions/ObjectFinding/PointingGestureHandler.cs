using System;
using Oculus.Interaction;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Interactions.ObjectFinding
{
    public class PointingGestureHandler : MonoBehaviour
    {
       [SerializeField] private SelectorUnityEventWrapper selectorUnityEventWrapper;
       [SerializeField] private bool isNetworkPointer = false;

        public UnityEvent onPointGestureStarted = new UnityEvent();
        public UnityEvent onPointGestureEnded = new UnityEvent();

        private void Awake()
        {
            selectorUnityEventWrapper.WhenSelected.AddListener(OnRightHandPointGestureStart);
            selectorUnityEventWrapper.WhenUnselected.AddListener(OnRightHandPointGestureEnd);
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
