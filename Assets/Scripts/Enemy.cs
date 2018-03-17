using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	private float AttackRadius = 1.0f;

	private float MovementSpeed = 1.0f;

	private float Damage = 20.0f;

	private float Range = 1.0f;

	private float Health = 100.0f;

	private float MaxHealth = 100.0f;

	private float Scale = 1.0f;

	private Color SpriteColor = Color.white;

	private float angle;

	private bool selfDestruct;
	private bool invunerable;

	BossHealth bossHealth;

	void Start () {
		bossHealth = GameObject.Find("Boss").GetComponent<BossHealth>(); // Should all units know of the hero's health?
	}

	void Update () {
		float step = MovementSpeed * Time.deltaTime;
		if (Vector3.Distance (Vector3.zero, transform.position) > Range)
		{
			transform.position = Vector3.MoveTowards (transform.position, Vector3.zero, step);

		}
		else // If in range, do appropriate attack.
		{
			if (selfDestruct) {
				doDamageToBoss ();
				KillSelf ();
			}
			// Mele attack
			else if (Range <= Parameters.MAX_MELE_RANGE && !IsInvoking ("doDamageToBoss"))
			{
				InvokeRepeating ("doDamageToBoss", 0, 0.5f);
			}
			// Ranged attack
			else if (Range > Parameters.MAX_MELE_RANGE && !IsInvoking ("spawnProjectile"))
			{
				InvokeRepeating ("spawnProjectile", 0, 0.5f);
			}
		}

	}

	void doDamageToBoss() {
		bossHealth.bossTakeDamage(Damage);
	}

	/// <summary>
	/// Spawns a projectile. Projectiles are fast, invunerable and self destructs on impact with the hero.
	/// </summary>
	void spawnProjectile ()
	{
		GameObject preInitEnemy = Resources.Load ("Prefabs/Enemy", typeof(GameObject)) as GameObject;
		GameObject initEnemy = Instantiate(preInitEnemy);
		initEnemy.GetComponent<Enemy> ().SetStats (
			Parameters.PROJECTILE_SPEED,
			this.Damage,
			Parameters.PROJECTILE_RANGE,
			1.0f, //Health of projectile does not matter since they are invunerable.
			Parameters.PROJECTILE_SCALE,
			Parameters.PROJECTILE_COLOR,
			true,
			true,
			this.angle
		);
		initEnemy.name = "Projectile";
		initEnemy.transform.position = transform.position;
	}

	public bool isInAttackArea(float lowAngle, float highAngle, float nearRadius, float farRadius){

		float spriteRadius = transform.Find("Sprite").GetComponent<SpriteRenderer>().bounds.size.x / 2;
		float distanceToBossActual = Mathf.Max(Vector3.Distance(Vector3.zero, transform.position), 0);
		float distanceToBossFar = distanceToBossActual + spriteRadius;
		float distanceToBossNear = distanceToBossActual - spriteRadius;

		float enemyWidthAngle = Mathf.Rad2Deg * Mathf.Acos(1 - Mathf.Pow(spriteRadius / Mathf.Sqrt(2 * distanceToBossActual), 2));
		float enemyHighAngle = angle + enemyWidthAngle;
		float enemyLowAngle = angle - enemyWidthAngle;

		bool inHighAngle = RotationUtils.InCounterClockwiseLimits(enemyHighAngle, lowAngle, highAngle);
		bool inLowAngle = RotationUtils.InCounterClockwiseLimits(enemyLowAngle, lowAngle, highAngle);
		bool bossLargerThanRadius = RotationUtils.InCounterClockwiseLimits(lowAngle, enemyLowAngle, enemyHighAngle) 
										&& RotationUtils.InCounterClockwiseLimits(highAngle, enemyLowAngle, enemyHighAngle);
		bool inRadius =  distanceToBossNear <= farRadius && distanceToBossFar >= nearRadius; 
		
		return (inLowAngle || inHighAngle || bossLargerThanRadius) && inRadius;
	}

	public void applyDamageTo(float damage)
	{
		if (invunerable)
		{
			return;
		}

		Health -= damage;
		UnityUtils.RecursiveFind(transform, "HealthBar").GetComponent<ProgressBarBehaviour>().UpdateFill(Health / MaxHealth);
		if (Health <= 0){
			KillSelf ();
		} else {
			transform.Find("Sprite").GetComponent<SpriteRenderer>().color = Color.red;
			StartCoroutine(UnityUtils.ChangeToColorAfterTime(transform.Find("Sprite").GetComponent<SpriteRenderer>(), SpriteColor, 0.5f));
		}
	}

	public void SetStats(float movementSpeed, float damage, float range,
		float health, float scale, Color color, bool selfDestruct, bool invunerable, float angle)
	{
		this.selfDestruct = selfDestruct;
		this.invunerable = invunerable;
		if (invunerable) //Don't show healthbar for invunerable units (projectiles)
		{
			Destroy(UnityUtils.RecursiveFind(transform, "HealthBar").gameObject);
		}
		this.angle = angle;
		MovementSpeed = movementSpeed;
		Damage = damage;
		Range = range;
		Health = health;
		MaxHealth = health;
		Scale = scale;
		Transform sprite = transform.Find("Sprite");
		sprite.transform.localScale *= scale;
		if (scale > 1) {
			Transform canvas = transform.Find("Canvas");
			canvas.localPosition = new Vector3(canvas.localPosition.x, canvas.localPosition.y * 1.5f, canvas.localPosition.z);

		}
		SpriteColor = color;
		sprite.GetComponent<SpriteRenderer>().color = color;
	}

	void OnDestroy(){
		CancelInvoke();
	}

	private void KillSelf(){
		//Invunerable units' helthbars are already removed so when they die, healthbar is null.
		Transform healthBarTransform = UnityUtils.RecursiveFind (transform, "HealthBar");
		if (healthBarTransform != null) {
			Destroy (healthBarTransform.gameObject);
		}

		Destroy(gameObject);
	}
}
