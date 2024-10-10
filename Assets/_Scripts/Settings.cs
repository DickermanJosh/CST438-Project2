using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;

namespace _Scripts
{
    public class Settings : MonoBehaviour
    {
        // UI elements for different settings
        public Button englishButton;
        public Button spanishButton;
        public Button twoHandedButton;
        public Button leftHandedButton;
        public Button rightHandedButton;
        public Button colorBlindButton;
        public Slider masterAudioSlider;

        // Temporary variables for settings changes
        private float _tempVolume;
        private int _tempLocale;
        private int _tempScheme;
        private bool _isColorBlind;

        void Start()
        {
            // Load current settings from GameManager into temp variables and UI
            _tempVolume = GameManager.instance.volume;
            _tempLocale = GameManager.instance.locale;
            _tempScheme = GameManager.instance.controlScheme;
            _isColorBlind = GameManager.instance.isColorBlind;

            // Set UI based on loaded settings
            masterAudioSlider.value = _tempVolume;
            UpdateLocaleButtons(_tempLocale);
            UpdateControlSchemeButtons(_tempScheme);
            UpdateColorBlindButton(_isColorBlind);
        }

        public void ChangeControlScheme(int schemeNum)
        {
            _tempScheme = schemeNum; // Update temp value
            UpdateControlSchemeButtons(schemeNum); // Update UI appearance
        }

        private void UpdateControlSchemeButtons(int schemeNum)
        {
            // Enable/disable buttons visually based on the selected scheme
            twoHandedButton.interactable = schemeNum != 0;
            leftHandedButton.interactable = schemeNum != 1;
            rightHandedButton.interactable = schemeNum != 2;
        }

        public void OnClickChangeLocale(int localeNumber)
        {
            _tempLocale = localeNumber; // Update temp value
            StartCoroutine(ChangeLocale(localeNumber)); // Apply locale change
            UpdateLocaleButtons(localeNumber); // Update UI
        }

        IEnumerator ChangeLocale(int localeNumber)
        {
            yield return LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeNumber];
            Debug.Log("Locale changed to: " + LocalizationSettings.SelectedLocale.LocaleName);
        }

        private void UpdateLocaleButtons(int localeNumber)
        {
            // Update button states visually to reflect active/inactive status
            englishButton.interactable = localeNumber != 0;
            spanishButton.interactable = localeNumber != 1;
        }

        public void ChangeColorBlindMode()
        {
            _isColorBlind = !_isColorBlind; // Toggle color blind mode
            UpdateColorBlindButton(_isColorBlind); // Update UI
        }

        private void UpdateColorBlindButton(bool isColorBlind)
        {
            // Change button appearance based on color blind mode
            colorBlindButton.GetComponentInChildren<TextMeshProUGUI>().text = isColorBlind ? "Color Blind: ON" : "Color Blind: OFF";
        }

        public void ChangeVolume(float volume)
        {
            _tempVolume = volume; // Update temp volume value from slider
        }

        public void QuitAndSave()
        {
            // Save temp settings into GameManager and PlayerPrefs
            GameManager.instance.volume = _tempVolume;
            GameManager.instance.locale = _tempLocale;
            GameManager.instance.controlScheme = _tempScheme;
            GameManager.instance.isColorBlind = _isColorBlind;

            GameManager.instance.SaveSettings(); // Save preferences
        }

        public void JustQuit()
        {
            // Reset the UI to the original settings from GameManager
            _tempVolume = GameManager.instance.volume;
            _tempLocale = GameManager.instance.locale;
            _tempScheme = GameManager.instance.controlScheme;
            _isColorBlind = GameManager.instance.isColorBlind;

            // Update UI elements to reflect original settings
            masterAudioSlider.value = _tempVolume;
            UpdateLocaleButtons(_tempLocale);
            UpdateControlSchemeButtons(_tempScheme);
            UpdateColorBlindButton(_isColorBlind);
        }
    }
}