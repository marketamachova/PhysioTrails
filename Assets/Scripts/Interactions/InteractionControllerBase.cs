using System;
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
        
        public abstract void SetDifficulty(InteractionConfigurator.DifficultyType difficultyType);
        public abstract void SetHandType(InteractionConfigurator.HandType handType);
        
        public bool ShouldWaitForInteractionStart => shouldWaitForInteractionStart;
    }
}
