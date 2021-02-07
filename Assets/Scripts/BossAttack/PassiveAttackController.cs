using UnityEngine;
using UnityEngine.UI;

public class PassiveAttackController : MonoBehaviour
{

    private BossAttack currentAttack;
    private ColorModifier aimColorModifier;

    private CooldownBehaviour cooldownBehaviour;
    private GameObject projectile;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        this.projectile = Resources.Load<GameObject>("Prefabs/BossProjectile");
        this.aimColorModifier = UnityUtils.RecursiveFind(transform, "Aim").GetComponent<ColorModifier>();
        this.spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public void SetAttack(int attackNumber)
    {
        CancelInvoke();
        BossAttack newAttack = AttackLists.selectedAttacks[attackNumber - 1];
        this.currentAttack = newAttack;
        this.aimColorModifier.Select();

        InvokeRepeating("DoAttack", 0, this.currentAttack.frequency);

        GameObject currentAttackButton = GameObject.Find("Passive" + attackNumber + "Button");
        this.cooldownBehaviour = currentAttackButton.GetComponentInChildren<CooldownBehaviour>();
        this.cooldownBehaviour.StartCooldown(this.currentAttack.frequency);

        if (this.currentAttack.isProjectile)
        {
            this.aimColorModifier.DeSelect();
        }
    }

    public void DoAttack()
    {
        float unitCircleRotation = RotationUtils.MakePositiveAngle(transform.eulerAngles.z + 90);

        if (this.currentAttack.isProjectile)
        {
            Vector3 projectilePos = RotationUtils.RadialPosToXY(new RadialPosition(0.25f, unitCircleRotation));
            GameObject spawnedProjectile = Instantiate(this.projectile, projectilePos, Quaternion.identity);
            spawnedProjectile.GetComponent<BossProjectile>().Attack = this.currentAttack;
        }
        else
        {
            object[] obj = GameObject.FindObjectsOfType(typeof(GameObject));
            foreach (Enemy enemy in GameObject.FindObjectsOfType(typeof(Enemy)))
            {
                if (!enemy.SetForDeath && enemy.isInAttackArea(unitCircleRotation - this.currentAttack.angle,
                        unitCircleRotation + this.currentAttack.angle,
                        this.currentAttack.closeRadius,
                        this.currentAttack.farRadius))
                {
                    enemy.applyDamageTo(this.currentAttack.damage);
                }
            }
            this.aimColorModifier.FadeToSelected(0.0f);
            this.aimColorModifier.FadeToDeselected(this.currentAttack.frequency);
        }

        // For now, change color of boss when he is attacking
        // TODO: Change when areas of damage is implemented
        this.spriteRenderer.color = Color.red;
        this.cooldownBehaviour.RestartCooldown();

        StartCoroutine(UnityUtils.ChangeToColorAfterTime(gameObject.GetComponent<SpriteRenderer>(), Parameters.BOSS_COLOR, 0.5f));
    }
}