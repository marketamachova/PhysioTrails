using System;
using UnityEngine;

namespace Interactions.WireLoop
{
    public class WireLoopNetworkHelper : MonoBehaviour
    {
        [SerializeField] private TorusDataHolder torusDataHolder;
        [SerializeField] private WireLoopSceneManager wireLoopSceneManager;
        
        private WireLoopController _wireLoopController;
        private TorusDataHolder _torusNetworkDataHolder;
        private Transform _currentTorusSizeTransform;
        private Transform _currentNetworkTorusSizeTransform;

        private void Awake()
        {
            wireLoopSceneManager.onTorusGrabStarted.AddListener(OnTorusGrabStart);
            wireLoopSceneManager.onTorusGrabEnded.AddListener(OnTorusGrabEnd);
            wireLoopSceneManager.onTorusCollisionStart.AddListener(OnTorusCollisionStart);
            wireLoopSceneManager.onTorusCollisionEnd.AddListener(OnTorusCollisionEnd);
        }

        private void Start()
        {
            _wireLoopController = wireLoopSceneManager.WireLoopController;
            _torusNetworkDataHolder = _wireLoopController.NetworkTorusDataHolder;
            
            var difficulty = wireLoopSceneManager.Difficulty;
            EnableTorusBasedOnDifficulty(difficulty);
        }

        private void Update()
        {
            SyncTorusPositionAndRotation();
        }
        
        private void SyncTorusPositionAndRotation()
        {
            var torusPosition = _currentTorusSizeTransform.position;
            var torusRotation = _currentTorusSizeTransform.rotation;

            _currentNetworkTorusSizeTransform.position = torusPosition;
            _currentNetworkTorusSizeTransform.rotation = torusRotation;
        }

        private void OnTorusCollisionStart()
        {
            _torusNetworkDataHolder.WireLoopVisualisers.ForEach(visualiser => visualiser.OnCollisionStart());
        }
        
        private void OnTorusCollisionEnd()
        {
            _torusNetworkDataHolder.WireLoopVisualisers.ForEach(visualiser => visualiser.OnCollisionEnd());
        }
        
        private void OnTorusGrabStart()
        {
            _torusNetworkDataHolder.TorusGhost.SetActive(false);
        }
        
        private void OnTorusGrabEnd()
        {
            _torusNetworkDataHolder.TorusGhost.SetActive(true);
        }
        
        private void EnableTorusBasedOnDifficulty(InteractionConfigurator.DifficultyType difficulty)
        {
            _torusNetworkDataHolder.TorusSizes.ForEach(torusSize => torusSize.SetActive(false));

            switch (difficulty)
            {
                case InteractionConfigurator.DifficultyType.Easy:
                    _torusNetworkDataHolder.TorusSizes[0].SetActive(true);
                    _currentTorusSizeTransform = torusDataHolder.TorusGrabbableRigidbodies[0].transform;
                    _currentNetworkTorusSizeTransform = _torusNetworkDataHolder.TorusSizes[0].transform;
                    break;
                case InteractionConfigurator.DifficultyType.Medium:
                    _torusNetworkDataHolder.TorusSizes[1].SetActive(true);
                    _currentTorusSizeTransform = torusDataHolder.TorusGrabbableRigidbodies[1].transform;
                    _currentNetworkTorusSizeTransform = _torusNetworkDataHolder.TorusSizes[1].transform;
                    break;
                case InteractionConfigurator.DifficultyType.Hard:
                    _torusNetworkDataHolder.TorusSizes[2].SetActive(true);
                    _currentTorusSizeTransform = torusDataHolder.TorusGrabbableRigidbodies[2].transform;
                    _currentNetworkTorusSizeTransform = _torusNetworkDataHolder.TorusSizes[2].transform;
                    break;
            }
        }

        public TorusDataHolder TorusNetworkDataHolder
        {
            get => _torusNetworkDataHolder;
            set => _torusNetworkDataHolder = value;
        }
    }
}
