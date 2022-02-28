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
        private List<Resolution> resolutions = new List<Resolution>();

        [Header("Aspect Ratio Dropdown")]
        public TMP_Dropdown aspectRatioDropdown;
        Dictionary<int, string> aspectRatios = new Dictionary<int, string>()
        {
            { 0, "16:9" },
            { 1, "4:3" }
        };

        #endregion

        private void Start()
        {
            InitializeScreenModeDropdown();
            InitializeResolution();
            InitializeAspectRatios();
        }

        private void InitializeAspectRatios()
        {
            aspectRatioDropdown.ClearOptions();

            aspectRatioDropdown.AddOptions(aspectRatios.OrderBy(x => x.Key).Select(x => x.Value).ToList());
            aspectRatioDropdown.value = isSixteenNineAspect(resolutions[_resolutionIndex].width, resolutions[_resolutionIndex].height) ? 0 : 1;
            aspectRatioDropdown.RefreshShownValue();
        }

        public void SetAspectRatio(int aspectRatioIndex)
        {
            var resolution = resolutions.Where(r => (aspectRatioIndex == 0 && isSixteenNineAspect(r.width, r.height)) 
                    || (aspectRatioIndex == 1 && isFourThreeAspect(r.width, r.height))).OrderBy(r => Math.Abs(_resolutionIndex - resolutions.IndexOf(r))).FirstOrDefault();

            _resolutionIndex = resolutions.IndexOf(resolution);
            resolutionDropdown.value = _resolutionIndex;
            resolutionDropdown.RefreshShownValue();
        }

        private void InitializeResolution()
        {
            var screenResolutions = Screen.resolutions;
            resolutionDropdown.ClearOptions();

            List<string> options = new List<string>();
            int currentResolutionIndex = 0;
            int counter = 0;

            for (int i = 0; i < screenResolutions.Length; i++)
            {
                if (isFourThreeAspect(screenResolutions[i].width, screenResolutions[i].height) || isSixteenNineAspect(screenResolutions[i].width, screenResolutions[i].height))
                {
                    resolutions.Add(screenResolutions[i]);
                    var option = $"{screenResolutions[i].width} x {screenResolutions[i].height}";
                    options.Add(option);

                    if (screenResolutions[i].width == Screen.width && screenResolutions[i].height == Screen.height)
                    {
                        currentResolutionIndex = counter;
                    }
                    counter++;
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

            aspectRatioDropdown.value = isSixteenNineAspect(resolution.width, resolution.height) ? 0 : 1;
            aspectRatioDropdown.RefreshShownValue();
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
                PlayerPrefs.SetInt("masterFullscreen", 0);
            }

            resolutionDropdown.enabled = screenModes[_screenModeIndex] != "Fullscreen";
            aspectRatioDropdown.enabled = screenModes[_screenModeIndex] != "Fullscreen";

            screenModeDropdown.RefreshShownValue();
        }

        public void SetScreenMode(int screenMode)
        {
            _screenModeIndex = screenMode;
            PlayerPrefs.SetInt("masterFullscreen", screenMode);
            Screen.fullScreen = screenModes[screenMode] == "Fullscreen";

            resolutionDropdown.enabled = screenModes[_screenModeIndex] != "Fullscreen";
            aspectRatioDropdown.enabled = screenModes[_screenModeIndex] != "Fullscreen";

            if (screenModes[screenMode] == "Fullscreen")
            {
                var resolution = resolutions.First(r => r.width == Screen.width && r.height == Screen.height);
                _resolutionIndex = resolutions.IndexOf(resolution);
                resolutionDropdown.value = _resolutionIndex;
                Screen.SetResolution(resolution.width, resolution.height, screenModes[_screenModeIndex] == "Fullscreen");
                resolutionDropdown.RefreshShownValue();

                aspectRatioDropdown.value = isSixteenNineAspect(resolution.width, resolution.height) ? 0 : 1;
                aspectRatioDropdown.RefreshShownValue();
            }
            else if(screenModes[screenMode] == "Windowed")
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
