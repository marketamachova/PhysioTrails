using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

namespace Interactions.ObjectFinding
{
    public class FindableObjectSpawner : MonoBehaviour
    {
        [SerializeField] private List<FindableObjectData> objectDataList;
        [SerializeField] private ObjectFindingController objectFindingController;
        [SerializeField] private GameObject spawnPointsParent;

        [SerializeField] private float scaleFactor = 5f;
        
        private List<Transform> _spawnPoints;

        
        private void Start()
        {
            var difficulty = objectFindingController.Difficulty;
            var spawnPointsCount = GetSpawnPointsCount(difficulty);
            
            SpawnObjectsRandomly(spawnPointsCount);
        }
        
        private int GetSpawnPointsCount(InteractionConfigurator.DifficultyType difficulty)
        {
            var count = 0;
            switch (difficulty)
            {
                case InteractionConfigurator.DifficultyType.Easy:
                    count = 30;
                    break;
                case InteractionConfigurator.DifficultyType.Medium:
                    count = 60;
                    break;
                case InteractionConfigurator.DifficultyType.Hard:
                    count = 84;
                    break;
            }

            return count;
        }

        /**
         * Spawns objects randomly at the spawn points
         */
        private void SpawnObjectsRandomly(int spawnPointsCount)
        {
            _spawnPoints = spawnPointsParent.GetComponentsInChildren<Transform>().ToList();
            
            var spawnPointsSubset = ListUtils.GetRandomSubset(_spawnPoints, spawnPointsCount);
            
            ListUtils.Shuffle(spawnPointsSubset);

            // iterate through the spawn points and spawn an object at each one
            for (int i = 0; i < spawnPointsSubset.Count; i++)
            {
                Transform spawnPoint = spawnPointsSubset[i];
                
                // Get a random object from the list
                FindableObjectData objectData = objectDataList[i % objectDataList.Count];
                var spawnedObject = InstantiateObject("FindableObjectWrapper", spawnPoint);
                FindableObject findableObject = spawnedObject.GetComponent<FindableObject>();
                findableObject.ObjectFindingController = objectFindingController;
                findableObject.Data = objectData;

                var spawnedObjectTransform = spawnedObject.transform;
                spawnedObjectTransform.parent = spawnPoint;
                // Make sure the object's position and rotation is oriented as the parent
                spawnedObjectTransform.localPosition = Vector3.zero;
                spawnedObjectTransform.localRotation = Quaternion.identity;
                spawnedObjectTransform.localScale = Vector3.one * scaleFactor;

                var mesh = findableObject.MeshHolder;
                mesh.transform.localScale = Vector3.one;
                mesh.gameObject.GetComponent<MeshFilter>().mesh = objectData.Mesh;
                mesh.gameObject.GetComponent<MeshRenderer>().material = objectData.Material;
                
                Debug.Log("Spawned object " + objectData.ObjectName + " at " + spawnPoint.name + " with scale " + scaleFactor + " and mesh " + objectData.Mesh.name + " and material " + objectData.Material.name);
            }
            
            Debug.Log("Spawned "  + spawnPointsSubset.Count + " objects");
        }

        private GameObject InstantiateObject(string objectName, Transform parent)
        {
            GameObject prefabToInstantiate = Resources.Load<GameObject>(objectName);

            if (prefabToInstantiate != null)
            {
                // Assign the prefab to instantiate
                return Instantiate(prefabToInstantiate, parent);
            }
            else
            {
                Debug.LogError("Prefab to instantiate not found in resources!");
                return null;
            }
        }
    }
}
