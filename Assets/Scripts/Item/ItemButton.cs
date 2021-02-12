using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemButton : MonoBehaviour
{
    private BossHealth bossHealth;
    private AttackController attackController;
    public int Index;

    void Awake()
    {
        this.bossHealth = GameObject.Find("Boss").GetComponent<BossHealth>();
        this.attackController = GameObject.FindObjectOfType<AttackController>();
    }

    public void UseItem()
    {
        ItemData item = BossItemHolder.BossItems[this.Index];

        this.bossHealth.HealBossPercentage(item.PercentHealthToHeal);
        this.bossHealth.MakeInvunerable(item.InvunerableSeconds);

        if (item.ResetCooldowns)
        {
            foreach (CooldownBehaviour cooldownBehaviour in GameObject.FindObjectsOfType<CooldownBehaviour>())
            {
                cooldownBehaviour.ResetCooldown();
            }
            this.attackController.ResetCooldown();
        }

        if (item.FreezeEnemiesSeconds > 0)
        {
            foreach (Enemy enemy in GameObject.FindObjectsOfType<Enemy>())
            {
                enemy.Freeze(item.FreezeEnemiesSeconds);
            }
        }

        this.gameObject.SetActive(false);
        BossItemHolder.BossItems[this.Index] = null;
    }
}
