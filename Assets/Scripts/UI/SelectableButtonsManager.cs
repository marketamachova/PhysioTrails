using UnityEngine;

namespace UI
{
    public class SelectableButtonsManager : MonoBehaviour
    {
        [SerializeField] private SelectableButton[] buttons;
        [SerializeField] private int defaultSelectedButtonIndex = 0;

        private void Start()
        {
            buttons[defaultSelectedButtonIndex].SelectedDefault = true;
            SelectButton(defaultSelectedButtonIndex);
        }

        public void SelectButton(int index)
        {
            foreach (var button in buttons)
            {
                button.IsSelected = false;
            }
            buttons[index].IsSelected = true;
        }
    }
}
