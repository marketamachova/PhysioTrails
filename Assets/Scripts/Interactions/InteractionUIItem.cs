using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Interactions
{
    public class InteractionUIItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI interactionNameText;
        [SerializeField] private TextMeshProUGUI handTypeText;
        [SerializeField] private TextMeshProUGUI difficultyText;
        
        [SerializeField] private Image handTypeImage;
        [SerializeField] private Image interactionTypeImage;
        [SerializeField] private Image difficultyImage;
        
        [SerializeField] private Sprite leftHandSprite;
        [SerializeField] private Sprite rightHandSprite;
        [SerializeField] private Sprite wireLoopSprite;
        [SerializeField] private Sprite objectFindingSprite;
        [SerializeField] private Sprite avoidObstaclesSprite;
        [SerializeField] private Sprite easySprite;
        [SerializeField] private Sprite mediumSprite;
        [SerializeField] private Sprite hardSprite;
        
        public void UpdateUI(InteractionConfigurator.InteractionType interactionType, InteractionConfigurator.HandType handType, InteractionConfigurator.DifficultyType difficultyType)
        {
            interactionNameText.text = interactionType.ToString();
            handTypeText.text = handType.ToString();
            difficultyText.text = difficultyType.ToString();
            
            UpdateImages(interactionType, handType, difficultyType);
        }
        
        private void UpdateImages(InteractionConfigurator.InteractionType interactionType, InteractionConfigurator.HandType handType, InteractionConfigurator.DifficultyType difficultyType)
        {
            switch (handType)
            {
                case InteractionConfigurator.HandType.Left:
                    handTypeImage.sprite = leftHandSprite;
                    break;
                case InteractionConfigurator.HandType.Right:
                    handTypeImage.sprite = rightHandSprite;
                    break;
            }
            
            switch (interactionType)
            {
                case InteractionConfigurator.InteractionType.WireLoop:
                    interactionTypeImage.sprite = wireLoopSprite;
                    break;
                case InteractionConfigurator.InteractionType.ObjectFinding:
                    interactionTypeImage.sprite = objectFindingSprite;
                    break;
                case InteractionConfigurator.InteractionType.AvoidObstacles:
                    interactionTypeImage.sprite = avoidObstaclesSprite;
                    break;
            }
            
            switch (difficultyType)
            {
                case InteractionConfigurator.DifficultyType.Easy:
                    difficultyImage.sprite = easySprite;
                    break;
                case InteractionConfigurator.DifficultyType.Medium:
                    difficultyImage.sprite = mediumSprite;
                    break;
                case InteractionConfigurator.DifficultyType.Hard:
                    difficultyImage.sprite = hardSprite;
                    break;
            }
        }
    }
}
