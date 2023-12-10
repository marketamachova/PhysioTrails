using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SelectableButton : MonoBehaviour
    {
        [SerializeField] private Button button;

        [Header("Settings")] [SerializeField] private Color defaultColor = Color.white;
        [SerializeField] private Color selectedColor = Color.cyan;

        private bool _isSelected;
        private bool _selectedDefault;

        private void Start()
        {
            if (_selectedDefault)
            {
                IsSelected = true;
            }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                if (_isSelected)
                {
                    Select();
                }
                else
                {
                    Deselect();
                }
            }
        }

        public bool SelectedDefault
        {
            get => _selectedDefault;
            set
            {
                _selectedDefault = value;
                if (_selectedDefault)
                {
                    IsSelected = true;
                }
            }
        }

        private void Select()
        {
            ChangeColor(true);
        }

        private void Deselect()
        {
            ChangeColor(false);
        }

        private void ChangeColor(bool selected)
        {
            var colors = button.colors;
            colors.normalColor = selected ? selectedColor : defaultColor;
            button.colors = colors;
        }
    }
}
