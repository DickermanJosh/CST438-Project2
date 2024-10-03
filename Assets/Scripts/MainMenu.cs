using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Localization.Settings;

public class MainMenu : MonoBehaviour
{
    // Coroutine to switch locales
        IEnumerator ChangeLocale()
    {
        // Get the current locale index
        var currentLocaleIndex = LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[0] ? 0 : 1;

        // Switch between English (0) and Spanish (1)
        var otherLocaleIndex = currentLocaleIndex == 0 ? 1 : 0;

        // Change the locale
        yield return LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[otherLocaleIndex];
        Debug.Log("Locale changed to: " + LocalizationSettings.SelectedLocale.LocaleName);
    }
        
    //We can't call Coroutines directly with buttons so call it with a public method
    public void OnClickChangeLocale()
    {
        StartCoroutine(ChangeLocale());
    }
    
    //Loads scene zero. Change depending on scene order.
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(0);

        if (SceneManager.LoadSceneAsync(0) == null)
        {
            Debug.Log("No Scene selected.");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    
}
