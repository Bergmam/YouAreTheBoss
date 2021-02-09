using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private WaveHandler waveHandler;
    private BossLookAt bossLookAtScript;
    private ItemButtons itemButtons;
    public int PercentHealthToHeal;
    public int InvunerableSeconds;
    public int FreezeEnemiesSeconds;
    public bool ResetCooldowns;

    void Awake()
    {
        this.waveHandler = GameObject.FindObjectOfType<WaveHandler>();
        this.bossLookAtScript = GameObject.FindObjectOfType<BossLookAt>();
        this.itemButtons = GameObject.FindObjectOfType<ItemButtons>();
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
        item.ResetCooldowns = this.ResetCooldowns;
        item.Sprite = this.gameObject.GetComponent<SpriteRenderer>().sprite;

        this.itemButtons.AddItem(item);
        this.waveHandler.ItemRemoved();

        // Leave the item on screen for a minimum amount of time so that the
        // bossLookAtScript can prevent the boss from rotating if the user
        // taps the item quickly.
        this.bossLookAtScript.MarkObjectForDeletion(this.gameObject);
    }
}
