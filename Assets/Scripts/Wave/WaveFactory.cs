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
                subWave.AddEnemy(new StatsHolder("Slow Enemy", 0.3f, 6.0f, 1.0f, 300.0f, 2.0f, Color.black));
                wave.Add(subWave);
                n -= 10;
            }
            else if (n % 3 == 0 && n > 0)
            { // Add a subwave containing a fast, rotating enemy.
                SubWave subWave = new SubWave(0.5f);
                StatsHolder holder = new StatsHolder("Rotator", 0.7f, 2.5f, 1.0f, 100.0f, 0.7f, Color.blue);
                // Alternate between enemies rotating clockwise and counterclockwise
                if (spawnClockwiseRotator)
                {
                    holder.SetAngluarSpeed(-50f);
                }
                else
                {
                    holder.SetAngluarSpeed(50f);
                }
                spawnClockwiseRotator = !spawnClockwiseRotator;
                subWave.AddEnemy(holder);
                wave.Add(subWave);
                n -= 3;
            }
            else if (n % 2 == 0 && n > 0)
            { // Add a subwave containing a fast enemy.
                SubWave subWave = new SubWave(0.5f);
                subWave.AddEnemy(new StatsHolder("Fast Enemy", 3.0f, 1.0f, 2.0f, 50.0f, 0.5f, Color.yellow));
                wave.Add(subWave);
                n -= 2;
            }
            else
            {
                SubWave subWave = new SubWave(0.5f);  // 
                subWave.AddEnemy(new StatsHolder("Standard Enemy", 1.0f, 2.5f, 1.0f, 100.0f, 1.0f, Color.white));
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

