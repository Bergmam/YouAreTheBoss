using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private WaveHandler waveHandler;
    private BossLookAt bossLookAtScript;
    public int PercentHealthToHeal;
    public int InvunerableSeconds;
    public int FreezeEnemiesSeconds;
    public Sprite Sprite;

    void Awake()
    {
        this.waveHandler = GameObject.FindObjectOfType<WaveHandler>();
        this.bossLookAtScript = GameObject.FindObjectOfType<BossLookAt>();
    }

    void OnMouseDown()
    {
        OnPressed();
    }

    public void OnPressed()
    {
        ItemData item = new ItemData();
        item.PercentHealthToHeal = this.PercentHealthToHeal;
        item.InvunerableSeconds = this.InvunerableSeconds;
        item.FreezeEnemiesSeconds = this.FreezeEnemiesSeconds;
        item.Sprite = this.gameObject.GetComponent<SpriteRenderer>().sprite;

        GameObject.FindObjectOfType<ItemButtons>().AddItem(item);
        this.waveHandler.ItemRemoved();

        // Leave the item on screen for a minimum amount of time so that the
        // bossLookAtScript can prevent the boss from rotating if the user
        // taps the item quickly.
        this.bossLookAtScript.MarkObjectForDeletion(this.gameObject);
    }
}
