using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        public TMP_Dropdown screenModeDropdown;
        private int _screenMode;
        Dictionary<int, string> screenModes = new Dictionary<int, string>()
        {
            { 0, "Fullscreen" },
            { 1, "Windowed" },
            { 2, "Borderless" }
        };

        #endregion

        private void Start()
        {
            InitializeScreenModeDropdown();
        }

        private void InitializeScreenModeDropdown()
        {
            screenModeDropdown.ClearOptions();

            screenModeDropdown.AddOptions(screenModes.OrderBy(s => s.Key).Select(s => s.Value).ToList());

            if (screenModes.ContainsKey(PlayerPrefs.GetInt("masterFullscreen", -1)))
            {
                _screenMode = PlayerPrefs.GetInt("masterFullscreen");
                screenModeDropdown.value = _screenMode;
            }
            else
            {
                _screenMode = 0;
                screenModeDropdown.value = 0;
            }

            screenModeDropdown.RefreshShownValue();
        }

        public void SetScreenMode(int screenMode)
        {
            _screenMode = screenMode;
            PlayerPrefs.SetInt("masterFullscreen", screenMode);
            Screen.fullScreen = screenModes[screenMode] == "Fullscreen";

            if(screenModes[screenMode] == "Windowed")
            {
                Screen.fullScreenMode = FullScreenMode.Windowed;
            }
            else if (screenModes[screenMode] == "Borderless")
            {
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            }
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}
