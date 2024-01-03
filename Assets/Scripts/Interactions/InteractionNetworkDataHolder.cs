using System;
using Interactions.ObjectFinding;
using Interactions.WireLoop;
using UnityEngine;

namespace Interactions
{
    public class InteractionNetworkDataHolder : MonoBehaviour
    {
        [Header("Wire Loop")]
        [SerializeField] private TorusDataHolder networkTorusDataHolder;
        private WireLoopController _wireLoopController;
        
        [Header("Object Finding")]
        [SerializeField] private Transform networkPointerTransform;
        private ObjectFindingController _objectFindingController;

        private void Start()
        {
            _wireLoopController = FindObjectOfType<WireLoopController>(true);
            _wireLoopController.NetworkTorusDataHolder = networkTorusDataHolder;
            _wireLoopController.InteractionNetworkDataHolder = this;

            _objectFindingController = FindObjectOfType<ObjectFindingController>(true);
            _objectFindingController.NetworkPointerTransform = networkPointerTransform;
            Debug.Log("InteractionNetworkDataHolder: " + _objectFindingController.NetworkPointerTransform);
            _objectFindingController.InteractionNetworkDataHolder = this;
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
                    networkTorusDataHolder.TorusSizes[1].SetActive(true);
                    break;
                case InteractionConfigurator.DifficultyType.Hard:
                    networkTorusDataHolder.TorusSizes[2].SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null);
            }
        }

        public Transform NetworkPointerTransform => networkPointerTransform;
    }
}
