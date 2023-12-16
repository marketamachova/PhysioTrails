using System;
using TMPro;
using UI.SO;
using UnityEngine;
using UnityEngine.UI;
using VRLogger.Classes;
using ColorPalette = UI.ColorPalette;

namespace PatientManagement_
{
    public class PatientUIItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI patientName;
        [SerializeField] private TextMeshProUGUI patientId;
        [SerializeField] private Image backdrop;
        [SerializeField] private Image shape;
        [SerializeField] private Button button;
        [SerializeField] private ColorPalette colorPalette;
        [SerializeField] private ShapesPalette shapesPalette;
        
        [SerializeField] private bool isButton;

        
        private Participant _patientData;
        private int _index;

        private void Awake()
        {
            if (isButton)
            {
                button.onClick.AddListener(OnPatientChosen);
            }
        }

        private void OnPatientChosen()
        {
            PatientsManagerUI.Instance.ChoosePatient(_patientData);
        }

        private void UpdateUI(Participant participant)
        {
            patientName.text = participant.nickname;
        } 
        
        private void UpdateUIIndex(int index)
        {
            patientId.text = (index + 1).ToString();
        }

        private void ChangeColor(int index)
        {
            if (index > 0)
            {
                backdrop.color = colorPalette.Colors[index % colorPalette.Colors.Count];
            }
            else
            {
                backdrop.color = colorPalette.Colors[0]; 
            }
            shape.color = colorPalette.Colors[(index + 2) % colorPalette.Colors.Count];
        }
        
        private void ChangeShape(int index)  
        {
            if (index > 0)
            {
                shape.sprite = shapesPalette.Shapes[index % shapesPalette.Shapes.Count];
            }
            else
            {
                shape.sprite = shapesPalette.Shapes[0];
            }
        }

        [ContextMenu("Click")]
        public void Click()
        {
            OnPatientChosen();
        }

        public Participant PatientData
        {
            get => _patientData;
            set
            {
                _patientData = value;
                UpdateUI(_patientData);
            }
        }

        public int Index
        {
            get => _index;
            set
            {
                _index = value;
                ChangeColor(_index);
                ChangeShape(_index);
                UpdateUIIndex(_index);
            }
        }
    }
}
