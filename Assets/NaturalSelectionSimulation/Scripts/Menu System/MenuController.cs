using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace NaturalSelectionSimulation
{
    public class MenuController : MonoBehaviour
    {
        #region Options Menu Members/Properties

        [Header("Option Tabs")]
        public GameObject DisplayOptionsView;
        public GameObject AudioOptionsView;
        public GameObject ControlsOptionsView;

        #endregion

        #region Display Settings Members/Properties

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

        #region Audio Settings Members/Properties

        [Header("Master Volume Setting")]
        public Text masterSliderValue = null;
        public Slider masterSlider = null;

        #endregion

        #region Controls Settings Members/Properties

        [Header("Invert Y Toggle")]
        public Toggle invertYToggle = null;

        [Header("Mouse Sensitivity Setting")]
        public Text mouseSensitivitySliderValue = null;
        public Slider mouseSensitivitySlider = null;

        #endregion

        private void Start()
        {
            InitializeScreenModeDropdown();
            InitializeResolution();
            InitializeAspectRatios();
            InitializeMasterVolume();
            InitializeMouseSensitivity();
            InitializeInvertYToggle();
        }

        #region Display Settings Methods

        public void DisplayTabClick()
        {
            DisplayOptionsView.SetActive(true);
            AudioOptionsView.SetActive(false);
            ControlsOptionsView.SetActive(false);
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

        #endregion

        #region Audio Settings Methods

        public void AudioTabClick()
        {
            DisplayOptionsView.SetActive(false);
            AudioOptionsView.SetActive(true);
            ControlsOptionsView.SetActive(false);
        }

        private void InitializeMasterVolume()
        {
            SetMasterVolume(PlayerPrefs.GetFloat("masterVolume", -1) != -1 ? PlayerPrefs.GetFloat("masterVolume") : 50);
        }

        public void SetMasterVolume(float volume)
        {
            AudioListener.volume = volume;
            masterSlider.value = volume;
            masterSliderValue.text = volume.ToString("0");

            PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
        }

        #endregion

        #region Controls Settings Methods

        public void ControlsTabClick()
        {
            DisplayOptionsView.SetActive(false);
            AudioOptionsView.SetActive(false);
            ControlsOptionsView.SetActive(true);
        }

        private void InitializeMouseSensitivity()
        {
            SetMouseSensitivity(PlayerPrefs.GetFloat("masterMouseSensitivity", -1) != -1 ? PlayerPrefs.GetFloat("masterMouseSensitivity") : 1);
        }

        public void SetMouseSensitivity(float sensitivity)
        {
            mouseSensitivitySlider.value = sensitivity;
            mouseSensitivitySliderValue.text = sensitivity.ToString("0");

            PlayerPrefs.SetFloat("masterMouseSensitivity", sensitivity);
        }

        private void InitializeInvertYToggle()
        {
            invertYToggle.isOn = PlayerPrefs.GetInt("masterInvertY", -1) == 1;
            InvertYControls(invertYToggle.isOn);
        }

        public void InvertYToggled()
        {
            PlayerPrefs.SetInt("masterInvertY", invertYToggle.isOn ? 1 : 0);
            InvertYControls(invertYToggle.isOn);
        }

        private void InvertYControls(bool invert)
        {
            //Handle Controls
        }

        #endregion

        public void ExitGame()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }

        #region SceneNavigationRegion

        public void LoadSimulationScene()
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }

        public void MainMenuScene()
        {
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }

        #endregion
    }
}
