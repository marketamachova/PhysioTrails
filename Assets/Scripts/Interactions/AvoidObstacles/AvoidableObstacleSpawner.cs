using System.Collections.Generic;
using Interactions.ObjectFinding;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

namespace Interactions.AvoidObstacles
{
    public class AvoidableObstacleSpawner : MonoBehaviour
    {
        [SerializeField] private List<Transform> spawnPoints;
        [SerializeField] private GameObject avoidableObstacleLeftPrefab;
        [SerializeField] private GameObject avoidableObstacleRightPrefab;
        [SerializeField] private AvoidObstaclesController avoidObstaclesController; // TODO maybe assign the other way around

        [SerializeField] private float scaleFactor = 5f;
        [SerializeField] private int spawnPointsCount = 5;
        private void Start()
        {
            SpawnObjectsRandomly();
        }

        /**
         * Choose some subset of spawn points and populate those with avoidable obstacles of each type (left, right)
         */
        private void SpawnObjectsRandomly()
        {
            var spawnPointsSubsetAvoidLeft = ListUtils.GetRandomSubset(spawnPoints, spawnPointsCount / 2); // Z toho puvodniho listu se musi odebrat ten substet
            var spawnPointsSubsetAvoidRight = ListUtils.GetRandomSubset(spawnPoints, spawnPointsCount / 2);
            
            // iterate through the spawn points and spawn an object at each one
            foreach (var pointAvoidLeft in spawnPointsSubsetAvoidLeft)
            {
                var spawnedObject = Instantiate(avoidableObstacleLeftPrefab, pointAvoidLeft);
                AvoidableObstacle avoidableObstacleLeft = spawnedObject.GetComponent<AvoidableObstacle>();
                avoidableObstacleLeft.AvoidObstaclesController = avoidObstaclesController;

                var spawnedObjectTransform = spawnedObject.transform;
                spawnedObjectTransform.parent = pointAvoidLeft;
                // // Make sure the object's position and rotation is oriented as the parent
                // spawnedObjectTransform.localPosition = Vector3.zero;
                // spawnedObjectTransform.localRotation = Quaternion.identity;
                // spawnedObjectTransform.localScale = Vector3.one * scaleFactor;
            }
            
            foreach (var pointAvoidRight in spawnPointsSubsetAvoidRight)
            {
                var spawnedObject = Instantiate(avoidableObstacleRightPrefab, pointAvoidRight);
                AvoidableObstacle avoidableObstacleRight = spawnedObject.GetComponent<AvoidableObstacle>();
                avoidableObstacleRight.AvoidObstaclesController = avoidObstaclesController;

                var spawnedObjectTransform = spawnedObject.transform;
                spawnedObjectTransform.parent = pointAvoidRight;
                // // Make sure the object's position and rotation is oriented as the parent
                // spawnedObjectTransform.localPosition = Vector3.zero;
                // spawnedObjectTransform.localRotation = Quaternion.identity;
                // spawnedObjectTransform.localScale = Vector3.one * scaleFactor;
            }
        }

        public int SpawnPointsCount
        {
            get => spawnPointsCount;
            set => spawnPointsCount = value;
        }
    }
}
