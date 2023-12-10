using System;
using System.Collections;
using System.Collections.Generic;
using Cart;
using Interactions;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class VRLobbyUIController : BaseUIController
    {
        [Header("References")]
        [SerializeField] private CartCreator cartCreator;
        [SerializeField] private InteractionConfigurator interactionConfigurator;
        [SerializeField] private GameObject interactionSelectionUI;
        [SerializeField] private GameObject sceneSelectionUI;
        [SerializeField] private List<Button> sceneSelectionButtons;

        [Header("Settings")]
        [SerializeField] private int enableButtonsDelaySeconds = 1;

        // private void Awake()
        // {
        //     cartCreator.OnCartCreatorCalibrationComplete += EnableInteractionSelection;
        //     interactionConfigurator.OnInteractionsConfigurationComplete += EnableSceneSelection;
        // }
        //
        // private void Start()
        // {
        //     sceneSelectionUI.SetActive(false);
        //     interactionSelectionUI.SetActive(false);
        // }
        //
        // private void EnableInteractionSelection()
        // {
        //     interactionSelectionUI.SetActive(true);
        // }
        //
        // private void EnableSceneSelection()
        // {
        //     sceneSelectionUI.SetActive(false);
        //     // StartCoroutine(EnableButtonsAfterDelayCoroutine(enableButtonsDelaySeconds));
        // }
        //
        // private IEnumerator EnableButtonsAfterDelayCoroutine(float delaySeconds = 1f)
        // {
        //     sceneSelectionButtons.ForEach(but => but.interactable = false);
        //     yield return new WaitForSecondsRealtime(enableButtonsDelaySeconds);
        //     sceneSelectionButtons.ForEach(but => but.interactable = true);
        // }
    }
}
