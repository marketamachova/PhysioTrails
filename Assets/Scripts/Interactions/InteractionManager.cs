using System;
using System.Collections.Generic;
using Interactions.AvoidObstacles;
using Interactions.ObjectFinding;
using Interactions.WireLoop;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Interactions
{
    /**
     * Collects data about interactions, handles interaction configurations from the mobile app
     * communicates with the VR Controller
     */
    public class InteractionManager : MonoBehaviour
    {
        [SerializeField] private List<InteractionControllerBase> interactionControllers;

        // TODO merge it into one controller base controller
        private VRController _vrController;
        private MobileController _mobileController;

        private int _playerMovementSpeed;
        private InteractionControllerBase _currentInteractionController;

        private List<InteractionSceneManagerBase> _interactionSceneManagers;
        private InteractionSceneManagerBase _currentInteractionSceneManager; // Interaction manager situated in the VR world scene (Forest, Winter, Rural)
        private InteractionConfigurator.InteractionType _type = InteractionConfigurator.InteractionType.None;
        private InteractionConfigurator.DifficultyType _difficulty = InteractionConfigurator.DifficultyType.Easy;
        private InteractionConfigurator.HandType _handType = InteractionConfigurator.HandType.Right;
        private int _findableObjectType = 0;
        [SerializeField] private bool isVr = true;

        private InteractionConfigurator _interactionConfigurator;
        private ScoreController _currentScoreController;

        private void Start()
        {
            _interactionConfigurator = FindObjectOfType<InteractionConfigurator>();
            Debug.Assert(_interactionConfigurator != null, nameof(_interactionConfigurator) + " != null");
            
            _interactionConfigurator.onInteractionsConfigurationComplete.AddListener(OnInteractionsConfigurationComplete);
            SceneManager.sceneLoaded += OnSceneLoaded;

            CurrentInteractionType = _interactionConfigurator.Type;
        }
        
        private void OnInteractionsConfigurationComplete(string serializedData)
        {
            CurrentInteractionType = _interactionConfigurator.Type;
            CurrentDifficulty = _interactionConfigurator.Difficulty;
            CurrentHandType = _interactionConfigurator.Hand;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name.Contains("Scene")) // TODO stupid check, change to something better, maybe get rid of this check
            {
                if (isVr)
                {
                    _vrController = FindObjectOfType<VRController>();
                    _vrController.onSpeedChange.AddListener(OnSpeedChange);
                    
                    if (_currentInteractionController != null)
                    {
                        _currentInteractionController.SetSpeed(_vrController.CustomSpeed);
                        _currentScoreController = _currentInteractionController.CurrentScoreController;
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
                }
                else
                {
                    _mobileController = FindObjectOfType<MobileController>();
                    // _mobileController.onSpeedChange.AddListener(OnSpeedChange); // TODO
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
            if (isVr)
            {
                _vrController.InteractionReady = true;
            }
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
                    _currentInteractionSceneManager =
                        _interactionSceneManagers.Find(sceneManager => sceneManager is WireLoopSceneManager);
                    break;
                case InteractionConfigurator.InteractionType.ObjectFinding:
                    _currentInteractionSceneManager =
                        _interactionSceneManagers.Find(sceneManager => sceneManager is ObjectFindingSceneManager);
                    break;
                case InteractionConfigurator.InteractionType.AvoidObstacles:
                    _currentInteractionSceneManager =
                        _interactionSceneManagers.Find(sceneManager => sceneManager is AvoidObstaclesSceneManager);
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
                    Debug.Log("Kuk Setting difficulty to " + _difficulty + " in interaction manager, DONW");
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
        
        public int CurrentFindableObjectType
        {
            get => _findableObjectType;
            set
            {
                _findableObjectType = value;
                if (_currentInteractionController != null)
                {
                    _currentInteractionController.SetFindableObjectType(_findableObjectType);
                }
            }
        }

        public ScoreController CurrentScoreController => _currentScoreController;
    }
}
