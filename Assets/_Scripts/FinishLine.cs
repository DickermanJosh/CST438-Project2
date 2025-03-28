using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    public Transform spawnPoint; // Reference to your spawn point

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.gameObject.CompareTag("Player"))
    //     {
    //         Debug.Log("Player crossed finish line!");
    //
    //         if (SpeedrunManager.Instance != null)
    //         {
    //             SpeedrunManager.Instance.FinishRun();
    //             StartCoroutine(ResetLevel(other.gameObject));
    //         }
    //         
    //         SpeedrunTimer.Instance.SaveTime();
    //         BuffManager.Instance.RespawnAllCollectables();
    //         CheckpointManager.Instance.ResetLastCheckpointPosition();
    //         var checkpoints = FindObjectsOfType<Checkpoint>();
    //         foreach (var point in checkpoints)
    //         {
    //             point.DeactivateCheckpoint();
    //         }
    //         
    //         SceneManager.LoadScene("VictoryScreen"); 
    //     }
    // }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("Player crossed finish line!");

            if (SpeedrunManager.Instance != null)
            {
                SpeedrunManager.Instance.FinishRun();
                SpeedrunTimer.timer.Reset();
                SpeedrunTimer.timer.Start();
                SpeedrunTimer.Instance.SaveTime();
            }

            BuffManager.Instance.RespawnAllCollectables();
            CheckpointManager.Instance.ResetLastCheckpointPosition();
            var checkpoints = FindObjectsOfType<Checkpoint>();
            foreach (var point in checkpoints)
            {
                point.DeactivateCheckpoint();
            }

            SceneManager.LoadScene("VictoryScreen");
        }
    }

    private IEnumerator ResetLevel(GameObject player)
    {
        // Small delay to ensure everything is recorded
        yield return new WaitForSeconds(0.5f);

        // Reset timer
        SpeedrunTimer.timer.Reset();
        SpeedrunTimer.timer.Start();

        
        yield break;

        // Teleport player to spawn point
        if (spawnPoint != null)
        {
            player.transform.position = spawnPoint.position;
            Debug.Log("Player reset to spawn point - Watch for ghost to appear!");
        }
        else
        {
            Debug.LogWarning("No spawn point assigned to FinishLine!");
        }
    }
}
