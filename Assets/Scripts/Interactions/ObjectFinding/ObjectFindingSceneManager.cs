using System;
using UnityEngine;

namespace Interactions.ObjectFinding
{
    public class ObjectFindingSceneManager : InteractionSceneManagerBase
    {
        private ObjectFindingController _objectFindingController;
        private void Start()
        {
            _objectFindingController = FindObjectOfType<ObjectFindingController>();
            _objectFindingController.ObjectFindingSceneManager = this;
            _objectFindingController.OnSceneLoaded();
        }
    }
}
