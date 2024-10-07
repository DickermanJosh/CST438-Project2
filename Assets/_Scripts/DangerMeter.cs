using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DangerMeter : MonoBehaviour
{
    [SerializeField] private float maxAmount = 10f; 
    [SerializeField]private float _currentAmount;
    public float CurrentAmount() =>_currentAmount;
    #region Singleton

    public static DangerMeter Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType(typeof(DangerMeter)) as DangerMeter;

            return _instance;
        }
        set
        {
            _instance = value;
        }
    }
    private static DangerMeter _instance;
    #endregion

    private void Update()
    {
        // Meter completely full, reload the level
        if (_currentAmount >= maxAmount)
        {
            // TODO: Make this load into a fade to black transition then reload the scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }

        if (_currentAmount > 0.001f)
        {
            _currentAmount -= Time.deltaTime;
        }
    }

    public void Increment(float amount)
    {
        _currentAmount += amount;
    }
}
