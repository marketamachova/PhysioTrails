using System.Collections;
using Player;
using Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class EscapeGestureHandler : MonoBehaviour
    {
        [SerializeField] private float gestureTimeThreshold = 1.0f;
        [SerializeField] private Animator spinnerLoader;

        [SerializeField] private string IdleStateName = "Idle";
        [SerializeField] private string SpinStateName = "Spin";

        private VRController _vrController;

        public VRController VRController
        {
            get => _vrController;
            set => _vrController = value;
        }

        private bool _gestureDetected;
        private bool _escapingToLobby;

        void Start()
        {
            spinnerLoader.playbackTime = 0;
            spinnerLoader.gameObject.SetActive(false);
        }

        public void OnGestureDetectionStart()
        {
            if (_vrController == null || _escapingToLobby)
            {
                return;
            }
            _gestureDetected = true;
            spinnerLoader.gameObject.SetActive(true);
            spinnerLoader.playbackTime = 0;
            spinnerLoader.Play(SpinStateName);

            StartCoroutine(EscapeToLobby());
        }

        public void OnGestureDetectionEnd()
        {
            _gestureDetected = false;
            spinnerLoader.Play(IdleStateName);
            spinnerLoader.gameObject.SetActive(false);
        }

        private IEnumerator EscapeToLobby()
        {
            yield return new WaitForSecondsRealtime(gestureTimeThreshold);
            if (_gestureDetected)
            {
                _vrController.TriggerGoToLobby();
                spinnerLoader.gameObject.SetActive(false);
                _escapingToLobby = true;
                
                yield return new WaitForSecondsRealtime(gestureTimeThreshold + 1);
                _escapingToLobby = false;
            }
        }
    }
}
