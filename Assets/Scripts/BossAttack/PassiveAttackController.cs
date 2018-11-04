using UnityEngine;
using UnityEngine.UI;

public class PassiveAttackController : MonoBehaviour
{

    private BossAttack currentAttack;
    private ColorModifier aimColorModifier;

    private CooldownBehaviour cooldownBehaviour;

    void Awake()
    {
        this.aimColorModifier = UnityUtils.RecursiveFind(transform, "Image").GetComponent<ColorModifier>();
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
    }

    public void DoAttack()
    {
        float unitCircleRotation = RotationUtils.MakePositiveAngle(transform.eulerAngles.z + 90);

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

        // For now, change color of boss when he is attacking
        // TODO: Change when areas of damage is implemented
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        this.cooldownBehaviour.RestartCooldown();

        this.aimColorModifier.FadeToSelected(0.0f);
        this.aimColorModifier.FadeToDeselected(this.currentAttack.frequency);

        StartCoroutine(UnityUtils.ChangeToColorAfterTime(gameObject.GetComponent<SpriteRenderer>(), new Color(0, 0.89f, 1), 0.5f));
    }
}