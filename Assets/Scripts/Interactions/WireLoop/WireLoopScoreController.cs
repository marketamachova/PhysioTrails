using UnityEngine;

namespace Interactions.WireLoop
{
    public class WireLoopScoreController : ScoreController
    {
        [SerializeField] private int scoreDecreaseStep = 1;
        [SerializeField] private int initScore = 30;
        protected override void IncreaseScore()
        {
            base.IncreaseScore();
            // Nothing is increased
        }

        protected override void DecreaseScore(   )
        {
            base.DecreaseScore();
            CurrentScore -= scoreDecreaseStep;
        }

        protected override void InitializeScore()
        {
            base.InitializeScore();
            CurrentScore = initScore;
        }
    }
}
