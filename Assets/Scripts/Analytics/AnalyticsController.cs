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
        private bool _tracking;
        
        // TODO have list of AnalyticsTriggers - which each have collider and name and then subsribe here when the collider hits to send event to vrLogger

        private void Start()
        {
            Debug.Log("Kuk AnalyticsController start"); 
            InitializeVrLogger();
        }

        public void InitializeVrLogger()
        {
            vrLogger.SetOrganisation("13FgFz");
        }

        public void StartTracking(string sceneName, List<VREventTrigger> eventTriggers)
        {
            if (_tracking)
            {
                return;
            }
            
            Debug.Log("vrLogger " + vrLogger.name);
            vrLogger.InitializeLogger();

            EventTriggers = eventTriggers;
            
            // TODO add other data, dont know which now
            vrLogger.SetCustomData("{\"custom\": " + interactionManager.CurrentInteractionType + "}");
            vrLogger.StartLogging(sceneName);
            
            _vrLoggerInitialized = true;
            _tracking = true;
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
            if (!_tracking)
            {
                return;
            }
            
            _tracking = false;
            vrLogger.StopLogging();

#if UNITY_EDITOR
            vrLogger.SendActivity(response =>
            {
                Debug.Log(response);
            }, false);
#else
            vrLogger.SendActivity(response =>
            {
                Debug.Log(response);
            }, true);
#endif
        }
        
        private void OnTriggerEventEnter(string eventName)
        {
            Debug.Log("Kuk Trigger event enter: " + eventName);
            var translatedEventName = EventTriggerNameTranslator.Instance.TranslateEventName(eventName);
            // vrLogger.SetRecordCustomData("{\"custom\": \"" + translatedEventName + "\"}");
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
                if (_eventTriggers != null && _eventTriggers.Count > 0)
                {
                    _eventTriggers.ForEach(trigger => trigger.onTriggerEnter.AddListener(OnTriggerEventEnter));
                }
            }
        }
    }
}
