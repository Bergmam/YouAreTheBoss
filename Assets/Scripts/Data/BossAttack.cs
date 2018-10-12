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

    public BossAttack(string name, float angle, RangeLevel closeRange, RangeLevel farRange, float damage, float frequency)
    {
        float closeRangeFloat = RangeUtils.rangeLevelToFloatRange(closeRange);
        float farRangeFloat = RangeUtils.rangeLevelToFloatRange(farRange);
        float maxRange = RangeUtils.rangeLevelToFloatRange(RangeLevel.LONG);
        this.name = name;
        this.angle = angle;
        this.closeRadiusScale = closeRangeFloat / maxRange;
        this.farRadiusScale = farRangeFloat / maxRange;
        this.damage = damage;
        this.frequency = frequency;

        if (closeRange == farRange)
        {
            this.closeRadiusScale -= 0.05f;
            this.farRadiusScale += 0.05f;
        }
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