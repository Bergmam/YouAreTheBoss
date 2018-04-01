using System.Collections.Generic;
using UnityEngine;
using System;


public class WaveFactory
{

	public static List<SubWave> GenerateWave(int numberOfEnemies)
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

