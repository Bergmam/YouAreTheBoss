using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack
{
    public string name;
    public float angle;
    public float closeRadius;
    public float farRadius;
    public float damage;
    public float frequency;
    public bool isProjectile;

    public BossAttack(string name, float angle, RangeLevel closeRange, RangeLevel farRange, float damage, float frequency, bool isProjectile)
    {
        this.name = name;
        this.angle = angle;
        this.closeRadius = Mathf.Max(RangeUtils.rangeLevelToFloatRange(closeRange) - 0.25f, 0);
        this.farRadius = Mathf.Min(RangeUtils.rangeLevelToFloatRange(farRange) + 0.75f, Parameters.MAX_ATTACK_RADIUS);
        this.damage = damage;
        this.frequency = frequency;
        this.isProjectile = isProjectile;
    }

    bool Equals(BossAttack other)
    {
        return this.name == other.name &&
                this.angle == other.angle &&
                this.closeRadius == other.closeRadius &&
                this.farRadius == other.farRadius &&
                this.damage == other.damage &&
                this.frequency == other.frequency &&
                this.isProjectile == other.isProjectile;
    }
}