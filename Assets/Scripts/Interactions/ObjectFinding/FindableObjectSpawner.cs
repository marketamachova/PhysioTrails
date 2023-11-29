using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

namespace Interactions.ObjectFinding
{
    public class FindableObjectSpawner : MonoBehaviour
    {
        [SerializeField] private List<Transform> spawnPoints;
        [SerializeField] private List<FindableObjectData> objectDataList;
        [SerializeField] private ObjectFindingController objectFindingController;

        [SerializeField] private float scaleFactor = 5f;
        private void Start()
        {
            SpawnObjectsRandomly();
        }

        /**
         * Spawns objects randomly at the spawn points
         */
        private void SpawnObjectsRandomly()
        {
            ListUtils.Shuffle(spawnPoints);

            // iterate through the spawn points and spawn an object at each one
            for (int i = 0; i < spawnPoints.Count; i++)
            {
                Transform spawnPoint = spawnPoints[i];
                
                // Get a random object from the list
                FindableObjectData objectData = objectDataList[i];
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
            }
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
