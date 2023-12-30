using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRLogger.Classes;

namespace VRLogger
{
    public class LoggerBase : MonoBehaviour
    {
        public GameObject head;
        public GameObject leftHand;
        public GameObject rightHand;
        
        protected readonly List<string> Events = new List<string>();
        protected string Environment;
        protected string RecordCustomData;
        protected Activity Activity;
        
        protected LoggerHelper LoggerHelper= new LoggerHelper();
        
        private bool _isHeadNotNull;
        private bool _isLeftHandNotNull;
        private bool _isRightHandNotNull;
        private int _tick;
       

        public void Start()
        {
            _isHeadNotNull = head != null;
            _isLeftHandNotNull = leftHand != null;
            _isRightHandNotNull = rightHand != null;
        }
        
        protected IEnumerator LoggingCoroutine(float waitTime)
        {
            while (true)
            {
                yield return new WaitForSeconds(waitTime);

                Logging();
            }
        }

        private void Logging()
        {
            string[] events = null;
            string customData = null;
            PositionAndRotation headData = null;
            PositionAndRotation leftHandData = null;
            PositionAndRotation rightHandData = null;

            if (Events.Count > 0)
            {
                events = Events.ToArray();
                Events.Clear();
            }

            if (!string.IsNullOrEmpty(RecordCustomData))
            {
                customData = RecordCustomData;
                RecordCustomData = null;
            }

            if (_isHeadNotNull)
            {
                headData = PositionAndRotation.GetLocalPositionAndRotation(head);
            }

            if (_isLeftHandNotNull)
            {
                leftHandData = PositionAndRotation.GetLocalPositionAndRotation(leftHand);
            }

            if (_isRightHandNotNull)
            {
                rightHandData = PositionAndRotation.GetLocalPositionAndRotation(rightHand);
            }

            var record = new Record(DateTime.Now, _tick, Environment, headData, leftHandData, rightHandData,
                customData, events);
            Activity.data.records.Add(record);

            _tick++;
        }
    }
}

