using System;
using UnityEngine;

namespace Interactions.ObjectFinding
{
    public class FindableObject : MonoBehaviour
    {
        [SerializeField] private FindableObjectData data;
        [SerializeField] private FindableObjectVisual findableObjectVisual;
        [SerializeField] private GameObject meshHolder;

        [SerializeField] private ObjectFindingController objectFindingController;
        
        private bool _collected = false;

        private void Start()
        {
            findableObjectVisual.DisplayHighlight(false);
        }

        public void OnPointed()
        {
            findableObjectVisual.DisplayHighlight(true);

            if (!_collected)
            {
                // _collected = true;
                if (PointingValid())
                {
                    objectFindingController.OnPointedCorrectly();
                    findableObjectVisual.HighlightCorrect();
                }
                else
                {
                    objectFindingController.OnPointedIncorrectly();
                    findableObjectVisual.HighlightIncorrect();
                }
            }
        }

        public void OnUnpointed()
        {
            findableObjectVisual.Reset();
        }

        private bool PointingValid()
        {
            return objectFindingController.CollectType == data.Type;
        }

        public FindableObjectData Data
        {
            get => data;
            set => data = value;
        }
        
        public GameObject MeshHolder
        {
            get => meshHolder;
            set => meshHolder = value;
        }

        public FindableObjectVisual FindableObjectVisual
        {
            get => findableObjectVisual;
            set => findableObjectVisual = value;
        }
        
        public ObjectFindingController ObjectFindingController
        {
            get => objectFindingController;
            set => objectFindingController = value;
        }
    }
}
