using System;
using UnityEngine;
using Utils;

namespace Interactions
{
    public class InteractionConfigurator : MonoBehaviour
    {
        public enum InteractionType {WireLoop, ObjectFinding, AvoidObstacles, None}
        public enum HandType {Right, Left}
        public enum DifficultyType {Easy, Medium, Hard}

        [SerializeField] private InteractionType type = InteractionType.None;
        [SerializeField] private HandType hand = HandType.Right;
        [SerializeField] private DifficultyType difficulty = DifficultyType.Easy;
        
        [SerializeField] private  InteractionManager interactionManager;
        
        public event Action OnInteractionsConfigurationComplete;

        public void SetInteractionTypeWireLoop()
        {
            Type = InteractionType.WireLoop;
        }
        
        public void SetInteractionTypeObjectFinding()
        {
            Type = InteractionType.ObjectFinding;
        }
        
        public void SetInteractionTypeAvoidObstacles()
        {
            Type = InteractionType.AvoidObstacles;
        }
        
        public void SetInteractionTypeNone()
        {
            Type = InteractionType.None;
        }
        
        public void SetHandTypeRight()
        {
            Hand = HandType.Right;
        }
        
        public void SetHandTypeLeft()
        {
            Hand = HandType.Left;
        }
        
        public void SetDifficultyTypeEasy()
        {
            Difficulty = DifficultyType.Easy;
        }
        
        public void SetDifficultyTypeMedium()
        {
            Difficulty = DifficultyType.Medium;
        }
        
        public void SetDifficultyTypeHard()
        {
            Difficulty = DifficultyType.Hard;
        }

        public void Submit()
        {
            OnInteractionsConfigurationComplete?.Invoke();
        }
        
        public InteractionType Type
        {
            get => type;
            set
            {
                type = value;
                interactionManager.CurrentInteractionType = type;
            }
        }

        public HandType Hand
        {
            get => hand;
            set => hand = value;
        }

        public DifficultyType Difficulty
        {
            get => difficulty;
            set => difficulty = value;
        }
    }
}
