using UnityEngine;

namespace Interactions.AvoidObstacles
{
    public class AvoidObstaclesController : InteractionControllerBase
    {
        [SerializeField] private AvoidObstaclesScoreController scoreController;
        
        private AvoidObstaclesSceneManager _avoidObstaclesSceneManager;
        
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

        public AvoidObstaclesSceneManager AvoidObstaclesSceneManager
        {
            get => _avoidObstaclesSceneManager;
            set => _avoidObstaclesSceneManager = value;
        }
    }
}
