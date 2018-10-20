using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PassiveAttack : MonoBehaviour
{

    float fadeTime = 0;
    BossAttack currentAttack;
    BossAttack currentActiveAttack;
    BossAttack previousAttack;
    int previousAttackNumber;
    int currentAttackNumber;
    Dictionary<int, BossAttack> attackDict = new Dictionary<int, BossAttack>();
    RadialFillControl radialFillControl;
    AttackMaskControl attackMaskControl;
    CooldownBehaviour currentCooldownBehaviour;
    private ColorModifier aimColorModifier;
    private bool aimingActiveAttack = false;
    private GameObject backgroundFade;
    GameObject currentAttackButton;
    Color currentAttackButtonOriginalColor;
    private CameraShake camShake;
    private GameObject bossButtons;

    private GameObject activeAttackScreenButton;
    private GameObject activeAttackBossButton;

    void Awake()
    {
        this.radialFillControl = GameObject.FindObjectOfType<RadialFillControl>();
        this.attackMaskControl = GameObject.FindObjectOfType<AttackMaskControl>();
        Transform aim = UnityUtils.RecursiveFind(transform, "Image");
        this.aimColorModifier = aim.GetComponent<ColorModifier>();
        this.backgroundFade = GameObject.Find("BackgroundFade");
        this.backgroundFade.SetActive(false);
        this.activeAttackScreenButton = GameObject.Find("ActiveAttackScreenButton");
        this.bossButtons = GameObject.Find("BossButtons");
        camShake = GameObject.Find("Handler").GetComponent<CameraShake>();
    }

    void Start()
    {
        this.activeAttackScreenButton.SetActive(false);

        int dictIndex = 1;
        foreach (BossAttack attack in AttackLists.selectedAttacks)
        {
            if (attack != null)
            {
                attackDict.Add(dictIndex, attack);
                dictIndex++;
            }
        }

        if (attackDict[1].frequency < Parameters.SLOW_ATTACK_LIMIT)
        {
            SetAttack(1);
        }
        else
        {
            foreach (KeyValuePair<int, BossAttack> keyVal in attackDict)
            {
                if (keyVal.Value.frequency < Parameters.SLOW_ATTACK_LIMIT)
                {
                    SetAttack(keyVal.Key);
                    break;
                }
            }
        }

        aimColorModifier.SetDefaultColor(Parameters.AIM_DEFAULT_COLOR);
        aimColorModifier.SetSelectedColor(Parameters.AIM_DAMAGE_COLOR);

        UnityUtils.RecursiveFind(transform, "Image").GetComponent<Image>().color = Parameters.AIM_DEFAULT_COLOR;
    }

    void DoAttack()
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

        if (currentCooldownBehaviour != null)
        {
            currentCooldownBehaviour.RestartCooldown();
        }

        this.aimColorModifier.FadeToSelected(0.0f);
        this.aimColorModifier.FadeToDeselected(this.currentAttack.frequency);

        if (this.aimingActiveAttack)
        {
            this.activeAttackScreenButton.SetActive(false);
            this.activeAttackBossButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("Art/UI_Button_Standard_Sky_2");
            this.activeAttackBossButton.transform.Find("Image").gameObject.SetActive(true);
            this.camShake.Shake(0.1f, 0.2f);
            this.currentAttackButton.transform.parent.GetComponent<Image>().color = this.currentAttackButtonOriginalColor;
        }
        foreach (Transform child in bossButtons.transform)
        {
            child.Find("Overlay").GetComponent<Image>().color = new Color(0, 0, 0, 0.0f);
            child.Find("Highlight").GetComponent<Image>().color = new Color(0, 0, 0, 0.0f);
        }
        this.aimingActiveAttack = false;
        this.backgroundFade.SetActive(false);
        StartCoroutine(UnityUtils.ChangeToColorAfterTime(gameObject.GetComponent<SpriteRenderer>(), new Color(0, 0.89f, 1), 0.5f));
    }

    public void SetAttack(int attackNumber)
    {
        CancelInvoke();

        BossAttack newAttack = attackDict[attackNumber];

        // If the new attack is slow, and this is not the first attack we are doing,
        // save the attack as a reference to the previous attack
        if (newAttack.frequency > Parameters.SLOW_ATTACK_LIMIT && this.currentAttack != null && !aimingActiveAttack)
        {
            this.previousAttack = this.currentAttack;
            this.previousAttackNumber = this.currentAttackNumber;
        }

        // Set the current attack to be the new attack
        this.currentAttack = newAttack;
        this.currentAttackNumber = attackNumber;

        // Set fill and mask for the attack area
        if (radialFillControl != null)
        {
            this.aimColorModifier.FadeToSelected(0.0f);
            radialFillControl.SetMirroredFill(this.currentAttack.angle);
        }

        if (attackMaskControl != null)
        {
            attackMaskControl.SetSize(this.currentAttack.closeRadius, this.currentAttack.farRadius);
        }

        this.currentAttackButton = GameObject.Find("Passive" + attackNumber + "Button");

        if (newAttack.frequency > Parameters.SLOW_ATTACK_LIMIT && !aimingActiveAttack)
        {
            this.currentAttackButtonOriginalColor = this.currentAttackButton.transform.parent.GetComponent<Image>().color;
            gameObject.GetComponent<SpriteRenderer>().color = Color.magenta;
            this.currentAttackButton.transform.parent.GetComponent<Image>().color = new Color(1.0f, 0.3f, 1.0f, 1.0f);
            this.currentAttackButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("Art/UI_Icon_FullScreenExit");
            this.currentAttackButton.transform.Find("Image").gameObject.SetActive(false);
            this.activeAttackBossButton = this.currentAttackButton;
            this.backgroundFade.SetActive(true);
            Color aimColor = Color.magenta;
            aimColor.a = 0.6f;
            this.aimColorModifier.SetColor(aimColor);
            foreach (Transform child in GameObject.Find("BossButtons").transform)
            {
                if (!child.name.Contains(currentAttackNumber.ToString()))
                {
                    child.Find("Overlay").GetComponent<Image>().color = new Color(0, 0, 0, 0.2f);
                }
                else
                {
                    child.Find("Highlight").GetComponent<Image>().color = Color.magenta;
                }
            }
            this.aimingActiveAttack = true;
            this.activeAttackScreenButton.SetActive(true);
            this.activeAttackScreenButton.GetComponent<EventTimer>().AddTimedTrigger(() => SetAttack(currentAttackNumber));
            return;
        }
        else if (newAttack.frequency <= Parameters.SLOW_ATTACK_LIMIT && aimingActiveAttack)
        {
            this.activeAttackBossButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("Art/UI_Button_Standard_Sky_2");
            this.activeAttackBossButton.transform.Find("Image").gameObject.SetActive(true);
            this.activeAttackScreenButton.SetActive(false);
        }


        if (currentAttackButton != null)
        {
            this.currentCooldownBehaviour = currentAttackButton.GetComponentInChildren<CooldownBehaviour>();
            if (this.currentCooldownBehaviour != null)
            {
                this.currentCooldownBehaviour.StartCooldown(this.currentAttack.frequency);
            }
        }

        InvokeRepeating("DoAttack", 0, this.currentAttack.frequency);

        // If the current attack is slow, wait a little and set back to the previous attack
        if (this.currentAttack.frequency > Parameters.SLOW_ATTACK_LIMIT)
        {
            this.currentActiveAttack = this.currentAttack;
            StartCoroutine(WaitAndSetBackAttack(1.0f));
        }
    }

    IEnumerator WaitAndSetBackAttack(float time)
    {
        yield return new WaitForSeconds(time);
        if (this.currentActiveAttack.Equals(this.currentAttack))
        {
            SetAttack(previousAttackNumber);
        }
    }

    void Update()
    {
        if (this.aimingActiveAttack)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.magenta;
        }
    }
}
