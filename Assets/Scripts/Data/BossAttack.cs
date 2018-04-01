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

	bool Equals(BossAttack other) {
		return this.name == other.name &&
				this.angle == other.angle && 
				this.closeRadius == other.closeRadius && 
				this.farRadius == other.farRadius &&
				this.damage == other.damage &&
				this.frequency == other.frequency;
	}
}