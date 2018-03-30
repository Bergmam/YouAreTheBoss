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
	}

    public void SetAngluarSpeed(float angularSpeed)
    {
        this.angularSpeed = angularSpeed;
    }
}