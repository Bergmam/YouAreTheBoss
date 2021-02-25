using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemButton : MonoBehaviour
{
    private BossHealth bossHealth;
    private AttackController attackController;
    public int Index;
    private Transform bossTransform;

    void Awake()
    {
        GameObject bossGameObject = GameObject.Find("Boss");
        this.bossTransform = bossGameObject.transform;
        this.bossHealth = bossGameObject.GetComponent<BossHealth>();
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

        if (item.PushBackForce != 0)
        {
            float bossRotationAngle = RotationUtils.MakePositiveAngle(this.bossTransform.eulerAngles.z + 90);
            foreach (Enemy enemy in GameObject.FindObjectsOfType<Enemy>())
            {
                // +-40 for a hardcoded 80 degree angle. This is just an arbitrary angle. We could change it whenever we feel like it.
                if (enemy.isInAttackArea( bossRotationAngle - 40, bossRotationAngle + 40, 0, 100))
                {
                    enemy.GetComponent<Rigidbody2D>().AddForce(enemy.transform.position.normalized * item.PushBackForce);
                }
            }
        }

        this.gameObject.SetActive(false);
        BossItemHolder.BossItems[this.Index] = null;
    }
}
