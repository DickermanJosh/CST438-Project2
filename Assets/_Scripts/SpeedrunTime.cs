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
        UnityEngine.Debug.Log("Initializing reset operation");
        decryptSave();

        TimeText.text = elapsedTime;

        StreamWriter sw = new StreamWriter("saveCopy.txt", true);

        timer.Stop();
        timer.Reset();

        int saveTime = (int) ts.TotalMilliseconds;
            
        UnityEngine.Debug.Log("Writing " + saveTime + " to save file");
        sw.Write(saveTime + ",");
        sw.Dispose();

        encryptSave();

        TimeText.text = "00:00:00.00";
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

    public void decryptSave() {
        StreamReader sr = new StreamReader("saveFile.txt", true);
        StreamWriter sw = new StreamWriter("saveCopy.txt", true);

        while(!sr.EndOfStream) {
            char current = (char) sr.Read();
            current = (char) (current - 10);
            sw.Write(current);
        }

        sr.Dispose();
        sw.Dispose();

        File.WriteAllText("saveFile.txt", string.Empty);
    }

    public void encryptSave() {
        StreamReader sr = new StreamReader("saveCopy.txt", true);
        StreamWriter sw = new StreamWriter("saveFile.txt", true);

        while(!sr.EndOfStream) {
            char current = (char) sr.Read();
            current = (char) (current + 10);
            sw.Write(current);
        }

        sr.Dispose();
        sw.Dispose();

        File.WriteAllText("saveCopy.txt", string.Empty);
    }
}
