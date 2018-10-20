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

    public BossAttack(string name, float angle, RangeLevel closeRange, RangeLevel farRange, float damage, float frequency)
    {
        this.name = name;
        this.angle = angle;
        this.closeRadius = RangeUtils.rangeLevelToFloatRange(closeRange) - 0.25f;
        this.farRadius = RangeUtils.rangeLevelToFloatRange(farRange) + 0.25f;
        this.damage = damage;
        this.frequency = frequency;
    }

    bool Equals(BossAttack other)
    {
        return this.name == other.name &&
                this.angle == other.angle &&
                this.closeRadius == other.closeRadius &&
                this.farRadius == other.farRadius &&
                this.damage == other.damage &&
                this.frequency == other.frequency;
    }
}