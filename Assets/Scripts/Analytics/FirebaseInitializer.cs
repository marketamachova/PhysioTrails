using System;
using Firebase;
using UnityEngine;
using UnityEngine.Events;

namespace Analytics
{
    public class FirebaseInitializer : MonoBehaviour
    {
        public UnityEvent OnFirebaseInitialized;

        private async void Start()
        {
            var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
            if (dependencyStatus == DependencyStatus.Available)
            {
                OnFirebaseInitialized.Invoke();
            }
        }
    }
}
