using UnityEngine;
using UnityEngine.Events;

namespace Interactions
{
    public abstract class InteractionControllerBase : MonoBehaviour
    {
        public UnityEvent onInteractionReady = new UnityEvent();
        [SerializeField] protected bool shouldWaitForInteractionStart = false;
        protected abstract void InvokeInteractionReady();
        
        public abstract void SetSpeed(int speed);

        public bool ShouldWaitForInteractionStart => shouldWaitForInteractionStart;
    }
}
