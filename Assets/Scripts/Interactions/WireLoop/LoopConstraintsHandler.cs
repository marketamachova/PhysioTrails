using System;
using UnityEngine;

namespace Interactions.WireLoop
{
    public class LoopConstraintsHandler : MonoBehaviour
    {
        [SerializeField] private float movementXZDistanceConstraint = 0.03f;
        [SerializeField] private float rotationAngleConstraint = 30f;

        private void LateUpdate()
        {
            Debug.Log("LU");
            var newPos = transform.localPosition;
            newPos.z = Mathf.Clamp(newPos.z, newPos.z - movementXZDistanceConstraint,
                newPos.x + movementXZDistanceConstraint);
            newPos.y = Mathf.Clamp(newPos.y, newPos.y - movementXZDistanceConstraint,
                newPos.y + movementXZDistanceConstraint);

            transform.localPosition = newPos;

            var newRot = transform.localEulerAngles;
            newRot.x = ClampAngle(newRot.x, newRot.x - rotationAngleConstraint, newRot.x + rotationAngleConstraint);
            newRot.y = ClampAngle(newRot.y, newRot.y - rotationAngleConstraint, newRot.y + rotationAngleConstraint);
            newRot.z = ClampAngle(newRot.z, newRot.z - rotationAngleConstraint, newRot.z + rotationAngleConstraint);

            transform.localEulerAngles = newRot;
        }

        private float ClampAngle(float angle, float min, float max)
        {
            angle = NormalizeAngle(angle);
            return Mathf.Clamp(angle, min, max);
        }

        private float NormalizeAngle(float angle)
        {
            while (angle > 360)
            {
                angle -= 360;
            }

            while (angle < 0)
            {
                angle += 360;
            }

            return angle;
        }


    }
}
