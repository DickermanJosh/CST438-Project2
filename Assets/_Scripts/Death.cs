using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour
{
    // Reference to the player object and spawn point
    // public Transform player;
    // public Transform spawnPoint;

    // Trigger event for when the player reaches the house
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // TODO: Make this load into a fade to black transition then reload the scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
