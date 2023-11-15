using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactions
{
    public abstract class ScoreController : MonoBehaviour
    {
        [SerializeField] protected VOPlayer voPlayer;
        [SerializeField] protected ScoreVisualiser scoreVisualiser;
        [SerializeField] private int delaySecondsBetweenScoreChange = 2;
        
        protected int CurrentScore;
        private bool _enableScoreChange = true;

        protected void Start()
        {
            InitializeScore();
            scoreVisualiser.UpdateScore(CurrentScore);
        }

        public void OnMiss()
        {
            if (_enableScoreChange)
            {
                DecreaseScore();
                StartCoroutine(EnableScoreDecreaseAfterDelay(delaySecondsBetweenScoreChange));
                Debug.Log("On miss " + CurrentScore);
                voPlayer.PlayMiss();
                scoreVisualiser.UpdateScore(CurrentScore);
            }
            
        }

        public void OnHit()
        {
            if (_enableScoreChange)
            {
                IncreaseScore();
                Debug.Log("On hit " + CurrentScore);
                voPlayer.PlayHit();
                scoreVisualiser.UpdateScore(CurrentScore);   
            }
        }

        protected abstract void IncreaseScore();
        protected abstract void DecreaseScore();

        protected abstract void InitializeScore();
        
            
        private IEnumerator EnableScoreDecreaseAfterDelay(int delaySeconds)
        {
            _enableScoreChange = false;
            yield return new WaitForSecondsRealtime(delaySeconds);
            _enableScoreChange = true;
        }
    }
}
