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
    string saveFilePath = Path.Combine(Application.persistentDataPath, saveFileName); 
    
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
        decryptSave();

        TimeText.text = elapsedTime;

        string saveCopyPath = Path.Combine(Application.persistentDataPath, "saveCopy.txt");

        using (StreamWriter sw = new StreamWriter(saveCopyPath, true))
        {
            timer.Stop();
            timer.Reset();

            int saveTime = (int)ts.TotalMilliseconds;

            Debug.Log("Writing " + saveTime + " to save file");
            sw.Write(saveTime + ",");
        }

        encryptSave();

        TimeText.text = "00:00:00.00";
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

    public void encryptSave()
    {
        string saveFilePath = Path.Combine(Application.persistentDataPath, "saveFile.txt");
        string saveCopyPath = Path.Combine(Application.persistentDataPath, "saveCopy.txt");

        if (!File.Exists(saveCopyPath))
        {
            Debug.LogWarning("Save copy file not found during encryption.");
            return;
        }

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
}
