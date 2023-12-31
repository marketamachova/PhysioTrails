using System;
using UnityEngine;
using UnityEngine.Events;

namespace Interactions.ObjectFinding
{
    public class FindableObject : MonoBehaviour
    {
        [SerializeField] private FindableObjectData data;
        [SerializeField] private FindableObjectVisual findableObjectVisual;
        [SerializeField] private GameObject meshHolder;
        [SerializeField] private string objectFoundEventKey = "MushroomFound";

        private ObjectFindingController _objectFindingController;
        
        private bool _collected = false;
        
        public UnityEvent<string> onObjectFound = new UnityEvent<string>();

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
                    _objectFindingController.OnPointedCorrectly();
                    findableObjectVisual.HighlightCorrect();
                }
                else
                {
                    _objectFindingController.OnPointedIncorrectly();
                    findableObjectVisual.HighlightIncorrect();
                }
                
                onObjectFound.Invoke(objectFoundEventKey);
            }
        }

        public void OnUnpointed()
        {
            findableObjectVisual.Reset();
        }

        private bool PointingValid()
        {
            return _objectFindingController.CollectType == data.Type;
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
            get => _objectFindingController;
            set => _objectFindingController = value;
        }
    }
}
