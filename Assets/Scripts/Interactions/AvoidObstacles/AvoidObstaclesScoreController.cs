using UnityEngine;

namespace Interactions.AvoidObstacles
{
    public class AvoidObstaclesScoreController : ScoreController
    {
        protected override void IncreaseScore()
        {
            Debug.Log("Kuk increase score");
        }

        protected override void DecreaseScore()
        {
            Debug.Log("Kuk decrease score");
        }

        protected override void InitializeScore()
        {
            Debug.Log("Kuk init score");
        }
    }
}
