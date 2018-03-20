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
		attackDict.Add(1, new BossAttack("WideMelee", 30, 0, 2.0f, 50, 1.2f));
		attackDict.Add(2, new BossAttack("NarrowMeleeAndRanged", 5, 0, 5.0f, 300, 5.0f));
		attackDict.Add(3, new BossAttack("WideRanged", 30, 2.0f, 5.0f, 50, 1.2f));
		setAttack(1);
	}

	void doAttack() { 
		Color color = UnityUtils.RecursiveFind(transform, "Image").GetComponent<Image>().color;
		color.a = 0.0f;

		Color zeroAlphaColor = color;
		zeroAlphaColor.a = 0.0f;
		object[] obj = GameObject.FindObjectsOfType(typeof (GameObject));
		foreach (object o in obj){
			GameObject g = (GameObject) o;
			if (g.name.Contains("Enemy")){
				Enemy enemy = g.GetComponent<Enemy>();
				if (enemy.isInAttackArea(360 - transform.eulerAngles.z - this.currentAttack.angle, 
										360 - transform.eulerAngles.z + this.currentAttack.angle, 
										this.currentAttack.closeRadius, 
										this.currentAttack.farRadius)){
					enemy.applyDamageTo(this.currentAttack.damage);
				}
			}
		}

		// For now, change color of boss when he is attacking
		// TODO: Change when areas of damage is implemented
	 	gameObject.GetComponent<SpriteRenderer>().color = Color.red;

		if (currentCooldownBehaviour != null) {
			currentCooldownBehaviour.RestartCooldown ();
		}
		
		StartCoroutine(UnityUtils.ChangeToColorAfterTime(gameObject.GetComponent<SpriteRenderer>(), Color.white, 0.5f));
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
	
	public class BossAttack {
		public string name;
		public float angle;
		public float closeRadius;
		public float farRadius;
		public float damage;
		public float frequency;

		public BossAttack(string name, float angle, float closeRadius, float farRadius, float damage, float frequency){
			this.name = name;
			this.angle = angle;
			this.closeRadius = closeRadius;
			this.farRadius = farRadius;
			this.damage = damage;
			this.frequency = frequency;
		}
	}
}
