using System;
using System.Collections.Generic;
using UnityEngine;

namespace Interactions.WireLoop
{
    [RequireComponent(typeof(Renderer))]
    public class WireLoopVisualiser : MonoBehaviour
    {
        [SerializeField] private Material collidedMaterial;
        [SerializeField] private Material defaultMaterial;

        private Renderer _renderer;

        private void Start()
        {
            _renderer = GetComponent<Renderer>();
        }

        public void OnCollisionStart()
        {
            _renderer.material = collidedMaterial;
        }
        
        public void OnCollisionEnd()
        {
            _renderer.material = defaultMaterial;
        }
    }
}
