using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;

public class LeaderboardDisplayScript
{

    public TMP_Text firstTime;
    public TMP_Text secondTime;
    public TMP_Text thirdTime;
    public TMP_Text FourthTime;
    public TMP_Text fifthTime;

    public void decryptSave() {
        StreamReader sr = new StreamReader("saveFile.txt");
        StreamWriter sw = new StreamWriter("saveCopy.txt");

        while(!sr.EndOfStream) {
            char current = (char) sr.Read();
            current = (char) (current - 10);
            sw.Write(current);
        }

        
        File.WriteAllText("saveFile.txt", string.Empty);

        sr.Close();
        sw.Close();
    }

    public void encryptSave() {
        StreamReader sr = new StreamReader("saveCopy.txt");
        StreamWriter sw = new StreamWriter("saveFile.txt");

        while(!sr.EndOfStream) {
            char current = (char) sr.Read();
            current = (char) (current + 10);
            sw.Write(current);
        }

        
        File.WriteAllText("saveCopy.txt", string.Empty);

        sr.Close();
        sw.Close();
    }

    public void calculateAssignPlacements() {
        decryptSave();
        StreamReader sr = new StreamReader("saveCopy.txt");
        
        String [] times;

        string fullList = sr.ReadToEnd();
        times = fullList.Split(',');

        timesIntArray = Array.ConvertAll<string, int>(mystring, int.Parse);

        QuickSort(timesIntArray, 0, timesIntArray.length-1);

        
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
