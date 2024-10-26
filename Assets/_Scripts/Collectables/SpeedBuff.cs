using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class SpeedBuff : MonoBehaviour
{
    [Tooltip("Amount to increment PlayerMovement MaxSpeed when collecting the buff")]
    [SerializeField]private float speedIncrease = 2f;
    [Tooltip("The amount that this item increments the DangerMeter when collected")]
    [SerializeField]private float meterIncrement = 2f;
    private float _maxPossibleSpeed; // Don't want the player to ever be able to pass this max speed
    
    private void Start()
    {
        _maxPossibleSpeed = PlayerMovement.Instance.maxSpeed * 5f;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer != LayerMask.NameToLayer("Player")) return;
        DangerMeter.Instance.Increment(meterIncrement);
        // ApplyBuff();
        // Destroy(gameObject);
        gameObject.SetActive(false);
    }
}
