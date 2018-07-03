﻿using System.Collections.Generic;
using UnityEngine;
using System;


public class WaveFactory
{

    public static Wave GenerateWave(int numberOfEnemies)
    {
        Wave wave = new Wave();
        SubWave subWave;
        StatsHolder stats;
        switch (numberOfEnemies)
        {

            case 3:
                for (int i = 0; i < 360; i += 45)
                {
                    subWave = new SubWave(2.0f);
                    stats = EnemyFactory.StandardEnemy();
                    stats.spawnAngle = i;
                    stats.predefinedPosition = true;
                    subWave.AddEnemy(stats);
                    wave.AddSubWave(subWave);
                }
                break;

            case 5:
                SubWave bigEnemySubWave = new SubWave(0.0f);
                StatsHolder bigEnemy = EnemyFactory.SlowEnemy();
                bigEnemy.predefinedPosition = true;
                bigEnemy.spawnAngle = 225f;
                bigEnemySubWave.AddEnemy(bigEnemy);
                wave.AddSubWave(bigEnemySubWave);
                for (int i = 0; i < 20; i++)
                {
                    StatsHolder minon1 = EnemyFactory.Minion();
                    minon1.spawnAngle = 65f;
                    minon1.predefinedPosition = true;
                    StatsHolder minon2 = EnemyFactory.Minion();
                    minon2.spawnAngle = 45f;
                    minon2.predefinedPosition = true;
                    StatsHolder minon3 = EnemyFactory.Minion();
                    minon3.spawnAngle = 25f;
                    minon3.predefinedPosition = true;
                    SubWave sub1 = new SubWave(3.0f);
                    sub1.AddEnemy(minon1);
                    wave.AddSubWave(sub1);
                    SubWave sub2 = new SubWave(3.0f);
                    sub2.AddEnemy(minon2);
                    wave.AddSubWave(sub2);
                    SubWave sub3 = new SubWave(3.0f);
                    sub3.AddEnemy(minon3);
                    wave.AddSubWave(sub3);
                }
                break;

            case 7:
                float angle = UnityEngine.Random.value * 360;
                for (int i = 0; i < 4; i++)
                {
                    subWave = new SubWave(0.1f);
                    stats = EnemyFactory.Rotator(true);
                    stats.spawnAngle = angle + i * 2;
                    stats.predefinedPosition = true;
                    subWave.AddEnemy(stats);
                    wave.AddSubWave(subWave);
                }
                subWave = new SubWave(2.0f);
                stats = EnemyFactory.Rotator(true);
                stats.spawnAngle = angle + 8;
                stats.predefinedPosition = true;
                subWave.AddEnemy(stats);
                wave.AddSubWave(subWave);
                for (int i = 0; i < 5; i++)
                {
                    subWave = new SubWave(0.1f);
                    stats = EnemyFactory.Rotator(false);
                    stats.spawnAngle = (angle + 180) % 360 + i * 2;
                    stats.predefinedPosition = true;
                    subWave.AddEnemy(stats);
                    wave.AddSubWave(subWave);
                }
                break;

            case 8:
                subWave = new SubWave(0.0f);
                for (int i = 0; i < 360; i += 45)
                {
                    stats = EnemyFactory.Rotator(true);
                    stats.spawnAngle = i;
                    stats.predefinedPosition = true;
                    subWave.AddEnemy(stats);
                }
                wave.AddSubWave(subWave);
                break;

            case 11:
                subWave = new SubWave(2.0f);
                stats = EnemyFactory.RangedSpawner();
                subWave.AddEnemy(stats);
                wave.AddSubWave(subWave);
                break;

            default:
                wave = AutoGenerateWave(numberOfEnemies);
                break;
        }

        return wave;
    }

    public static Wave AutoGenerateWave(int numberOfEnemies)
    {
        bool clockwise = false;
        Wave wave = new Wave();

        while (numberOfEnemies > 0)
        {
            if (numberOfEnemies % 10 == 0)
            {
                SubWave subWave = new SubWave(0.5f);
                subWave.AddEnemy(EnemyFactory.SlowEnemy());
                wave.AddSubWave(subWave);
            }
            else if (numberOfEnemies % 9 == 0)
            {
                SubWave subWave = new SubWave(0.5f);  // 
                subWave.AddEnemy(EnemyFactory.StandardEnemy());
                wave.AddSubWave(subWave);
            }
            else if (numberOfEnemies % 8 == 0)
            {
                SubWave subWave = new SubWave(0.5f);
                subWave.AddEnemy(EnemyFactory.RangedCirclingEnemy(clockwise));
                clockwise = !clockwise;
                wave.AddSubWave(subWave);
            }
            else if (numberOfEnemies % 7 == 0)
            {
                SubWave subWave = new SubWave(0.5f);
                subWave.AddEnemy(EnemyFactory.ZigZag());
                wave.AddSubWave(subWave);
            }
            else if (numberOfEnemies % 6 == 0)
            {
                SubWave subWave = new SubWave(0.5f);
                subWave.AddEnemy(EnemyFactory.FastEnemy());
                wave.AddSubWave(subWave);
            }
            else if (numberOfEnemies % 5 == 0)
            {
                SubWave subWave = new SubWave(0.5f);
                subWave.AddEnemy(EnemyFactory.Rotator(clockwise));
                clockwise = !clockwise;
                wave.AddSubWave(subWave);
            }
            else if (numberOfEnemies % 4 == 0)
            {
                SubWave subWave = new SubWave(0.5f);
                subWave.AddEnemy(EnemyFactory.RangedCirclingEnemy(clockwise));
                clockwise = !clockwise;
                wave.AddSubWave(subWave);
            }
            else if (numberOfEnemies % 3 == 0)
            {
                SubWave subWave = new SubWave(0.5f);
                subWave.AddEnemy(EnemyFactory.SmallBomber());
                wave.AddSubWave(subWave);
            }
            else
            {
                SubWave subWave = new SubWave(0.5f);
                subWave.AddEnemy(EnemyFactory.FastEnemy());
                wave.AddSubWave(subWave);
            }

            numberOfEnemies--;
        }
        SubWave lastSubWave = new SubWave(0.5f);  // 
        lastSubWave.AddEnemy(EnemyFactory.StandardEnemy());
        wave.AddSubWave(lastSubWave);
        //Shuffle the order of the subwaves within the wave.
        System.Random rng = new System.Random((int)DateTime.Now.Ticks);
        for (int i = 0; i < wave.Count(); i++)
        {
            int j = rng.Next(0, wave.Count());
            wave.SwapSubWaves(i, j);
        }

        return wave;
    }
}

