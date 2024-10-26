using System;
using System.Collections;
using UnityEditor.Tilemaps;
using UnityEngine;

namespace _Scripts.Collectables
{
    public class SpeedDebuff : MonoBehaviour
    {
        [Tooltip("Amount of time the debuff will last")]
        public float debuffTime = 2f;
        [Tooltip("Amount to decrease PlayerMovement MaxSpeed when collecting the debuff")]
        public float speedDecrease = 2f;
        [Tooltip("The amount that this item increments the DangerMeter when collected")]
        [SerializeField]private float meterIncrement = 2f;

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.layer != LayerMask.NameToLayer("Player")) return;
            DangerMeter.Instance.Increment(meterIncrement);
            DangerMeter.Instance.ApplyDebuff(debuffTime, speedDecrease, meterIncrement);
            // Destroy(gameObject);
            gameObject.SetActive(false);
        }


    }
}