using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Settings")]
public class EnemySettings : ScriptableObject
{
    public string Name = "New Enemy";
    public float MovementSpeed = Parameters.BASIC_ENEMY_SPEED;
    public float Damage = 1.0f;

    public RangeLevel Range = RangeLevel.MELE;
    public float Health = Parameters.BASIC_ENEMY_HEALTH * Parameters.HEALTH_SCALE;
    public float Scale = 1.0f;
    public Color Color = Color.black;
    public Sprite Sprite;
    public Material Material;

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
    public float InitialAttackDelay = 1.0f;
    public EnemyType enemyType = EnemyType.ENEMY;
    public float zigZagAngle = 0f;
    public bool zigZag = false;
    public float TurnBackDistance = 0f;
    public float TurnForwardDistance = 0f;
    public int NumberOfTurns = 0; // Negative value means keep turning forever.

    public List<HealthInitiatedEnemyAction> HealthInitiatedEnemyActions;
    public List<TimeInitiatedEnemyAction> TimeInitiatedEnemyActions;
    public float SpawnHealth = -1;
}
