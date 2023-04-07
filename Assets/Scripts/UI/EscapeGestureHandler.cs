using UnityEngine;

namespace UI
{
    public class EscapeGestureHandler : MonoBehaviour
    {
        [SerializeField] private float gestureTimeThreshold = 2.0f;
        [SerializeField] private Animator spinnerLoader;

        private bool _gestureDetected;

        void Start()
        {
            spinnerLoader.playbackTime = 0;
            spinnerLoader.gameObject.SetActive(false);
        }

        public void OnGestureDetectionStart()
        {
            _gestureDetected = true;
            spinnerLoader.gameObject.SetActive(true);
            spinnerLoader.playbackTime = 0;
            spinnerLoader.StartPlayback();
        }

        public void OnGestureDetectionEnd()
        {
            _gestureDetected = false;
            spinnerLoader.StopPlayback();
        }
    }
}
