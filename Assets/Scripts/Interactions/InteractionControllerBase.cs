using System;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace Interactions
{
    public abstract class InteractionControllerBase : MonoSingleton<InteractionControllerBase>
    {
        public UnityEvent onInteractionReady = new UnityEvent();
        [SerializeField] protected bool shouldWaitForInteractionStart = false;
        protected InteractionNetworkDataHolder interactionNetworkDataHolder;

        protected abstract void InvokeInteractionReady();
        
        public abstract void SetSpeed(int speed);
        
        public abstract void SetDifficulty(InteractionConfigurator.DifficultyType difficultyType);
        public abstract void SetHandType(InteractionConfigurator.HandType handType);

        public virtual void SetFindableObjectType(int findableObjectType)
        {
        }
        
        public bool ShouldWaitForInteractionStart => shouldWaitForInteractionStart;

        public InteractionNetworkDataHolder InteractionNetworkDataHolder
        {
            get => interactionNetworkDataHolder;
            set => interactionNetworkDataHolder = value;
        }
    }
}
