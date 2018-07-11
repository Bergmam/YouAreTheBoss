﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    void Awake()
    {
        this.radialFillControl = GameObject.FindObjectOfType<RadialFillControl>();
        this.attackMaskControl = GameObject.FindObjectOfType<AttackMaskControl>();
        Transform aim = UnityUtils.RecursiveFind(transform, "Image");
        this.aimColorModifier = aim.GetComponent<ColorModifier>();
    }

    void Start()
    {


        int dictIndex = 1;
        foreach (BossAttack attack in AttackLists.chosenAttacksArray)
        {
            if (attack != null)
            {
                attackDict.Add(dictIndex, attack);
                dictIndex++;
            }
        }

        if (attackDict[1].frequency < Parameters.SLOW_ATTACK_LIMIT)
        {
            setAttack(1);
        }
        else
        {
            foreach (KeyValuePair<int, BossAttack> keyVal in attackDict)
            {
                if (keyVal.Value.frequency < Parameters.SLOW_ATTACK_LIMIT)
                {
                    setAttack(keyVal.Key);
                    break;
                }
            }
        }

        aimColorModifier.SetDefaultColor(Parameters.AIM_DEFAULT_COLOR);
        aimColorModifier.SetSelectedColor(Parameters.AIM_DAMAGE_COLOR);

        UnityUtils.RecursiveFind(transform, "Image").GetComponent<Image>().color = Parameters.AIM_DEFAULT_COLOR;
    }

    void doAttack()
    {
        Color color = UnityUtils.RecursiveFind(transform, "Image").GetComponent<Image>().color;
        color.a = 0.0f;

        float unitCircleRotation = RotationUtils.MakePositiveAngle(transform.eulerAngles.z + 90);

        Color zeroAlphaColor = color;
        zeroAlphaColor.a = 0.0f;
        object[] obj = GameObject.FindObjectsOfType(typeof(GameObject));
        foreach (Enemy enemy in GameObject.FindObjectsOfType(typeof(Enemy)))
        {
            if (enemy.isInAttackArea(unitCircleRotation - this.currentAttack.angle,
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

        this.aimColorModifier.FadeToSelected(this.currentAttack.frequency);

        StartCoroutine(UnityUtils.ChangeToColorAfterTime(gameObject.GetComponent<SpriteRenderer>(), Color.white, 0.5f));
    }

    public void setAttack(int attackNumber)
    {
        CancelInvoke();

        BossAttack newAttack = attackDict[attackNumber];

        // If the new attack is slow, and this is not the first attack we are doing,
        // save the attack as a reference to the previous attack
        if (newAttack.frequency > Parameters.SLOW_ATTACK_LIMIT && this.currentAttack != null)
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
            radialFillControl.SetMirroredFill((int)this.currentAttack.angle);
        }

        if (attackMaskControl != null)
        {
            attackMaskControl.SetSize(this.currentAttack.closeRadius, this.currentAttack.farRadius);
        }

        GameObject currentAttackButton = GameObject.Find("Passive" + attackNumber + "Button");
        if (currentAttackButton != null)
        {
            this.currentCooldownBehaviour = currentAttackButton.GetComponentInChildren<CooldownBehaviour>();
            if (this.currentCooldownBehaviour != null)
            {
                this.currentCooldownBehaviour.StartCooldown(this.currentAttack.frequency);
            }
        }

        InvokeRepeating("doAttack", 0, this.currentAttack.frequency);

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
            setAttack(previousAttackNumber);
        }
    }

    public BossAttack GetAttack(int number)
    {
        return this.attackDict[number];
    }
}
