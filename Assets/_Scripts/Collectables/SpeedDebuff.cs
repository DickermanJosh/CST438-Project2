using System;
using UnityEngine;

namespace _Scripts.Collectables
{
    public class SpeedDebuff : MonoBehaviour
    {
        [Tooltip("Amount to decrease PlayerMovement MaxSpeed when collecting the debuff")]
        public float speedDecrease = 2f;
        [Tooltip("The amount that this item increments the DangerMeter when collected")]
        [SerializeField]private float meterIncrement = 2f;
        private float _minPossibleSpeed; // Don't want the player to ever be able to get below this speed as their max
        private void Start()
        {
            _minPossibleSpeed = PlayerMovement.Instance.maxSpeed / 2f;
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.layer != LayerMask.NameToLayer("Player")) return;
            DangerMeter.Instance.Increment(meterIncrement);
            ApplyDebuff();
            Destroy(gameObject);
        }

        private void ApplyDebuff()
        {
            if (PlayerMovement.Instance.maxSpeed <= _minPossibleSpeed) return;
            PlayerMovement.Instance.maxSpeed -= speedDecrease;
        } 
    }
}