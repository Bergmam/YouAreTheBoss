using System.Collections.Generic;
using UnityEngine;
using System;


public class WaveFactory
{

    public static List<SubWave> GenerateWave(int numberOfEnemies){
        List<SubWave> wave = new List<SubWave> ();
        SubWave subWave;
        StatsHolder stats;
        switch(numberOfEnemies){

            case 3:
                for(int i = 0; i < 360; i += 45){
                    subWave = new SubWave(2.0f);
                    stats = EnemyFactory.StandardEnemy();
                    stats.spawnAngle = i;
                    stats.predefinedPosition = true;
                    subWave.AddEnemy(stats);
                    wave.Add(subWave);
                }
                break;

            case 5:
                SubWave bigEnemySubWave = new SubWave(0.0f);
                StatsHolder bigEnemy = EnemyFactory.SlowEnemy();
                bigEnemy.predefinedPosition = true;
                bigEnemy.spawnAngle = 225f;
                bigEnemySubWave.AddEnemy(bigEnemy);
                wave.Add(bigEnemySubWave);
                for(int i = 0; i < 20; i++){
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
                    wave.Add(sub1);
                    SubWave sub2 = new SubWave(3.0f);
                    sub2.AddEnemy(minon2);
                    wave.Add(sub2);
                    SubWave sub3 = new SubWave(3.0f);
                    sub3.AddEnemy(minon3);
                    wave.Add(sub3);
                }
                break;

            case 8:
                subWave = new SubWave(0.0f);
                for(int i = 0; i < 360; i += 45){
                    stats = EnemyFactory.Rotator(true);
                    stats.spawnAngle = i;
                    stats.predefinedPosition = true;
                    subWave.AddEnemy(stats);
                }
                wave.Add(subWave);
                break;

            case 11:
                subWave = new SubWave(2.0f);
                stats = EnemyFactory.RangedSpawner();
                subWave.AddEnemy(stats);
                wave.Add(subWave);
                break;

            default:
                wave = AutoGenerateWave(numberOfEnemies);
                break;
        }

        return wave;
    }

	public static List<SubWave> AutoGenerateWave(int numberOfEnemies)
	{
        bool clockwise = false;
        List<SubWave> wave = new List<SubWave> ();

        while(numberOfEnemies > 0){
            if (numberOfEnemies % 10 == 0)
            {
                SubWave subWave = new SubWave(0.5f);
                subWave.AddEnemy(EnemyFactory.SlowEnemy());
                wave.Add(subWave);
            }
            else if(numberOfEnemies % 9 == 0){
                SubWave subWave = new SubWave(0.5f);  // 
                subWave.AddEnemy(EnemyFactory.StandardEnemy());
                wave.Add(subWave);
            }
            else if(numberOfEnemies % 8 == 0){
                SubWave subWave = new SubWave(0.5f);
                subWave.AddEnemy(EnemyFactory.RangedCirclingEnemy(clockwise));
                clockwise = !clockwise;
                wave.Add(subWave);
            }
            else if(numberOfEnemies % 7 == 0){
                SubWave subWave = new SubWave(0.5f);
                subWave.AddEnemy(EnemyFactory.Rotator(clockwise));
                clockwise = !clockwise;
                wave.Add(subWave);
            }
            else if(numberOfEnemies % 6 == 0){
                SubWave subWave = new SubWave(0.5f);
                subWave.AddEnemy(EnemyFactory.FastEnemy());
                wave.Add(subWave);
            }
            else if(numberOfEnemies % 5 == 0){
                SubWave subWave = new SubWave(0.5f);
                subWave.AddEnemy(EnemyFactory.Rotator(clockwise));
                clockwise = !clockwise;
                wave.Add(subWave);
            }
            else if(numberOfEnemies % 4 == 0){
                SubWave subWave = new SubWave(0.5f);  // 
                subWave.AddEnemy(EnemyFactory.StandardEnemy());
                wave.Add(subWave);
            }
            else if(numberOfEnemies % 3 == 0){
                SubWave subWave = new SubWave(0.5f);
                subWave.AddEnemy(EnemyFactory.RangedCirclingEnemy(clockwise));
                clockwise = !clockwise;
                wave.Add(subWave);
            }
            else {
                SubWave subWave = new SubWave(0.5f);
                subWave.AddEnemy(EnemyFactory.FastEnemy());
                wave.Add(subWave);
            }
                
            numberOfEnemies--;
        }
        SubWave lastSubWave = new SubWave(0.5f);  // 
        lastSubWave.AddEnemy(EnemyFactory.StandardEnemy());
        wave.Add(lastSubWave);
		//Shuffle the order of the subwaves within the wave.
		System.Random rng = new System.Random ((int)DateTime.Now.Ticks);
		for (int i = 0; i < wave.Count; i++)
		{
			int j = rng.Next (0, wave.Count);
			SubWave tmp = wave [i];
			wave [i] = wave [j];
			wave [j] = tmp;
		}

		return wave;
	}
}

