using UnityEngine;

namespace Analytics.SO
{
    [CreateAssetMenu(fileName = "EventTriggerNamesMapping", menuName = "Custom/Event Names Mapping")]
    public class EventTriggerName : ScriptableObject
    {
        public string EventName;
        public string ReadableName;
    }
}
