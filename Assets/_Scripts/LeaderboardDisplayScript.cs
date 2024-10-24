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
        calculateAssignPlacements();
    }

    void Update() {
        //nothing
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

    public void calculateAssignPlacements() {
        decryptSave();
        StreamReader sr = new StreamReader("saveCopy.txt");
        
        String [] times;

        string fullList = sr.ReadToEnd();
        sr.Dispose();
        times = fullList.Split(',');

        for(int i = 0; i < times.Length; i++) {
            UnityEngine.Debug.Log("Array value: " + times[i]);
        }

        int [] timesIntArray = new int[times.Length-1];
        for(int i = 0; i < times.Length-1; i++) {
            UnityEngine.Debug.Log("Converting " + times[i] + " into an int");
            timesIntArray [i] = Int32.Parse(times[i]);
        }

        QuickSort(timesIntArray, 0, timesIntArray.Length-1);


        //Sets the first time
        if(timesIntArray.Length >= 1) {
            int time1 = timesIntArray[0];
            TimeSpan ts1 = TimeSpan.FromMilliseconds(time1);
            string formattedTime1 = string.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts1.Hours, ts1.Minutes, ts1.Seconds, ts1.Milliseconds / 10);
            firstTime.text = "1. " + formattedTime1;
        }
        else {
            firstTime.text = "No Records Yet";
        }

        //Sets the second time
        if(timesIntArray.Length >= 2) {
            int time2 = timesIntArray[1];
            TimeSpan ts2 = TimeSpan.FromMilliseconds(time2);
            string formattedTime2 = string.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts2.Hours, ts2.Minutes, ts2.Seconds, ts2.Milliseconds / 10);
            secondTime.text = "2. " + formattedTime2;
        }
        else {
            secondTime.text = "No Records Yet";
        }

        //Sets the third time
        if(timesIntArray.Length >= 3) {
            int time3 = timesIntArray[2];
            TimeSpan ts3 = TimeSpan.FromMilliseconds(time3);
            string formattedTime3 = string.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts3.Hours, ts3.Minutes, ts3.Seconds, ts3.Milliseconds / 10);
            thirdTime.text = "3. " + formattedTime3;
        }
        else {
            thirdTime.text = "No Records Yet";
        }


        //Sets the fourth time
        if(timesIntArray.Length >= 4) {
            int time4 = timesIntArray[3];
            TimeSpan ts4 = TimeSpan.FromMilliseconds(time4);
            string formattedTime4 = string.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts4.Hours, ts4.Minutes, ts4.Seconds, ts4.Milliseconds / 10);
            fourthTime.text = "4. " + formattedTime4;
        }
        else {
            fourthTime.text = "No Records Yet";
        }


        //Sets the fifth time
        if(timesIntArray.Length >= 5) {
            int time5 = timesIntArray[4];
            TimeSpan ts5 = TimeSpan.FromMilliseconds(time5);
            string formattedTime5 = string.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts5.Hours, ts5.Minutes, ts5.Seconds, ts5.Milliseconds / 10);
            fifthTime.text = "5. " + formattedTime5;
        }
        else {
            fifthTime.text = "No Records Yet";
        }

        encryptSave();
    }

    static int Partition(int[] arr, int low, int high) {
        
        int pivot = arr[high];
        int i = low - 1;

        for (int j = low; j <= high - 1; j++) {
            if (arr[j] < pivot) {
                i++;
                Swap(arr, i, j);
            }
        }
        
        Swap(arr, i + 1, high);  
        return i + 1;
    }

    static void Swap(int[] arr, int i, int j) {
        int temp = arr[i];
        arr[i] = arr[j];
        arr[j] = temp;
    }

    static void QuickSort(int[] arr, int low, int high) {
        if (low < high) {
            
            int pi = Partition(arr, low, high);

            QuickSort(arr, low, pi - 1);
            QuickSort(arr, pi + 1, high);
        }
    }
}
