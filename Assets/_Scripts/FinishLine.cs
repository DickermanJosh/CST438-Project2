using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public Transform spawnPoint; // Reference to your spawn point

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player crossed finish line!");

            if (SpeedrunManager.Instance != null)
            {
                SpeedrunManager.Instance.FinishRun();
                StartCoroutine(ResetLevel(other.gameObject));
            }
        }
    }

    private IEnumerator ResetLevel(GameObject player)
    {
        // Small delay to ensure everything is recorded
        yield return new WaitForSeconds(0.5f);

        // Reset timer
        SpeedrunTimer.timer.Reset();
        SpeedrunTimer.timer.Start();

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
