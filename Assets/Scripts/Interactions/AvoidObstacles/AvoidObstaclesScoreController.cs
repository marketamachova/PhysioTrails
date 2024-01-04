using UnityEngine;

namespace Interactions.AvoidObstacles
{
    public class AvoidObstaclesScoreController : ScoreController
    {
        [SerializeField] private int avoidCorrectlyIncreaseStep = 3;
        [SerializeField] private int collideDecreaseStep = 2;
        [SerializeField] private int avoidIncorrectlyDecreaseStep = 1;
        private int _obstaclesCount;
        
        [ContextMenu("IncreaseScore")]
        protected override void IncreaseScore()
        {
            CurrentScore += avoidCorrectlyIncreaseStep;
            base.IncreaseScore();
            Debug.Log("Kuk increase score");
        }

        [ContextMenu("DecreaseScore")]
        protected override void DecreaseScore()
        {
            CurrentScore -= avoidIncorrectlyDecreaseStep;
            base.DecreaseScore();
            Debug.Log("Kuk decrease score");
        }

        [ContextMenu("InitializeScore")]
        protected override void InitializeScore()
        {
            CurrentScore = 0;
            base.InitializeScore();
            Debug.Log("Kuk init score");
        }
        
        public int ObstaclesCount
        {
            get => _obstaclesCount;
            set => _obstaclesCount = value;
        }
    }
}
