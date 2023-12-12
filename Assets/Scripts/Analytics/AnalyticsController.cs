using System;
using System.Collections.Generic;
using Interactions;
using UnityEngine;
using Utils;
using VRLogger;
using VRLogger.Classes;

namespace Analytics
{
    public class AnalyticsController : MonoSingleton<AnalyticsController>
    {
        [SerializeField] private VrLogger vrLogger;
        [SerializeField] private InteractionManager interactionManager;

        private List<VREventTrigger> _eventTriggers;
        private bool _vrLoggerInitialized;
        
        // TODO have list of AnalyticsTriggers - which each have collider and name and then subsribe here when the collider hits to send event to vrLogger

        private void Awake()
        {
        }

        private void Start()
        {
            InitializeVrLogger();
        }

        public void InitializeVrLogger()
        {
            vrLogger.SetOrganisation("KOyLfT");
        }

        public void StartTracking(string sceneName, List<VREventTrigger> eventTriggers)
        {
            EventTriggers = eventTriggers;
            
            vrLogger.InitializeLogger();
            
            // TODO add other data, dont know which now
            vrLogger.SetCustomData("{\"interaction\": " + interactionManager.CurrentInteractionType + "}");
            vrLogger.StartLogging(sceneName);
        }

        public void GetParticipants(Action<List<Participant>> callback)
        {
            vrLogger.GetParticipants(callback);
        }
        
        public void SetParticipant(string participantId)
        {
            vrLogger.SetParticipant(participantId);
        }

        public void EndTracking()
        {
            vrLogger.StopLogging();
            
            vrLogger.SendActivity(response =>
            {
                Debug.Log(response);
            }, true);
        }
        
        private void OnTriggerEventEnter(string eventName)
        {
            Debug.Log("Kuk Trigger event enter: " + eventName);
            vrLogger.SetEvent(eventName);
        }

        // [ContextMenu("Test Tracking")]
        // public void TestTracking()
        // {
        //     StartCoroutine(TestTrackingCoroutine());
        // }

        // private IEnumerator TestTrackingCoroutine()
        // {
        //     StartTracking("Forest scene", );
        //     yield return new WaitForSecondsRealtime(1);
        //     EndTracking();
        // }

        public bool VRLoggerInitialized
        {
            get => _vrLoggerInitialized;
            set => _vrLoggerInitialized = value;
        }
        
        public List<VREventTrigger> EventTriggers
        {
            get => _eventTriggers;
            set
            {
                _eventTriggers = value;
                _eventTriggers.ForEach(trigger => trigger.onTriggerEnter.AddListener(OnTriggerEventEnter));
            }
        }
    }
}
