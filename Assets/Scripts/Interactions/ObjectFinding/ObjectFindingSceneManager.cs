using System;
using UnityEngine;

namespace Interactions.ObjectFinding
{
    public class ObjectFindingSceneManager : InteractionSceneManagerBase
    {
        [SerializeField] private FindableObjectSpawner findableObjectSpawner;
        private ObjectFindingController _objectFindingController;
        
        private void Start()
        {
            _objectFindingController = FindObjectOfType<ObjectFindingController>();
            _objectFindingController.ObjectFindingSceneManager = this;
            findableObjectSpawner.Initialize(_objectFindingController);
            _objectFindingController.OnSceneLoaded();
        }

        public ObjectFindingController ObjectFindingController => _objectFindingController;
    }
}
