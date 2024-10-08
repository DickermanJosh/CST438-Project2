using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SpawnPoint : MonoBehaviour
{
    public Transform spawnPoint;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = spawnPoint.position;
        }
    }
}
