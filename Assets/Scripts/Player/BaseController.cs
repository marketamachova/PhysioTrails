
using Interactions;
using UnityEngine;
using NetworkPlayer = Network.NetworkPlayer;
using Mirror.Discovery;
using Network;
using PatientManagement;
using Scenes;
using UI;
using UnityEngine.SceneManagement;
using UnityEngine.Localization.Settings;
using UnityEngine.Serialization;

namespace Player
{
    public class BaseController : MonoBehaviour
    {
        [SerializeField] protected MyNetworkManager networkManager;
        [SerializeField] protected SceneLoader sceneLoader;
        [SerializeField] protected BaseUIController uiController;
        [SerializeField] protected PatientsManager patientsManager;
        [SerializeField] protected InteractionConfigurator interactionConfigurator;
        
        protected NetworkPlayer[] NetworkPlayers;
        protected NetworkPlayer LocalNetworkPlayer;
        protected NetworkPlayer RemoteNetworkPlayer;

        protected virtual void Awake()
        {
            Debug.Log("Kuk base controller awake 0");

            networkManager.OnServerAddPlayerAction += AssignPlayers;
            patientsManager.onPatientSelectionComplete.AddListener(SetPatientSelectionComplete);
            
            if (interactionConfigurator == null)
            {
                interactionConfigurator = FindObjectOfType<InteractionConfigurator>();
            }
            interactionConfigurator.onInteractionsConfigurationComplete.AddListener(SetInteractionSelectionComplete);
            
            Debug.Log("Kuk base controller awake");
            // Initialize();
        }

        public virtual void OnDisconnect()
        {
            if (SceneManager.sceneCount > 1)
            {
                sceneLoader.UnloadScene();
            }
            uiController.DisplayError();
        }

        /**
         * assigns LocalNetworkPlayer and RemoteNetworkPlayer variables from all player objects in the scene 
         */
        public virtual void AssignPlayers()
        {
            NetworkPlayers = FindObjectsOfType<NetworkPlayer>();
            foreach (var networkPlayer in NetworkPlayers)
            {
                if (networkPlayer.isLocalPlayer)
                {
                    LocalNetworkPlayer = networkPlayer;
                    LocalNetworkPlayer.OnCalibrationComplete += OnCalibrationComplete;
                    LocalNetworkPlayer.OnInteractionSelectionComplete += OnInteractionSelectionComplete;
                    LocalNetworkPlayer.OnPatientSelectionComplete += OnPatientSelectionComplete;
                }
                else
                {
                    RemoteNetworkPlayer = networkPlayer;
                }
            }
        }

        protected virtual void OnCalibrationComplete() { }
        
        protected virtual void OnInteractionSelectionComplete() { }
        protected virtual void OnPatientSelectionComplete() { }

        public virtual void SkipCalibration() { }

        /**
         * synchronises calibration complete syncVar with all NetworkPlayers in the scene
         */
        protected virtual void SetCalibrationComplete()
        {
            if (NetworkPlayers.Length < 2)
            {
                AssignPlayers();
            }
            
            foreach (var networkPlayer in NetworkPlayers)
            {
                networkPlayer.CmdSetCalibrationComplete(true);
            }
        }
        
        /**
         * synchronises interaction selection complete syncVar with all NetworkPlayers in the scene
         */
        protected virtual void SetInteractionSelectionComplete(string serializedInteractionData)
        {
            if (NetworkPlayers.Length < 2)
            {
                AssignPlayers();
            }
            
            foreach (var networkPlayer in NetworkPlayers)
            {
                networkPlayer.CmdSetInteractionSelectionComplete(true);
                networkPlayer.CmdSetInteractionData(serializedInteractionData);
            }
        }
        
        /**
         * synchronises patient selection complete syncVar with all NetworkPlayers in the scene
         */
        protected virtual void SetPatientSelectionComplete(string patientId)
        {
            if (NetworkPlayers.Length < 2)
            {
                AssignPlayers();
            }
            
            foreach (var networkPlayer in NetworkPlayers)
            {
                networkPlayer.CmdSetPatientSelectionComplete(true);
                networkPlayer.CmdSetPatientId(patientId);
            }
        }

        /**
         * handles selecting a scene (if no world scene is loaded/loading)
         * and triggers network synchronisation with Command CmdHandleSelectedWorld on all NetworkPlayers in the scene
         */
        public virtual void OnSceneSelected(string sceneName)
        {
            if (SceneManager.sceneCount > 1)
            {
                Debug.Log("scene count > 1");
                return;
            }

            foreach (var networkPlayer in NetworkPlayers)
            {
                networkPlayer.CmdHandleSelectedWorld(sceneName);
            }
        }

        public virtual void OnGoToLobby(bool wait = true)
        {
            if (NetworkPlayers != null)
            {
                foreach (var networkPlayer in NetworkPlayers)
                {
                    networkPlayer.CmdSetPatientSelectionComplete(false);
                    networkPlayer.CmdSetInteractionSelectionComplete(false);
                }
            }
        }

        public virtual void Initialize()
        {
        }

        /**
         * synchronises triggerGoToLobby  syncVar with all NetworkPlayers in the scene
         */
        public virtual void TriggerGoToLobby()
        {
            foreach (var networkPlayer in NetworkPlayers)
            {
                networkPlayer.CmdGoToLobby(false);
            }
        }

        protected virtual void OnSceneLoaded() { }

        /**
         * handles selected language with given index of the language
         */
        public void OnLanguageSelected(int index)
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
        }
        


    }
}