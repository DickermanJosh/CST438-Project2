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

public class VictoryMenu : MonoBehaviour
{


    [Header("UI Elements")] 
    public TMP_Text TimeDisplay;

    // Start is called before the first frame update
    void Start()
    {
        decryptSave();

        StreamReader sr = new StreamReader("saveCopy.txt");
        
        String [] times;
        string fullList = sr.ReadToEnd();
        sr.Dispose();
        times = fullList.Split(',');

        if(times.Length > 2) {
            String finalTime = times[times.Length-2];

            int finalTimeInt = Int32.Parse(finalTime);

            TimeSpan ts = TimeSpan.FromMilliseconds(finalTimeInt);
            string formattedFinalTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            TimeDisplay.text = formattedFinalTime;
        }
        else {
            TimeDisplay.text = "Error";
        }

    }

    // Update is called once per frame
    void Update()
    {
        //none
    }

    public void onClickRestart() {
        if (SpeedrunTimer.Instance != null)
            SpeedrunTimer.Instance.ResetTime();
        SceneManager.LoadScene("Level copy");
    }

    public void onClickMenu() {
        if (SpeedrunTimer.Instance != null)
            SpeedrunTimer.Instance.ResetTime();
        SceneManager.LoadScene("MainMenuSettingsLeaderboard");
    }

    public void decryptSave() {
        StreamReader sr = new StreamReader("saveFile.txt");
        StreamWriter sw = new StreamWriter("saveCopy.txt");

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
