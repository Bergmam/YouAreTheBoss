using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	GameObject preInitEnemy;
	int numberOfEnemies;
	List<SubWave> currentWave;
	int currentSubWaveNumber;
	float delay;
	private WaveHandler waveHandler;

	void Awake()
	{
		this.waveHandler = GameObject.FindObjectOfType<WaveHandler>();
		preInitEnemy = Resources.Load ("Prefabs/Enemy", typeof(GameObject)) as GameObject;
	}

	void Update()
	{
		if (delay > 0) // Delay used to wait for the duration of each subwave until next one is spawned.
		{
			delay -= Time.deltaTime;
		}
		else
		{
			if (currentSubWaveNumber < currentWave.Count) // Spawn all subwaves of a wave, one at a time with delay between. 
			{
				SubWave subWave = currentWave [currentSubWaveNumber];
				SpawnSubWave (subWave);
				delay = subWave.GetDuration ();
				currentSubWaveNumber++;
			}
		}
	}

	public void SpawnWave(List<SubWave> wave)
	{
		this.numberOfEnemies = 0;
		this.currentSubWaveNumber = 0;
		this.delay = 0;
		this.currentWave = wave;
	}

	// Spawn all enemies of a subwave.
	public void SpawnSubWave(SubWave subWave)
	{
		foreach (StatsHolder enemy in subWave.GetEnemies())
		{
			waveHandler.NofifyWaveUnitsSpawned();
			InstantiateEnemyPrefab (enemy);
		}
	}

	public void InstantiateEnemyPrefab(StatsHolder stats)
	{
		GameObject initEnemy = Instantiate (preInitEnemy);
		if(!stats.predefinedPosition)
		{
			stats.spawnAngle = Random.value * 360;
		}
		
		initEnemy.GetComponent<Enemy> ().SetStats (
            stats
		);

		initEnemy.name = stats.Name + numberOfEnemies;
		numberOfEnemies++;
	}
}
