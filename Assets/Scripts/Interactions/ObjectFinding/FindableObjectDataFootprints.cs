using UnityEngine;

namespace Interactions.ObjectFinding
{
    [CreateAssetMenu(fileName = "FindableObjectDataFootprints", menuName = "FindableObjectData Footprints")]
    public class FindableObjectDataFootprints : FindableObjectData
    {
        public enum FootprintType
        {
            Carnivore,
            Herbivore,
            Omnivore,
        }
    }
}
