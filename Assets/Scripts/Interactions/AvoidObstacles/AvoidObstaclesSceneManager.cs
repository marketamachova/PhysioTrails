using System.Collections.Generic;
using UnityEngine;

namespace Interactions.AvoidObstacles
{
    public class AvoidObstaclesSceneManager : InteractionSceneManagerBase
    {
        private AvoidObstaclesController _avoidObstaclesController;
        private List<AvoidableObstacle> _avoidableObstacles;
        
        void Start()
        {
            _avoidObstaclesController = FindObjectOfType<AvoidObstaclesController>();
            _avoidObstaclesController.AvoidObstaclesSceneManager = this;
            _avoidObstaclesController.OnSceneLoaded();
            
            _avoidableObstacles = new List<AvoidableObstacle>(FindObjectsOfType<AvoidableObstacle>());
            
        }

        public List<AvoidableObstacle> AvoidableObstacles
        {
            get => _avoidableObstacles;
            set => _avoidableObstacles = value;
        }
    }
}
