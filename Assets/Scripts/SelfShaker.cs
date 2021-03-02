using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfShaker : MonoBehaviour
{
    private Vector3 originalPosition;
    private float shakeAmount = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Shake(0.1f, 0.2f);
        }
    }

    public void Shake(float amount, float length)
    {
        originalPosition = this.transform.position;
        this.shakeAmount = amount;
        InvokeRepeating("DoShake", 0, 0.01f);
        Invoke("StopShake", length);
    }

    void DoShake()
    {
        if (shakeAmount > 0)
        {
            float shakeAmtX = Random.value * shakeAmount * 2 - shakeAmount;
            float shakeAmtY = Random.value * shakeAmount * 2 - shakeAmount;

            Vector3 pos = this.transform.position;
            pos.x += shakeAmtX;
            pos.y += shakeAmtY;
            this.transform.position = pos;
        }
    }

    void StopShake()
    {
        CancelInvoke("DoShake");
        this.transform.position = originalPosition;
    }
}
