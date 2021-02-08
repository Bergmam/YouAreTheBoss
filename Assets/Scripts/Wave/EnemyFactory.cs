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

    public static EnemySettings Projectile()
    {
        return Resources.Load<EnemySettings>("ScriptableObjects/Enemies/Projectile");
    }

    public static EnemySettings Minion()
    {
        return Resources.Load<EnemySettings>("ScriptableObjects/Enemies/Minion");
    }

    public static EnemySettings RangedCirclingMinon()
    {
        return Resources.Load<EnemySettings>("ScriptableObjects/Enemies/RangedCirclingMinion");
    }


    public static EnemySettings RangedSpawner()
    {
        return Resources.Load<EnemySettings>("ScriptableObjects/Enemies/RangedSpawner");
    }

    public static EnemySettings CirclingSpawnerMinion()
    {
        return Resources.Load<EnemySettings>("ScriptableObjects/Enemies/CirclingSpawnerMinion");
    }

    public static EnemySettings CirclingSpawner()
    {
        return Resources.Load<EnemySettings>("ScriptableObjects/Enemies/CirclingSpawner");
    }

    public static EnemySettings SmallBomber()
    {
        return Resources.Load<EnemySettings>("ScriptableObjects/Enemies/SmallBomber");
    }

    public static EnemySettings ZigZag()
    {
        return Resources.Load<EnemySettings>("ScriptableObjects/Enemies/ZigZag");
    }

    public static EnemySettings Jitterer()
    {
        return Resources.Load<EnemySettings>("ScriptableObjects/Enemies/Jitterer");
    }

}
