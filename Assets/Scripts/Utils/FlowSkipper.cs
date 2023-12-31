using Cart;
using Interactions;
using PatientManagement;
using PatientManagement_;
using Player;
using UnityEngine;

namespace Utils
{
    public class FlowSkipper : MonoBehaviour
    {
        [SerializeField] private CartCreator cartCreator;
        [SerializeField] private VRLobbyController vrLobbyController;
        [SerializeField] private InteractionConfigurator interactionConfigurator;
        [SerializeField] private PatientsManager patientsManager;

        [SerializeField] private string testPatientId = "9e7ab45e-546d-4d6f-b78b-70f81f4bdfca";
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                cartCreator.SkipCalibration();
            }
            else if (Input.GetKeyDown(KeyCode.P))
            {
                patientsManager.SetPatient(testPatientId);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                interactionConfigurator.Submit();
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                vrLobbyController.OnSceneSelected("WinterScene");
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                vrLobbyController.OnSceneSelected("MainScene");
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                vrLobbyController.OnSceneSelected("RuralScene");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                interactionConfigurator.SetInteractionTypeWireLoop();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                interactionConfigurator.SetInteractionTypeObjectFinding();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                interactionConfigurator.SetInteractionTypeAvoidObstacles();
            }
        }
    }
}