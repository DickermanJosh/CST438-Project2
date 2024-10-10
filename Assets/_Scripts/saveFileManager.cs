using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class saveFileManager {
    static string SAVE_FILE_PATH = "Assets/Persistance/saveGame.txt";
    static string SAVE_BUFFER_PATH = "Assets/Persistance/tempCopy.txt";

    void onStartDecrypt() {
        StreamReader st = new StreamReader(SAVE_FILE_PATH);
        StreamWriter sw = new StreamWriter(SAVE_BUFFER_PATH);

        while(!st.EndOfStream) {
            char current = (char) st.Read();
            current = (char) (current - 10);
            sw.Write(current);
        }

        
        File.WriteAllText(SAVE_FILE_PATH, string.Empty);

        st.Close();
        sw.Close();
        
    }

    void encryptSave() {
        StreamReader st = new StreamReader(SAVE_BUFFER_PATH);
        StreamWriter sw = new StreamWriter(SAVE_FILE_PATH);

        while(!st.EndOfStream) {
            char current = (char) st.Read();
            current = (char) ((current + 10)%255);
            sw.Write(current);
        }

        
        File.WriteAllText(SAVE_BUFFER_PATH, string.Empty);

        st.Close();
        sw.Close();
        
    }
}