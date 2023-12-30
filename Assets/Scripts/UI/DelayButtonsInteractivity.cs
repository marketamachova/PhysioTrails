using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DelayButtonsInteractivity : MonoBehaviour
    {
        [SerializeField] private float delay = 1f;
        [SerializeField] private List<Button> buttons = new List<Button>();
        [SerializeField] private bool loadButtonsFromParent = false;
        [SerializeField] private Transform buttonsParent;

        private void OnEnable()
        {
            if (loadButtonsFromParent)
            {
                buttons.Clear();
                foreach (Transform child in buttonsParent)
                {
                    buttons.Add(child.GetComponent<Button>());
                }
            }
            StartCoroutine(DelayInteractivity());
        }

        private IEnumerator DelayInteractivity()
        {
            foreach (var button in buttons)
            {
                button.interactable = false;
            }
            
            yield return new WaitForSeconds(delay);
            
            foreach (var button in buttons)
            {
                button.interactable = true;
            }
        }
    }
}
