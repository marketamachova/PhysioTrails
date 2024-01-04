using TMPro;
using UnityEngine;
using Utils;

namespace UI
{
    public class ScoreUIManager : MonoSingleton<ScoreUIManager>
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private GameObject scorePanel;

        [ContextMenu("OnInteractionStarted")]
        public void OnInteractionStarted()  // TODO call this somewhere!!!!!!!!!!!!!!!!!!!
        {
            scorePanel.SetActive(true);
        }
        
        [ContextMenu("OnInteractionEnded")]
        public void OnInteractionEnded() // TODO call this somewhere
        {
            scorePanel.SetActive(false); 
        }
            
        public void UpdateScore(int score)
        {
            scoreText.text = score.ToString();
            Debug.Log("Score: " + score);
        }
    }
}
