using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassiveAttack : MonoBehaviour {

	float fadeTime = 0;

	BossAttack currentAttack;
	BossAttack previousAttack;
	int previousAttackNumber;
	int currentAttackNumber;

	Dictionary<int, BossAttack> attackDict = new Dictionary<int, BossAttack>();

	RadialFillControl radialFillControl;
	AttackMaskControl attackMaskControl;

	CooldownBehaviour currentCooldownBehaviour;

	void Start () {
		radialFillControl = GameObject.FindObjectOfType<RadialFillControl> ();
		attackMaskControl = GameObject.FindObjectOfType<AttackMaskControl> ();
		int dictIndex = 1;
		foreach(BossAttack attack in AttackLists.chosenAttacks){
			attackDict.Add(dictIndex, attack);
			dictIndex++;
		}
		setAttack(1);

		UnityUtils.RecursiveFind(transform,"Image").GetComponent<Image>().color = Parameters.AIM_DEFAULT_COLOR;
	}

	void doAttack() { 
		Color color = UnityUtils.RecursiveFind(transform, "Image").GetComponent<Image>().color;
		color.a = 0.0f;

        float unitCircleRotation = RotationUtils.MakePositiveAngle(transform.eulerAngles.z + 90);

		Color zeroAlphaColor = color;
		zeroAlphaColor.a = 0.0f;
		object[] obj = GameObject.FindObjectsOfType(typeof (GameObject));
		foreach (object o in obj){
			GameObject g = (GameObject) o;
			if (g.name.Contains("Enemy")){
				Enemy enemy = g.GetComponent<Enemy>();
                if (enemy.isInAttackArea(unitCircleRotation - this.currentAttack.angle, 
                                         unitCircleRotation + this.currentAttack.angle, 
										this.currentAttack.closeRadius, 
										this.currentAttack.farRadius)){
                    enemy.applyDamageTo(this.currentAttack.damage);
				}
			}
		}

		// For now, change color of boss when he is attacking
		// TODO: Change when areas of damage is implemented
	 	gameObject.GetComponent<SpriteRenderer>().color = Color.red;
		Transform aim = UnityUtils.RecursiveFind(transform,"Image");
		Image aimImage = aim.GetComponent<Image>();
		aimImage.color = Parameters.AIM_DAMAGE_COLOR;

		if (currentCooldownBehaviour != null) {
			currentCooldownBehaviour.RestartCooldown ();
		}
		
		StartCoroutine(UnityUtils.ChangeToColorAfterTime(gameObject.GetComponent<SpriteRenderer>(), Color.white, 0.5f));
		StartCoroutine(UnityUtils.ChangeToColorAfterTime(aimImage, Parameters.AIM_DEFAULT_COLOR, 0.5f));
	}

	public void setAttack(int attackNumber) {
		CancelInvoke();
		
		BossAttack newAttack = attackDict[attackNumber];

		// If the new attack is slow, and this is not the first attack we are doing,
		// save the attack as a reference to the previous attack
		if (newAttack.frequency > Parameters.SLOW_ATTACK_LIMIT && this.currentAttack != null) {
			this.previousAttack = this.currentAttack;
			this.previousAttackNumber = this.currentAttackNumber;
		}

		// Set the current attack to be the new attack
		this.currentAttack = newAttack;
		this.currentAttackNumber = attackNumber;

		// Set fill and mask for the attack area
		if (radialFillControl != null)
		{
			radialFillControl.SetMirroredFill ((int)this.currentAttack.angle);
		}

		if (attackMaskControl != null)
		{
			attackMaskControl.SetSize (this.currentAttack.closeRadius, this.currentAttack.farRadius);
		}

		GameObject currentAttackButton = GameObject.Find ("Passive " + attackNumber);
		if (currentAttackButton != null) {
			this.currentCooldownBehaviour = currentAttackButton.GetComponentInChildren<CooldownBehaviour> ();
			if (this.currentCooldownBehaviour != null) {
				this.currentCooldownBehaviour.StartCooldown (this.currentAttack.frequency);
			}
		}

		InvokeRepeating("doAttack", 0, this.currentAttack.frequency);

		// If the current attack is slow, wait a little and set back to the previous attack
		if (this.currentAttack.frequency > Parameters.SLOW_ATTACK_LIMIT) {
			StartCoroutine(WaitAndSetBackAttack(1.0f));
		}
	}

	IEnumerator WaitAndSetBackAttack(float time) {
		yield return new WaitForSeconds(time);
		setAttack(previousAttackNumber);
	}
}
