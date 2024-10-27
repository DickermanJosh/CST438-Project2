using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using System.IO;
using UnityEngine.UIElements;

public class LeaderboardDisplayScript : MonoBehaviour
{

    [Header("UI Elements")] 
    public TMP_Text firstTime;
    public TMP_Text secondTime;
    public TMP_Text thirdTime;
    public TMP_Text fourthTime;
    public TMP_Text fifthTime;

    void Start() {
        CalculateAssignPlacements();
    }

 private void CalculateAssignPlacements()
    {
        try
        {
            DecryptSave();

            string saveCopyPath = Path.Combine(Application.persistentDataPath, "saveCopy.txt");

            FileInfo fileInfo = new FileInfo(saveCopyPath);

            if (!File.Exists(saveCopyPath) || fileInfo.Length == 0)
            {
                Debug.LogWarning("Save copy file not found or is empty.");
                SetDefaultTimes();
                return;
            }

            string fullList = File.ReadAllText(saveCopyPath);

            string[] times = fullList.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            int[] timesIntArray = new int[times.Length];
            for (int i = 0; i < times.Length; i++)
            {
                if (int.TryParse(times[i], out int parsedTime))
                {
                    timesIntArray[i] = parsedTime;
                }
                else
                {
                    Debug.LogWarning($"Failed to parse time at index {i}: {times[i]}");
                    timesIntArray[i] = int.MaxValue; // Assign a large number to push it to the end
                }
            }

            Array.Sort(timesIntArray);

            // Assign times to UI elements
            AssignTimeToText(firstTime, timesIntArray, 0);
            AssignTimeToText(secondTime, timesIntArray, 1);
            AssignTimeToText(thirdTime, timesIntArray, 2);
            AssignTimeToText(fourthTime, timesIntArray, 3);
            AssignTimeToText(fifthTime, timesIntArray, 4);

            EncryptSave();
        }
        catch (Exception ex)
        {
            Debug.LogError("Error in CalculateAssignPlacements: " + ex.Message);
            SetDefaultTimes();
        }
    }

    private void AssignTimeToText(TMP_Text textElement, int[] timesArray, int index)
    {
        if (index < timesArray.Length && timesArray[index] != int.MaxValue)
        {
            TimeSpan ts = TimeSpan.FromMilliseconds(timesArray[index]);
            string formattedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            textElement.text = formattedTime;
        }
        else
        {
            textElement.text = "XX:XX:XX.XX";
        }
    }

    private void SetDefaultTimes()
    {
        firstTime.text = "XX:XX:XX.XX";
        secondTime.text = "XX:XX:XX.XX";
        thirdTime.text = "XX:XX:XX.XX";
        fourthTime.text = "XX:XX:XX.XX";
        fifthTime.text = "XX:XX:XX.XX";
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
            using (StreamReader sr = new StreamReader(saveFilePath, true))
            using (StreamWriter sw = new StreamWriter(saveCopyPath, true))
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
            using (StreamReader sr = new StreamReader(saveCopyPath, true))
            using (StreamWriter sw = new StreamWriter(saveFilePath, true))
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
