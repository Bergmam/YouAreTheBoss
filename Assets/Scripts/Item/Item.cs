using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private WaveHandler waveHandler;
    public int PercentHealthToHeal;
    public int InvunerableSeconds;
    public Sprite Sprite;

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
        ItemData item = new ItemData();
        item.PercentHealthToHeal = this.PercentHealthToHeal;
        item.InvunerableSeconds = this.InvunerableSeconds;
        item.Sprite = this.gameObject.GetComponent<SpriteRenderer>().sprite;

        GameObject.FindObjectOfType<ItemButtons>().AddItem(item);
        this.waveHandler.ItemRemoved();
        Destroy(this.gameObject);
    }
}
