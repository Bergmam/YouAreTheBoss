using System.Collections.Generic;
using UnityEngine;

public class StatsHolder {
	public string Name;
    public float MovementSpeed;
    public float angularSpeed;
	public float Damage;
	public float Range;
	public float Health;
	public float Scale;
	public Color Color;
	public bool predefinedPosition;
	public float spawnAngle;
	public float spawnRadius;
	public bool selfDestruct;
	public bool invunerable;
	public float circlingSpeed;
	public bool requiredKill;
	public StatsHolder projectile;
	public float attackDelay;

	public float zigZagAngle;
	public bool zigZag;

	public StatsHolder(string name, 
		float MovementSpeed, 
		float Damage, 
		float Range, 
		float Health, 
		float Scale,
		Color Color) {
		this.Name = name;
		this.MovementSpeed = MovementSpeed;
		this.Damage = Damage;
		this.Range = Range;
		this.Health = Health;
		this.Scale = Scale;
		this.Color = Color;
        this.angularSpeed = 0f;
		this.predefinedPosition = false;
		this.spawnRadius = Parameters.ENEMY_SPAWN_RADIUS;
		this.requiredKill = true;
		this.attackDelay = 0.5f;
		this.zigZag = false;
	}

	public void SetRadialSpawnPosition(float spawnAngle, float spawnRadius)
	{
		this.predefinedPosition = true;
		this.spawnAngle = spawnAngle;
		this.spawnRadius = spawnRadius;
	}

	public bool IsDurable(){
		return this.Health >= Parameters.DURABLE_ENEMY_MIN_HEALTH;
	}

	public bool IsStrong(){
		return this.Damage >= Parameters.STRONG_ENEMY_MIN_DAMAGE;
	}

	public bool IsFast(){
		return this.MovementSpeed >= Parameters.FAST_ENEMY_MIN_SPEED;
	}

	public bool IsRotating(){
		return this.angularSpeed != 0 || this.circlingSpeed != 0;
	}

	public bool IsRanged(){
		return this.Range > Parameters.MAX_MELE_RANGE;
	}

	public Dictionary<string, bool> GetAttributes()
	{
		Dictionary<string,bool> attributes = new Dictionary<string,bool>();
		attributes.Add("strong", this.IsStrong());
		attributes.Add("fast", this.IsFast());
		attributes.Add("rotating", this.IsRotating());
		attributes.Add("ranged", IsRanged());
		attributes.Add("durable", IsDurable());
		attributes.Add("mele", !IsRanged());
		attributes.Add("self_destruct", this.selfDestruct);
		return attributes;
	}
}