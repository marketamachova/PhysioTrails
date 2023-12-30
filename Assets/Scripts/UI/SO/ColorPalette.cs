using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    [CreateAssetMenu(fileName = "ColorPalette", menuName = "UI/ColorPalette", order = 1)]
    public class ColorPalette : ScriptableObject
    {
        [SerializeField] private List<Color> colors;

        public List<Color> Colors => colors;
    }
}
