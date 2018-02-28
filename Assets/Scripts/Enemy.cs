using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	public float AttackRadius = 1.0f;

	public float MovementSpeed = 1.0f;

	public float Damage = 1.0f;

	public float Range = 1.0f;

	public float Health = 100.0f;

	float angle;

	void Start () {
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
		}   
	}
	public bool isInAttackArea(float lowAngle, float highAngle, float closeRadius, float farRadius){

		bool inAngle = RotationUtils.InCounterClockwiseLimits(angle, lowAngle, highAngle);

		float distanceToBoss = Vector3.Distance(Vector3.zero, transform.position);
		bool inRadius =  distanceToBoss >= closeRadius && distanceToBoss <= farRadius; 

		return inAngle && inRadius;
	}

	public void applyDamageTo(float damage){
		Health -= damage;
		UnityUtils.RecursiveFind(transform, "HealthBar").GetComponent<ProgressBarBehaviour>().UpdateFill(Health / 100.0f);
		if (Health <= 0){
			Destroy(UnityUtils.RecursiveFind(transform, "HealthBar").gameObject);
			Destroy(gameObject);
		} else {
			gameObject.GetComponent<SpriteRenderer>().color = Color.red;
			StartCoroutine(WhiteColorAfterTime(gameObject.GetComponent<SpriteRenderer>()));
		}
	}

	IEnumerator WhiteColorAfterTime(SpriteRenderer renderer)
    {
        yield return new WaitForSeconds(0.5f);
		renderer.color = Color.white;
    }
}
