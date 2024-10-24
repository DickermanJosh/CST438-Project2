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


public class SpeedrunTimer : MonoBehaviour
{

    public int testCount = 0;

    [Header("UI Elements")]
    public Text TimeText;

    // Creates the speedrun timer
    public static Stopwatch timer = new Stopwatch();

    // Start is called before the first frame update
    void Start()
    {

        // Starts the speedrun timer
        timer.Start();

        TimeSpan ts = timer.Elapsed;
        string elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

        TimeText.text = elapsedTime;

    }

    // Update is called once per frame
    void Update()
    {

        testCount++;

        TimeSpan ts = timer.Elapsed;
        string elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

        // Win condition, reached rehab
        if (testCount >= 3000)
        {
            UnityEngine.Debug.Log("Initializing reset operation");

            TimeText.text = elapsedTime;

            StreamWriter sw = new StreamWriter("saveCopy.txt", true);

            timer.Stop();
            timer.Reset();

            UnityEngine.Debug.Log("Writing " + elapsedTime + " to save file");
            sw.Write(elapsedTime + ",");
            sw.Close();

            TimeText.text = "00:00:00.00";

            // Resets the scene, should probably add a victory thing idk
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;

        }


        // Else just updates the timer image
        TimeText.text = elapsedTime;


    }
}
