﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	private float AttackRadius = 1.0f;

    private float MovementSpeed = 1.0f;
    private float angularSpeed = 0f;

	private float Damage = 20.0f;

	private float Range = 1.0f;

	private float Health = 100.0f;

	private float MaxHealth = 100.0f;

	private float Scale = 1.0f;
	private float circlingSpeed;

	private bool selfDestruct;
	private bool invunerable;
	private float attackFrequency;

	BossHealth bossHealth;
	private ColorModifier colorModifier;
	private EnemySpawner enemySpawner;

	private GameObject hitParticle;
	private StatsHolder projectile;
	private float zigZagAngleLow;
	private float zigZagAngleHigh;
	private bool zigZag;

	void Awake()
	{
		this.enemySpawner = GameObject.FindObjectOfType<EnemySpawner>();
		Transform sprite = transform.Find("Sprite");
		this.colorModifier = sprite.GetComponent<ColorModifier>();
		this.hitParticle = Resources.Load("Prefabs/hitParticleSystem", typeof(GameObject)) as GameObject;
	}

	void Start () {
		bossHealth = GameObject.Find("Boss").GetComponent<BossHealth>(); // Should all units know of the hero's health?
	}

	void Update () {
        RadialPosition radialPosition = RotationUtils.XYToRadialPos(this.transform.position);
        float step = MovementSpeed * Time.deltaTime;
        float angularStep = this.angularSpeed * Time.deltaTime;
        float circlingStep = this.circlingSpeed * Time.deltaTime;
		if (Vector3.Distance (Vector3.zero, transform.position) > Range)
		{
			radialPosition.AddRadius ((-1) * step);
            radialPosition.AddAngle(angularStep);

            MoveTo(radialPosition);

			if(RotationUtils.InCounterClockwiseLimits(radialPosition.GetAngle(),zigZagAngleHigh, zigZagAngleLow) && zigZag){
				angularSpeed = -1 * angularSpeed;
			}

		}
		else // If in range, do appropriate attack.
		{
			radialPosition.AddAngle(circlingStep);

            MoveTo(radialPosition);

			if(RotationUtils.InCounterClockwiseLimits(radialPosition.GetAngle(),zigZagAngleHigh, zigZagAngleLow) && zigZag){
				circlingSpeed = -1 * circlingSpeed;
			}

			MoveTo(radialPosition);
			if (selfDestruct) {
				doDamageToBoss ();
				KillSelf ();
			}
			// Mele attack
			else if (Range <= Parameters.MAX_MELE_RANGE && !IsInvoking ("doDamageToBoss"))
			{
				InvokeRepeating ("doDamageToBoss", 0, this.attackFrequency);
			}
			// Ranged attack
			else if (Range > Parameters.MAX_MELE_RANGE && !IsInvoking ("spawnProjectile"))
			{
				InvokeRepeating ("spawnProjectile", 0, this.attackFrequency);
			}
		}

	}

	void doDamageToBoss() {
		this.colorModifier.FadeToDelected(this.attackFrequency / 3f);
		GameObject hitParticle = Instantiate(this.hitParticle, transform.position / 2, transform.rotation);
		var main = hitParticle.GetComponent<ParticleSystem>().main;
		main.startColor = new Color(0.3f, 0.082f, 0.3945f, 0.6f);
		Destroy(hitParticle, hitParticle.GetComponent<ParticleSystem>().main.duration);
		bossHealth.bossTakeDamage(Damage);
	}

	/// <summary>
	/// Spawns a projectile. Projectiles are fast, invunerable and self destructs on impact with the hero.
	/// </summary>
	void spawnProjectile ()
	{
		this.colorModifier.FadeToDelected(this.attackFrequency / 3f);
		RadialPosition thisRadialPos = RotationUtils.XYToRadialPos(transform.position);
		this.projectile.SetRadialSpawnPosition(thisRadialPos.GetAngle(),thisRadialPos.GetRadius());
		this.enemySpawner.InstantiateEnemyPrefab(this.projectile);
	}

	public bool isInAttackArea(float lowAngle, float highAngle, float nearRadius, float farRadius){

		float spriteRadius = transform.Find("Sprite").GetComponent<SpriteRenderer>().bounds.size.x / 2;
		float distanceToBossActual = Mathf.Max(Vector3.Distance(Vector3.zero, transform.position), 0);
		float distanceToBossFar = distanceToBossActual + spriteRadius;
		float distanceToBossNear = distanceToBossActual - spriteRadius;

        RadialPosition radialPosition = RotationUtils.XYToRadialPos(this.transform.position);

		float enemyWidthAngle = Mathf.Rad2Deg * Mathf.Acos(1 - Mathf.Pow(spriteRadius / Mathf.Sqrt(2 * distanceToBossActual), 2));
        float enemyHighAngle = radialPosition.GetAngle() + enemyWidthAngle;
        float enemyLowAngle = radialPosition.GetAngle() - enemyWidthAngle;

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

		GameObject hitParticle = Instantiate(this.hitParticle, transform.position, transform.rotation);
		var main = hitParticle.GetComponent<ParticleSystem>().main;
		main.startColor = new Color(1.0f, 0.0f, 0.0f, 1.0f);
		Destroy(hitParticle, hitParticle.GetComponent<ParticleSystem>().main.duration);

		Health -= damage;
		UnityUtils.RecursiveFind(transform, "HealthBar").GetComponent<ProgressBarBehaviour>().UpdateFill(Health / MaxHealth);
		if (Health <= 0){
			KillSelf ();
		} else {
			transform.Find("Sprite").GetComponent<SpriteRenderer>().color = Color.red;
			StartCoroutine(UnityUtils.ChangeToDefaultColorAfterTime(colorModifier, 0.5f));
		}
	}

	public void SetStats(StatsHolder stats)
	{
		transform.name = stats.Name;
		this.circlingSpeed = stats.circlingSpeed;
		this.selfDestruct = stats.selfDestruct;
		this.invunerable = stats.invunerable;
		if (invunerable) //Don't show healthbar for invunerable units (projectiles)
		{
			Destroy(UnityUtils.RecursiveFind(transform, "HealthBar").gameObject);
		}
        MoveTo(new RadialPosition(stats.spawnRadius, stats.spawnAngle));
		MovementSpeed = stats.MovementSpeed;
        this.angularSpeed = stats.angularSpeed; 
		Damage = stats.Damage;
		Range = stats.Range;
		Health = stats.Health;
		MaxHealth = stats.Health;
		Scale = stats.Scale;
		Transform sprite = transform.Find("Sprite");
		sprite.transform.localScale *= stats.Scale;
		if (stats.Scale > 1) {
			Transform canvas = transform.Find("Canvas");
			canvas.localPosition = new Vector3(canvas.localPosition.x, canvas.localPosition.y * stats.Scale * 0.8f, canvas.localPosition.z);
		}
		if(stats.projectile == null){
			this.projectile = EnemyFactory.Projectile(this.Damage);
		}else{
			this.projectile = stats.projectile;
		}
		this.attackFrequency = stats.attackDelay;
		this.zigZag = stats.zigZag;
		if(stats.angularSpeed > 0){
			this.zigZagAngleLow = stats.spawnAngle;
			this.zigZagAngleHigh = stats.spawnAngle + stats.zigZagAngle;
		} else {
			this.zigZagAngleHigh = stats.spawnAngle;
			this.zigZagAngleLow = stats.spawnAngle - stats.zigZagAngle;
		}
		colorModifier.SetDefaultColor(stats.Color);
		colorModifier.SetSelectedColor(Parameters.ENEMY_ATTACK_COLOR);
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

    public void MoveTo(RadialPosition radialPosition)
    {
        transform.position = RotationUtils.RadialPosToXY(radialPosition);
    }

}
