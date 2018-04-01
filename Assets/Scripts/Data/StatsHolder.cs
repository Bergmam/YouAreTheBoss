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
	}

	public void SetRadialSpawnPosition(float spawnAngle, float spawnRadius)
	{
		this.predefinedPosition = true;
		this.spawnAngle = spawnAngle;
		this.spawnRadius = spawnRadius;
	}
}