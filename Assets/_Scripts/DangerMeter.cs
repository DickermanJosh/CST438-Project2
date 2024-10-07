using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DangerMeter : MonoBehaviour
{
    [SerializeField] private float maxAmount = 10f; 
    [SerializeField]private float _currentAmount;
    public float GetCurrentAmount() =>_currentAmount;
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

    public void ApplyDebuff(float debuffTime, float speedDecrease, float meterIncrement)
    {
        StartCoroutine(DebuffTimer(debuffTime, speedDecrease, meterIncrement));
    }
    
    public IEnumerator DebuffTimer(float debuffTime, float speedDecrease, float meterIncrement)
    {
        if (PlayerMovement.Instance.currentMaxSpeed <= 6.0f)
        {
            yield return null;
        }
        
        Debug.Log("Timer start");
        PlayerMovement.Instance.maxSpeed -= (speedDecrease + meterIncrement);
        yield return new WaitForSeconds(debuffTime);
        Debug.Log("Timer end");
        PlayerMovement.Instance.maxSpeed += (speedDecrease + meterIncrement);
        yield return null;
    } 
}
