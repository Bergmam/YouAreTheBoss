using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private WaveHandler waveHandler;
    public int PercentHealthToHeal;
    public int InvunerableSeconds;
    public int FreezeEnemiesSeconds;
    public Sprite Sprite;

    void Awake()
    {
        this.waveHandler = GameObject.FindObjectOfType<WaveHandler>();
    }

    void Update()
    {
        // Any finger can pick up items.
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), Vector2.zero);
                if (hitInfo && hitInfo.transform.gameObject == this.gameObject)
                {
                    OnPressed();
                }
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
        item.FreezeEnemiesSeconds = this.FreezeEnemiesSeconds;
        item.Sprite = this.gameObject.GetComponent<SpriteRenderer>().sprite;

        GameObject.FindObjectOfType<ItemButtons>().AddItem(item);
        this.waveHandler.ItemRemoved();
        Destroy(this.gameObject);
    }
}
