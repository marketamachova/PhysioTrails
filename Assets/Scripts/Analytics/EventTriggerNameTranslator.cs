using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using Utils;

namespace Analytics
{
    public class EventTriggerNameTranslator : MonoSingleton<EventTriggerNameTranslator>
    {
        private readonly Dictionary<string, string> _eventNameDictionary = new Dictionary<string, string> {
            {"Curve", "Zatáčka"},
            {"SteepCurve", "Příkrá zatáčka"},
            {"Bump", "Boule"},
            {"BridgeBump", "Most"},
            {"Goats", "Kozy"},
            {"Ducks", "Kachny"},
            {"SlowCurve", "Pomalá zatáčka"},
            {"Chicken", "Kuřata"},
            {"CurveUphill", "Zatáčka do kopce"},
            {"Downhill", "Sjezd"},
            {"DownhillCurve", "Sjezdová zatáčka"},
            {"Cows", "Krávy"},
            {"Deers", "Jeleni"},
            {"UphillStart", "Začátek cesty nahoru"},
            {"UphillMiddle", "Uprostřed cesty nahoru"},
            {"UphillEnd", "Konec cesty nahoru"},
            {"DownHillStart", "Začátek cesty dolů"},
            {"DownHillMiddle", "Uprostřed cesty dolů"},
            {"DownHillEnd", "Konec cesty dolů"},
            {"VegetationClose", "Blízká vegetace"},
            {"SteepDownhillStart", "Začátek prudkého sjezdu"},
            {"SteepDownhillEnd", "Konec prudkého sjezdu"},
            {"SteepUphillMiddle", "Uprostřed prudkého stoupání"},
            {"SteepUphillCurve", "Zatáčka prudkého stoupání"},
            {"SteepUphillEnd", "Konec prudkého stoupání"},
            {"Rabbit", "Králík"},
            {"Butterfly", "Motýl"},
            {"LongDownhillCurveSmall", "Dlouhá sjezdová zatáčka"},
            {"LongDownhillMiddle", "Uprostřed dlouhého sjezdu"},
            {"DownhillEndCurve", "Konec sjezdu"},
        };

        public string TranslateEventName(string eventName)
        {
            if (_eventNameDictionary.TryGetValue(eventName, out string readableName))
            {
                return readableName;
            }
            Debug.LogWarning("Event name not found in dictionary: " + eventName);
            return eventName;
        }
    }
}
