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

        public void StartTracking(string sceneName)
        {
            _hmdTracker.StartTrackingData(sceneName);
        }

        public void EndTracking()
        {
            _hmdTracker.EndTrackingData();
            var data = _hmdTracker.GetData();
            var sceneName = _hmdTracker.GetSceneName();
            
            // FileWriter fileWriter = new FileWriter(Constants.FileName, Constants.FormatTXT, true, Constants.LogDirectoryName);
            // fileWriter.WriteData(fileWriter.ParseList(data));

            if (data == null)
            {
                Debug.LogError("Tracking data is null.");
                return;
            }
            
            //Sending data to Firebase
            _firebaseStorageHandler.UploadFile(data, sceneName);
        }

        [ContextMenu("Test Tracking")]
        public void TestTracking()
        {
            StartCoroutine(TestTrackingCoroutine());
        }

        private IEnumerator TestTrackingCoroutine()
        {
            StartTracking("Forest scene");
            yield return new WaitForSecondsRealtime(1);
            EndTracking();
        }
        
    }
}
