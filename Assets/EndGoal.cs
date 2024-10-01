using UnityEngine;

public class EndGoal : MonoBehaviour
{
    // Reference to the player object and spawn point
    public Transform player;
    public Transform spawnPoint;

    // Trigger event for when the player reaches the house
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Move the player back to the spawn point
            player.position = spawnPoint.position;
        }
    }
}
