using Cart;
using Interactions;
using Player;
using UnityEngine;

namespace Utils
{
    public class FlowSkipper : MonoBehaviour
    {
        [SerializeField] private CartCreator cartCreator;
        [SerializeField] private VRLobbyController vrLobbyController;
        [SerializeField] private InteractionConfigurator interactionConfigurator;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                cartCreator.SkipCalibration();
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
        }
    }
}