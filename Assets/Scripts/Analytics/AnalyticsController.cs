using System;
using System.Collections;
using UnityEngine;
using Utils;

namespace Analytics
{
    public class AnalyticsController : MonoSingleton<AnalyticsController>
    {
        private HmdTracker _hmdTracker;
        private void OnEnable()
        {
            _hmdTracker = FindObjectOfType<HmdTracker>();
            Debug.Assert(_hmdTracker != null, "HmdTracker is null");
        }

        public void StartTracking()
        {
            _hmdTracker.StartTrackingData();
        }

        public void EndTracking()
        {
            _hmdTracker.EndTrackingData();
            var data = _hmdTracker.GetData();
            
            FileWriter fileWriter = new FileWriter(Constants.LogDirectoryName, Constants.FileName, Constants.FormatTXT, true);
            fileWriter.WriteData(fileWriter.ParseList(data));
        }

        [ContextMenu("Test Tracking")]
        public void TestTracking()
        {
            StartCoroutine(TestTrackingCoroutine());
        }

        private IEnumerator TestTrackingCoroutine()
        {
            StartTracking();
            yield return new WaitForSecondsRealtime(1);
            EndTracking();
        }
        
    }
}
