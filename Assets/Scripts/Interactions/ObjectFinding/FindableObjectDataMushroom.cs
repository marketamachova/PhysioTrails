using UnityEngine;

namespace Interactions.ObjectFinding
{
    [CreateAssetMenu(fileName = "FindableObjectDataMushrooms", menuName = "FindableObjectData Mushrooms")]
    public class FindableObjectDataMushroom : FindableObjectData
    {
        public enum MushroomType
        {
            Edible,
            Inedible,
            Poisonous
        }
    }
}
