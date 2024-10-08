using UnityEngine;
using UnityEngine.Localization.Settings;
using System.Collections;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // Player settings
    public float volume;
    public int locale;
    public int controlScheme;
    public bool isColorBlind;

    private void Awake()
    {
        // Ensure there's only one instance of GameManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep this GameManager in all scenes
        }
        else
        {
            Destroy(gameObject); // Only ONE!
        }
    }

    private void Start()
    {
        // Load settings from PlayerPrefs when game starts
        LoadSettings();
        //sets locale from player prefs at the start.
        StartCoroutine(InitializeAndChangeLocale(locale)); 
    }

    public void LoadSettings()
    {
        // Load settings from PlayerPrefs or set defaults
        volume = PlayerPrefs.GetFloat("MasterVolume", 0.5f);
        locale = PlayerPrefs.GetInt("Locale", 0);  // 0 = English
        controlScheme = PlayerPrefs.GetInt("ControlScheme", 0);
        isColorBlind = PlayerPrefs.GetInt("ColorBlindMode", 0) == 1;
    }

    public void SaveSettings()
    {
        // Save all settings to PlayerPrefs
        PlayerPrefs.SetFloat("MasterVolume", volume);
        PlayerPrefs.SetInt("Locale", locale);
        PlayerPrefs.SetInt("ControlScheme", controlScheme);
        PlayerPrefs.SetInt("ColorBlindMode", isColorBlind ? 1 : 0);
        PlayerPrefs.Save();
    }
    
    IEnumerator InitializeAndChangeLocale(int localeNumber)
    {
        // Wait until the LocalizationSettings are fully initialized
        //If we don't locale string tables aren't initialized and we get yelled at! I would know!
        yield return LocalizationSettings.InitializationOperation;
        
        // Now we can safely change the locale
        yield return LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeNumber];
    }
}