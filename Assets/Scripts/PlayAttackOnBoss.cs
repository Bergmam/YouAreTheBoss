﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayAttackOnBoss : MonoBehaviour
{

    RadialFillControl radialFillControl;
    AttackMaskControl attackMaskControl;
    private ColorModifier aimColorModifier;
    BossAttack currentAttack;
    CooldownBehaviour currentCooldownBehaviour;

    // Use this for initialization
    void Start()
    {

        radialFillControl = UnityUtils.RecursiveFind(transform, "Mask").GetComponent<RadialFillControl>();
        attackMaskControl =  UnityUtils.RecursiveFind(transform, "Mask").GetComponent<AttackMaskControl>();

        Transform aim = UnityUtils.RecursiveFind(transform, "Image");
        this.aimColorModifier = aim.GetComponent<ColorModifier>();
        aimColorModifier.SetDefaultColor(Parameters.AIM_DEFAULT_COLOR);
        aimColorModifier.SetSelectedColor(Parameters.AIM_DAMAGE_COLOR);

        UnityUtils.RecursiveFind(transform, "Image").GetComponent<Image>().color = Parameters.AIM_DEFAULT_COLOR;
    }

    public void setAttack(BossAttack attack)
    {
		CancelInvoke();
        // Set the current attack to be the new attack
        this.currentAttack = attack;

        // Set fill and mask for the attack area
        if (radialFillControl != null)
        {
            radialFillControl.SetMirroredFill((int)this.currentAttack.angle);
        }

        if (attackMaskControl != null)
        {
            attackMaskControl.SetSize(this.currentAttack.closeRadius, this.currentAttack.farRadius);
        }

        InvokeRepeating("doAttack", 0, this.currentAttack.frequency);
    }

	void doAttack() { 
		Color color = UnityUtils.RecursiveFind(transform, "Image").GetComponent<Image>().color;
		color.a = 0.0f;

        float unitCircleRotation = RotationUtils.MakePositiveAngle(transform.eulerAngles.z + 90);

		Color zeroAlphaColor = color;
		zeroAlphaColor.a = 0.0f;
		object[] obj = GameObject.FindObjectsOfType(typeof (GameObject));
		foreach (Enemy enemy in GameObject.FindObjectsOfType(typeof (Enemy))){
			if (enemy.isInAttackArea(unitCircleRotation - this.currentAttack.angle, 
					unitCircleRotation + this.currentAttack.angle, 
					this.currentAttack.closeRadius, 
					this.currentAttack.farRadius)){
				enemy.applyDamageTo(this.currentAttack.damage);
			}
		}

		// For now, change color of boss when he is attacking
		// TODO: Change when areas of damage is implemented
	 	gameObject.GetComponent<SpriteRenderer>().color = Color.red;

		this.aimColorModifier.FadeToSelected(this.currentAttack.frequency);
		gameObject.GetComponent<SpriteRenderer>().color = Color.white;
		
		//StartCoroutine(UnityUtils.ChangeToColorAfterTime(gameObject.GetComponent<SpriteRenderer>(), Color.white, 0.5f));
	}
}
