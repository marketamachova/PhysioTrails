using TMPro;
using UnityEngine;
using Utils;

namespace UI
{
    public class ScoreUIManager : MonoSingleton<ScoreUIManager>
    {
        [SerializeField] private TextMeshProUGUI scoreText;
            
        public void UpdateScore(int score)
        {
            scoreText.text = score.ToString();
            Debug.Log("Score: " + score);
        }
    }
}
