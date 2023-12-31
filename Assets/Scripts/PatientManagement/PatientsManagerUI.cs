using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Utils;
using VRLogger.Classes;

namespace PatientManagement_
{
    public class PatientsManagerUI : MonoSingleton<PatientsManagerUI>
    {
        [SerializeField] private GameObject patientPrefab;
        [SerializeField] private Transform patientsWrapperTransform;
        [SerializeField] private PatientUIItem currentPatientIndicator;
        
        public UnityEvent<string> onPatientChosen = new UnityEvent<string>();

        public void SetPatients(List<Participant> participants)
        {
            foreach (var participant in participants)
            {
                Debug.Log(participant.id);
                var patient = Instantiate(patientPrefab, patientsWrapperTransform);
                var uiItem = patient.GetComponent<PatientUIItem>();
                uiItem.PatientData = participant;
                uiItem.Index = participants.IndexOf(participant);
            }
        }
        
        public void ChoosePatient(Participant participant, int index)
        {
            currentPatientIndicator.PatientData = participant;
            currentPatientIndicator.Index = index;
            onPatientChosen.Invoke(participant.id);
        }
    }
}