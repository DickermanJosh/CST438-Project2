using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Sprite redFlagSprite;
    public Sprite greenFlagSprite;
    private SpriteRenderer spriteRenderer;
    private bool isActivated = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component not found on the Checkpoint object!");
        }
        else
        {
            spriteRenderer.sprite = redFlagSprite;
        }

        if (CheckpointManager.Instance == null)
        {
            Debug.LogError("CheckpointManager instance is null. Make sure you have a CheckpointManager in your scene!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isActivated)
        {
            ActivateCheckpoint();
        }
    }

    private void ActivateCheckpoint()
    {
        isActivated = true;
        if (CheckpointManager.Instance != null)
        {
            CheckpointManager.Instance.SetCheckpoint(transform.position);
            Debug.Log("Checkpoint activated at: " + transform.position);
        }
        else
        {
            Debug.LogError("Failed to activate checkpoint: CheckpointManager instance is null");
        }

        if (spriteRenderer != null && greenFlagSprite != null)
        {
            spriteRenderer.sprite = greenFlagSprite;
            Debug.Log("Changed flag sprite to green");
        }
        else
        {
            Debug.LogError("Failed to change flag sprite: " +
                (spriteRenderer == null ? "SpriteRenderer is null. " : "") +
                (greenFlagSprite == null ? "Green flag sprite is null." : ""));
        }
    }
}