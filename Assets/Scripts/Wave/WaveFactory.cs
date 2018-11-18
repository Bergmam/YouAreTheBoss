using System.Collections.Generic;
using UnityEngine;
using System;


public class WaveFactory
{

    private static List<Func<int, Wave>> fastWaveComponents = new List<Func<int, Wave>>()
    {
        RotatingWorm,
        ClosingRotatingCircle,
        RangedShooters,
        TwoZigZags,
        BomberCluster,
        LineOfRanged
    };

    private static List<Func<int, Wave>> slowWaveComponents = new List<Func<int, Wave>>()
    {
        OppositeSides,
        RangedSpawner,
        OneZigZag,
        OneBigGuy,
        CirclingSpawner
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
                Wave wave = RandomSlowWaveComponent(5);
                level -= 5;
                int waves = 0;
                while (level >= 5)
                {
                    waves++;
                    Wave waveComponent = waves % 3 == 0 ? RandomSlowWaveComponent(5) : RandomFastWaveComponent(5);
                    wave.Merge(waveComponent, Parameters.STANDARD_WAVE_DURATION * waves);
                    level -= 5;
                }

                if (level > 0)
                {
                    wave.Merge(RandomFastWaveComponent(level), Parameters.STANDARD_WAVE_DURATION * waves);
                }
                return wave;
        }
    }

    private static Wave RandomSlowWaveComponent(int difficulty)
    {
        int randomIndex = (int)Mathf.Round(UnityEngine.Random.Range(0, slowWaveComponents.Count));
        return slowWaveComponents[randomIndex](difficulty);
    }

    private static Wave RandomFastWaveComponent(int difficulty)
    {
        int randomIndex = (int)Mathf.Round(UnityEngine.Random.Range(0, fastWaveComponents.Count));
        return fastWaveComponents[randomIndex](difficulty);
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

    public static Wave CirclingSpawner(int difficulty)
    {
        Wave wave = new Wave();
        SubWave subWave = new SubWave();
        StatsHolder stats = EnemyFactory.CirclingSpawner();
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

    public static Wave OppositeSides(int difficulty)
    {
        Wave wave = new Wave();
        float angleA = UnityEngine.Random.value * 360;
        float angleB = (angleA + 180) % 360;
        for (int i = 0; i < difficulty; i++)
        {
            SubWave subWave = new SubWave();
            Wave tempWave = new Wave();
            StatsHolder enemy = EnemyFactory.StandardEnemy();
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

    public static Wave BomberCluster(int difficulty)
    {
        Wave wave = new Wave();
        float angle = UnityEngine.Random.value * 360;
        for (int i = 0; i < difficulty; i++)
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

    public static Wave LineOfRanged(int difficulty)
    {
        int numberOfEnemies = difficulty + 2;
        Wave wave = new Wave();
        float timeStamp = 0.0f;
        float angle = UnityEngine.Random.value * 360;
        for (int i = 0; i < numberOfEnemies; i++)
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

    public static Wave OneZigZag(int difficulty)
    {
        Wave wave = new Wave();
        SubWave subWave = new SubWave();
        subWave.AddEnemy(EnemyFactory.ZigZag());
        subWave.ScaleSubWaveSpeed(0.5f);
        subWave.ScaleSubWaveSize(1.5f);
        subWave.ScaleSubWaveHealth(difficulty / 4.0f);
        subWave.ScaleSubWaveAngularSpeed(((float)difficulty) / 5.0f);
        subWave.ScaleSubWaveDamage(((float)difficulty) / 6.0f);
        wave.AddSubWave(subWave, 0.0f);
        return wave;
    }

    public static Wave OneBigGuy(int difficulty)
    {
        Wave wave = new Wave();
        SubWave subWave = new SubWave();
        subWave.AddEnemy(EnemyFactory.SlowEnemy());
        subWave.ScaleSubWaveHealth(difficulty / 4.0f);
        subWave.ScaleSubWaveDamage(((float)difficulty) / 6.0f);
        wave.AddSubWave(subWave, 0.0f);
        return wave;
    }

    public static Wave TwoZigZags(int difficulty)
    {
        float angle = UnityEngine.Random.value * 360;
        Wave wave = new Wave();
        for (int i = 0; i < (difficulty / 3) + 2; i++)
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

