using System.Collections;
using _Scripts;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class DangerMeter : MonoBehaviour
{
    [SerializeField] private float maxAmount = 10f; 
    [SerializeField] private float currentAmount;

    public GameObject DoubleJumpText;
    public float GetCurrentAmount() => currentAmount;
    public float GetCurrentMeterPercentage() => currentAmount / maxAmount;

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

    // Reference to the UI Image (the bar)
    [Header("UI Elements")]
    public Image dangerBarImage;

    // Threshold colors
    [Header("Thresholds")]
    [SerializeField] private Color firstColor = Color.green;
    [SerializeField] private Color secondColor = Color.yellow;
    [SerializeField] private Color thirdColor = Color.red;

    private void Start()
    {
        // Initialize the bar
        UpdateDangerBar();
    }

    private void Update()
    {
        // Meter completely full, reload the level
        if (currentAmount >= maxAmount)
        {
            // TODO: Implement fade to black transition
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }
        
        // Decrease the meter over time
        if (currentAmount > 0.001f)
        {
            currentAmount -= Time.deltaTime;
            currentAmount = Mathf.Clamp(currentAmount, 0f, maxAmount);
            UpdateDangerBar();
        }
    }

    private void UpdateDangerBar()
    {
        if (dangerBarImage != null)
        {
            float fillPercentage = currentAmount / maxAmount; // Between 0 and 1

            // Adjust the x-scale of the bar between 0 (empty) and 4 (full)
            var newScale = dangerBarImage.rectTransform.localScale;
            newScale.x = fillPercentage * 4f;
            dangerBarImage.rectTransform.localScale = newScale;

            // Change color based on thresholds
            if (fillPercentage < 0.33f)
            {
                dangerBarImage.color = firstColor;
                DoubleJumpText.SetActive(false);
            }
            else if (fillPercentage < 0.66f)
            {
                dangerBarImage.color = secondColor;
                DoubleJumpText.SetActive(false);
            }
            else
            {
                dangerBarImage.color = thirdColor;
                DoubleJumpText.SetActive(true);
            }
        }
    }

    public void Increment(float amount)
    {
        currentAmount += amount;
        currentAmount = Mathf.Clamp(currentAmount, 0f, maxAmount);
        UpdateDangerBar();
    }

    public void ApplyDebuff(float debuffTime, float speedDecrease, float meterIncrement)
    {
        StartCoroutine(DebuffTimer(debuffTime, speedDecrease, meterIncrement));
    }

    private IEnumerator DebuffTimer(float debuffTime, float speedDecrease, float meterIncrement)
    {
        if (PlayerMovement.Instance.currentMaxSpeed <= 6.0f)
        {
            yield break;
        }

        Debug.Log("Timer start");
        PlayerMovement.Instance.maxSpeed -= (speedDecrease + meterIncrement);
        yield return new WaitForSeconds(debuffTime);
        Debug.Log("Timer end");
        PlayerMovement.Instance.maxSpeed += (speedDecrease + meterIncrement);
    }
}