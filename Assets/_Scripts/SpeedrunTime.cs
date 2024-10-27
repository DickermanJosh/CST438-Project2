using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts;
using UnityEngine.UI;
using UnityEngine;
using System.Diagnostics;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using TMPro;
using Debug = UnityEngine.Debug;


public class SpeedrunTimer : MonoBehaviour {

    public int testCount = 0;

    [Header("UI Elements")]
    public TMP_Text TimeText;

    // Creates the speedrun timer
    public static Stopwatch timer = new Stopwatch();
    static string saveFileName = "saveFile.txt";
    private string saveFilePath;
    
    public static SpeedrunTimer Instance;
    private void Awake()
    {
        // Ensure there's only one instance of GameManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this GameManager in all scenes
        }
        else
        {
            Destroy(gameObject); // Only ONE!
        }
        saveFilePath = Path.Combine(Application.persistentDataPath, saveFileName);
    }
    private TimeSpan ts;
    string elapsedTime;
    // Start is called before the first frame update
    void Start() {

        ts = new TimeSpan();
        // Starts the speedrun timer
        timer.Start();

        ts = timer.Elapsed;
        string elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

        TimeText.text = elapsedTime; 
        
    }

    // Update is called once per frame
    void Update() {

        testCount++;

        ts = timer.Elapsed;
        elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

        // GameObject player = GameObject.Find("Player");
        // // Win condition, reached rehab
        // if((float) player.transform.position.x >= 296.5) {
        //   
        // }

        // Else just updates the timer image
        TimeText.text = elapsedTime;
    }

    public void ResetTimeWithoutRestart()
    {
        timer.Reset();
        timer.Stop();
    }

    public void ResetTime()
    {
        timer.Reset();
        timer.Start();
    }

    public void StartTime()
    {
        timer.Start();
    }

    public void StopTime()
    {
        timer.Stop();
    }

    public void SaveTime()
    {
        Debug.Log("Initializing save operation");
        try
        {
            string saveFilePath = Path.Combine(Application.persistentDataPath, "saveFile.txt");
            string saveCopyPath = Path.Combine(Application.persistentDataPath, "saveCopy.txt");

            // Ensure the directory exists
            if (!Directory.Exists(Application.persistentDataPath))
            {
                Directory.CreateDirectory(Application.persistentDataPath);
            }

            // Stop and reset the timer
            timer.Stop();
            timer.Reset();

            int saveTime = (int)ts.TotalMilliseconds;

            // Write the save time to saveCopy.txt
            using (StreamWriter sw = new StreamWriter(saveCopyPath, true))
            {
                Debug.Log("Writing " + saveTime + " to save copy file");
                sw.Write(saveTime + ",");
            }

            // Encrypt and save
            encryptSave();

            TimeText.text = "00:00:00.00";
        }
        catch (Exception ex)
        {
            Debug.LogError("Error during SaveTime: " + ex.Message);
        }
    }

    public void decryptSave()
    {
        string saveFilePath = Path.Combine(Application.persistentDataPath, "saveFile.txt");
        string saveCopyPath = Path.Combine(Application.persistentDataPath, "saveCopy.txt");

        if (!File.Exists(saveFilePath))
        {
            Debug.LogWarning("Save file not found during decryption.");
            return;
        }

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

    public void encryptSave()
    {
        string saveFilePath = Path.Combine(Application.persistentDataPath, "saveFile.txt");
        string saveCopyPath = Path.Combine(Application.persistentDataPath, "saveCopy.txt");

        if (!File.Exists(saveCopyPath))
        {
            Debug.LogWarning("Save copy file not found during encryption.");
            return;
        }

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
}
