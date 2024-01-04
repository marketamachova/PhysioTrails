using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Network;
using UnityEngine;

namespace Interactions
{
    /**
     * For VR
     */
    public abstract class ScoreController : MonoBehaviour
    {
        [SerializeField] protected VOPlayer voPlayer;
        [SerializeField] protected ScoreVisualiser scoreVisualiser;
        [SerializeField] private int delaySecondsBetweenScoreChange = 2;
        
        protected int CurrentScore;
        private bool _enableScoreChange = true;
        
        private List<InteractionNetworkPlayer> _interactionNetworkPlayers;

        protected void Start()
        {
            _interactionNetworkPlayers = FindObjectsOfType<InteractionNetworkPlayer>().ToList();
            
            // if (voPlayer == null)
            // {
            //     voPlayer = FindObjectOfType<VOPlayer>();
            // }
            //
            // if (scoreVisualiser == null)
            // {
            //     scoreVisualiser = FindObjectOfType<ScoreVisualiser>();
            // }
            
            InitializeScore();
            // scoreVisualiser.UpdateScore(CurrentScore);
        }

        public void OnMiss()
        {
            if (_enableScoreChange)
            {
                DecreaseScore();
                StartCoroutine(EnableScoreDecreaseAfterDelay(delaySecondsBetweenScoreChange));
                Debug.Log("On miss " + CurrentScore);
                // TODO assign proper references
                // voPlayer.PlayMiss();
                // scoreVisualiser.UpdateScore(CurrentScore);
            }
            
        }

        [ContextMenu("OnHit")]
        public void OnHit()
        {
            if (_enableScoreChange)
            {
                IncreaseScore();
                Debug.Log("On hit " + CurrentScore);
                // TODO assign proper references
                // voPlayer.PlayHit();
                // scoreVisualiser.UpdateScore(CurrentScore);   
            }
        }
        
        protected virtual void IncreaseScore()
        {
            SyncScore();
        }

        protected virtual void DecreaseScore()
        {
            SyncScore();
        }

        protected virtual void InitializeScore()
        {
            SyncScore();
        }
        
        private void SyncScore()
        {
            if (_interactionNetworkPlayers != null)
            {
                foreach (var interactionNetworkPlayer in _interactionNetworkPlayers)
                {
                    interactionNetworkPlayer.CmdSetScore(CurrentScore);
                }
            }
            else
            {
                Debug.LogError("InteractionNetworkPlayers null");
            }
        }
        
            
        private IEnumerator EnableScoreDecreaseAfterDelay(int delaySeconds)
        {
            _enableScoreChange = false;
            yield return new WaitForSecondsRealtime(delaySeconds);
            _enableScoreChange = true;
        }
        
        public void AddNetworkPlayer(InteractionNetworkPlayer interactionNetworkPlayer)
        {
            if (_interactionNetworkPlayers == null)
            {
                _interactionNetworkPlayers = new List<InteractionNetworkPlayer>();
            }
            Debug.Log("Adding network player");
            _interactionNetworkPlayers.Add(interactionNetworkPlayer);
        }

        public int CurrentScore1 => CurrentScore;
    }
}
