using System.Collections.Generic;
using System.Linq;
using Interactions.ObjectFinding;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

namespace Interactions.AvoidObstacles
{
    public class AvoidableObstacleSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject avoidableObstacleLeftPrefab;
        [SerializeField] private GameObject avoidableObstacleRightPrefab;
        [SerializeField] private AvoidObstaclesController avoidObstaclesController; // TODO maybe assign the other way around
        [SerializeField] private GameObject spawnPointsParent;

        [SerializeField] private float scaleFactor = 5f;
        [SerializeField] private int obstaclesCount = 50;
        
        private List<Transform> spawnPoints;
        private int spawnPointsCount;

        private void Start()
        {
            var difficulty = avoidObstaclesController.Difficulty;
            obstaclesCount = GetObstaclesCount(difficulty);
            
            SpawnObjectsRandomly(obstaclesCount);
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
            spawnPoints = spawnPointsParent.GetComponentsInChildren<Transform>().ToList();
            
            var spawnPointsSubsetAvoidLeft = ListUtils.GetRandomSubset(spawnPoints, numberOfObstacles / 2); // Z toho puvodniho listu se musi odebrat ten substet
            
            // Remove each used spawn point from the spawn points list
            spawnPointsSubsetAvoidLeft.ForEach(usedSpawnPoint => spawnPoints.Remove(usedSpawnPoint));
            
            var spawnPointsSubsetAvoidRight = ListUtils.GetRandomSubset(spawnPoints, numberOfObstacles / 2);

            // iterate through the spawn points and spawn an object at each one
            foreach (var pointAvoidLeft in spawnPointsSubsetAvoidLeft)
            {
                var spawnedObject = Instantiate(avoidableObstacleLeftPrefab, pointAvoidLeft);
                AvoidableObstacle avoidableObstacleLeft = spawnedObject.GetComponent<AvoidableObstacle>();
                avoidableObstacleLeft.AvoidObstaclesController = avoidObstaclesController;

                var spawnedObjectTransform = spawnedObject.transform;
                spawnedObjectTransform.parent = pointAvoidLeft;
            }
            
            foreach (var pointAvoidRight in spawnPointsSubsetAvoidRight)
            {
                var spawnedObject = Instantiate(avoidableObstacleRightPrefab, pointAvoidRight);
                AvoidableObstacle avoidableObstacleRight = spawnedObject.GetComponent<AvoidableObstacle>();
                avoidableObstacleRight.AvoidObstaclesController = avoidObstaclesController;

                var spawnedObjectTransform = spawnedObject.transform;
                spawnedObjectTransform.parent = pointAvoidRight;
            }
            Debug.Log(spawnPointsSubsetAvoidLeft.Count);
            Debug.Log(spawnPointsSubsetAvoidRight.Count);
            
            Debug.Log("Spawned " + numberOfObstacles + " obstacles");
        }

        public int ObstaclesCount
        {
            get => obstaclesCount;
            set => obstaclesCount = value;
        }
        
        public int MaxObstaclesCount
        {
            get => spawnPointsCount;
            set => spawnPointsCount = value;
        }
    }
}
