using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    private WaveHandler waveHandler;
    private ItemData item = new ItemData();

    void Awake()
    {
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
        GameObject.FindObjectOfType<ItemButtons>().AddItem(this.item);

        this.waveHandler.HealthPickupRemoved();
        Destroy(this.gameObject);
    }
}
