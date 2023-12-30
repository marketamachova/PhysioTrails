using System;
using System.Collections.Generic;
using Interactions.WireLoop;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Interactions
{
    /**
     * Collects data about interactions, handles interaction configurations from the mobile app
     * communicates with the VR Controller
     */
    public class InteractionManager : MonoBehaviour
    {
        [SerializeField] private InteractionConfigurator interactionConfigurator;
        [SerializeField] private List<InteractionControllerBase> interactionControllers;

        private VRController _vrController;

        private int _playerMovementSpeed;
        private InteractionControllerBase _currentInteractionController;

        private List<InteractionSceneManagerBase> _interactionSceneManagers;
        private InteractionSceneManagerBase _currentInteractionSceneManager; // Interaction manager situated in the VR world scene (Forest, Winter, Rural)
        private InteractionConfigurator.InteractionType _type = InteractionConfigurator.InteractionType.None;
        private InteractionConfigurator.DifficultyType _difficulty = InteractionConfigurator.DifficultyType.Easy;
        private InteractionConfigurator.HandType _handType = InteractionConfigurator.HandType.Right;
        
        private bool _shouldWaitForInteractionStart = false;

        // TODO delete this, will be job of configurator
        private void Start()
        {
            // CurrentInteractionType = InteractionConfigurator.InteractionType.WireLoop;
            interactionConfigurator.OnInteractionsConfigurationComplete += OnInteractionsConfigurationComplete;
            SceneManager.sceneLoaded += OnSceneLoaded;

            CurrentInteractionType = interactionConfigurator.Type;
        }
        
        private void OnInteractionsConfigurationComplete()
        {
            CurrentInteractionType = interactionConfigurator.Type;
            CurrentDifficulty = interactionConfigurator.Difficulty;
            CurrentHandType = interactionConfigurator.Hand;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name.Contains("Scene")) // TODO stupid check, change to something better
            {
                _vrController = FindObjectOfType<VRController>();
                _vrController.onSpeedChange.AddListener(OnSpeedChange);
                if (_currentInteractionController != null)
                {
                    _currentInteractionController.SetSpeed(_vrController.CustomSpeed);
                    // _vrController.WaitForInteractions = _currentInteractionController.ShouldWaitForInteractionStart;
                    if (!_currentInteractionController.ShouldWaitForInteractionStart)
                    {
                        _vrController.InteractionReady = true;
                    }
                }
                else
                {
                    _vrController.InteractionReady = true;
                }

                _interactionSceneManagers = new List<InteractionSceneManagerBase>(FindObjectsOfType<InteractionSceneManagerBase>());
                AssignInteractionSceneManager(CurrentInteractionType);
                EnableSceneInteractionManager();
            }
        }

        private void EnableInteractionController()
        {
            interactionControllers.ForEach(controller => controller.gameObject.SetActive(false));
            if (_currentInteractionController != null)
            {
                _currentInteractionController.gameObject.SetActive(true);
            }
        }
        
        private void EnableSceneInteractionManager()
        {
            _interactionSceneManagers.ForEach(manager => manager.gameObject.SetActive(false));
            if (_currentInteractionSceneManager != null)
            {
                _currentInteractionSceneManager.gameObject.SetActive(true);
            }
        }


        [ContextMenu("Invoke interaction ready")]
        private void OnInteractionReady()
        {
            _vrController.InteractionReady = true;
        }
        
        private void OnSpeedChange(int newSpeed)
        {
            _playerMovementSpeed = newSpeed;
            _currentInteractionController.SetSpeed(_playerMovementSpeed);
        }
        
        private void AssignInteractionSceneManager(InteractionConfigurator.InteractionType newInteractionType)
        {
            switch (newInteractionType)
            {
                case InteractionConfigurator.InteractionType.WireLoop:
                    _currentInteractionSceneManager = _interactionSceneManagers[0];
                    break;
                case InteractionConfigurator.InteractionType.ObjectFinding:
                    _currentInteractionSceneManager = _interactionSceneManagers[1];
                    break;
                case InteractionConfigurator.InteractionType.AvoidObstacles:
                    _currentInteractionSceneManager = _interactionSceneManagers[2];
                    break;
                case InteractionConfigurator.InteractionType.None:
                    _currentInteractionSceneManager = null;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void AssignInteractionController(InteractionConfigurator.InteractionType newInteractionType)
        {
            switch (newInteractionType)
            {
                case InteractionConfigurator.InteractionType.WireLoop:
                    _currentInteractionController = interactionControllers[0];
                    _shouldWaitForInteractionStart = true;
                    break;
                case InteractionConfigurator.InteractionType.ObjectFinding:
                    _currentInteractionController = interactionControllers[1];
                    break;
                case InteractionConfigurator.InteractionType.AvoidObstacles:
                    _currentInteractionController = interactionControllers[2];
                    break;
                case InteractionConfigurator.InteractionType.None:
                    _currentInteractionController = null;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        

        private void HangListeners()
        {
            if (_currentInteractionController != null)
            {
                _currentInteractionController.onInteractionReady.AddListener(OnInteractionReady);
            }
        }
        
        public InteractionConfigurator.InteractionType CurrentInteractionType
        {
            get => _type;
            set
            {
                _type = value;
                AssignInteractionController(_type);
                EnableInteractionController();
                HangListeners();
            }
        }
        
        public InteractionConfigurator.DifficultyType CurrentDifficulty
        {
            get => _difficulty;
            set
            {
                _difficulty = value;
                if (_currentInteractionController != null)
                {
                    _currentInteractionController.SetDifficulty(_difficulty);
                }
            }
        }
        
        public InteractionConfigurator.HandType CurrentHandType
        {
            get => _handType;
            set
            {
                _handType = value;
                if (_currentInteractionController != null)
                {
                    _currentInteractionController.SetHandType(_handType);
                }
            }
        }
    }
}
