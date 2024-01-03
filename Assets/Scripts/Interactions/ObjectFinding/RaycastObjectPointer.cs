using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Utils;

namespace Interactions.ObjectFinding
{
    public class RaycastObjectPointer : MonoSingleton<RaycastObjectPointer>
    {
        [Header("References")]
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private PointingGestureHandler pointingGestureHandler;

        [Header("Settings")]
        [SerializeField] private bool isNetworkPointer = false;
        [SerializeField] private Transform indexFingerTip;
        [SerializeField] private float maxDistance = 20f;
        [SerializeField] private bool enableRaycastOnlyWhenPointingGesture = true;
        
        private FindableObject _currentPointedObject;
        private Coroutine _shootRayCoroutine;
        private LayerMask _layersToExclude;

        private void Awake()
        {
            _layersToExclude = LayerMask.GetMask("Floor");
            
            if (enableRaycastOnlyWhenPointingGesture && !isNetworkPointer)
            {
                pointingGestureHandler.onPointGestureStarted.AddListener(StartShootRay);
                pointingGestureHandler.onPointGestureEnded.AddListener(StopShootRay);
            }
        }

        private void Start()
        {
            lineRenderer.enabled = false;
            if (!enableRaycastOnlyWhenPointingGesture)
            {
                StartShootRay();
            }
        }

        public void StartShootRay()
        {
            if (_shootRayCoroutine == null)
            {
                lineRenderer.enabled = true;
                _shootRayCoroutine = StartCoroutine(ShootRayCoroutine());
            }
        }

        public void StopShootRay()
        {
            if (_shootRayCoroutine != null)
            {
                StopCoroutine(_shootRayCoroutine);
                lineRenderer.enabled = false;
                _shootRayCoroutine = null;
            }
        }
        
        private IEnumerator ShootRayCoroutine()
        {
            while (true)
            {
                ShootRay();
                yield return null;
            }
        }

        private void ShootRay()
        {
            Ray ray = new Ray(indexFingerTip.position, indexFingerTip.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxDistance))
            {
                // Object hit, handle interaction
                FindableObject findableObject = hit.collider.GetComponentInParent<FindableObject>();
                if (findableObject != null)
                {
                    _currentPointedObject = findableObject;
                    findableObject.OnPointed();
                }
                else
                {
                    // Debug.Log("Kuk " + hit.collider.name);
                }
                
                // Update Line Renderer positions
                lineRenderer.SetPosition(0, indexFingerTip.position);
                lineRenderer.SetPosition(1, hit.point);
            }
            else
            {
                if (_currentPointedObject != null)
                {
                    _currentPointedObject.OnUnpointed();
                    _currentPointedObject = null;
                }

                var position = indexFingerTip.position;
                lineRenderer.SetPosition(0, position);
                lineRenderer.SetPosition(1, position + indexFingerTip.forward * maxDistance);
            }
        }
    }
}
