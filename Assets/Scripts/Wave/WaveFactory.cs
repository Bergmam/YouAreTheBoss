using System.Collections.Generic;
using UnityEngine;
using System;


public class WaveFactory
{

    private static List<Func<Wave>> fastWaveComponents = new List<Func<Wave>>()
    {
        RotatingWorm,
        RangedShooters,
        TwoZigZags,
        BomberCluster,
        LineOfRanged,
        ZigZagWorm,
        WeakCluster,
        JittererWave,
        LineOfBackAndForthShooters
    };

    private static List<Func<Wave>> slowWaveComponents = new List<Func<Wave>>()
    {
        RangedSpawner,
        OneZigZag,
        OneBigGuy,
        CirclingSpawner,
        ThreeBombs
    };

    private static List<Func<Wave>> slowWaveComponents2 = new List<Func<Wave>>()
    {
        ThreeZigZags,
        OneBigGuy
    };

    private static List<Func<Wave>> fastWaveComponents2 = new List<Func<Wave>>()
    {
        ClosingRotatingCircle,
        JittererWave,
        RangedShooters2,
        BomberCluster,
        LineOfBackAndForthShooters
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
            default:
                Wave wave = new Wave();
                level -= 2;
                while (level > 0)
                {
                    Wave component;
                    if (level % 4 == 0)
                    {
                        component = RandomSlowWaveComponent(level);
                        component.Append(RandomFastWaveComponent(level));
                    }
                    else
                    {
                        component = RandomFastWaveComponent(level);
                    }
                    wave.Append(component);
                    level--;
                }
                return wave;
        }
    }

    private static Wave LineOfBackAndForthShooters()
    {
        Wave wave = new Wave();
        float timeStamp = 0.0f;
        float angle = UnityEngine.Random.value * 360;
        for (int i = 0; i < 7; i++)
        {
            SubWave subWave = new SubWave();
            EnemySettings enemy = GameObject.Instantiate(EnemyFactory.Ranged());
            enemy.predefinedPosition = true;
            enemy.spawnAngle = angle + 5.0f * i;
            enemy.TurnBackDistance = 1.0f;
            enemy.TurnForwardDistance = 3.0f;
            enemy.Color = Color.green;
            enemy.NumberOfTurns = -1;
            enemy.MovementSpeed = 1.5f;
            subWave.AddEnemy(enemy);
            wave.AddSubWave(subWave, timeStamp);
            timeStamp += 0.3f;
        }
        return wave;
    }

    private static Wave RandomSlowWaveComponent(int level)
    {
        if (level < 4)
        {
            int randomIndex = (int)Mathf.Round(UnityEngine.Random.Range(0, slowWaveComponents.Count));
            return slowWaveComponents[randomIndex]();
        }
        else
        {
            int randomIndex = (int)Mathf.Round(UnityEngine.Random.Range(0, slowWaveComponents2.Count));
            return slowWaveComponents2[randomIndex]();
        }
    }

    private static Wave RandomFastWaveComponent(int level)
    {
        if (level < 4)
        {
            int randomIndex = (int)Mathf.Round(UnityEngine.Random.Range(0, fastWaveComponents.Count));
            return fastWaveComponents[randomIndex]();
        }
        else
        {
            int randomIndex = (int)Mathf.Round(UnityEngine.Random.Range(0, fastWaveComponents2.Count));
            return fastWaveComponents2[randomIndex]();
        }
    }

    private static Wave FirstWave()
    {
        Wave wave = new Wave();
        SubWave subWave = new SubWave();
        EnemySettings enemy = GameObject.Instantiate(EnemyFactory.RandomBasicEnemy());
        subWave.AddEnemy(enemy);
        wave.AddSubWave(subWave, 0.0f);
        return wave;
    }


    private static Wave SecondWave()
    {
        Wave wave = new Wave();
        SubWave subWave = new SubWave();
        EnemySettings enemy = GameObject.Instantiate(EnemyFactory.RandomBasicEnemy());
        subWave.AddEnemy(enemy);
        subWave.AddEnemy(enemy);
        wave.AddSubWave(subWave, 0.0f);
        return wave;
    }

    private static Wave ThirdWave()
    {
        Wave wave = SecondWave();
        wave.Merge(SecondWave(), 3.0f);
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
            EnemySettings stats = GameObject.Instantiate(EnemyFactory.Rotator());
            stats.spawnAngle = angle + i * direction;
            stats.predefinedPosition = true;
            subWave.AddEnemy(stats);
            wave.AddSubWave(subWave, timeStamp);
            timeStamp += 0.1f;
        }
        return wave;
    }

    private static Wave JittererWave()
    {
        Wave wave = new Wave();

        int[] indexes = {0, 1, 2, 3, 4, 5};
        for (int i = 0; i < indexes.Length; i++)
        {
            int rand = UnityEngine.Random.Range(0, indexes.Length);
            int tmp = indexes[i];
            indexes[i] = indexes[rand];
            indexes[rand] = tmp;
        }

        float timeStamp = 0.0f;
        for (int i = 0; i < indexes.Length; i++)
        {
            SubWave subWave = new SubWave();
            EnemySettings enemy = GameObject.Instantiate(EnemyFactory.Jitterer());
            enemy.spawnAngle = (360 / indexes.Length) * indexes[i];
            enemy.predefinedPosition = true;
            subWave.AddEnemy(enemy);
            wave.AddSubWave(subWave, timeStamp);
            timeStamp += 0.85f;
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
            EnemySettings stats = GameObject.Instantiate(EnemyFactory.ZigZag());
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
        EnemySettings stats = GameObject.Instantiate(EnemyFactory.RangedSpawner());
        subWave.AddEnemy(stats);
        wave.AddSubWave(subWave, 0.0f);
        return wave;
    }

    public static Wave CirclingSpawner()
    {
        Wave wave = new Wave();
        SubWave subWave = new SubWave();
        EnemySettings stats = GameObject.Instantiate(EnemyFactory.CirclingSpawner());
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
            EnemySettings stats = GameObject.Instantiate(EnemyFactory.Rotator());
            stats.predefinedPosition = true;
            subWave.AddEnemy(stats);
        }
        subWave.ScaleSubWaveSpeed(0.8f);
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
            EnemySettings enemy = GameObject.Instantiate(EnemyFactory.StandardEnemy());
            enemy.Health = 100.0f;
            enemy.predefinedPosition = true;
            enemy.spawnAngle = i % 2 == 0 ? angleA : angleB;
            subWave.AddEnemy(enemy);
            EnemySettings enemy2 = GameObject.Instantiate(enemy);
            enemy2.spawnAngle = enemy.spawnAngle + 5.0f;
            subWave.AddEnemy(enemy2);
            EnemySettings enemy3 = GameObject.Instantiate(enemy);
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
            EnemySettings enemy = GameObject.Instantiate(EnemyFactory.SmallBomber());
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
            EnemySettings enemy = GameObject.Instantiate(EnemyFactory.CirclingSpawnerMinion());
            enemy.predefinedPosition = true;
            enemy.spawnAngle = spawnAangle;
            subWave.AddEnemy(enemy);
            wave.AddSubWave(subWave, UnityEngine.Random.value * 0.5f);
        }
        return wave;
    }

    public static Wave RangedShooters()
    {
        return RangedShootersWave(5);
    }

    public static Wave RangedShooters2()
    {
        return RangedShootersWave(9);
    }

    public static Wave RangedShootersWave(int numberOfShooters)
    {
        Wave wave = new Wave();
        float timeStamp = 0.0f;
        for (int i = 0; i < numberOfShooters; i++)
        {
            SubWave subWave = new SubWave();
            subWave.AddEnemy(GameObject.Instantiate(EnemyFactory.RangedCirclingEnemy()));
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
            EnemySettings enemy = GameObject.Instantiate(EnemyFactory.Ranged());
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
        subWave.AddEnemy(GameObject.Instantiate(EnemyFactory.ZigZag()));
        subWave.ScaleSubWaveSpeed(0.4f);
        subWave.ScaleSubWaveSize(2.5f);
        subWave.ScaleSubWaveHealth(2.5f);
        subWave.ScaleSubWaveAngularSpeed(1);
        subWave.ScaleSubWaveDamage(5.0f / 6.0f);
        wave.AddSubWave(subWave, 0.0f);
        return wave;
    }

    public static Wave ThreeZigZags()
    {
        Wave wave = new Wave();
        SubWave subWave = new SubWave();
        for (int i = 0; i < 3; i++)
        {
            subWave.AddEnemy(GameObject.Instantiate(EnemyFactory.ZigZag()));
        }
        subWave.ScaleSubWaveSpeed(0.4f);
        subWave.ScaleSubWaveSize(2.5f);
        subWave.ScaleSubWaveHealth(2.5f);
        subWave.ScaleSubWaveAngularSpeed(1);
        subWave.ScaleSubWaveDamage(5.0f / 6.0f);
        subWave.SpreadOut();
        wave.AddSubWave(subWave, 0.0f);
        return wave;
    }

    public static Wave OneBigGuy()
    {
        Wave wave = new Wave();
        SubWave subWave = new SubWave();
        subWave.AddEnemy(GameObject.Instantiate(EnemyFactory.SlowEnemy()));
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
            EnemySettings bomber = GameObject.Instantiate(EnemyFactory.SmallBomber());
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
            EnemySettings zigZag1 = GameObject.Instantiate(EnemyFactory.ZigZag());
            EnemySettings zigZag2 = GameObject.Instantiate(EnemyFactory.ZigZag());
            zigZag1.zigZagAngle = 20;
            zigZag2.zigZagAngle = 20;
            zigZag2.angularSpeed = -zigZag2.angularSpeed;

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

