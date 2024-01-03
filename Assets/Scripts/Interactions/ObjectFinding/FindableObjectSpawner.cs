using System;
using System.Collections.Generic;
using System.Linq;
using Analytics;
using Mirror;
using Network;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

namespace Interactions.ObjectFinding
{
    public class FindableObjectSpawner : MonoSingleton<FindableObjectSpawner>
    {
        public struct SpawnPointFindableDataPair
        {
            public string SpawnPointName;
            public int FindableDataIndex;
        }
        
        [SerializeField] private List<FindableObjectData> objectDataList; 
        [SerializeField] private GameObject spawnPointsParent;

        [SerializeField] private float scaleFactor = 5f;
        [SerializeField] private string findableObjectWrapperPrefabName = "FindableObjectWrapper";
        
        private ObjectFindingController _objectFindingController;
        private List<Transform> _spawnPoints;
        private List<SpawnPointFindableDataPair> _spawnedItemsAtIndices;
        private List<InteractionNetworkPlayer> _interactionNetworkPlayers;

        public void Initialize(ObjectFindingController objectFindingController, bool spawnItems)
        {
            _spawnedItemsAtIndices = new List<SpawnPointFindableDataPair>();

            _interactionNetworkPlayers = FindObjectsOfType<InteractionNetworkPlayer>().ToList();
            
            _objectFindingController = objectFindingController;
            var difficulty = _objectFindingController.Difficulty;
            
            var spawnPointsCount = GetSpawnPointsCount(difficulty);
            
            _spawnPoints = spawnPointsParent.GetComponentsInChildren<Transform>().ToList();

            if (spawnItems)
            {
               SpawnObjectsRandomly(spawnPointsCount); 
            }
        }

        /**
         * Set the spawn points to the ones specified by the indices
         * Values are the type of the FindableObjectData
         */
        public void SetSpawnPoints(List<SpawnPointFindableDataPair> spawnedItemsAtIndices)
        {
            Debug.Log("Kuk mobile received spawn points at FindableObjectSpawner");
            _spawnedItemsAtIndices = spawnedItemsAtIndices;
            for (int i = 0; i < _spawnedItemsAtIndices.Count; i++)
            {
                FindableObjectData objectData = objectDataList[_spawnedItemsAtIndices[i].FindableDataIndex];
                Transform spawnPoint = _spawnPoints.Find(sp => sp.name == _spawnedItemsAtIndices[i].SpawnPointName);
                SpawnFindableObjectAndAssignData(spawnPoint, objectData);
            }
        }
        
        public void AddSpawnPoint(SpawnPointFindableDataPair spawnPointFindableDataPair)
        {
            Debug.Log("Kuk mobile received spawn point at FindableObjectSpawner");
            _spawnedItemsAtIndices.Add(spawnPointFindableDataPair);
            FindableObjectData objectData = objectDataList[spawnPointFindableDataPair.FindableDataIndex];
            Transform spawnPoint = _spawnPoints.Find(sp => sp.name == spawnPointFindableDataPair.SpawnPointName);
            SpawnFindableObjectAndAssignData(spawnPoint, objectData);
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
            var spawnPointsSubset = ListUtils.GetRandomSubset(_spawnPoints, spawnPointsCount);
            
            var allFindableObjects = new List<FindableObject>();
            
            ListUtils.Shuffle(spawnPointsSubset);

            // iterate through the spawn points and spawn an object at each one
            for (int i = 0; i < spawnPointsSubset.Count; i++)
            {
                Transform spawnPoint = spawnPointsSubset[i];
                
                // objectDataList is a list of all objects that can be spawned
                int objectDataIndex = i % objectDataList.Count;
                FindableObjectData objectData = objectDataList[objectDataIndex];
                
                var findableObject = SpawnFindableObjectAndAssignData(spawnPoint, objectData);
                allFindableObjects.Add(findableObject);
                
                SpawnPointFindableDataPair spawnPointFindableDataPair = new SpawnPointFindableDataPair
                {
                    SpawnPointName = spawnPoint.name,
                    FindableDataIndex = objectDataIndex
                };
                _spawnedItemsAtIndices.Add(spawnPointFindableDataPair);
                
                // Send the spawn point to all network players
                _interactionNetworkPlayers.ForEach(networkPlayer =>
                {
                    if (networkPlayer.LoadedScene)
                    {
                        SyncSpawnedObjects(networkPlayer, spawnPointFindableDataPair);
                    }
                    else
                    {
                        Debug.Log("Kuk Network player " + networkPlayer.name + " not loaded scene yet");
                        (networkPlayer.onSceneLoadedEvent).AddListener(() =>  SyncSpawnedObjects(networkPlayer, spawnPointFindableDataPair));
                    }
                });

                Debug.Log("Spawned object " + objectData.ObjectName + " at " + spawnPoint.name + " with scale " + scaleFactor + " and mesh " + objectData.Mesh.name + " and material " + objectData.Material.name);
            }
            
            AnalyticsController.Instance.FindableObjects = allFindableObjects;

            Debug.Log("Spawned "  + spawnPointsSubset.Count + " objects");
        }

        private void SyncSpawnedObjects(InteractionNetworkPlayer networkPlayer, SpawnPointFindableDataPair spawnPointFindableDataPair)
        {
            networkPlayer.CmdAddPopulatedSpawnPoint(spawnPointFindableDataPair);
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
        
        private FindableObject SpawnFindableObjectAndAssignData(Transform spawnPoint, FindableObjectData objectData)
        {
            var spawnedObject = InstantiateObject(findableObjectWrapperPrefabName, spawnPoint);
            FindableObject findableObject = spawnedObject.GetComponent<FindableObject>();
            findableObject.ObjectFindingController = _objectFindingController;
            findableObject.Data = objectData;
                
            var spawnedObjectTransform = spawnedObject.transform;
            spawnedObjectTransform.parent = spawnPoint;
            
            spawnedObjectTransform.localPosition = Vector3.zero;
            spawnedObjectTransform.localRotation = Quaternion.identity;
            spawnedObjectTransform.localScale = Vector3.one * scaleFactor;

            var mesh = findableObject.MeshHolder;
            mesh.transform.localScale = Vector3.one;
            mesh.gameObject.GetComponent<MeshFilter>().mesh = objectData.Mesh;
            mesh.gameObject.GetComponent<MeshRenderer>().material = objectData.Material;

            return findableObject;
        }
    }
}
