using System.Collections;
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
    private int previousAttackNumber;
    private ColorModifier aimColorModifier;
    private GameObject activeAttackScreenButton;
    private GameObject backgroundFade;

    void Awake()
    {
        this.backgroundFade = GameObject.Find("BackgroundFade");
        this.attackMaskControl = GameObject.FindObjectOfType<AttackMaskControl>();
        this.passiveAttackController = gameObject.AddComponent<PassiveAttackController>();
        this.activeAttackController = gameObject.AddComponent<ActiveAttackController>();
        this.aimImage = UnityUtils.RecursiveFind(transform, "Image").GetComponent<Image>();
        this.radialFillControl = GameObject.FindObjectOfType<RadialFillControl>();
        this.attackMaskControl = GameObject.FindObjectOfType<AttackMaskControl>();
        Transform aim = UnityUtils.RecursiveFind(transform, "Image");
        this.aimColorModifier = aim.GetComponent<ColorModifier>();
        this.activeAttackScreenButton = GameObject.Find("ActiveAttackScreenButton");
    }

    void Start()
    {
        SetAttack(1);
        aimColorModifier.SetDefaultColor(Parameters.AIM_DEFAULT_COLOR);
        aimColorModifier.SetSelectedColor(Parameters.AIM_DAMAGE_COLOR);
        UnityUtils.RecursiveFind(transform, "Image").GetComponent<Image>().color = Parameters.AIM_DEFAULT_COLOR;
    }

    public void SetAttack(int attackNumber)
    {
        this.passiveAttackController.CancelInvoke();

        if (aimingActiveAttack && previousAttackNumber == attackNumber)
        {
            this.activeAttackController.DoAttack();
            // Switch to an attack with ?
        }
        else
        {
            aimColorModifier.SetDefaultColor(Parameters.AIM_DEFAULT_COLOR);
            aimColorModifier.SetSelectedColor(Parameters.AIM_DAMAGE_COLOR);
            for (int i = 1; i <= 3; i++)
            {
                GameObject currentAttackButton = GameObject.Find("Passive" + i + "Button");
                currentAttackButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("Art/UI_Button_Standard_Sky_2");
                currentAttackButton.transform.Find("Image").gameObject.SetActive(true);
                this.backgroundFade.SetActive(false);
                // Reset colors of buttons maybe???
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
                passiveAttackController.SetAttack(attackNumber);
            }
            attackMaskControl.SetSize(newAttack.closeRadius, newAttack.farRadius);
            radialFillControl.SetMirroredFill(newAttack.angle);
        }
    }
}