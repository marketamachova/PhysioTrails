using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

namespace Analytics
{
    public class HmdTracker : MonoBehaviour
    {
        [SerializeField] private Transform centerEyeAnchor;
        [SerializeField] private OVRSkeleton rightHand;
        [SerializeField] private OVRSkeleton leftHand;
        [SerializeField] private Transform leftHandTransform;

        private float _startTrackingTime = 0;
        private List<string> data = new List<string>();
    

        // Start tracking data every second
        public void StartTrackingData()
        {
            _startTrackingTime = Time.time;
            InvokeRepeating("TrackData", 0, 1f);
        }
    
        // Start tracking data every second
        public void EndTrackingData()
        {
            CancelInvoke("TrackData");
        }

        /**
         * Tracks center eye position and rotation
         * Tracks position and rotation of hands' wrists
         */
        private void TrackData()
        {
            int time = (int) Math.Round(Time.time - _startTrackingTime);

            // Get position and rotation data for head
            Vector3 headPos = centerEyeAnchor.position;
            Vector3 headRot = centerEyeAnchor.rotation.eulerAngles;
        
            if (leftHand.Bones.Count == 0) return;
            
            // Get position and rotation data for hands
            Vector3 leftHandPos =  leftHand.Bones[0].Transform.position; 
            Vector3 rightHandPos = rightHand.Bones[0].Transform.position;
        
            Vector3 leftHandRot = leftHand.Bones[0].Transform.rotation.eulerAngles;
            Vector3 rightHandRot = rightHand.Bones[0].Transform.rotation.eulerAngles;

            // Add data to list as comma-separated string
            string rowData = $"T {time}\n" +
                             $"H p({headPos.x} {headPos.y} {headPos.z}) " +
                             $"r({headRot.x} {headRot.y} {headRot.z})\n" +
                             $"LH p({leftHandPos.x} {leftHandPos.y} {leftHandPos.z}) " +
                             $"r({leftHandRot.x} {leftHandRot.y} {leftHandRot.z})\n" +
                             $"RH p({rightHandPos.x} {rightHandPos.y} {rightHandPos.z}) " +
                             $"r({rightHandRot.x} {rightHandRot.y} {rightHandRot.z})\n";

            data.Add(rowData);
        }

        public List<string> GetData()
        {
            return data;
        }
    }
}
