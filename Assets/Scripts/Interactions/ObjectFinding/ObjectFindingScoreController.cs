using System;
using UnityEngine;

namespace Interactions.ObjectFinding
{
    public class ObjectFindingScoreController : ScoreController
    {
        [SerializeField] private int scoreIncreaseStep = 1;
        [SerializeField] private int scoreDecreaseStep = 1;
        private int _obstaclesCount;
        

        [ContextMenu("IncreaseScore")]
        protected override void IncreaseScore()
        {
            CurrentScore += scoreIncreaseStep;
            base.IncreaseScore();
        }

        [ContextMenu("DecreaseScore")]
        protected override void DecreaseScore()
        {
            CurrentScore -= scoreDecreaseStep;
            base.DecreaseScore();
        }

        protected override void InitializeScore()
        {
            CurrentScore = 0;
            base.InitializeScore();
        }

        public int ObstaclesCount
        {
            get => _obstaclesCount;
            set => _obstaclesCount = value;
        }
    }
}
