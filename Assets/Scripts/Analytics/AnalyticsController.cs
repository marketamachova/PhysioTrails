using System.Collections;
using UnityEngine;
using Utils;

namespace Analytics
{
    public class AnalyticsController : MonoSingleton<AnalyticsController>
    {
        private HmdTracker _hmdTracker;
        private FirebaseStorageHandler _firebaseStorageHandler;

        private void Awake()
        {
            _firebaseStorageHandler = new FirebaseStorageHandler();
        }
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
            
            // FileWriter fileWriter = new FileWriter(Constants.FileName, Constants.FormatTXT, true, Constants.LogDirectoryName);
            // fileWriter.WriteData(fileWriter.ParseList(data));

            if (data == null)
            {
                Debug.LogError("Tracking data is null.");
                return;
            }
            
            //Sending data to Firebase
            _firebaseStorageHandler.UploadFile(data);
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
