using System;
using UnityEngine;

namespace VRLogger.Classes
{
    [Serializable]
    public class Participant
    {
        public string id; 
        public string nickname;
        
        
        public override string ToString()
        {
            return string.Format("[Participant: id={0}, nickname={1}]", id, nickname);
        }
    }
}
