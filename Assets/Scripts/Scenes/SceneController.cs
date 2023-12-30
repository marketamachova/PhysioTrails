﻿using System.Collections.Generic;
using System.Linq;
using Analytics;
using Player;
using Unity.Collections;
using UnityEngine;
using NetworkPlayer = Network.NetworkPlayer;


namespace Scenes
{ 
    public class SceneController : MonoBehaviour
    {
        [SerializeField] private Transform startingPoint;
        [SerializeField] private Vector3 startingPositionLobby;
        [SerializeField] private Quaternion startingRotationLobby;
        [SerializeField] private GameObject eventTriggersParent;

        private GameObject _player;
        private GameObject _mainCamera;
        private GameObject _rtCamera;
        private NetworkPlayer _vrNetworkPlayer;
        private List<VREventTrigger> _eventTriggers;
        
        void Start()
        {
            _player = GameObject.FindWithTag(GameConstants.NetworkCamera);
            _mainCamera = GameObject.FindWithTag(GameConstants.MainCamera);
            _eventTriggers = eventTriggersParent.GetComponentsInChildren<VREventTrigger>()?.ToList();
            AnalyticsController.Instance.EventTriggers = _eventTriggers;

            var players = FindObjectsOfType<NetworkPlayer>();
            
            MovePlayersAtStartingPosition();

            foreach (var player in players)
            {
                if (!player.mobile)
                {
                    player.CmdSetWorldLoaded(true);
                }
            }
        }

        public void MovePlayersAtStartingPosition()
        {
            DontDestroyOnLoad(_player);
            _player.transform.position = startingPoint.position;
            _player.transform.rotation = startingPoint.rotation;

            if (_mainCamera)
            {
                _mainCamera.transform.parent = _player.transform.GetChild(0); // Move camera rig under player wrapper which enables some offseting
                // _mainCamera.transform.position = _player.transform.GetChild(0).transform.position;
                // _mainCamera.transform.localPosition = new Vector3(0, 1.5f, 0);
            }
        }
        
        public void MovePlayersAtStartingPositionLobby()
        {
            if (!_player)
            {
                _player = GameObject.FindWithTag(GameConstants.NetworkCamera);
            }

            _player.transform.position = startingPositionLobby;
            _player.transform.rotation = startingRotationLobby;
        }

        public List<VREventTrigger> EventTriggers => _eventTriggers;
    }
}