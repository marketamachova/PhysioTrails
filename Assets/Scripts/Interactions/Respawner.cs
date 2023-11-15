using Interactions.WireLoop;
using UnityEngine;

namespace Interactions
{
    public class Respawner : MonoBehaviour
    {
        [SerializeField] private float xThreshold;
        [SerializeField] private float yThreshold;
        [SerializeField] private float zThreshold;
        [SerializeField] private WireLoopVisualiser wireLoopVisualiser;

        private bool IsLocalPositionWithinThreshold()
        {
            var localPos = transform.localPosition;
            return Mathf.Abs(localPos.x - xThreshold) < 0 && Mathf.Abs(localPos.y - yThreshold) < 0 &&
                Mathf.Abs(localPos.z - zThreshold) < 0;
        }

        private void Reposition()
        {
            transform.localPosition = new Vector3();
        }

        void Update()
        {
            if (!IsLocalPositionWithinThreshold())
            {
                // Debug.Log("Kuk reposition");
                // wireLoopVisualiser.OnCollisionStart();
                // TODO show ghost
                //Reposition();
            }
            else
            {
                // wireLoopVisualiser.OnCollisionEnd();
            }
        }
    }
}
