using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CartCreationUIController : BaseUIController
    {
        [SerializeField] private Image backdrop;
        
        public void DisplayCalibrationStep1()
        {
            backdrop.enabled = true;
            EnablePanelExclusive(UIConstants.Step1);
            EnableFalse(UIConstants.DoneButton);
        }

        public void DisplayCalibrationStep2()
        {
            backdrop.enabled = true;
            EnablePanelExclusive(UIConstants.Step2);
        }

        public void DisplayCalibrationStep3()
        {
            backdrop.enabled = false;
            EnablePanelExclusive(UIConstants.Step3);
            EnableTrue(UIConstants.DoneButton);
        }
    }
}
