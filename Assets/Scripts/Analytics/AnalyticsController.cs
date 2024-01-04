using System;
using System.Collections.Generic;
using Interactions;
using Interactions.AvoidObstacles;
using Interactions.ObjectFinding;
using Newtonsoft.Json;
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
        private List<FindableObject> _findableObjects;
        private List<AvoidableObstacle> _avoidableObstacles;
        
        private bool _vrLoggerInitialized;
        private bool _tracking;
        
        // TODO have list of AnalyticsTriggers - which each have collider and name and then subsribe here when the collider hits to send event to vrLogger

        public class InteractionData
        {
            public string InteractionType { get; set; }
            public string Difficulty { get; set; }
            public string HandType { get; set; }
        }
        
        private void Start()
        {
            InitializeVrLogger();
        }

        private void SendInteractionData()
        {
            var data = new InteractionData
            {
                InteractionType = interactionManager.CurrentInteractionType.ToString(),
                Difficulty = interactionManager.CurrentDifficulty.ToString(),
                HandType = interactionManager.CurrentHandType.ToString()
            };


            var jsonString = JsonConvert.SerializeObject(data);
            Debug.Log("Kuk Interaction data: " + jsonString);
            vrLogger.SetCustomData(jsonString);
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
            
            vrLogger.InitializeLogger();
            SendInteractionData();

            EventTriggers = eventTriggers;

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
            Debug.Log("Kuk End tracking. Current score: " + interactionManager.CurrentScoreController.CurrentScore1);
            vrLogger.SetRecordCustomData("{\"score\": " + 2 + "}");
            
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
        
        public void OnTriggerEventEnter(string eventName)
        {
            Debug.Log("Kuk Trigger event enter: " + eventName);
            var translatedEventName = EventTriggerNameTranslator.Instance.TranslateEventName(eventName);
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
        
        public List<FindableObject> FindableObjects
        {
            get => _findableObjects;
            set
            {
                _findableObjects = value;
                if (_findableObjects != null && _findableObjects.Count > 0)
                {
                    _findableObjects.ForEach(obj => obj.onObjectFound.AddListener(OnTriggerEventEnter));
                }
            }
        }
        
        public List<AvoidableObstacle> AvoidableObstacles
        {
            get => _avoidableObstacles;
            set
            {
                _avoidableObstacles = value;
                if (_avoidableObstacles != null && _avoidableObstacles.Count > 0)
                {
                    _avoidableObstacles.ForEach(obj => obj.onHit.AddListener(OnTriggerEventEnter));
                }
            }
        }
    }
}
