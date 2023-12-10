using UnityEngine;

namespace Interactions.AvoidObstacles
{
    public class AvoidObstaclesSceneManager : InteractionSceneManagerBase
    {
        private AvoidObstaclesController _avoidObstaclesController;
        void Start()
        {
            _avoidObstaclesController = FindObjectOfType<AvoidObstaclesController>();
            _avoidObstaclesController.AvoidObstaclesSceneManager = this;
            _avoidObstaclesController.OnSceneLoaded();
        }

    }
}
