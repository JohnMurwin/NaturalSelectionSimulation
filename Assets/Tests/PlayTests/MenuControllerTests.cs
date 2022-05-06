using System;
using System.Collections;
using System.Collections.Generic;
using NaturalSelectionSimulation;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace NSS.PlayTests
{
    public class MenuControllerTests
    {
        [UnityTest]
        public IEnumerator DisplayOptionsView_WillBeSetActive_WhenDisplayTabIsClicked()
        {
            var gameObject = new GameObject();
            var menuController = gameObject.AddComponent<MenuController>();
            SetUpMenuController(menuController);
            menuController.DisplayOptionsView.SetActive(false);

            menuController.DisplayTabClick();

            yield return null;

            Assert.AreEqual(menuController.DisplayOptionsView.activeSelf, true);
        }
        
        [UnityTest]
        public IEnumerator AudioOptionsView_WillBeSetInactive_WhenDisplayTabIsClicked()
        {
            var gameObject = new GameObject();
            var menuController = gameObject.AddComponent<MenuController>();
            SetUpMenuController(menuController);
            menuController.AudioOptionsView.SetActive(true);

            menuController.DisplayTabClick();

            yield return null;

            Assert.AreEqual(menuController.AudioOptionsView.activeSelf, false);
        }

        [UnityTest]
        public IEnumerator ControlsOptionsView_WillBeSetInactive_WhenDisplayTabIsClicked()
        {
            var gameObject = new GameObject();
            var menuController = gameObject.AddComponent<MenuController>();
            SetUpMenuController(menuController);
            menuController.ControlsOptionsView.SetActive(true);

            menuController.DisplayTabClick();

            yield return null;

            Assert.AreEqual(menuController.ControlsOptionsView.activeSelf, false);
        }

        [UnityTest]
        public IEnumerator DisplayOptionsView_WillBeSetInactive_WhenAudioTabIsClicked()
        {
            var gameObject = new GameObject();
            var menuController = gameObject.AddComponent<MenuController>();
            SetUpMenuController(menuController);
            menuController.DisplayOptionsView.SetActive(true);

            menuController.AudioTabClick();

            yield return null;

            Assert.AreEqual(menuController.DisplayOptionsView.activeSelf, false);
        }

        [UnityTest]
        public IEnumerator AudioOptionsView_WillBeSetActive_WhenAudioTabIsClicked()
        {
            var gameObject = new GameObject();
            var menuController = gameObject.AddComponent<MenuController>();
            SetUpMenuController(menuController);
            menuController.AudioOptionsView.SetActive(false);

            menuController.AudioTabClick();

            yield return null;

            Assert.AreEqual(menuController.AudioOptionsView.activeSelf, true);
        }

        [UnityTest]
        public IEnumerator ControlsOptionsView_WillBeSetInactive_WhenAudioTabIsClicked()
        {
            var gameObject = new GameObject();
            var menuController = gameObject.AddComponent<MenuController>();
            SetUpMenuController(menuController);
            menuController.ControlsOptionsView.SetActive(true);

            menuController.AudioTabClick();

            yield return null;

            Assert.AreEqual(menuController.ControlsOptionsView.activeSelf, false);
        }

        [UnityTest]
        public IEnumerator DisplayOptionsView_WillBeSetInactive_WhenControlsTabIsClicked()
        {
            var gameObject = new GameObject();
            var menuController = gameObject.AddComponent<MenuController>();
            SetUpMenuController(menuController);
            menuController.DisplayOptionsView.SetActive(true);

            menuController.ControlsTabClick();

            yield return null;

            Assert.AreEqual(menuController.DisplayOptionsView.activeSelf, false);
        }

        [UnityTest]
        public IEnumerator AudioOptionsView_WillBeSetInactive_WhenControlsTabIsClicked()
        {
            var gameObject = new GameObject();
            var menuController = gameObject.AddComponent<MenuController>();
            SetUpMenuController(menuController);
            menuController.AudioOptionsView.SetActive(true);

            menuController.ControlsTabClick();

            yield return null;

            Assert.AreEqual(menuController.AudioOptionsView.activeSelf, false);
        }

        [UnityTest]
        public IEnumerator ControlsOptionsView_WillBeSetActive_WhenControlsTabIsClicked()
        {
            var gameObject = new GameObject();
            var menuController = gameObject.AddComponent<MenuController>();
            SetUpMenuController(menuController);
            menuController.ControlsOptionsView.SetActive(false);

            menuController.ControlsTabClick();

            yield return null;

            Assert.AreEqual(menuController.ControlsOptionsView.activeSelf, true);
        }

        [UnityTest]
        public IEnumerator MasterVolumePlayerPref_WillBeSetToPassedInValue_WhenSetMasterVolumeIsCalled()
        {
            var gameObject = new GameObject();
            var menuController = gameObject.AddComponent<MenuController>();
            SetUpMenuController(menuController);

            menuController.SetMasterVolume(75f);

            yield return null;

            Assert.AreEqual(PlayerPrefs.GetFloat("masterVolume"), 75f);
        }

        [UnityTest]
        public IEnumerator MasterMouseSensitivityPlayerPref_WillBeSetToPassedInValue_WhenSetMouseSensitivityIsCalled()
        {
            var gameObject = new GameObject();
            var menuController = gameObject.AddComponent<MenuController>();
            SetUpMenuController(menuController);

            menuController.SetMouseSensitivity(3f);

            yield return null;

            Assert.AreEqual(PlayerPrefs.GetFloat("masterMouseSensitivity"), 3f);
        }

        [UnityTest]
        public IEnumerator MasterInvertYPlayerPref_WillBeSetToZero_WhenInvertYToggleIsOffAndInvertYToggledIsCalled()
        {
            var gameObject = new GameObject();
            var menuController = gameObject.AddComponent<MenuController>();
            SetUpMenuController(menuController);
            menuController.invertYToggle.isOn = false;

            menuController.InvertYToggled();

            yield return null;

            Assert.AreEqual(PlayerPrefs.GetInt("masterInvertY"), 0);
        }

        [UnityTest]
        public IEnumerator MasterInvertYPlayerPref_WillBeSetToOne_WhenInvertYToggleIsOnAndInvertYToggledIsCalled()
        {
            var gameObject = new GameObject();
            var menuController = gameObject.AddComponent<MenuController>();
            SetUpMenuController(menuController);
            menuController.invertYToggle.isOn = true;

            menuController.InvertYToggled();

            yield return null;

            Assert.AreEqual(PlayerPrefs.GetInt("masterInvertY"), 1);
        }

        private void SetUpMenuController(MenuController menuController)
        {
            var displayOptionsViewGameObject = new GameObject();
            var audioOptionsViewGameObject = new GameObject();
            var controlsOptionsViewGameObject = new GameObject();

            menuController.DisplayOptionsView = displayOptionsViewGameObject;
            menuController.AudioOptionsView = audioOptionsViewGameObject;
            menuController.ControlsOptionsView = controlsOptionsViewGameObject;

            var screenModeDropdownGameObject = new GameObject();
            var screenModeDropdown = screenModeDropdownGameObject.AddComponent<TMP_Dropdown>();

            var resolutionDropdownGameObject = new GameObject();
            var resolutionDropdown = resolutionDropdownGameObject.AddComponent<TMP_Dropdown>();

            var aspectRatioDropdownGameObject = new GameObject();
            var aspectRatioDropdown = aspectRatioDropdownGameObject.AddComponent<TMP_Dropdown>();

            menuController.screenModeDropdown = screenModeDropdown;
            menuController.resolutionDropdown = resolutionDropdown;
            menuController.aspectRatioDropdown = aspectRatioDropdown;

            var masterSliderValueGameObject = new GameObject();
            var masterSliderValue = masterSliderValueGameObject.AddComponent<Text>();

            var masterSliderGameObject = new GameObject();
            var masterSlider = masterSliderGameObject.AddComponent<Slider>();

            menuController.masterSliderValue = masterSliderValue;
            menuController.masterSlider = masterSlider;

            var invertYToggleGameObject = new GameObject();
            var invertYToggle = invertYToggleGameObject.AddComponent<Toggle>();

            menuController.invertYToggle = invertYToggle;

            var mouseSensitivitySliderValueGameObject = new GameObject();
            var mouseSensitivitySliderValue = mouseSensitivitySliderValueGameObject.AddComponent<Text>();

            var mouseSensitivitySliderGameObject = new GameObject();
            var mouseSensitivitySlider = mouseSensitivitySliderGameObject.AddComponent<Slider>();

            menuController.mouseSensitivitySliderValue = mouseSensitivitySliderValue;
            menuController.mouseSensitivitySlider = mouseSensitivitySlider;
        }
    }
}
