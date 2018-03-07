using System.Collections.Generic;
using UnityEngine;
using System;


public class WaveFactory
{

	public static List<SubWave> GenerateWave(int n)
	{

		List<SubWave> wave = new List<SubWave> ();

		// Generate subwaves
		while (n >= 0) {
			
			if (n % 10 == 0 && n > 0) { // Every 10th wave: Add a subwave containing a slow enemy.
				SubWave subWave = new SubWave (0.0f);
				subWave.AddEnemy (new StatsHolder ("SlowEnemy", 0.3f, 6.0f, 1.0f, 300.0f, 2.0f, Color.black));
				wave.Add (subWave);
				n -= 10;
			} else if (n % 2 == 0 && n > 0) { // Every 2nd wave: Add a subwave containing a fast enemy.
				SubWave subWave = new SubWave (1.0f);
				subWave.AddEnemy (new StatsHolder ("FastEnemy", 3.0f, 1.0f, 2.0f, 50.0f, 0.5f, Color.yellow));
				wave.Add (subWave);
				n -= 2;
			} else {
				SubWave subWave = new SubWave (4.0f);  // 
				subWave.AddEnemy (new StatsHolder ("StandardEnemy", 1.0f, 2.5f, 1.0f, 100.0f, 1.0f, Color.white));
				wave.Add (subWave);
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

