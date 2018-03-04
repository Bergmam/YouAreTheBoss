using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveAttack : MonoBehaviour {


	float angle, closeRadius, farRadius;

	float damage;

	Dictionary<int, BossAttack> attackDict = new Dictionary<int, BossAttack>();

	RadialFillControl radialFillControl;
	AttackMaskControl attackMaskControl;

	void Start () {
		radialFillControl = GameObject.FindObjectOfType<RadialFillControl> ();
		attackMaskControl = GameObject.FindObjectOfType<AttackMaskControl> ();
		attackDict.Add(1, new BossAttack("WideMelee", 30, 0, 2.0f, 50, 1.2f));
		attackDict.Add(2, new BossAttack("NarrowMeleeAndRanged", 5, 0, 5.0f, 300, 2.0f));
		attackDict.Add(3, new BossAttack("WideRanged", 30, 2.0f, 5.0f, 50, 1.2f));
		setAttack(1);
	}

	void doAttack() {
		object[] obj = GameObject.FindObjectsOfType(typeof (GameObject));
		foreach (object o in obj){
			GameObject g = (GameObject) o;
			if (g.name.Contains("Enemy")){
				Enemy enemy = g.GetComponent<Enemy>();
				if (enemy.isInAttackArea(360 - transform.eulerAngles.z - angle, 360 - transform.eulerAngles.z + angle, closeRadius, farRadius)){
					enemy.applyDamageTo(damage);
				}
			}
		}

		// For now, change color of boss when he is attacking
		// TODO: Change when areas of damage is implemented
	 	gameObject.GetComponent<SpriteRenderer>().color = Color.red;
		
		StartCoroutine(UnityUtils.ChangeToColorAfterTime(gameObject.GetComponent<SpriteRenderer>(), Color.white, 0.5f));
		
	}

	public void setAttack(int attackNumber) {
		CancelInvoke();
		BossAttack attack = attackDict[attackNumber];
		this.angle = attack.angle;
		this.farRadius = attack.farRadius;
		this.closeRadius = attack.closeRadius;
		this.damage = attack.damage;

		if (radialFillControl != null)
		{
			radialFillControl.SetMirroredFill ((int)this.angle);
		}

		if (attackMaskControl != null)
		{
			attackMaskControl.SetSize (closeRadius, farRadius);
		}
		
		InvokeRepeating("doAttack", 0, attack.frequency);
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
