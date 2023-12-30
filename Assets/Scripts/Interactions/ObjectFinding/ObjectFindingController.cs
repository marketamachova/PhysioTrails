using UnityEngine;

namespace Interactions.ObjectFinding
{
    public class ObjectFindingController : InteractionControllerBase
    {
        [SerializeField] private ScoreController scoreController;
        
        [SerializeField] private int collectType = 0; 
        
        private ObjectFindingSceneManager _objectFindingSceneManager;
        private InteractionConfigurator.DifficultyType _difficulty;
        private InteractionConfigurator.HandType _handType;
        
        public void OnSceneLoaded()
        {
            Debug.Log("Kuk Scene loaded");
 
            InvokeInteractionReady();
        }
        
        public void OnPointedCorrectly()
        {
            scoreController.OnHit();
        }
        
        public void OnPointedIncorrectly()
        {
            scoreController.OnMiss();
        }

        public int CollectType => collectType;
        protected override void InvokeInteractionReady()
        {
            onInteractionReady.Invoke();
        }

        public override void SetSpeed(int speed)
        {
        }

        public override void SetDifficulty(InteractionConfigurator.DifficultyType difficulty)
        {
            _difficulty = difficulty;
        }

        public override void SetHandType(InteractionConfigurator.HandType handType)
        {
            _handType = handType;
        }

        public ObjectFindingSceneManager ObjectFindingSceneManager
        {
            get => _objectFindingSceneManager;
            set => _objectFindingSceneManager = value;
        }

        public InteractionConfigurator.DifficultyType Difficulty => _difficulty;
    }
}
