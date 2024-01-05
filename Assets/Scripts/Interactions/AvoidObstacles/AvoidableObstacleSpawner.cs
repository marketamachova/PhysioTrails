using System.Collections.Generic;
using System.Linq;
using Analytics;
using Interactions.ObjectFinding;
using Network;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

namespace Interactions.AvoidObstacles
{
    public class AvoidableObstacleSpawner : MonoSingleton<AvoidableObstacleSpawner>
    {
        public struct SpawnPointObstaclePair
        {
            public string SpawnPointName;
            public int ObstacleType;
        }

        [SerializeField] private GameObject avoidableObstacleLeftPrefab;
        [SerializeField] private GameObject avoidableObstacleRightPrefab;
        
        [SerializeField] private GameObject spawnPointsParent;

        [SerializeField] private float scaleFactor = 5f;
        [SerializeField] private int obstaclesCount = 50;

        private List<Transform> _spawnPoints;
        private int _spawnPointsCount;
        private List<InteractionNetworkPlayer> _interactionNetworkPlayers;
        private AvoidObstaclesController _avoidObstaclesController;

        public void Initialize(AvoidObstaclesController avoidObstaclesController, bool spawnPoints)
        {
            Debug.Log("Initializing AvoidableObstacleSpawner");
            _avoidObstaclesController = avoidObstaclesController;
            var difficulty = _avoidObstaclesController.Difficulty;
            
            _interactionNetworkPlayers = FindObjectsOfType<InteractionNetworkPlayer>()?.ToList();

            obstaclesCount = GetObstaclesCount(difficulty);
            _spawnPoints = spawnPointsParent.GetComponentsInChildren<Transform>().ToList();

            if (spawnPoints)
            {
                SpawnObjectsRandomly(obstaclesCount);
            }

            if (_interactionNetworkPlayers != null)
            {
                Debug.Log("Found " + _interactionNetworkPlayers.Count + " interaction network players");
            }
        }

        private int GetObstaclesCount(InteractionConfigurator.DifficultyType difficulty)
        {
            var count = 0;
            switch (difficulty)
            {
                case InteractionConfigurator.DifficultyType.Easy:
                    count = 15;
                    break;
                case InteractionConfigurator.DifficultyType.Medium:
                    count = 50;
                    break;
                case InteractionConfigurator.DifficultyType.Hard:
                    count = 90;
                    break;
            }

            return count;
        }

        /**
         * Choose some subset of spawn points and populate those with avoidable obstacles of each type (left, right)
         */
        private void SpawnObjectsRandomly(int numberOfObstacles)
        {
            var allObstacles = new List<AvoidableObstacle>();

            var spawnPointsSubsetAvoidLeft =
                ListUtils.GetRandomSubset(_spawnPoints,
                    numberOfObstacles / 2); // Z toho puvodniho listu se musi odebrat ten substet

            // Remove each used spawn point from the spawn points list
            spawnPointsSubsetAvoidLeft.ForEach(usedSpawnPoint => _spawnPoints.Remove(usedSpawnPoint));

            var spawnPointsSubsetAvoidRight = ListUtils.GetRandomSubset(_spawnPoints, numberOfObstacles / 2);

            // iterate through the spawn points and spawn an object at each one
            foreach (var pointAvoidLeft in spawnPointsSubsetAvoidLeft)
            {
                var avoidableObstacleLeft = SpawnObstacle(pointAvoidLeft, avoidableObstacleLeftPrefab);

                allObstacles.Add(avoidableObstacleLeft);
                
                _interactionNetworkPlayers.ForEach(networkPlayer => SyncSpawnedObjects(networkPlayer,
                    new SpawnPointObstaclePair
                    {
                        SpawnPointName = pointAvoidLeft.name,
                        ObstacleType = 0
                    }
                ));
            }

            foreach (var pointAvoidRight in spawnPointsSubsetAvoidRight)
            {
                var avoidableObstacleRight = SpawnObstacle(pointAvoidRight, avoidableObstacleRightPrefab);

                allObstacles.Add(avoidableObstacleRight);
                
                _interactionNetworkPlayers.ForEach(networkPlayer => SyncSpawnedObjects(networkPlayer,
                    new SpawnPointObstaclePair
                    {
                        SpawnPointName = pointAvoidRight.name,
                        ObstacleType = 1
                    }
                ));
            }

            AnalyticsController.Instance.AvoidableObstacles = allObstacles;

            Debug.Log("Spawned " + numberOfObstacles + " obstacles");
        }

        private void SyncSpawnedObjects(InteractionNetworkPlayer networkPlayer,
            SpawnPointObstaclePair spawnPointObstaclePair)
        {
            if (networkPlayer.LoadedScene)
            {
                networkPlayer.CmdAddPopulatedSpawnPointObstacle(new SpawnPointObstaclePair
                {
                    SpawnPointName = spawnPointObstaclePair.SpawnPointName,
                    ObstacleType = spawnPointObstaclePair.ObstacleType
                });
            }
            else
            {
                networkPlayer.onSceneLoadedEvent.AddListener(() =>
                {
                    networkPlayer.CmdAddPopulatedSpawnPointObstacle(new SpawnPointObstaclePair
                    {
                        SpawnPointName = spawnPointObstaclePair.SpawnPointName,
                        ObstacleType = spawnPointObstaclePair.ObstacleType
                    });
                });
            }
        }

        private AvoidableObstacle SpawnObstacle(Transform parent, GameObject prefab)
        {
            var spawnedObject = Instantiate(prefab, parent);
            AvoidableObstacle avoidableObstacle = spawnedObject.GetComponent<AvoidableObstacle>();
            avoidableObstacle.AvoidObstaclesController = _avoidObstaclesController;
            spawnedObject.transform.localPosition = Vector3.zero;

            return avoidableObstacle;
        }

        public void AddPopulatedSpawnPoint(SpawnPointObstaclePair spawnPointObstaclePair)
        {
            Debug.Log("Kuk mobile received spawn point at AvpodableObstacleSpawner");
            var spawnPoint = _spawnPoints.Find(sp => sp.name == spawnPointObstaclePair.SpawnPointName);
            var isLeft = spawnPointObstaclePair.ObstacleType == 0;
            Instantiate(isLeft ? avoidableObstacleLeftPrefab : avoidableObstacleRightPrefab, spawnPoint);
        }

        public int ObstaclesCount
        {
            get => obstaclesCount;
            set => obstaclesCount = value;
        }

        public int MaxObstaclesCount
        {
            get => _spawnPointsCount;
            set => _spawnPointsCount = value;
        }
    }
}