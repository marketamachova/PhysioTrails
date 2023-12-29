using UnityEngine;
using UnityEngine.Events;

namespace Interactions.AvoidObstacles
{
    public class AvoidObstaclesController : InteractionControllerBase
    {
        [SerializeField] private AvoidObstaclesScoreController scoreController;
        [SerializeField] private bool displayArrows = true;

        private AvoidObstaclesSceneManager _avoidObstaclesSceneManager;
        private InteractionConfigurator.DifficultyType _difficulty;

        public UnityEvent<bool> onDisplayArrowsChanged = new UnityEvent<bool>();

        public void OnSceneLoaded()
        {
            InvokeInteractionReady();
        }
        
        public void OnHit()
        {
            Debug.Log("Kuk HIT");
        }
        
        public void OnAvoidCorrect()
        {
            Debug.Log("Kuk avoid CORRECT");

        }
        
        public void OnAvoidIncorrect()
        {
            Debug.Log("Kuk avoid INCORRECT");

        }

        protected override void InvokeInteractionReady()
        {
            onInteractionReady.Invoke();
        }

        public override void SetSpeed(int speed)
        {
        }

        public override void SetDifficulty(InteractionConfigurator.DifficultyType difficultyType)
        {
            _difficulty = difficultyType;
        }

        public AvoidObstaclesSceneManager AvoidObstaclesSceneManager
        {
            get => _avoidObstaclesSceneManager;
            set => _avoidObstaclesSceneManager = value;
        }

        public bool DisplayArrows
        {
            get => displayArrows;
            set
            {
                displayArrows = value;
                onDisplayArrowsChanged.Invoke(value);
            }
        }

        public InteractionConfigurator.DifficultyType Difficulty => _difficulty;

        [ContextMenu("ToggleDisplayArrows")]
        public void TestToggleDisplayArrows()
        {
            DisplayArrows = !DisplayArrows;
        }
    }
}
