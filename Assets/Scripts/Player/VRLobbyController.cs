using System;
using System.Collections;
using Cart;
using Interactions;
using Mirror;
using Mirror.Discovery;
using PatientManagement;
using PatientManagement_;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    public class VRLobbyController : BaseController
    {
        [SerializeField] private CartCreator cartCreator;
        [SerializeField] private NetworkDiscovery networkDiscovery;
        
        protected override void Awake()
        {
            base.Awake();
            
            sceneLoader.SceneLoadingEnd += OnSceneLoaded;
            networkManager.OnClientConnectAction += OnClientConnected;
            networkManager.OnMobileClientConnectAction += OnClientMobileConnected;
            networkManager.OnClientDisconnectAction += OnClientDisonnected;
            networkManager.OnMobileClientDisconnectAction += OnClientMobileDisconnected;
            cartCreator.OnCartCreatorCalibrationComplete += SetCalibrationComplete;
            patientsManager.onPatientSelectionComplete.AddListener(SetPatientSelectionComplete);
            interactionConfigurator.OnInteractionsConfigurationComplete += SetInteractionSelectionComplete;
        }

        private IEnumerator Start()
        {
            yield return new WaitForSecondsRealtime(1);
            NetworkManager.singleton.StartHost();
            networkDiscovery.AdvertiseServer();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                OnSceneSelected("WinterScene");
            }

            if (Input.GetKeyDown(KeyCode.M))
            {
                OnSceneSelected("MainScene");
            }
            
            if (Input.GetKeyDown(KeyCode.R))
            {
                OnSceneSelected("RuralScene");
            }
            
        }

        private void OnClientConnected()
        {
            uiController.Activate(UIConstants.AvailabilityIndicatorVR);
        }

        private void OnClientMobileConnected()
        {
            uiController.Activate(UIConstants.AvailabilityIndicatorMobile);
        }

        private void OnClientDisonnected()
        {
            LocalNetworkPlayer.CmdSetPlayerMoving(true);
            uiController.Deactivate(UIConstants.AvailabilityIndicatorVR);
        }

        private void OnClientMobileDisconnected()
        {
            uiController.Deactivate(UIConstants.AvailabilityIndicatorMobile);
        }

        protected override void OnCalibrationComplete()
        {
            base.OnCalibrationComplete();
            uiController.EnablePanelExclusive(UIConstants.PatientSelection);
        }
        
        protected override void OnPatientSelectionComplete()
        {
            base.OnPatientSelectionComplete();
            uiController.EnablePanelExclusive(UIConstants.InteractionSelection);
        }
        
        protected override void OnInteractionSelectionComplete()
        {
            base.OnInteractionSelectionComplete();
            uiController.EnablePanelExclusive(UIConstants.SceneSelection);
        }

        public override void SkipCalibration()
        {
            base.SkipCalibration();
            cartCreator.SkipCalibration();
        }

        protected override void OnSceneLoaded()
        {
            uiController.gameObject.SetActive(false);

            var scene = SceneManager.GetSceneByName(LocalNetworkPlayer.chosenWorld);
            SceneManager.SetActiveScene(scene);
        }

        public override void OnGoToLobby(bool wait)
        {
            uiController.gameObject.SetActive(true);
            uiController.EnablePanelExclusive(UIConstants.PatientSelection);
            LocalNetworkPlayer.CmdSetWorldLoaded(false);
        }

        public override void Initialize()
        {
            uiController.gameObject.SetActive(true);
            LocalNetworkPlayer.CmdSetWorldLoaded(false);
        }
    }
}