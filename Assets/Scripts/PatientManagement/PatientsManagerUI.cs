using System;
using System.Collections.Generic;
using PatientManagement_;
using UnityEngine;
using UnityEngine.Events;
using Utils;
using VRLogger.Classes;

namespace PatientManagement
{
    public class PatientsManagerUI : MonoSingleton<PatientsManagerUI>
    {
        [SerializeField] private GameObject patientPrefab;
        [SerializeField] private Transform patientsWrapperTransform;
        [SerializeField] private PatientUIItem currentPatientIndicator;
        
        private List<Participant> _participants;
        
        public UnityEvent<string> onPatientChosen = new UnityEvent<string>();

        public void SetPatients(List<Participant> participants)
        {
            _participants = participants;
            
            foreach (var participant in participants)
            {
                var patient = Instantiate(patientPrefab, patientsWrapperTransform);
                var uiItem = patient.GetComponent<PatientUIItem>();
                uiItem.PatientData = participant;
                uiItem.Index = participants.IndexOf(participant);
            }
        }
        
        public void OnPatientChosen(string participantId)
        {
            var participantTuple = GetParticipant(participantId);
            currentPatientIndicator.PatientData = participantTuple.Item1;
            currentPatientIndicator.Index = participantTuple.Item2;
        }
        
        public void ChoosePatient(Participant participant, int index)
        {
            onPatientChosen.Invoke(participant.id);
        }
        
        public static Tuple<Participant, int> GetParticipant(string participantId)
        {
            var participant = Instance._participants.Find(p => p.id == participantId);
            var index = Instance._participants.IndexOf(participant);
            return new Tuple<Participant, int>(participant, index);
        }
    }
}
