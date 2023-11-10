using System;
using System.Collections;
using System.Collections.Generic;
using Cart;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class VRLobbyUIController : BaseUIController
    {
        [SerializeField] private CartCreator cartCreator;
        [SerializeField] private List<Button> sceneSelectionButtons;

        [SerializeField] private int enableButtonsDelaySeconds = 1;

        private void Awake()
        {
            cartCreator.OnCartCreatorCalibrationComplete += EnableButtonsAfterDelay;
        }

        private void EnableButtonsAfterDelay()
        {
            StartCoroutine(EnableButtonsAfterDelayCoroutine());
        }

        private IEnumerator EnableButtonsAfterDelayCoroutine()
        {
            sceneSelectionButtons.ForEach(but => but.interactable = false);
            yield return new WaitForSecondsRealtime(enableButtonsDelaySeconds);
            sceneSelectionButtons.ForEach(but => but.interactable = true);
        }
    }
}
