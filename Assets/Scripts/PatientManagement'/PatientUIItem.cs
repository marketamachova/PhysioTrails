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
            patientId.text = participant.id;
        }

        private void ChangeColor(int index)
        {
            backdrop.color = colorPalette.Colors[colorPalette.Colors.Count % index];
        }
        
        private void ChangeShape(int index)
        {
            shape.sprite = shapesPalette.Shapes[shapesPalette.Shapes.Count % index];
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
            }
        }
    }
}
