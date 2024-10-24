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

public class WinConditionCheck : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.Find("Player");
        UnityEngine.Debug.Log("Player position is: "+ player.transform.position.x);
        
        if((float) player.transform.position.x >= 169) {
            // Win screen scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
