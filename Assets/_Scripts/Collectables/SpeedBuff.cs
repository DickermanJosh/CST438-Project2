using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class SpeedBuff : MonoBehaviour
{
    [Tooltip("Amount to increment PlayerMovement MaxSpeed when collecting the buff")]
    public float speedIncrease = 2f;
    private float _maxPossibleSpeed; // Don't want the player to ever be able to pass this max speed
    // Start is called before the first frame update
    private void Start()
    {
        _maxPossibleSpeed = PlayerMovement.Instance.maxSpeed * 5f;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer != LayerMask.NameToLayer("Player")) return;
        ApplyBuff();
        Destroy(gameObject);
    }

    private void ApplyBuff()
    {
        if (PlayerMovement.Instance.maxSpeed >= _maxPossibleSpeed) return;
        PlayerMovement.Instance.maxSpeed += speedIncrease;
    }
}
