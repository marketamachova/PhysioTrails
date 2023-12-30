using UnityEngine;

namespace Interactions.ObjectFinding
{
    public class ObjectFindingController : InteractionControllerBase
    {
        [SerializeField] private ScoreController scoreController;
        [SerializeField] private GameObject leftHandPointer;
        [SerializeField] private GameObject rightHandPointer;
        
        private ObjectFindingSceneManager _objectFindingSceneManager;
        private InteractionConfigurator.DifficultyType _difficulty;
        private InteractionConfigurator.HandType _handType;
        private int _collectType;
        
        private void Start()
        {
            leftHandPointer.SetActive(false);
            rightHandPointer.SetActive(false);
        }
        
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

        public int CollectType => _collectType;
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
            leftHandPointer.SetActive(handType == InteractionConfigurator.HandType.Left);
            rightHandPointer.SetActive(handType == InteractionConfigurator.HandType.Right);
        }
        
        public override void SetFindableObjectType(int findableObjectType)
        {
            base.SetFindableObjectType(findableObjectType);
            _collectType = findableObjectType;
        }

        public ObjectFindingSceneManager ObjectFindingSceneManager
        {
            get => _objectFindingSceneManager;
            set => _objectFindingSceneManager = value;
        }

        public InteractionConfigurator.DifficultyType Difficulty => _difficulty;
        
        public InteractionConfigurator.HandType HandType => _handType;
        
        public int FindableObjectType => _collectType;
    }
}
