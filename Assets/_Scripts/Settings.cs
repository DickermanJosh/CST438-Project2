using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;
using UnityEngine.Serialization;

namespace _Scripts
{
    public class Settings : MonoBehaviour
    {
        // UI elements for different settings
        //row 1
        public Button twoHandedButton;
        
        //row 2
        public Button leftHandedButton;
        public Button rightHandedButton;
        
        //row3
        public Slider masterAudioSlider;
        
        //row4
        public Button ColorBlindEnableButton;
        public Button[] colorBlindButtons; //first index is the disable button
        
        //row 5
        public Button spanishButton;
        public Button englishButton; 
        
        //row 6
        public Button saveExitButton;

        // Temporary variables for settings changes
        private float _tempVolume;
        private int _tempLocale;
        private int _tempScheme;
        private int _colorBlindMode;

            private GameObject previousSelectedObject;
            
            private Color originalDisabledColor = new(0.02f, 0.486f, 0.137f, 0.9f);
            public Color highlightedDisabledColor = new(0.02f, 0.486f, 0.137f, 0.9f);

        void Start()
        {
            //load locale based on player prefs and changes UI
            OnClickChangeLocale(GameManager.Instance.locale);
            
            // Load current settings from GameManager into temp variables and UI
            _tempVolume = GameManager.Instance.volume;
            _tempLocale = GameManager.Instance.locale;
            _tempScheme = GameManager.Instance.controlScheme;
            _colorBlindMode = GameManager.Instance.isColorBlind;

            // Set UI based on loaded settings
            masterAudioSlider.value = _tempVolume;
            UpdateLocaleButtons(_tempLocale);
            UpdateControlSchemeButtons(_tempScheme);
            UpdateColorBlindButtons(_colorBlindMode);
            
            EventSystem.current.SetSelectedGameObject(twoHandedButton.gameObject);
        }

        void Update()
        {
            // Get the currently selected UI object
            GameObject currentSelectedObject = EventSystem.current.currentSelectedGameObject;

            // Check if the selected object has changed
            if (currentSelectedObject != previousSelectedObject)
            {
                // Revert the disabled color of the previous button, if it exists and is disabled
                if (previousSelectedObject != null &&
                    previousSelectedObject.TryGetComponent<Button>(out Button previousButton))
                {
                    if (!previousButton.interactable)
                    {
                        ColorBlock colors = previousButton.colors;
                        colors.disabledColor = originalDisabledColor; // Reset to the original disabled color
                        previousButton.colors = colors;
                    }
                }

                // Update the color of the new selected button if it's disabled
                if (currentSelectedObject != null &&
                    currentSelectedObject.TryGetComponent<Button>(out Button currentButton))
                {
                    if (!currentButton.interactable)
                    {
                        // Store the original disabled color if not already stored
                        originalDisabledColor = currentButton.colors.disabledColor;

                        // Create a new ColorBlock for the current button
                        ColorBlock colors = currentButton.colors;
                        colors.disabledColor = highlightedDisabledColor; // Apply highlighted disabled color
                        currentButton.colors = colors; // Assign the new color block

                        // Make sure to set interactable to false to apply the disabled color properly
                        currentButton.interactable = false; 
                    }
                }

                // Update previousSelectedObject to the current one for the next frame
                previousSelectedObject = currentSelectedObject;
            }
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
            
            ApplyControlScheme(schemeNum);
        }
        
        private void ApplyControlScheme(int schemeNum)
        {
            string schemeName;
            switch (schemeNum)
            {
                case 0:
                    schemeName = "TwoHanded";
                    break;
                case 1:
                    schemeName = "LeftHanded";
                    break;
                case 2:
                    schemeName = "RightHanded";
                    break;
                default:
                    Debug.Log("Error applying control scheme. Case out of bounds");
                    return;
            }
                
            InputHandler.Instance.SwitchControlScheme(schemeName);
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
        
        public void UpdateColorBlindMode(int mode)
        {
            _colorBlindMode = mode;
            UpdateColorBlindButtons(mode);
            //should update immediately when pressed.
            
        }

        private void UpdateColorBlindButtons(int mode)
        {
            for (int i = 0; i < colorBlindButtons.Length; i++)
            {
                // Disable the button for the selected mode, enable others
                colorBlindButtons[i].interactable = (i != mode);
            }
        }

        public void ChangeVolume(float volume)
        {
            _tempVolume = volume; // Update temp volume value from slider
            
            GameManager.Instance.ApplyVolume(volume);
        }



        public void QuitAndSave()
        {
            // Save temp settings into GameManager and PlayerPrefs
            GameManager.Instance.volume = _tempVolume;
            GameManager.Instance.locale = _tempLocale;
            GameManager.Instance.controlScheme = _tempScheme;
            GameManager.Instance.isColorBlind = _colorBlindMode;

            GameManager.Instance.SaveSettings(); // Save preferences
        }

        public void JustQuit()
        {
            // Reset the UI to the original settings from GameManager
            _tempVolume = GameManager.Instance.volume;
            _tempLocale = GameManager.Instance.locale;
            _tempScheme = GameManager.Instance.controlScheme;
            _colorBlindMode = GameManager.Instance.isColorBlind;

            // Update UI elements to reflect original settings
            masterAudioSlider.value = _tempVolume;
            UpdateLocaleButtons(_tempLocale);
            UpdateControlSchemeButtons(_tempScheme);
            UpdateColorBlindButtons(_colorBlindMode);
        }
    }
}