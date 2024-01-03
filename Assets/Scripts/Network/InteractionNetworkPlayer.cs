using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Interactions;
using Interactions.AvoidObstacles;
using Interactions.ObjectFinding;
using Interactions.WireLoop;
using Mirror;
using PatientManagement;
using Player;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using SpawnPointFindableDataPair = Interactions.ObjectFinding.FindableObjectSpawner.SpawnPointFindableDataPair;
using SpawnPointObstaclePair = Interactions.AvoidObstacles.AvoidableObstacleSpawner.SpawnPointObstaclePair;

namespace Network
{
    public class InteractionNetworkPlayer : NetworkBehaviour
    {
        [SyncVar(hook = "OnSetScore")] public int score;

        [SyncVar(hook = "OnSetUsingPointingGesture")]
        public bool usingPointingGesture;

        [SyncVar(hook = "OnSetWireLoopColliding")]
        public bool wireLoopColliding;

        public readonly SyncList<SpawnPointFindableDataPair> spawnPointsObjectFinding = new SyncList<SpawnPointFindableDataPair>();
        
        public readonly SyncList<SpawnPointObstaclePair> spawnPointsAvoidObstacles = new SyncList<SpawnPointObstaclePair>();

        // [SyncVar(hook = "OnSetSpawnPointsAvoidObstacles")]
        // public readonly SyncList<int> spawnPointsAvoidObstacles = new SyncList<int>();

        // Triggers
        [SyncVar(hook = "OnSpawningItemsFinished")]
        public bool spawningItemsFinished = false;


        private BaseUIController _uiController;
        private VRController _vrController;
        private BaseController _controller;
        private NetworkPlayer[] _networkPlayers;
        private FindableObjectSpawner _findableObjectSpawner;
        private AvoidableObstacleSpawner _avoidableObstacleSpawner;
        private List<WireLoopVisualiser> _wireLoopVisualisers = new List<WireLoopVisualiser>();
        
        private bool _loadedScene = false;
        
        public UnityEvent onSceneLoadedEvent = new UnityEvent();
        private void Start()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;

            spawnPointsObjectFinding.Callback += OnAddToSpawnPointsObjectFinding;
            spawnPointsAvoidObstacles.Callback += OnAddToSpawnPointsAvoidObstacles;
            _findableObjectSpawner = FindObjectOfType<FindableObjectSpawner>();
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            Debug.Log("on scene loaded");
            _wireLoopVisualisers = FindObjectsOfType<WireLoopVisualiser>().ToList();
            _findableObjectSpawner = FindObjectOfType<FindableObjectSpawner>();
            _avoidableObstacleSpawner = FindObjectOfType<AvoidableObstacleSpawner>();
            _loadedScene = true;
        }
        
        private void OnSceneUnloaded(Scene arg0)
        {
            Debug.Log("on scene unloaded");
            _loadedScene = false;
        }
        
        
        public class SpawnPointPairWriter : NetworkWriter
        {
            public void WriteCustomStruct(SpawnPointFindableDataPair customStruct)
            {
                Write(customStruct.SpawnPointName);
                Write(customStruct.FindableDataIndex);
            }
        }
        public class CustomPairListWriter : NetworkWriter
        {
            public void WriteCustomStructList(SyncList<SpawnPointFindableDataPair> spawnPointFindableDataPair)
            {
                Write(spawnPointFindableDataPair.Count);

                foreach (var pair in spawnPointFindableDataPair)
                {
                    new SpawnPointPairWriter().WriteCustomStruct(pair);
                }
            }
        }
        private void Awake()
        {
            _controller = FindObjectOfType<BaseController>();
            _vrController = FindObjectOfType<VRController>();
            _uiController = FindObjectOfType<BaseUIController>();
            _networkPlayers = FindObjectsOfType<NetworkPlayer>();
        }




        /**
         * CALLBACKS
         *
         * hooks below are called whenever a specific SyncVar is changed 
         */

        public void OnSetScore(int oldScore, int newScore)
        {
            if (isMobileClient())
            {
                ScoreUIManager.Instance.UpdateScore(newScore);
            }
        }

        /**
         * Called when the vR user performs pointing gesture with their hand
         */
        public void OnSetUsingPointingGesture(bool oldValue, bool isPointing)
        {
            if (isMobileClient())
            {
                if (isPointing)
                {
                    Debug.Log("Kuk Will start shoot ray");
                    RaycastObjectPointer.Instance.StartShootRay();
                    Debug.Log("Kuk started shoot ray");
                }
                else
                {
                    Debug.Log("Kuk Will stop shoot ray");
                    RaycastObjectPointer.Instance.StopShootRay();
                }

            }
        }

        public void OnSetWireLoopColliding(bool oldValue, bool isColliding)
        {
            if (isMobileClient())
            {
                if (isColliding)
                {
                    // TODO assign wire loop visualiser to the player
                    _wireLoopVisualisers.ForEach(wireLoopVisualiser => wireLoopVisualiser.OnCollisionStart());
                }
                else
                {
                    _wireLoopVisualisers.ForEach(wireLoopVisualiser => wireLoopVisualiser.OnCollisionEnd());
                }
            }
        }

        public void OnAddToSpawnPointsObjectFinding(SyncList<SpawnPointFindableDataPair>.Operation op, int index, SpawnPointFindableDataPair oldItem, SpawnPointFindableDataPair newItem)
        {
            if (isMobileClient())
            {
               StartCoroutine(SyncSpawnPointToSpawner(newItem));
            }
        }

        private IEnumerator SyncSpawnPointToSpawner(SpawnPointFindableDataPair newItem)
        {
            if (_findableObjectSpawner == null)  
            {
                StartCoroutine(WaitForObjectNotNullCoroutine(_findableObjectSpawner));
            }

            _findableObjectSpawner.AddSpawnPoint(newItem);
            // CmdSetSpawningItemsFinished(true); // TODO musi se vratit zpet on scene load a taky nevim, kdy to vlastne volat
            yield return null;
        }

        public void OnAddToSpawnPointsAvoidObstacles(SyncList<SpawnPointObstaclePair>.Operation op, int index, SpawnPointObstaclePair oldItem, SpawnPointObstaclePair newItem)
        {
            if (isMobileClient())
            {
                StartCoroutine(SyncSpawnPointToObstacleSpawner(newItem));
            }
        }
        
        private IEnumerator SyncSpawnPointToObstacleSpawner(SpawnPointObstaclePair newItem)
        {
            if (_avoidableObstacleSpawner == null)  
            {
                StartCoroutine(WaitForObjectNotNullCoroutine(_avoidableObstacleSpawner));
            }

            _avoidableObstacleSpawner.AddPopulatedSpawnPoint(newItem);
            // CmdSetSpawningItemsFinished(true); // TODO musi se vratit zpet on scene load a taky nevim, kdy to vlastne volat
            yield return null;
        }
        
        public void OnSpawningItemsFinished(bool oldValue, bool finished)
        {
            if (isMobileClient())
            {
                if (finished)  
                {
                    Debug.Log("Kuk spawning items finished");
                    // TODO vymyslet logiku, jak cekat na spawnovani - interactable play tlacitka
                }
            }
        }

       
        /**
         * COMMANDS
         *
         * methods below change the values of SyncVars 
         * - called from clients but executed on the server
         * - requiresAuthority set to false to be able to sync SyncVars across all players  
         */

        [Command(requiresAuthority = false)]
        public void CmdSetScore(int newScore)
        {
            this.score = newScore;
        }
        
        [Command(requiresAuthority = false)]
        public void CmdSetUsingPointingGesture(bool isUsingPointingGesture)
        {
            this.usingPointingGesture = isUsingPointingGesture;
        }
        
        [Command(requiresAuthority = false)]
        public void CmdSetWireLoopColliding(bool colliding)
        {
            wireLoopColliding = colliding;
        }
        
        [Command(requiresAuthority = false)]
        public void CmdAddPopulatedSpawnPoint(SpawnPointFindableDataPair spawnPointFindableDataPair)
        {
            Debug.Log("Kuk adding spawn point to sync list");
            spawnPointsObjectFinding.Add(spawnPointFindableDataPair);
        }
        
        [Command(requiresAuthority = false)]
        public void CmdAddPopulatedSpawnPointObstacle(SpawnPointObstaclePair spawnPointObstaclePair)
        {
            Debug.Log("Kuk adding spawn point to sync list");
            spawnPointsAvoidObstacles.Add(spawnPointObstaclePair);
        }
        
        
        [Command(requiresAuthority = false)]
        public void CmdSetSpawningItemsFinished(bool isSpawningItemsFinished)
        {
            this.spawningItemsFinished = isSpawningItemsFinished;
        }

        private bool isMobileClient()
        {
            return SceneManager.GetActiveScene().name.Contains("Mobile") || SceneManager.GetActiveScene().name == GameConstants.AppOffline;
        }
        
        private IEnumerator WaitForObjectNotNullCoroutine<T>(T obj) where T : MonoBehaviour
        {
            while (obj == null)
            {
                obj = FindObjectOfType<T>();
                yield return null;
            }
        }

        public bool LoadedScene => _loadedScene;
    }
}
