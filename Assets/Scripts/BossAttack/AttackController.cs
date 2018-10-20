using UnityEngine;
using UnityEngine.UI;

public class AttackController : MonoBehaviour
{

    private Image aimImage;
    private AttackMaskControl attackMaskControl;
    private RadialFillControl radialFillControl;
    private PassiveAttackController passiveAttackController;
    private ActiveAttackController activeAttackController;
    private bool aimingActiveAttack;

    void Awake()
    {
        this.attackMaskControl = GameObject.FindObjectOfType<AttackMaskControl>();

        this.passiveAttackController = gameObject.AddComponent<PassiveAttackController>();
        this.activeAttackController = gameObject.AddComponent<ActiveAttackController>();

        this.aimImage = UnityUtils.RecursiveFind(transform, "Image").GetComponent<Image>();
    }

    void DoAttack()
    {

    }

    void SetAttack(int attackNumber)
    {
        CancelInvoke();

        if (aimingActiveAttack)
        {
            this.activeAttackController.DoAttack();
            this.aimingActiveAttack = false;
        }
        else
        {
            BossAttack newAttack = AttackLists.selectedAttacks[attackNumber - 1];
            if (newAttack.frequency > Parameters.SLOW_ATTACK_LIMIT)
            {
                passiveAttackController.SetAttack(attackNumber);
                this.aimingActiveAttack = true;
            }
            else
            {
                activeAttackController.SetAttack(attackNumber);
            }
            attackMaskControl.SetSize(newAttack.closeRadius, newAttack.farRadius);
            radialFillControl.SetMirroredFill(newAttack.angle);
        }
    }
}