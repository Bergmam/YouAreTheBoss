using System.Collections;
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

	private Color SpriteColor = Color.white;

	private bool selfDestruct;
	private bool invunerable;
	private float attackFrequency;

	BossHealth bossHealth;
	//private ColorModifier colorModifier;

	void Start () {
		bossHealth = GameObject.Find("Boss").GetComponent<BossHealth>(); // Should all units know of the hero's health?
		this.attackFrequency = 0.5f;
	}

	void Update () {
        float step = MovementSpeed * Time.deltaTime;
        float angularStep = this.angularSpeed * Time.deltaTime;
		if (Vector3.Distance (Vector3.zero, transform.position) > Range)
		{
            RadialPosition radialPosition = RotationUtils.XYToRadialPos(this.transform.position);
			radialPosition.AddRadius ((-1) * step);
            radialPosition.AddAngle(angularStep);

            MoveTo(radialPosition);

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
		//this.colorModifier.FadeToDelected(this.attackFrequency / 3f);
		bossHealth.bossTakeDamage(Damage);
	}

	/// <summary>
	/// Spawns a projectile. Projectiles are fast, invunerable and self destructs on impact with the hero.
	/// </summary>
	void spawnProjectile ()
	{
		//this.colorModifier.FadeToDelected(this.attackFrequency / 3f);
		GameObject preInitEnemy = Resources.Load ("Prefabs/Enemy", typeof(GameObject)) as GameObject;
		GameObject initEnemy = Instantiate(preInitEnemy);
		initEnemy.GetComponent<Enemy> ().SetStats (
			Parameters.PROJECTILE_SPEED,
            0, // Projectiles do not have an angular speed;
			this.Damage,
			Parameters.PROJECTILE_RANGE,
			1.0f, //Health of projectile does not matter since they are invunerable.
			Parameters.PROJECTILE_SCALE,
			Parameters.PROJECTILE_COLOR,
			true,
			true,
            RotationUtils.XYToRadialPos(this.transform.position)
		);
		initEnemy.name = "Projectile";
		initEnemy.transform.position = transform.position;
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

		Health -= damage;
		UnityUtils.RecursiveFind(transform, "HealthBar").GetComponent<ProgressBarBehaviour>().UpdateFill(Health / MaxHealth);
		if (Health <= 0){
			KillSelf ();
		} else {
			transform.Find("Sprite").GetComponent<SpriteRenderer>().color = Color.red;
			StartCoroutine(UnityUtils.ChangeToColorAfterTime(transform.Find("Sprite").GetComponent<SpriteRenderer>(), SpriteColor, 0.5f));
		}
	}

	public void SetStats(float movementSpeed, float angularSpeed, float damage, float range,
		float health, float scale, Color color, bool selfDestruct, bool invunerable, RadialPosition radialPosition)
	{
		this.selfDestruct = selfDestruct;
		this.invunerable = invunerable;
		if (invunerable) //Don't show healthbar for invunerable units (projectiles)
		{
			Destroy(UnityUtils.RecursiveFind(transform, "HealthBar").gameObject);
		}
        MoveTo(radialPosition);
		MovementSpeed = movementSpeed;
        this.angularSpeed = angularSpeed; 
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
		/*this.colorModifier = sprite.GetComponent<ColorModifier>();
		colorModifier.SetDefaultColor(color);
		colorModifier.SetSelectedColor(Parameters.ENEMY_ATTACK_COLOR);*/
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
