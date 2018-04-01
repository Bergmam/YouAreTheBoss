using System.Collections.Generic;
using UnityEngine;
using System;


public class WaveFactory
{

	public static List<SubWave> GenerateWave(int n)
	{
		n += 7;

		List<SubWave> wave = new List<SubWave> ();

        bool spawnClockwiseRotator = true;

        // Generate subwaves
        while (n >= 0)
        {
            if (n % 10 == 0 && n > 0)
            { // Add a subwave containing a slow enemy.
                SubWave subWave = new SubWave(0.5f);
                subWave.AddEnemy(EnemyFactory.SlowEnemy());
                wave.Add(subWave);
                n -= 10;
            }
            else if (n % 3 == 0 && n > 0)
            { // Add a subwave containing a fast, rotating enemy.
                SubWave subWave = new SubWave(0.5f);
                StatsHolder holder = EnemyFactory.Rotator(spawnClockwiseRotator);
                spawnClockwiseRotator = !spawnClockwiseRotator; // Alternate between enemies rotating clockwise and counterclockwise
                subWave.AddEnemy(holder);
                wave.Add(subWave);
                n -= 3;
            }
            else if (n % 2 == 0 && n > 0)
            { // Add a subwave containing a fast enemy.
                SubWave subWave = new SubWave(0.5f);
                subWave.AddEnemy(EnemyFactory.FastEnemy());
                wave.Add(subWave);
                n -= 2;
            }
            else
            {
                SubWave subWave = new SubWave(0.5f);  // 
                subWave.AddEnemy(EnemyFactory.StandardEnemy());
                wave.Add(subWave);
                n--;
            }
        }

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

