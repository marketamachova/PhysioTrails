using Analytics;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils
{
    public class DeviceTypeChecker : MonoSingleton<DeviceTypeChecker>
    {
        [SerializeField] private bool isVr = false;
        private void Start()
        {
            Debug.Log("Device type: " + SystemInfo.deviceType);
            Debug.Log("Device model: " + SystemInfo.deviceModel);
        }
        
        public bool IsMobile()
        {
            return SceneManager.GetActiveScene().name == GameConstants.AppOffline ||
                   SceneManager.GetActiveScene().name.Contains("Mobile");
        }
        
        public bool IsVr => isVr;
    }
}
