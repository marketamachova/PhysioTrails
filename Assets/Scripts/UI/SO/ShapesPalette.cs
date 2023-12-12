using System.Collections.Generic;
using UnityEngine;

namespace UI.SO
{
    [CreateAssetMenu(fileName = "ShapesPalette", menuName = "UI/ShapesPalette", order = 0)]
    public class ShapesPalette : ScriptableObject
    {
        [SerializeField] private List<Sprite> shapes;

        public List<Sprite> Shapes => shapes;
    }
}
