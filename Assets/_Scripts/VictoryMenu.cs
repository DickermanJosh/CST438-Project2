using System;
using UnityEngine;
using System.Diagnostics;
using System.IO;
using UnityEngine.SceneManagement;
using TMPro;
using Debug = UnityEngine.Debug;

public class VictoryMenu : MonoBehaviour
{


    [Header("UI Elements")] 
    public TMP_Text TimeDisplay;

    // Start is called before the first frame update
    private void Start()
    {
        try
        {
            DecryptSave();

            string saveCopyPath = Path.Combine(Application.persistentDataPath, "saveCopy.txt");

            if (File.Exists(saveCopyPath))
            {
                string fullList = File.ReadAllText(saveCopyPath);
                Debug.Log("Times taken from save file: " + fullList);

                string[] times = fullList.Split(',');

                if (times.Length >= 2)
                {
                    string finalTime = times[times.Length - 2];

                    if (int.TryParse(finalTime, out int finalTimeInt))
                    {
                        TimeSpan ts = TimeSpan.FromMilliseconds(finalTimeInt);
                        string formattedFinalTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                            ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                        Debug.Log("Time to display: " + formattedFinalTime);
                        TimeDisplay.text = formattedFinalTime;
                    }
                    else
                    {
                        Debug.LogWarning("Failed to parse final time.");
                        TimeDisplay.text = "Error";
                    }
                }
                else
                {
                    Debug.LogWarning("No times found in save file.");
                    TimeDisplay.text = "No Times Available";
                }
            }
            else
            {
                Debug.LogWarning("Save copy file not found.");
                TimeDisplay.text = "No Times Available";
            }

            EncryptSave();
        }
        catch (Exception ex)
        {
            Debug.LogError("Error in VictoryMenu Start: " + ex.Message);
            TimeDisplay.text = "Error";
        }
    }

    public void OnClickRestart()
    {
        SpeedrunTimer.Instance?.ResetTime();
        SceneManager.LoadScene("Level copy");
    }

    public void OnClickMenu()
    {
        SpeedrunTimer.Instance?.ResetTime();
        SceneManager.LoadScene("MainMenuSettingsLeaderboard");
    }

    private void DecryptSave()
    {
        string saveFilePath = Path.Combine(Application.persistentDataPath, "saveFile.txt");
        string saveCopyPath = Path.Combine(Application.persistentDataPath, "saveCopy.txt");

        if (!File.Exists(saveFilePath))
        {
            Debug.LogWarning("Save file not found during decryption.");
            return;
        }

        try
        {
            using (StreamReader sr = new StreamReader(saveFilePath))
            using (StreamWriter sw = new StreamWriter(saveCopyPath, false))
            {
                while (!sr.EndOfStream)
                {
                    char current = (char)sr.Read();
                    current = (char)(current - 10);
                    sw.Write(current);
                }
            }

            File.WriteAllText(saveFilePath, string.Empty);
        }
        catch (Exception ex)
        {
            Debug.LogError("Error during DecryptSave: " + ex.Message);
        }
    }

    private void EncryptSave()
    {
        string saveFilePath = Path.Combine(Application.persistentDataPath, "saveFile.txt");
        string saveCopyPath = Path.Combine(Application.persistentDataPath, "saveCopy.txt");

        if (!File.Exists(saveCopyPath))
        {
            Debug.LogWarning("Save copy file not found during encryption.");
            return;
        }

        try
        {
            using (StreamReader sr = new StreamReader(saveCopyPath))
            using (StreamWriter sw = new StreamWriter(saveFilePath, false))
            {
                while (!sr.EndOfStream)
                {
                    char current = (char)sr.Read();
                    current = (char)(current + 10);
                    sw.Write(current);
                }
            }

            File.WriteAllText(saveCopyPath, string.Empty);
        }
        catch (Exception ex)
        {
            Debug.LogError("Error during EncryptSave: " + ex.Message);
        }
    }
}
