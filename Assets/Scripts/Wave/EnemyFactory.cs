using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory
{

    private static Color orange = new Color(1.0f, 0.6f, 0.2f, 1.0f);
    private static Color lightGreen = new Color(0.6f, 1.0f, 0.6f, 1.0f);

    private static List<Func<EnemySettings>> basicEnemyGenerators = new List<Func<EnemySettings>>()
    {
        StandardEnemy,
        FastEnemy,
        Rotator,
        RangedCirclingEnemy,
        ZigZag
    };

    public static EnemySettings RandomBasicEnemy()
    {
        int randomIndex = (int)Mathf.Round(UnityEngine.Random.Range(0, basicEnemyGenerators.Count));
        return basicEnemyGenerators[randomIndex]();
    }

    public static EnemySettings SlowEnemy()
    {
        return Resources.Load<EnemySettings>("ScriptableObjects/Enemies/SlowEnemy");
    }

    public static EnemySettings Rotator()
    {
        return Resources.Load<EnemySettings>("ScriptableObjects/Enemies/Rotator");
    }

    public static EnemySettings FastEnemy()
    {
        return Resources.Load<EnemySettings>("ScriptableObjects/Enemies/FastEnemy");
    }

    public static EnemySettings RangedCirclingEnemy()
    {
        return Resources.Load<EnemySettings>("ScriptableObjects/Enemies/RangedCirclingEnemy");
    }

    public static EnemySettings StandardEnemy()
    {
        return Resources.Load<EnemySettings>("ScriptableObjects/Enemies/StandardEnemy");
    }

    public static EnemySettings Projectile(float damage)
    {
        // EnemySettings stats = new EnemySettings(
        //     "Projectile",
        //     Parameters.PROJECTILE_SPEED,
        //     damage,
        //     RangeLevel.SELF_DESTRUCT,
        //     1.0f, //Health of projectile does not matter since they are invunerable.
        //     Parameters.PROJECTILE_SCALE,
        //     Parameters.PROJECTILE_COLOR
        // );
        // stats.selfDestruct = true;
        // stats.invunerable = true;
        // stats.requiredKill = false;
        // stats.enemyType = EnemyType.PROJECTILE;
        // return stats;

        return Resources.Load<EnemySettings>("ScriptableObjects/Enemies/Projectile");
    }

    public static EnemySettings Minion()
    {
        // EnemySettings stats = new EnemySettings("Minion", 2.0f, 0.5f, RangeLevel.MELE, 0.5f, 0.5f, Color.black);
        // stats.requiredKill = false;
        // stats.enemyType = EnemyType.MINION;
        // return stats;

        return Resources.Load<EnemySettings>("ScriptableObjects/Enemies/Minion");
    }

    public static EnemySettings RangedCirclingMinon()
    {
        // EnemySettings stats = new EnemySettings("CirclingMinion", 4.0f, 2.0f, RangeLevel.MID, 10.0f, 0.5f, Color.white);
        // stats.circlingSpeed = -70f;
        // stats.requiredKill = false;
        // stats.enemyType = EnemyType.MINION;
        // return stats;
        return Resources.Load<EnemySettings>("ScriptableObjects/Enemies/RangedCirclingMinion");
    }


    public static EnemySettings RangedSpawner()
    {
        // EnemySettings stats = new EnemySettings("Mad Spawner", 4.0f, 1.0f, RangeLevel.MID, 400.0f, 3.0f, Color.grey);
        // stats.attackDelay = 2.0f;
        // stats.projectile = RangedCirclingMinon();
        // return stats;
        return Resources.Load<EnemySettings>("ScriptableObjects/Enemies/RangedSpawner");
    }

    public static EnemySettings CirclingSpawnerMinion()
    {
        // EnemySettings stats = new EnemySettings("Standard Enemy", 0.8f, 2.5f, RangeLevel.MELE, 50.0f, 1.0f, lightGreen);
        // stats.enemyType = EnemyType.MINION;
        // return stats;
        return Resources.Load<EnemySettings>("ScriptableObjects/Enemies/CirclingSpawnerMinion");
    }

    public static EnemySettings CirclingSpawner()
    {
        // EnemySettings stats = new EnemySettings("Circling Spawner", 4.0f, 1.0f, RangeLevel.MID, 300.0f, 1.0f, Color.green);
        // stats.attackDelay = 2.5f;
        // stats.projectile = CirclingSpawnerMinion();
        // bool clockwise = UnityEngine.Random.value >= 0.5;
        // if (clockwise)
        // {
        //     stats.circlingSpeed = -30f;
        // }
        // else
        // {
        //     stats.circlingSpeed = 30f;
        // }
        // return stats;
        return Resources.Load<EnemySettings>("ScriptableObjects/Enemies/CirclingSpawner");
    }

    public static EnemySettings SmallBomber()
    {
        // EnemySettings stats = new EnemySettings("Small Bomber", 7.5f, RangeLevel.SELF_DESTRUCT, 1.0f, orange);
        // stats.angularSpeed = 20f;
        // stats.selfDestruct = true;
        // return stats;
        return Resources.Load<EnemySettings>("ScriptableObjects/Enemies/SmallBomber");
    }

    public static EnemySettings ZigZag()
    {
        return Resources.Load<EnemySettings>("ScriptableObjects/Enemies/ZigZag");
    }

    public static EnemySettings Jitterer()
    {
        // EnemySettings stats = new EnemySettings("Jitterer", 7.5f, RangeLevel.SELF_DESTRUCT, Parameters.BASIC_RANGED_ENEMY_HEALTH, 1.5f, orange);
        // stats.MovementSpeed = 10.0f;
        // stats.selfDestruct = true;
        // stats.TurnBackDistance = 2.0999f;
        // stats.TurnForwardDistance = 2.1000f;
        // stats.NumberOfTurns = 20;

        // stats.angularSpeed = 100;
        // stats.zigZag = true;
        // stats.zigZagAngle = 5;

        // return stats;
        return Resources.Load<EnemySettings>("ScriptableObjects/Enemies/Jitterer");
    }

}
