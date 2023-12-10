using System;
using System.Collections.Generic;
using Mirror;
using Player;
using UnityEngine;

namespace Network
{
    public class MyNetworkManager : NetworkManager
    {
        public GameObject PlayerCamera { get; private set; }
        public static List<GameObject> Players { get; private set; } = new List<GameObject>();
        public event Action OnServerAddPlayerAction;
        public event Action OnClientConnectAction;
        public event Action OnClientDisconnectAction;
        public event Action OnMobileClientConnectAction;
        public event Action OnMobileClientDisconnectAction;


        /**
         * callback called automatically after server added player
         * 1. instantiates player object
         * 2. instantiate object carrying player and cameras
         * 3. invokes established connection events
         */
        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            InstantiatePlayer(conn);
            
            if (numPlayers <= 1)
            {
                InstantiateCamera();
            }
            else
            {
                var networkCamera = FindObjectOfType<PlayerCamera>();
                if (networkCamera)
                {
                    Debug.Log("Found network camera");
                    DontDestroyOnLoad(networkCamera);
                }
                else
                {
                    Debug.Log("Network camera null");
                }
                OnMobileClientConnectAction?.Invoke();
            }
            
            OnServerAddPlayerAction?.Invoke();
        }

        /**
         * instantiates player with given NetworkConnection
         * adds player to Players list
         * calls UpdateSceneConnected on the instantiated player to update UI
         */
        private void InstantiatePlayer(NetworkConnectionToClient conn)
        {
            GameObject player = Instantiate(playerPrefab);
            NetworkPlayer networkPlayer = player.GetComponent<NetworkPlayer>();

            NetworkServer.AddPlayerForConnection(conn, player);
            DontDestroyOnLoad(player);
            Players.Add(player);

            var networkCamera = FindObjectOfType<PlayerCamera>();
            if (networkCamera)
            {
                DontDestroyOnLoad(networkCamera);
            }
            else
            {
                Debug.Log("No network camera present");
            }

            networkPlayer.UpdateSceneConnected();
        }

        /**
         * instantiate gameObject carrying all cameras
         */
        private void InstantiateCamera()
        {
            Debug.Log("Kuk Will instantiate camera");
            PlayerCamera = Instantiate(spawnPrefabs.Find(prefab => prefab.name == GameConstants.NetworkPlayerAttachCamera));
            NetworkServer.Spawn(PlayerCamera);
            
            Debug.Log("Kuk Spawned camera");


            var mainCamera = GameObject.FindWithTag(GameConstants.MainCamera);
            mainCamera.transform.parent = PlayerCamera.transform.GetChild(0); // Parent it under the wrapper so that we can work with some offsetting
            
            DontDestroyOnLoad(PlayerCamera);
        }

        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            base.OnServerDisconnect(conn);

            if (numPlayers >= 1)
            {
                OnMobileClientDisconnectAction?.Invoke();
            }
            else
            {
                OnClientDisconnectAction?.Invoke();
            }
        }


        public override void OnClientDisconnect()
        {
            OnClientDisconnectAction?.Invoke();
            base.OnClientDisconnect();
        }

        public override void OnClientConnect()
        {
            base.OnClientConnect();
            OnClientConnectAction?.Invoke();
        }
    }
}