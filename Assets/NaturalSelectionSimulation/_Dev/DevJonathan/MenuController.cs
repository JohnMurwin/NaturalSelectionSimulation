using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

namespace NaturalSelectionSimulation
{
    public class MenuController : MonoBehaviour
    {
        #region Display Settings

        [Header("Screen Mode Dropdown")]
        public TMP_Dropdown screenModeDropdown;
        private int _screenModeIndex;
        Dictionary<int, string> screenModes = new Dictionary<int, string>()
        {
            { 0, "Fullscreen" },
            { 1, "Windowed" },
            { 2, "Borderless" }
        };

        [Header("Resolution Dropdown")]
        public TMP_Dropdown resolutionDropdown;
        private int _resolutionIndex;
        private Resolution[] resolutions;

        #endregion

        private void Start()
        {
            InitializeResolution();
            InitializeScreenModeDropdown();
        }

        private void InitializeResolution()
        {
            resolutions = Screen.resolutions;
            resolutionDropdown.ClearOptions();

            List<string> options = new List<string>();
            int currentResolutionIndex = 0;

            for (int i = 0; i < resolutions.Length; i++)
            {
                if (isFourThreeAspect(resolutions[i].width, resolutions[i].height) || isSixteenNineAspect(resolutions[i].width, resolutions[i].height))
                {
                    var option = $"{resolutions[i].width} x {resolutions[i].height}";
                    options.Add(option);

                    if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
                    {
                        currentResolutionIndex = i;
                    }
                }
            }

            resolutionDropdown.AddOptions(options);
            _resolutionIndex = currentResolutionIndex;
            resolutionDropdown.value = _resolutionIndex;
            resolutionDropdown.RefreshShownValue();
        }

        public void SetResolution(int resolutionIndex)
        {
            _resolutionIndex = resolutionIndex;
            var resolution = resolutions[_resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, screenModes[_screenModeIndex] == "Fullscreen");
        }

        private void InitializeScreenModeDropdown()
        {
            screenModeDropdown.ClearOptions();

            screenModeDropdown.AddOptions(screenModes.OrderBy(s => s.Key).Select(s => s.Value).ToList());

            if (screenModes.ContainsKey(PlayerPrefs.GetInt("masterFullscreen", -1)))
            {
                _screenModeIndex = PlayerPrefs.GetInt("masterFullscreen");
                screenModeDropdown.value = _screenModeIndex;
            }
            else
            {
                _screenModeIndex = 0;
                screenModeDropdown.value = 0;
            }

            screenModeDropdown.RefreshShownValue();
        }

        public void SetScreenMode(int screenMode)
        {
            _screenModeIndex = screenMode;
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

        public bool isFourThreeAspect(int width, int height)
        {
            int factor = gcd(width, height);
            int wFactor = width / factor;
            int hFactor = height / factor;

            if (wFactor == 4 && hFactor == 3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool isSixteenNineAspect(int width, int height)
        {
            int factor = gcd(width, height);
            int wFactor = width / factor;
            int hFactor = height / factor;

            if (wFactor == 16 && hFactor == 9)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int gcd(int a, int b)
        {
            return (b == 0) ? a : gcd(b, a % b);
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}
