using System;
using System.Collections;
using System.Collections.Generic;
using Analytics;
using Interactions;
using Network;
using PathCreation;
using Scenes;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using NetworkPlayer = Network.NetworkPlayer;

namespace Player
{
    public enum PTScene
    {
        Forest, Rural, Winter, None
    }
    /**
     * Controller managing events in VR travelling experience
     */
    [RequireComponent(typeof(AnalyticsController))]
    public class VRController : BaseController
    {
        [FormerlySerializedAs("currentScene")] [SerializeField] private PTScene currentPtScene;

        public List<GameObject> player = new List<GameObject>();

        [SerializeField] private bool isVR = true;
        
        private SceneController _sceneController;
        private SceneLoader _sceneLoader;
        private PathCreator _pathCreator;
        private MyNetworkManager _networkManager;
        private PlayerMovement[] _playerMovementScripts;
        private GameObject _cart;
        private GameObject _player;
        private Animator _cartAnimator;
        private AudioSource _cartAudio;
        private NetworkPlayer _networkPlayer;
        private EscapeGestureHandler _escapeGestureHandler;
        private Fader _fader;
        private AnalyticsController _analyticsController;
        
        private int _customSpeed = 10;
        private bool _interactionReady = false;

        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int Drive = Animator.StringToHash("Drive");
        
        public UnityEvent<int> onSpeedChange = new UnityEvent<int>();

        private void Awake()
        {
            _networkPlayer = FindObjectOfType<NetworkPlayer>();

            _customSpeed = _networkPlayer.speed;
            _cart = GameObject.FindWithTag(GameConstants.Cart);
            _player = GameObject.FindWithTag(GameConstants.NetworkCamera);
            _sceneController = FindObjectOfType<SceneController>();
            _sceneLoader = FindObjectOfType<SceneLoader>();
            _pathCreator = FindObjectOfType<PathCreator>();
            _networkManager = FindObjectOfType<MyNetworkManager>();

            if (isVR)
            {
                _escapeGestureHandler = FindObjectOfType<EscapeGestureHandler>();
                _analyticsController = GetComponent<AnalyticsController>();
                
                _escapeGestureHandler.VRController = this;
            }
            
            
            player.Add(_player);
            
            AssignPlayers();

            _playerMovementScripts = FindObjectsOfType<PlayerMovement>();
            foreach (var script in _playerMovementScripts)
            {
                script.SetPathCreator(_pathCreator);
                script.speed = _customSpeed;
            }
            
            onSpeedChange.Invoke(_customSpeed);
            Debug.Log("Kuk invoke speed to " + _customSpeed);

            if (_cart != null)
            {
                _cartAudio = _cart.GetComponent<AudioSource>();
                _cartAnimator = _cart.GetComponent<Animator>();
            }

            _networkManager.OnMobileClientDisconnectAction += TriggerPlayerMoving;
            _networkManager.OnClientDisconnectAction += TriggerPlayerMoving;
            _networkManager.OnServerAddPlayerAction += AssignPlayers;
        }

        private void OnEnable() // Not very nice. Should be done with events
        {
            _escapeGestureHandler = FindObjectOfType<EscapeGestureHandler>();

            _escapeGestureHandler.VRController = this;

        }

        public IEnumerator Start()
        {
            // VR is not controlled by the mobile app
            if (_networkManager.numPlayers == 1)
            {
                // Wait until interactions are ready
                while (!_interactionReady)
                {
                    yield return null;
                }
              
                // yield return new WaitForSecondsRealtime(4);
                TriggerPlayerMoving();
            }
            
            _analyticsController.StartTracking(currentPtScene.ToString());
            Debug.Log("Start tracking in scene " + currentPtScene.ToString());
        }


        public void StartMovement()
        {
            foreach (var script in _playerMovementScripts)
            {
                Enable(script);
            }

            if (_cart != null)
            {
                StartCart();
            }
        }

        public void PauseMovement()
        {
            foreach (var script in _playerMovementScripts)
            {
                Disable(script);
            }

            if (_cart != null)
            {
                StopCart();
            }
        }

        public void End()
        {
            var networkPlayers = FindObjectsOfType<NetworkPlayer>();
            foreach (var networkPlayer in networkPlayers)
            {
                networkPlayer.CmdGoToLobby(true);
            }
        }

        private void Enable(PlayerMovement script)
        {
            script.enabled = true;
        }

        private void Disable(PlayerMovement script)
        {
            script.enabled = false;
        }

        private void StartCart()
        {
            _cartAnimator.Play(Drive);
            _cartAudio.Play();
        }


        private void StopCart()
        {
            _cartAnimator = _cart.GetComponent<Animator>();
            _cartAnimator.Play(Idle);
            _cartAudio.Stop();
        }

        public override void OnGoToLobby(bool wait)
        {
            base.OnGoToLobby();
            
            foreach (var script in _playerMovementScripts)
            {
                Disable(script);
            }

            if (_cart != null)
            {
                StopCart();
            }
            
            _analyticsController.EndTracking();
            
            StartCoroutine(GoToLobbyCoroutine(wait));
        }

        // public void GoToLobby()
        // {
        //     StartCoroutine(GoToLobbyCoroutine());
        // }


        /**
         * coroutine of VR player returning to lobby
         * 1. Waits for defined waiting time
         * 2. Sets Lobby as active scene
         * 3. Moves player at accurate position in Lobby
         * 4. calls async scene unloading
         */
        private IEnumerator GoToLobbyCoroutine(bool wait = true)
        {
            if (wait)
            {
                yield return new WaitForSecondsRealtime(GameConstants.ReturnToLobbyWaitingTime);
            }
            
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(GameConstants.VROffline));

            _sceneController.MovePlayersAtStartingPositionLobby();
            
            _sceneLoader.UnloadScene();
        }

        public void SetMovementSpeed(int speed)
        {

            if (_playerMovementScripts.Length > 0) //TODO
            {
                foreach (var playerMovement in _playerMovementScripts)
                {
                    playerMovement.speed = speed;
                    _customSpeed = speed;
                }
            }
            else
            {
                _customSpeed = speed;
            }
            
            onSpeedChange.Invoke(speed);
        }

        /**
         * return time (float) spent in an ongoing VR experience
         */
        public float GetTimePlaying()
        {
            if (_playerMovementScripts.Length > 0)
            {
                return _playerMovementScripts[0].GetTime();
            }

            return 0f;
        }
        
        private void TriggerPlayerMoving()
        {
            _networkPlayer.CmdSetPlayerMoving(true);
        }

        public bool InteractionReady
        {
            get => _interactionReady;
            set
            {
                _interactionReady = value;
            }
        }

        public int CustomSpeed => _customSpeed;
    }
}