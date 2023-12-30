using System;
using Analytics;
using UnityEngine;
using Utils;

namespace PatientManagement_
{
    public class PatientsManager : MonoSingleton<PatientsManager>
    {
        [SerializeField] private AnalyticsController analyticsController;
        [SerializeField] private PatientsManagerUI patientsManagerUI;

        public event Action OnPatientSelectionComplete;

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
            OnPatientSelectionComplete?.Invoke();
        }
    }
}
