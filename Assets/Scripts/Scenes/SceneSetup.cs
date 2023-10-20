using System.Collections.Generic;
using UnityEngine;

namespace Scenes
{
    public class SceneSetup : MonoBehaviour
    {
        [SerializeField] private List<GameObject> objectsToDisplay = new List<GameObject>();
        [SerializeField] private List<GameObject> objectsToHide = new List<GameObject>();
        [SerializeField] private List<TerrainCollider> terrainColliders = new List<TerrainCollider>();
    
        void Start()
        {
            objectsToDisplay.ForEach(obj => obj.SetActive(true));
            objectsToHide.ForEach(obj => obj.SetActive(false));
            terrainColliders.ForEach(terrainCollider => terrainCollider.enabled = false);
        }
    }
}
