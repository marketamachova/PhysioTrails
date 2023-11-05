using TMPro;
using UnityEngine;

namespace Interactions
{
    public class ScoreVisualiser : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;

        public void UpdateScore(int newScore)
        {
            scoreText.text = newScore.ToString();
        }
    }
}
