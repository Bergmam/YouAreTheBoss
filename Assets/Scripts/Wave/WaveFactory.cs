using System.Collections.Generic;
using UnityEngine;
using System;


public class WaveFactory
{

    private static List<Func<Wave>> fastWaveComponents = new List<Func<Wave>>()
    {
        RotatingWorm,
        ClosingRotatingCircle,
        RangedShooters,
        TwoZigZags,
        BomberCluster,
        LineOfRanged,
        ZigZagWorm,
        WeakCluster
    };

    private static List<Func<Wave>> slowWaveComponents = new List<Func<Wave>>()
    {
        OppositeSides,
        RangedSpawner,
        OneZigZag,
        OneBigGuy,
        CirclingSpawner,
        ThreeBombs
    };

    public static Wave GenerateWave(int level)
    {
        level++;
        switch (level)
        {
            case 1:
                return FirstWave();
            case 2:
                return SecondWave();
            case 3:
                return ThirdWave();
            case 4:
                return FourthWave();
            default:
                Wave wave = new Wave();
                while (level > 0)
                {
                    Wave component = RandomFastWaveComponent();
                    if (level % 4 == 0)
                    {
                        component.Merge(RandomSlowWaveComponent());
                    }
                    wave.Append(component);
                    level--;
                }
                return wave;
        }

    }

    private static Wave RandomSlowWaveComponent()
    {
        int randomIndex = (int)Mathf.Round(UnityEngine.Random.Range(0, slowWaveComponents.Count));
        return slowWaveComponents[randomIndex]();
    }

    private static Wave RandomFastWaveComponent()
    {
        int randomIndex = (int)Mathf.Round(UnityEngine.Random.Range(0, fastWaveComponents.Count));
        return fastWaveComponents[randomIndex]();
    }

    private static Wave FirstWave()
    {
        Wave wave = new Wave();
        SubWave subWave = new SubWave();
        subWave.AddEnemy(EnemyFactory.StandardEnemy());
        wave.AddSubWave(subWave, 0.0f);
        return wave;
    }


    private static Wave SecondWave()
    {
        Wave wave = FirstWave();
        wave.Merge(FirstWave());
        return wave;
    }

    private static Wave ThirdWave()
    {
        Wave wave = new Wave();
        SubWave subWave = new SubWave();
        StatsHolder enemy = EnemyFactory.RandomBasicEnemy();
        subWave.AddEnemy(enemy);
        subWave.AddEnemy(enemy);
        wave.AddSubWave(subWave, 0.0f);
        return wave;
    }

    private static Wave FourthWave()
    {
        Wave wave = ThirdWave();
        wave.Merge(ThirdWave(), 3.0f);
        return wave;
    }

    public static Wave RotatingWorm()
    {
        bool clockwise = UnityEngine.Random.value >= 0.5;
        int direction = clockwise ? -2 : 2;
        Wave wave = new Wave();
        float timeStamp = 0f;
        float angle = UnityEngine.Random.value * 360;
        for (int i = 0; i < 5; i++)
        {
            SubWave subWave = new SubWave();
            StatsHolder stats = EnemyFactory.Rotator(clockwise);
            stats.spawnAngle = angle + i * direction;
            stats.predefinedPosition = true;
            subWave.AddEnemy(stats);
            wave.AddSubWave(subWave, timeStamp);
            timeStamp += 0.1f;
        }
        return wave;
    }

    public static Wave ZigZagWorm()
    {
        float direction = 3.0f;
        Wave wave = new Wave();
        float timeStamp = 0f;
        float angle = UnityEngine.Random.value * 360;
        for (int i = 0; i < 10; i++)
        {
            SubWave subWave = new SubWave();
            StatsHolder stats = EnemyFactory.ZigZag();
            stats.Health = 0.1f;
            stats.Damage = 0.5f;
            stats.zigZagAngle = 120.0f;
            stats.spawnAngle = angle + i * direction;
            stats.predefinedPosition = true;
            subWave.AddEnemy(stats);
            wave.AddSubWave(subWave, timeStamp);
            timeStamp += 0.05f;
        }
        return wave;
    }

    public static Wave RangedSpawner()
    {
        Wave wave = new Wave();
        SubWave subWave = new SubWave();
        StatsHolder stats = EnemyFactory.RangedSpawner();
        subWave.AddEnemy(stats);
        wave.AddSubWave(subWave, 0.0f);
        return wave;
    }

    public static Wave CirclingSpawner()
    {
        Wave wave = new Wave();
        SubWave subWave = new SubWave();
        StatsHolder stats = EnemyFactory.CirclingSpawner();
        subWave.AddEnemy(stats);
        wave.AddSubWave(subWave, 0.0f);
        return wave;
    }

    public static Wave ClosingRotatingCircle()
    {
        Wave wave = new Wave();
        SubWave subWave = new SubWave();
        for (int i = 0; i < 5; i++)
        {
            StatsHolder stats = EnemyFactory.Rotator(true);
            stats.predefinedPosition = true;
            subWave.AddEnemy(stats);
        }
        subWave.SpreadOut();
        wave.AddSubWave(subWave, 0.0f);
        return wave;
    }

    public static Wave OppositeSides()
    {
        Wave wave = new Wave();
        float angleA = UnityEngine.Random.value * 360;
        float angleB = (angleA + 180) % 360;
        for (int i = 0; i < 5; i++)
        {
            SubWave subWave = new SubWave();
            Wave tempWave = new Wave();
            StatsHolder enemy = EnemyFactory.StandardEnemy();
            enemy.Health = 100.0f;
            enemy.predefinedPosition = true;
            enemy.spawnAngle = i % 2 == 0 ? angleA : angleB;
            subWave.AddEnemy(enemy);
            StatsHolder enemy2 = enemy.Clone();
            enemy2.spawnAngle = enemy.spawnAngle + 5.0f;
            subWave.AddEnemy(enemy2);
            StatsHolder enemy3 = enemy.Clone();
            enemy3.spawnAngle = enemy.spawnAngle + 10.0f;
            subWave.AddEnemy(enemy3);
            tempWave.AddSubWave(subWave, 0.0f);
            wave.Merge(tempWave, 2.0f * i);
        }
        return wave;
    }

    public static Wave BomberCluster()
    {
        Wave wave = new Wave();
        float angle = UnityEngine.Random.value * 360;
        for (int i = 0; i < 5; i++)
        {
            SubWave subWave = new SubWave();
            float spawnAangle = angle + UnityEngine.Random.value * 30;
            StatsHolder enemy = EnemyFactory.SmallBomber();
            enemy.predefinedPosition = true;
            enemy.spawnAngle = spawnAangle;
            subWave.AddEnemy(enemy);
            wave.AddSubWave(subWave, UnityEngine.Random.value * 0.5f);
        }
        return wave;
    }

    public static Wave WeakCluster()
    {
        Wave wave = new Wave();
        float angle = UnityEngine.Random.value * 360;
        for (int i = 0; i < 5 * 2; i++)
        {
            SubWave subWave = new SubWave();
            float spawnAangle = angle + UnityEngine.Random.value * 30;
            StatsHolder enemy = EnemyFactory.CirclingSpawnerMinion();
            enemy.predefinedPosition = true;
            enemy.spawnAngle = spawnAangle;
            subWave.AddEnemy(enemy);
            wave.AddSubWave(subWave, UnityEngine.Random.value * 0.5f);
        }
        return wave;
    }

    public static Wave RangedShooters()
    {
        Wave wave = new Wave();
        float timeStamp = 0.0f;
        for (int i = 0; i < 5; i++)
        {
            SubWave subWave = new SubWave();
            subWave.AddEnemy(EnemyFactory.RangedCirclingEnemy());
            wave.AddSubWave(subWave, timeStamp);
            timeStamp += 0.5f;
        }
        return wave;
    }

    public static Wave LineOfRanged()
    {
        Wave wave = new Wave();
        float timeStamp = 0.0f;
        float angle = UnityEngine.Random.value * 360;
        for (int i = 0; i < 7; i++)
        {
            SubWave subWave = new SubWave();
            StatsHolder enemy = EnemyFactory.FastEnemy();
            enemy.predefinedPosition = true;
            enemy.spawnAngle = angle + 5.0f * i;
            subWave.AddEnemy(enemy);
            wave.AddSubWave(subWave, timeStamp);
            timeStamp += 0.3f;
        }
        return wave;
    }

    public static Wave OneZigZag()
    {
        Wave wave = new Wave();
        SubWave subWave = new SubWave();
        subWave.AddEnemy(EnemyFactory.ZigZag());
        subWave.ScaleSubWaveSpeed(0.5f);
        subWave.ScaleSubWaveSize(1.5f);
        subWave.ScaleSubWaveHealth(5 / 3.0f);
        subWave.ScaleSubWaveAngularSpeed(1);
        subWave.ScaleSubWaveDamage(5.0f / 6.0f);
        wave.AddSubWave(subWave, 0.0f);
        return wave;
    }

    public static Wave OneBigGuy()
    {
        Wave wave = new Wave();
        SubWave subWave = new SubWave();
        subWave.AddEnemy(EnemyFactory.SlowEnemy());
        subWave.ScaleSubWaveHealth(5.0f / 4.0f);
        subWave.ScaleSubWaveDamage(5.0f / 6.0f);
        wave.AddSubWave(subWave, 0.0f);
        return wave;
    }

    public static Wave ThreeBombs()
    {
        Wave wave = new Wave();
        SubWave subWave = new SubWave();
        for (int i = 0; i < 3; i++)
        {
            StatsHolder bomber = EnemyFactory.SmallBomber();
            bomber.Scale = 4.0f;
            bomber.Health = 200.0f;
            bomber.Damage = 20.0f;
            bomber.MovementSpeed = 0.2f;
            subWave.AddEnemy(bomber);
        }
        subWave.SpreadOut();
        subWave.ScaleSubWaveHealth(5.0f / 4.0f);
        subWave.ScaleSubWaveDamage(5.0f / 6.0f);
        wave.AddSubWave(subWave, 0.0f);
        return wave;
    }

    public static Wave TwoZigZags()
    {
        float angle = UnityEngine.Random.value * 360;
        Wave wave = new Wave();
        for (int i = 0; i < 5; i++)
        {
            SubWave subWave = new SubWave();
            StatsHolder zigZag1 = EnemyFactory.ZigZag(true);
            StatsHolder zigZag2 = EnemyFactory.ZigZag(false);
            zigZag1.zigZagAngle = 20;
            zigZag2.zigZagAngle = 20;
            zigZag1.predefinedPosition = true;
            zigZag2.predefinedPosition = true;
            zigZag1.spawnAngle = angle;
            zigZag2.spawnAngle = angle;
            subWave.AddEnemy(zigZag1);
            subWave.AddEnemy(zigZag2);
            wave.AddSubWave(subWave, i * 0.7f);
        }
        return wave;
    }
}

