using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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