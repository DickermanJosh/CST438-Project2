using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player died");
            RespawnPlayer();
        }
    }

    private void RespawnPlayer()
    {
        if (CheckpointManager.Instance != null)
        {
            if (DangerMeter.Instance != null)
                DangerMeter.Instance.SetCurrentAmount(DangerMeter.Instance.GetCurrentAmount() / 2f);
            if (BuffManager.Instance != null)
                BuffManager.Instance.RespawnAllCollectables();
            Vector3 respawnPosition = CheckpointManager.Instance.GetLastCheckpoint();
            if (respawnPosition != Vector3.zero)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    player.transform.position = respawnPosition;
                    Debug.Log("Player respawned at: " + respawnPosition);
                }
                else
                {
                    Debug.LogError("Player object not found!");
                }
            }
            else
            {
                Debug.Log("No checkpoint set, reloading scene");
                if (SpeedrunTimer.Instance != null)
                    SpeedrunTimer.Instance.ResetTime();
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
        else
        {
            Debug.LogError("CheckpointManager instance is null!");
            if (SpeedrunTimer.Instance != null)
                SpeedrunTimer.Instance.ResetTime();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}