using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; }
    private Vector3 lastCheckpointPosition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("CheckpointManager instance created");
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("Duplicate CheckpointManager destroyed");
        }
    }

    public void SetCheckpoint(Vector3 position)
    {
        lastCheckpointPosition = position;
        Debug.Log("Checkpoint set at: " + position);
    }

    public Vector3 GetLastCheckpoint()
    {
        Debug.Log("Retrieved checkpoint at: " + lastCheckpointPosition);
        return lastCheckpointPosition;
    }
}