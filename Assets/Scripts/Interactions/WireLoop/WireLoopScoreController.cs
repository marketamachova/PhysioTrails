using UnityEngine;

namespace Interactions.WireLoop
{
    public class WireLoopScoreController : ScoreController
    {
        [SerializeField] private int scoreDecreaseStep = 1;
        [SerializeField] private int initScore = 30;
        protected override void IncreaseScore()
        {
            // Nothing is increased
        }

        protected override void DecreaseScore()
        {
            CurrentScore -= scoreDecreaseStep;
        }

        protected override void InitializeScore()
        {
            CurrentScore = initScore;
        }
    }
}
