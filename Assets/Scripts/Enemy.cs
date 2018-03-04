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

	float angle;

	BossHealth bossHealth;

	void Start () {
		bossHealth = GameObject.Find("Boss").GetComponent<BossHealth>();
		Vector3 randomPosition = getRandomPosition(Vector3.zero, 5);
		transform.position = randomPosition;
	}

	Vector3 getRandomPosition(Vector3 center, float radius) {
		angle = Random.value * 360;
		Vector3 position;
		position.x = center.x + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
		position.y = center.y + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
		position.z = center.z;
		return position;
	}
	
	// Update is called once per frame
	void Update () {
		float step = MovementSpeed * Time.deltaTime;
		if (Vector3.Distance(Vector3.zero, transform.position) > Range) {
			transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, step);

		} else if (!IsInvoking("doDamageToBoss")) {
			InvokeRepeating("doDamageToBoss", 0, 0.5f);
		}
	}

	void doDamageToBoss() {
		bossHealth.bossTakeDamage(Damage);
	}
	public bool isInAttackArea(float lowAngle, float highAngle, float closeRadius, float farRadius){

		bool inAngle = RotationUtils.InCounterClockwiseLimits(angle, lowAngle, highAngle);
		float distanceToBoss = Vector3.Distance(Vector3.zero, transform.position);
		bool inRadius =  distanceToBoss >= closeRadius && distanceToBoss <= farRadius; 

		return inAngle && inRadius;
	}

	public void applyDamageTo(float damage){
		Health -= damage;
		
		UnityUtils.RecursiveFind(transform, "HealthBar").GetComponent<ProgressBarBehaviour>().UpdateFill(Health / MaxHealth);
		if (Health <= 0){
			Destroy(UnityUtils.RecursiveFind(transform, "HealthBar").gameObject);
			Destroy(gameObject);
		} else {
			transform.Find("Sprite").GetComponent<SpriteRenderer>().color = Color.red;
			StartCoroutine(UnityUtils.ChangeToColorAfterTime(transform.Find("Sprite").GetComponent<SpriteRenderer>(), SpriteColor, 0.5f));
		}
	}

	public void SetStats(float movementSpeed, float damage, float range, float health, float scale, Color color) {
		MovementSpeed = movementSpeed;
		Damage = damage;
		Range = range;
		Health = health;
		MaxHealth = health;
		Scale = scale;
		Transform sprite = transform.Find("Sprite");
		sprite.transform.localScale *= scale;
		SpriteColor = color;
		sprite.GetComponent<SpriteRenderer>().color = color;
	}

	void OnDestroy(){
		CancelInvoke();
	}
}
