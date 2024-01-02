using System;
using Interactions.WireLoop;
using UnityEngine;

namespace Interactions
{
    public class InteractionNetworkDataHolder : MonoBehaviour
    {
        [Header("Wire Loop")]
        [SerializeField] private TorusDataHolder networkTorusDataHolder;
        private WireLoopController _wireLoopController;

        private void Start()
        {
            _wireLoopController = FindObjectOfType<WireLoopController>(true);
            _wireLoopController.NetworkTorusDataHolder = networkTorusDataHolder;
            _wireLoopController.InteractionNetworkDataHolder = this;
        }

        public void EnableTorusSizeBasedOnDifficulty(InteractionConfigurator.DifficultyType difficulty)
        {
            networkTorusDataHolder.TorusSizes.ForEach(torusSize => torusSize.SetActive(false));

            switch (difficulty)
            {
                case InteractionConfigurator.DifficultyType.Easy:
                    networkTorusDataHolder.TorusSizes[0].SetActive(true);
                    break;
                case InteractionConfigurator.DifficultyType.Medium:
                    Debug.Log("Enabling medium torus size " + networkTorusDataHolder.TorusSizes[1].name);
                    networkTorusDataHolder.TorusSizes[1].SetActive(true);
                    break;
                case InteractionConfigurator.DifficultyType.Hard:
                    networkTorusDataHolder.TorusSizes[2].SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null);
            }
        }
    }
}
