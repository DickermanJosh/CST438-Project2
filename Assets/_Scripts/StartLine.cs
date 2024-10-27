using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLine : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        SpeedrunTimer.Instance.ResetTimeWithoutRestart();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        SpeedrunTimer.Instance.StopTime();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        SpeedrunTimer.Instance.ResetTime();
    }
}
