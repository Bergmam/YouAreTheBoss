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
	}

	public StatsHolder(string name, 
		float MovementSpeed, 
		float Damage, 
		float Range, 
		float Health, 
		float Scale,
		Color Color,
		float spawnAngle) {
		this.Name = name;
		this.MovementSpeed = MovementSpeed;
		this.Damage = Damage;
		this.Range = Range;
		this.Health = Health;
		this.Scale = Scale;
		this.Color = Color;
        this.angularSpeed = 0f;
		this.predefinedPosition = true;
		this.spawnAngle = spawnAngle;
	}

    public void SetAngluarSpeed(float angularSpeed)
    {
        this.angularSpeed = angularSpeed;
    }
}