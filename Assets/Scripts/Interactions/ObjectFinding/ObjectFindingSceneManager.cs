using System;
using UnityEngine;

namespace Interactions.ObjectFinding
{
    public class ObjectFindingSceneManager : InteractionSceneManagerBase
    {
        [SerializeField] private FindableObjectSpawner findableObjectSpawner;
        [SerializeField] private bool isVr = true;
        private ObjectFindingController _objectFindingController;
        
        private void Start()
        {
            _objectFindingController = FindObjectOfType<ObjectFindingController>();
            _objectFindingController.ObjectFindingSceneManager = this;
            findableObjectSpawner.Initialize(_objectFindingController, isVr);
            _objectFindingController.OnSceneLoaded();
        }

        public ObjectFindingController ObjectFindingController => _objectFindingController;
    }
}
