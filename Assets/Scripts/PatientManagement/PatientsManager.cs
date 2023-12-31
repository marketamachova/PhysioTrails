using System;
using Analytics;
using PatientManagement_;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Utils;

namespace PatientManagement
{
    public class PatientsManager : MonoSingleton<PatientsManager>
    {
        [SerializeField] private AnalyticsController analyticsController;
        [SerializeField] private PatientsManagerUI patientsManagerUI;

        public UnityEvent<string> onPatientSelectionComplete = new UnityEvent<string>();

        private void Awake()
        {
            patientsManagerUI.onPatientChosen.AddListener(SetPatient);
        }

        private void Start()
        {
            analyticsController.InitializeVrLogger();
            GetAllPatients();
        }

        private void GetAllPatients()
        {  
            analyticsController.GetParticipants((participants =>
            {
                if (participants == null)
                {
                    Debug.LogError("Participants are null");
                    return;
                }
                patientsManagerUI.SetPatients(participants);
            }));
        }
        
        public void SetPatient(string participantId)
        {
            analyticsController.SetParticipant(participantId);
            patientsManagerUI.OnPatientChosen(participantId);
            onPatientSelectionComplete?.Invoke(participantId);
        }
    }
}
