using System.Collections.Generic;
using UnityEngine;

namespace Interactions.AvoidObstacles
{
    public class AvoidObstaclesSceneManager : InteractionSceneManagerBase
    {
        private AvoidObstaclesController _avoidObstaclesController;
        
        [SerializeField] private AvoidableObstacleSpawner avoidableObstacleSpawner;
        [SerializeField] private bool isVr = true;
        
        void Start()
        {
            _avoidObstaclesController = FindObjectOfType<AvoidObstaclesController>();
            _avoidObstaclesController.AvoidObstaclesSceneManager = this;
            _avoidObstaclesController.OnSceneLoaded();
            
            
            avoidableObstacleSpawner.Initialize(_avoidObstaclesController, isVr);
        }
    }
}
