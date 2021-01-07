using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    private BossHealth bossHealth;
    private WaveHandler waveHandler;

    void Awake()
    {
        this.bossHealth = GameObject.Find("Boss").GetComponent<BossHealth>();
        this.waveHandler = GameObject.FindObjectOfType<WaveHandler>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                OnPressed();
            }
        }
    }
    
    void OnMouseDown()
    {
        OnPressed();
    }

    private void OnPressed()
    {
        bossHealth.HealBossPercentage(20);
        this.waveHandler.HealthPickupRemoved();
        Destroy(this.gameObject);
    }
}
