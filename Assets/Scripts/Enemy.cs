﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	public float AttackRadius = 1.0f;

	public float MovementSpeed = 1.0f;

	public float Damage = 1.0f;

	public float Range = 1.0f;

	public float Health = 100.0f;

	void Start () {
		Vector3 randomPosition = getRandomPosition(Vector3.zero, 5);
		transform.position = randomPosition;
	}

	Vector3 getRandomPosition(Vector3 center, float radius) {
		float angle = Random.value * 360;
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
}
