using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO:
// * Create random/non-random versions
// * Create instances of these settings corresponding to whatever enemies we have defined already
// * For Projectiles, must set damage of the Scriptable Object upon instantiation
// * Upon enemy creation, add the correct scriptable object settings to that enemy 
//   (Instead of setting the stats in the script like we do now)
// * Tests might be breaking?


[CreateAssetMenu(menuName = "Enemy Settings")]
public class EnemySettings : ScriptableObject
{
    public string Name = "New Enemy";
    public float MovementSpeed = Parameters.BASIC_ENEMY_SPEED;
    public float Damage = 1.0f;

    // Public set/Private get to only be able to set through RangeLevel but get everywhere
    private float rangeVal = 1.0f;
    public float RangeVal {get {return rangeVal;} private set {rangeVal = value;}}
    public RangeLevel Range = RangeLevel.MELE;
    public RangeLevel RangeLevel {
        get {
            return Range;
        }
        set {
            Range = value;
            switch (value)
            {
                case RangeLevel.SELF_DESTRUCT:
                    RangeVal = Parameters.SELF_DESTRUCT_RANGE;
                    break;
                case RangeLevel.MELE:
                    RangeVal = Parameters.MELEE_RANGE;
                    break;
                case RangeLevel.MID:
                    RangeVal = Parameters.MID_RANGE;
                    break;
                case RangeLevel.LONG:
                    RangeVal = Parameters.LONG_RANGE;
                    break;
                default:
                    RangeVal = 0.0f;
                    break;
            }
        }
    }

    public float Health = Parameters.BASIC_ENEMY_HEALTH * Parameters.HEALTH_SCALE;
    public float Scale = 1.0f;
    public Color Color = Color.black;


    // Speed for moving sideways when enemies are moving forward
    public float angularSpeed = 1.0f;
    public bool angularMoveClockwise = false;
    public bool randomizeAngularDirection = false;

    // Speed for moving when enemies have reached their innermost position
    public float circlingSpeed = 1.0f;
    public bool circlingMoveClockwise = false;
    public bool randomizeCirclingDirection = false;

    public bool predefinedPosition = false;
    public float spawnAngle = 0f;
    public float spawnRadius = Parameters.ENEMY_SPAWN_RADIUS;
    public bool selfDestruct = false;
    public bool invunerable = false;
    public bool requiredKill = true;
    public EnemySettings projectile;
    public float attackDelay = 0.5f;
    public EnemyType enemyType = EnemyType.ENEMY;
    public float zigZagAngle = 0f;
    public bool zigZag = false;
    public float TurnBackDistance = 0f;
    public float TurnForwardDistance = 0f;
    public int NumberOfTurns = 0; // Negative value means keep turning forever.
}
