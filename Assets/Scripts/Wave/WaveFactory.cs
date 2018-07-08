using System.Collections.Generic;
using UnityEngine;
using System;


public class WaveFactory
{

    private static List<Func<int, Wave>> waveComponents = new List<Func<int, Wave>>()
    {
        RotatingWorm,
        RangedSpawner,
        ClosingRotatingCircle,
        TwoStandardRings,
        RangedShooters,
        OneZigZag
    };

    public static Wave GenerateWave(int level)
    {
        switch (level)
        {
            case 0:
                return FirstWave();
            case 1:
                return SecondWave();
            case 2:
                return ThirdWave();
            case 3:
                return FourthWave();
            default:
                Wave wave = new Wave();
                while (level > 3)
                {
                    wave.Merge(RandomWaveComponent(level));
                    level -= 5;
                }
                return wave;
        }
    }

    private static Wave RandomWaveComponent(int difficulty)
    {
        int randomIndex = (int)Mathf.Round(UnityEngine.Random.Range(0, waveComponents.Count));
        return waveComponents[randomIndex](difficulty);
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
        wave.Append(ThirdWave());
        return wave;
    }

    public static Wave RotatingWorm(int difficulty)
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

    public static Wave RangedSpawner(int difficulty)
    {
        Wave wave = new Wave();
        SubWave subWave = new SubWave();
        StatsHolder stats = EnemyFactory.RangedSpawner();
        subWave.AddEnemy(stats);
        wave.AddSubWave(subWave, 0.0f);
        return wave;
    }

    public static Wave ClosingRotatingCircle(int difficulty)
    {
        Wave wave = new Wave();
        SubWave subWave = new SubWave();
        for (int i = 0; i < difficulty; i++)
        {
            StatsHolder stats = EnemyFactory.Rotator(true);
            stats.predefinedPosition = true;
            subWave.AddEnemy(stats);
        }
        subWave.SpreadOut();
        wave.AddSubWave(subWave, 0.0f);
        return wave;
    }

    public static Wave TwoStandardRings(int difficulty)
    {
        Wave wave = new Wave();
        SubWave subWaveA = new SubWave();
        for (int i = 0; i < difficulty; i++)
        {
            if (i % 2 == 0)
            {
                subWaveA.AddEnemy(EnemyFactory.StandardEnemy());
            }
        }
        subWaveA.SpreadOut();
        SubWave subWaveB = subWaveA.Clone();
        subWaveB.Shift((180 / subWaveB.GetEnemies().Count) % 360);
        wave.AddSubWave(subWaveA, 0.0f);
        wave.AddSubWave(subWaveB, 2.0f);
        return wave;
    }

    public static Wave RangedShooters(int difficulty)
    {
        Wave wave = new Wave();
        float timeStamp = 0.0f;
        for (int i = 0; i < difficulty; i++)
        {
            SubWave subWave = new SubWave();
            subWave.AddEnemy(EnemyFactory.RangedCirclingEnemy());
            wave.AddSubWave(subWave, timeStamp);
            timeStamp += 0.5f;
        }
        return wave;
    }

    public static Wave OneZigZag(int difficulty)
    {
        Wave wave = new Wave();
        SubWave subWave = new SubWave();
        subWave.AddEnemy(EnemyFactory.ZigZag());
        subWave.ScaleSubWaveSpeed(0.5f);
        subWave.ScaleSubWaveSize(1.5f);
        subWave.ScaleSubWaveHealth(difficulty);
        subWave.ScaleSubWaveAngularSpeed(((float)difficulty) / 5);
        subWave.ScaleSubWaveDamage(((float)difficulty) / 5);
        wave.AddSubWave(subWave, 0.0f);
        return wave;
    }
}

