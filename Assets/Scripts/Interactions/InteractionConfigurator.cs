using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
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
        [SerializeField] private bool displayArrowsAvoidObstacles = true;
        [Tooltip("0 - Edible (Herbivorous), 1 - Inedible (Omnivorous), 2 - Poisonous (Carnivorous)")]
        [SerializeField] private int findableObjectType = 0;
        
        [SerializeField] private  InteractionUIItem interactionUIItem;
        
        public UnityEvent<string> onInteractionsConfigurationComplete = new UnityEvent<string>();

        
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
        
        public void SetDisplayArrowsAvoidObstacles(bool display)
        {
            displayArrowsAvoidObstacles = display;
        }
        
        public void SetFindableObjectTypeEdible()
        {
            findableObjectType = 0;
        }
        
        public void SetFindableObjectTypeInedible()
        {
            findableObjectType = 1;
        }
        
        public void SetFindableObjectTypePoisonous()
        {
            findableObjectType = 2;
        }

        public void Submit()
        {
            // Serialize data
            var serializedData = InteractionDataSerializer.SerializeToJson(Type, Hand, Difficulty,
                displayArrowsAvoidObstacles, findableObjectType);
            Debug.Log("Submitted interaction data: " + serializedData);
            interactionUIItem.UpdateUI(Type, Hand, Difficulty);
            onInteractionsConfigurationComplete?.Invoke(serializedData);
        }

        public void SetData(string data)
        {
            InteractionData deserializedData = InteractionDataSerializer.DeserializeFromJson(data);
            Type = deserializedData.Type;
            Hand = deserializedData.Hand;
            Difficulty = deserializedData.Difficulty;
            displayArrowsAvoidObstacles = deserializedData.DisplayArrowsAvoidObstacles;
            findableObjectType = deserializedData.FindableObjectType;
            
            interactionUIItem.UpdateUI(Type, Hand, Difficulty);
            
            // To sync with local Interaction Manager
            onInteractionsConfigurationComplete?.Invoke(data);
        }
        
        public InteractionType Type
        {
            get => type;
            set
            {
                type = value;
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
