using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemButton : MonoBehaviour
{
    private BossHealth bossHealth;
    private AttackController attackController;
    public int Index;
    private Transform bossTransform;
    private GameObject itemAimGameObject;
    private ItemAim itemAim;
    private Renderer itemAimRenderer;
    private Material pushBackMaterial;

    void Awake()
    {
        GameObject bossGameObject = GameObject.Find("Boss");
        this.bossTransform = bossGameObject.transform;
        this.bossHealth = bossGameObject.GetComponent<BossHealth>();
        this.attackController = GameObject.FindObjectOfType<AttackController>();
        this.itemAimGameObject = GameObject.Find("ItemAim");
        this.itemAim = this.itemAimGameObject.GetComponent<ItemAim>();
        this.itemAimRenderer = this.itemAimGameObject.GetComponent<Renderer>();
        this.pushBackMaterial = Resources.Load<Material>("Materials/PushBackMaterial");
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
            float pushBackWidthAngle = 40; // +-40 for a hardcoded 80 degree angle. This is just an arbitrary angle. We could change it whenever we feel like it.
            foreach (Enemy enemy in GameObject.FindObjectsOfType<Enemy>())
            {
                if (enemy.isInAttackArea( bossRotationAngle - pushBackWidthAngle, bossRotationAngle + pushBackWidthAngle, 0, 100))
                {
                    enemy.GetComponent<Rigidbody2D>().AddForce(enemy.transform.position.normalized * item.PushBackForce);
                }
            }
            
            this.itemAimRenderer.material = this.pushBackMaterial;
            this.pushBackMaterial.SetVector("Direction", this.bossTransform.up);
            this.pushBackMaterial.SetFloat("Angle", pushBackWidthAngle);
            this.pushBackMaterial.SetFloat("InnerRadius", 0);
            this.pushBackMaterial.SetFloat("OuterRadius", Parameters.MAX_ATTACK_RADIUS);
            this.pushBackMaterial.SetFloat("Speed", 7.0f);
            this.itemAim.Activate(0.45f);
        }

        this.gameObject.SetActive(false);
        BossItemHolder.BossItems[this.Index] = null;
    }
}
