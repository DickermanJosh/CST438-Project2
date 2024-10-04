using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform platform;
    public Transform startPoint;
    public Transform endPoint;
    public float speed = 1.5f;

    private int direction = 1;
    private Vector2 lastPosition;
    private Rigidbody2D platformRb;

    private void Start()
    {
        lastPosition = platform.position;
        platformRb = platform.GetComponent<Rigidbody2D>();
        if (platformRb == null)
        {
            platformRb = platform.gameObject.AddComponent<Rigidbody2D>();
            platformRb.isKinematic = true;
        }
    }

    private void FixedUpdate()
    {
        Vector2 target = currentMovementTarget();
        Vector2 newPosition = Vector2.MoveTowards(platformRb.position, target, speed * Time.fixedDeltaTime);

        Vector2 movement = newPosition - lastPosition;
        platformRb.MovePosition(newPosition);

        float distance = (target - platformRb.position).magnitude;
        if (distance <= 0.1f)
        {
            direction *= -1;
        }

        lastPosition = platformRb.position;
    }

    Vector2 currentMovementTarget()
    {
        return (direction == 1) ? (Vector2)startPoint.position : (Vector2)endPoint.position;
    }

    private void OnDrawGizmos()
    {
        if (platform != null && startPoint != null && endPoint != null)
        {
            Gizmos.DrawLine(platform.position, startPoint.position);
            Gizmos.DrawLine(platform.position, endPoint.position);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                Vector2 platformVelocity = (platformRb.position - lastPosition) / Time.fixedDeltaTime;
                playerRb.velocity = new Vector2(playerRb.velocity.x + platformVelocity.x, playerRb.velocity.y);
            }
        }
    }
}
