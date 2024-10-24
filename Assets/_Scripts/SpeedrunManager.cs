using UnityEngine;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class SpeedrunManager : MonoBehaviour
{
    public static SpeedrunManager Instance { get; private set; }

    private List<GhostFrame> currentRunData;
    private List<GhostFrame> bestRunData;
    public GameObject ghostPrefab;
    private GameObject currentGhost;
    private int currentGhostIndex;
    private string bestTimeString = "99:99:99.99";
    private Stopwatch currentRunStopwatch;
    private bool hasCompletedFirstRun = false;

    [Serializable]
    private class GhostFrame
    {
        public Vector2 position;
        public TimeSpan timestamp;

        public GhostFrame(Vector2 pos, TimeSpan time)
        {
            position = pos;
            timestamp = time;
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Initialize()
    {
        currentRunData = new List<GhostFrame>();
        bestRunData = new List<GhostFrame>();
        LoadBestRun();

        // Only spawn ghost if we have previous run data
        if (bestRunData.Count > 0)
        {
            hasCompletedFirstRun = true;
            SpawnGhost();
        }
    }

    private void Update()
    {
        if (currentRunStopwatch != null && currentRunStopwatch.IsRunning)
        {
            RecordGhostFrame();
            if (hasCompletedFirstRun && currentGhost != null)
            {
                UpdateGhostPosition();
            }
        }
    }

    public void StartRun()
    {
        currentRunData = new List<GhostFrame>();
        currentGhostIndex = 0;

        // If there's no recorded best run, hide the ghost
        if (!hasCompletedFirstRun && currentGhost != null)
        {
            currentGhost.SetActive(false);
        }

        RecordGhostFrame();
    }

    private void RecordGhostFrame()
    {
        // Find the player GameObject
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            currentRunData.Add(new GhostFrame(
                player.transform.position,
                currentRunStopwatch.Elapsed
            ));
        }
    }

    public void FinishRun()
    {
        if (currentRunStopwatch == null) return;

        TimeSpan currentTime = currentRunStopwatch.Elapsed;
        string currentTimeString = FormatTimeSpan(currentTime);

        if (!hasCompletedFirstRun || string.Compare(currentTimeString, bestTimeString) < 0)
        {
            bestTimeString = currentTimeString;
            bestRunData = new List<GhostFrame>(currentRunData);
            SaveBestRun();

            if (!hasCompletedFirstRun)
            {
                hasCompletedFirstRun = true;
                SpawnGhost();
            }

            Debug.Log($"New best time: {bestTimeString}!");
        }
        else
        {
            Debug.Log($"Run finished: {currentTimeString}. Best: {bestTimeString}");
        }
    }

    private string FormatTimeSpan(TimeSpan ts)
    {
        return string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
    }

    private void UpdateGhostPosition()
    {
        if (!currentGhost || bestRunData.Count == 0) return;

        TimeSpan currentTime = currentRunStopwatch.Elapsed;

        while (currentGhostIndex < bestRunData.Count - 1 &&
               bestRunData[currentGhostIndex + 1].timestamp < currentTime)
        {
            currentGhostIndex++;
        }

        if (currentGhostIndex < bestRunData.Count)
        {
            currentGhost.transform.position = bestRunData[currentGhostIndex].position;
        }
    }

    private void SpawnGhost()
    {
        if (ghostPrefab != null && bestRunData.Count > 0)
        {
            if (currentGhost != null)
            {
                Destroy(currentGhost);
            }
            currentGhost = Instantiate(ghostPrefab);
            currentGhost.transform.position = bestRunData[0].position;
        }
    }

    private void SaveBestRun()
    {
        if (bestRunData.Count == 0) return;

        string[] timestamps = new string[bestRunData.Count];
        float[] positionsX = new float[bestRunData.Count];
        float[] positionsY = new float[bestRunData.Count];

        for (int i = 0; i < bestRunData.Count; i++)
        {
            timestamps[i] = FormatTimeSpan(bestRunData[i].timestamp);
            positionsX[i] = bestRunData[i].position.x;
            positionsY[i] = bestRunData[i].position.y;
        }

        PlayerPrefs.SetString("BestRunTime", bestTimeString);
        PlayerPrefs.SetString("BestRunTimestamps", string.Join(",", timestamps));
        PlayerPrefs.SetString("BestRunPositionsX", string.Join(",", positionsX));
        PlayerPrefs.SetString("BestRunPositionsY", string.Join(",", positionsY));
        PlayerPrefs.SetInt("HasCompletedFirstRun", 1);
        PlayerPrefs.Save();
    }

    private void LoadBestRun()
    {
        if (!PlayerPrefs.HasKey("BestRunTime")) return;

        hasCompletedFirstRun = PlayerPrefs.GetInt("HasCompletedFirstRun", 0) == 1;
        bestTimeString = PlayerPrefs.GetString("BestRunTime");

        string[] timestamps = PlayerPrefs.GetString("BestRunTimestamps").Split(',');
        string[] positionsX = PlayerPrefs.GetString("BestRunPositionsX").Split(',');
        string[] positionsY = PlayerPrefs.GetString("BestRunPositionsY").Split(',');

        bestRunData.Clear();
        for (int i = 0; i < timestamps.Length; i++)
        {
            if (TimeSpan.TryParse(timestamps[i], out TimeSpan timestamp) &&
                float.TryParse(positionsX[i], out float x) &&
                float.TryParse(positionsY[i], out float y))
            {
                bestRunData.Add(new GhostFrame(new Vector2(x, y), timestamp));
            }
        }
    }
}