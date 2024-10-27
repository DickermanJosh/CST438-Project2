using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("Player crossed finish line!");

            if (SpeedrunManager.Instance != null)
            {
                SpeedrunManager.Instance.FinishRun();
            }
                SpeedrunTimer.timer.Reset();
                SpeedrunTimer.timer.Start();
                SpeedrunTimer.Instance.SaveTime();

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
}
