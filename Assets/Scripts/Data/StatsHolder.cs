using System.Collections.Generic;
using UnityEngine;

public enum EnemyType {
    ENEMY,
    PROJECTILE,
    MINION
}

public class StatsHolder
{
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
    public EnemyType enemyType = EnemyType.ENEMY;
    public float zigZagAngle;
    public bool zigZag;
    public float TurnBackDistance;
    public float TurnForwardDistance;
    public int NumberOfTurns; // Negative value means keep turning forever.

    public StatsHolder(EnemySettings enemySettings)
    {
        this.Name = enemySettings.name;
        this.MovementSpeed = enemySettings.MovementSpeed;
        this.Damage = enemySettings.Damage;
        // this.Range = RangeUtils.rangeLevelToFloatRange(Range);
        this.Health = Health * Parameters.HEALTH_SCALE;
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

    public bool IsDurable()
    {
        return this.Health >= Parameters.DURABLE_ENEMY_MIN_HEALTH;
    }

    public bool IsStrong()
    {
        return this.Damage >= Parameters.STRONG_ENEMY_MIN_DAMAGE;
    }

    public bool IsFast()
    {
        return this.MovementSpeed >= Parameters.FAST_ENEMY_MIN_SPEED;
    }

    public bool IsRotating()
    {
        return this.angularSpeed != 0 || this.circlingSpeed != 0;
    }

    public bool IsRanged()
    {
        return this.Range > Parameters.MELEE_RANGE;
    }

    public Dictionary<string, bool> GetAttributes()
    {
        Dictionary<string, bool> attributes = new Dictionary<string, bool>();
        attributes.Add("strong", this.IsStrong());
        attributes.Add("fast", this.IsFast());
        attributes.Add("rotating", this.IsRotating());
        attributes.Add("ranged", IsRanged());
        attributes.Add("durable", IsDurable());
        attributes.Add("mele", !IsRanged());
        attributes.Add("self_destruct", this.selfDestruct);
        return attributes;
    }

    // public StatsHolder Clone()
    // {
    //     StatsHolder clone = new StatsHolder(this.Name,
    //         this.MovementSpeed,
    //         this.Damage,
    //         RangeUtils.floatRangeToRangeLevel(this.Range),
    //         this.Health,
    //         this.Scale,
    //         this.Color);
    //     clone.predefinedPosition = this.predefinedPosition;
    //     clone.spawnAngle = this.spawnAngle;
    //     clone.spawnRadius = this.spawnRadius;
    //     clone.selfDestruct = this.selfDestruct;
    //     clone.invunerable = this.invunerable;
    //     clone.angularSpeed = this.angularSpeed;
    //     clone.circlingSpeed = this.circlingSpeed;
    //     clone.requiredKill = this.requiredKill;
    //     if (this.projectile != null)
    //     {
    //         clone.projectile = this.projectile.Clone();
    //     }
    //     clone.projectile = this.projectile;
    //     clone.attackDelay = this.attackDelay;
    //     clone.zigZagAngle = this.zigZagAngle;
    //     clone.zigZag = this.zigZag;
    //     return clone;
    // }

    public void SetRange(RangeLevel range)
    {
        this.Range = RangeUtils.rangeLevelToFloatRange(range);
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        StatsHolder other = (StatsHolder)obj;

        bool equals = true;

        if (this.projectile == null)
        {
            equals &= this.projectile == other.projectile;
        }
        else
        {
            equals &= other.projectile == null;
        }

        equals &= (
            other.predefinedPosition == this.predefinedPosition &&
            other.spawnAngle == this.spawnAngle &&
            other.spawnRadius == this.spawnRadius &&
            other.selfDestruct == this.selfDestruct &&
            other.invunerable == this.invunerable &&
            other.angularSpeed == this.angularSpeed &&
            other.circlingSpeed == this.circlingSpeed &&
            other.requiredKill == this.requiredKill &&
            other.projectile == this.projectile &&
            other.attackDelay == this.attackDelay &&
            other.zigZagAngle == this.zigZagAngle &&
            other.zigZag == this.zigZag &&
            other.Name == this.Name &&
            other.MovementSpeed == this.MovementSpeed &&
            other.Damage == this.Damage &&
            other.Range == this.Range &&
            other.Health == this.Health &&
            other.Scale == this.Scale &&
            other.Color == this.Color
        );

        return equals;
    }

    public override int GetHashCode()
    {
        return (int)(
            3 * Name.GetHashCode() % 100 +
            3 * MovementSpeed +
            3 * angularSpeed +
            3 * Damage +
            3 * Range +
            3 * Health +
            3 * Scale +
            3 * Color.GetHashCode() % 100 +
            (predefinedPosition ? 3 : 0) +
            3 * spawnAngle +
            3 * spawnRadius +
            (selfDestruct ? 3 : 0) +
            (invunerable ? 3 : 0) +
            3 * circlingSpeed +
            (requiredKill ? 3 : 0) +
            (projectile != null ? 3 * projectile.GetHashCode() % 100 : 0) +
            3 * attackDelay +
            3 * zigZagAngle
        );
    }
}