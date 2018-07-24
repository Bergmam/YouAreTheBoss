using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack
{
    public string name;
    public float angle;
    public float closeRadiusScale;
    public float farRadiusScale;
    public float damage;
    public float frequency;

    public BossAttack(string name, float angle, float closeRadiusScale, float farRadiusClase, float damage, float frequency)
    {
        this.name = name;
        this.angle = angle;
        this.closeRadiusScale = closeRadiusScale;
        this.farRadiusScale = farRadiusClase;
        this.damage = damage;
        this.frequency = frequency;
    }

    bool Equals(BossAttack other)
    {
        return this.name == other.name &&
                this.angle == other.angle &&
                this.closeRadiusScale == other.closeRadiusScale &&
                this.farRadiusScale == other.farRadiusScale &&
                this.damage == other.damage &&
                this.frequency == other.frequency;
    }
}