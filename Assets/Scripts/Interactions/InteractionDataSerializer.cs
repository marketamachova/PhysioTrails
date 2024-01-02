using System;
using Interactions;
using UnityEngine;

namespace Interactions
{
    [System.Serializable]
    public class InteractionSerializedData
    {
        public string type;
        public string hand;
        public string difficulty;
        public bool displayArrowsAvoidObstacles;
        public int findableObjectType;
    }

    public class InteractionData
    {
        private InteractionConfigurator.InteractionType _type;
        private InteractionConfigurator.HandType _hand;
        private InteractionConfigurator.DifficultyType _difficulty;
        private bool _displayArrowsAvoidObstacles;
        private int _findableObjectType;
        
        public InteractionData(InteractionConfigurator.InteractionType type,
            InteractionConfigurator.HandType hand, InteractionConfigurator.DifficultyType difficulty,
            bool displayArrowsAvoidObstacles, int findableObjectType)
        {
            this._type = type;
            this._hand = hand;
            this._difficulty = difficulty;
            this._displayArrowsAvoidObstacles = displayArrowsAvoidObstacles;
            this._findableObjectType = findableObjectType;
        }
        
        public InteractionConfigurator.InteractionType Type => _type;
        public InteractionConfigurator.HandType Hand => _hand;
        public InteractionConfigurator.DifficultyType Difficulty => _difficulty;
        public bool DisplayArrowsAvoidObstacles => _displayArrowsAvoidObstacles;
        public int FindableObjectType => _findableObjectType;
    }

    public static class InteractionDataSerializer
    {
        public static string SerializeToJson(InteractionConfigurator.InteractionType type,
            InteractionConfigurator.HandType hand, InteractionConfigurator.DifficultyType difficulty,
            bool displayArrowsAvoidObstacles, int findableObjectType)
        {
            InteractionSerializedData data = new InteractionSerializedData
            {
                type = type.ToString(),
                hand = hand.ToString(),
                difficulty = difficulty.ToString(),
                displayArrowsAvoidObstacles = displayArrowsAvoidObstacles,
                findableObjectType = findableObjectType
            };

            return JsonUtility.ToJson(data);
        }

        public static InteractionData DeserializeFromJson(string jsonString)
        {
            InteractionSerializedData data = JsonUtility.FromJson<InteractionSerializedData>(jsonString);

            var type = EnumParse(data.type, InteractionConfigurator.InteractionType.None);
            var hand = EnumParse(data.hand, InteractionConfigurator.HandType.Right);
            var difficulty = EnumParse(data.difficulty, InteractionConfigurator.DifficultyType.Easy);
            var displayArrowsAvoidObstacles = data.displayArrowsAvoidObstacles;
            var findableObjectType = data.findableObjectType;
            
            return new InteractionData(type, hand, difficulty, displayArrowsAvoidObstacles, findableObjectType);
        }

        private static T EnumParse<T>(string value, T defaultValue)
        {
            if (Enum.IsDefined(typeof(T), value))
            {
                return (T)Enum.Parse(typeof(T), value);
            }
            else
            {
                Debug.LogWarning($"Failed to parse enum value {value} for type {typeof(T).Name}");
                return defaultValue;
            }
        }
    }
}