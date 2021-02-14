using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AttackController : MonoBehaviour
{
    private PassiveAttackController passiveAttackController;
    private ActiveAttackController activeAttackController;
    private bool aimingActiveAttack;
    private int previousAttackNumber;
    private ColorModifier aimColorModifier;
    private GameObject activeAttackScreenButton;
    private GameObject backgroundFade;
    private Material aimMaterial;

    void Awake()
    {
        this.backgroundFade = GameObject.Find("BackgroundFade");
        this.passiveAttackController = gameObject.AddComponent<PassiveAttackController>();
        this.activeAttackController = gameObject.AddComponent<ActiveAttackController>();
        Transform aim = UnityUtils.RecursiveFind(transform, "Aim");
        this.aimColorModifier = aim.GetComponent<ColorModifier>();
        this.aimMaterial = aim.gameObject.GetComponent<Renderer>().material;
        this.activeAttackScreenButton = GameObject.Find("ActiveAttackScreenButton");
    }


    void Start()
    {
        Color[] buttonColors = { Color.red, Color.blue, Color.green };
        for (int i = 1; i <= 3; i++)
        {
            GameObject attackButtonBackground = GameObject.Find("Passive" + i + "Background");
            ColorModifier colorModifier = attackButtonBackground.AddComponent<ColorModifier>();
            colorModifier.SetDefaultColor(buttonColors[i - 1]);
            colorModifier.SetSelectedColor(new Color(1.0f, 0.3f, 1.0f, 1.0f));
        }
        SetAttack(1);
    }

    public void SetAttack(int attackNumber)
    {
        this.passiveAttackController.CancelInvoke();

        if (aimingActiveAttack && previousAttackNumber == attackNumber)
        {
            this.activeAttackController.DoAttack();
        }
        else
        {
            aimColorModifier.SetDefaultColor(Parameters.AIM_DEFAULT_COLOR);
            aimColorModifier.SetSelectedColor(Parameters.AIM_DAMAGE_COLOR);
            for (int i = 1; i <= 3; i++)
            {
                GameObject currentAttackButton = GameObject.Find("Passive" + i + "Button");
                currentAttackButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("Art/button3");
                currentAttackButton.transform.Find("Image").gameObject.SetActive(true);
                this.backgroundFade.SetActive(false);
                this.activeAttackController.SetChargeSystem(false);
                GameObject.Find("Passive" + i + "Background").GetComponent<ColorModifier>().DeSelect();
            }
            this.previousAttackNumber = attackNumber;
            BossAttack newAttack = AttackLists.selectedAttacks[attackNumber - 1];
            if (newAttack.frequency > Parameters.SLOW_ATTACK_LIMIT)
            {
                activeAttackController.SetAttack(attackNumber);
                this.aimingActiveAttack = true;
                this.activeAttackScreenButton.GetComponent<EventTimer>().AddTimedTrigger(() => SetAttack(attackNumber));
            }
            else
            {
                this.aimingActiveAttack = false;
                this.activeAttackController.CancelReactivate();
                this.activeAttackController.CancelAiming();
                this.passiveAttackController.SetAttack(attackNumber);
            }
            this.aimMaterial.SetFloat("Angle", newAttack.angle);
            this.aimMaterial.SetFloat("InnerRadius", newAttack.closeRadius);
            this.aimMaterial.SetFloat("OuterRadius", newAttack.farRadius);
        }
    }

    public void ResetCooldown()
    {
        if (this.aimingActiveAttack)
        {
            this.activeAttackController.ResetCooldown();
        }
        else
        {
            this.passiveAttackController.ResetCooldown();
        }
    }

    public void StopAttacking()
    {
        this.aimColorModifier.SetDefaultColor(new Color(0, 0, 0, 0));
        this.aimingActiveAttack = false;
        this.activeAttackController.CancelReactivate();
        this.activeAttackController.CancelAiming();
        this.passiveAttackController.StopAttacking();
    }
}