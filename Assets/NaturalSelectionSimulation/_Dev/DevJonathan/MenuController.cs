using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace NaturalSelectionSimulation
{
    public class MenuController : MonoBehaviour
    {
        #region Display Settings

        [Header("Screen Mode Dropdown")]
        private TMP_Dropdown screenModeDropdown;

        #endregion

        private void Start()
        {
            //screenModeDropdown.ClearOptions();

            //var screenModes = new List<string>()
            //{
            //    "Fullscreen",
            //    "Windowed",
            //    "Borderless"
            //};

            //screenModeDropdown.AddOptions(screenModes);
            //screenModeDropdown.value = 0;
            //screenModeDropdown.RefreshShownValue();
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}
