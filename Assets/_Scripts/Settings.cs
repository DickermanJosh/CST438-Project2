using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;
using UnityEngine.Serialization;

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
        public Button enableColorBlindButton;
        public Button disableColorBlindButton;
        public Slider masterAudioSlider;

        // Temporary variables for settings changes
        private float _tempVolume;
        private int _tempLocale;
        private int _tempScheme;
        private bool _isColorBlind;

        void Start()
        {
            //load locale based on player prefs and changes UI
            OnClickChangeLocale(GameManager.Instance.locale);
            
            // Load current settings from GameManager into temp variables and UI
            _tempVolume = GameManager.Instance.volume;
            _tempLocale = GameManager.Instance.locale;
            _tempScheme = GameManager.Instance.controlScheme;
            _isColorBlind = GameManager.Instance.isColorBlind;

            // Set UI based on loaded settings
            masterAudioSlider.value = _tempVolume;
            UpdateLocaleButtons(_tempLocale);
            UpdateControlSchemeButtons(_tempScheme);
            UpdateColorBlindButtons(_isColorBlind);
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
            //Debug.Log("Locale changed to: " + LocalizationSettings.SelectedLocale.LocaleName);
        }

        private void UpdateLocaleButtons(int localeNumber)
        {
            // Update button states visually to reflect active/inactive status
            englishButton.interactable = localeNumber != 0;
            spanishButton.interactable = localeNumber != 1;
            
            englishButton.OnDeselect(null);
            spanishButton.OnDeselect(null);
        }

        public void EnableColorBlindMode()
        {
            _isColorBlind = true; // Toggle color blind mode to true
            UpdateColorBlindButtons(_isColorBlind); // Update UI
        }

        public void DisableColorBlindMode()
        {
            _isColorBlind = false;
            UpdateColorBlindButtons(_isColorBlind);
        }

        private void UpdateColorBlindButtons(bool isColorBlind)
        {
            // Change button appearance based on color blind mode
            enableColorBlindButton.interactable = isColorBlind != true;
            disableColorBlindButton.interactable = isColorBlind;
        }

        public void ChangeVolume(float volume)
        {
            _tempVolume = volume; // Update temp volume value from slider
        }

        public void QuitAndSave()
        {
            // Save temp settings into GameManager and PlayerPrefs
            GameManager.Instance.volume = _tempVolume;
            GameManager.Instance.locale = _tempLocale;
            GameManager.Instance.controlScheme = _tempScheme;
            GameManager.Instance.isColorBlind = _isColorBlind;

            GameManager.Instance.SaveSettings(); // Save preferences
        }

        public void JustQuit()
        {
            // Reset the UI to the original settings from GameManager
            _tempVolume = GameManager.Instance.volume;
            _tempLocale = GameManager.Instance.locale;
            _tempScheme = GameManager.Instance.controlScheme;
            _isColorBlind = GameManager.Instance.isColorBlind;

            // Update UI elements to reflect original settings
            masterAudioSlider.value = _tempVolume;
            UpdateLocaleButtons(_tempLocale);
            UpdateControlSchemeButtons(_tempScheme);
            UpdateColorBlindButtons(_isColorBlind);
        }
    }
}