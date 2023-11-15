using System.Collections.Generic;
using System.Linq;
using PathCreation;
using UnityEngine;

namespace Interactions.WireLoop
{
    public class PathController : MonoBehaviour
    {
        [SerializeField] private PathCreator originalPath;
        [SerializeField] private float wayPointShiftAmountX = 0.05f;
        [SerializeField] private float wayPointShiftAmountY = 0.1f;
        
        private Vector3[] _newWaypoints;
        private List<Vector3> _originalWaypoints;

        [ContextMenu("Generate Path")]
        public void GeneratePath()
        {
            ExtractWaypointsFromPath();
            RandomizeWaypoints();
            CreatePathFromWaypoints();
            // TODO maybe need to generate mesh collider
        }

        private void RandomizeWaypoints()
        {
            _newWaypoints = new Vector3[_originalWaypoints.Count];
            var index = 0;
            foreach (var originalWaypoint in _originalWaypoints)
            {
                // Generate random number within the range of the shift amount
                var randomX = Random.Range(-wayPointShiftAmountX, wayPointShiftAmountX);
                var randomY = Random.Range(-wayPointShiftAmountY, wayPointShiftAmountY);
                
                var newWaypoint = new Vector3(originalWaypoint.x + randomX, originalWaypoint.y + randomY, originalWaypoint.z);
                _newWaypoints[index] = newWaypoint;
                index++;
            }
        }

        private void ExtractWaypointsFromPath()
        {
            _originalWaypoints = new List<Vector3>();
            var segmentsCount = originalPath.bezierPath.NumSegments;
            for (int i = 0; i < segmentsCount; i++)
            {
                var pointsInSegment = originalPath.bezierPath.GetPointsInSegment(i).ToArray();
                foreach (var point in pointsInSegment)
                {
                    _originalWaypoints.Add(point);
                }
            }
        }

        private void CreatePathFromWaypoints()
        {
            if (_newWaypoints.Length > 0) {
                BezierPath bezierPath = new BezierPath (_newWaypoints, false, PathSpace.xyz);
                originalPath.bezierPath = bezierPath;
                
                // Create new mesh collider component and delete the old one
                var meshCollider = originalPath.GetComponent<MeshCollider>();
                Destroy(meshCollider);
                originalPath.gameObject.AddComponent<MeshCollider>();
            } 
        }
        
           
    }
}
