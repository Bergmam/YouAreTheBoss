using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	GameObject preInitEnemy;

	Dictionary<int, StatsHolder> enemyTypesDict = new Dictionary<int, StatsHolder>();

	int numberOfEnemies;
	int currentWaveNumber;
	List<SubWave> currentWave;
	int currentSubWaveNumber;
	float delay;

	void Start ()
	{
		preInitEnemy = Resources.Load ("Prefabs/Enemy", typeof(GameObject)) as GameObject;
		numberOfEnemies = 0;
		currentWaveNumber = 0;
		currentSubWaveNumber = 0;
		delay = 0;
		currentWave = WaveFactory.GenerateWave (currentWaveNumber);
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
			else // When all subwaves have spawned, start the next wave.
			{
				currentSubWaveNumber = 0;
				currentWaveNumber++;
				currentWave = WaveFactory.GenerateWave (currentWaveNumber);
			}
		}
	}

	// Spawn all enemies of a subwave.
	public void SpawnSubWave(SubWave subWave)
	{
		print ("Spawning wave: " + currentWaveNumber + ", subwave: " + currentSubWaveNumber);
		foreach (StatsHolder enemy in subWave.GetEnemies())
		{
			instantiateEnemyPrefab (enemy);
		}
	}

	void instantiateEnemyPrefab(StatsHolder currentStats)
	{
		GameObject initEnemy = Instantiate (preInitEnemy);

		float angle = Random.value * 360;
		float radius = 5f;
		Vector3 center = Vector3.zero;
		Vector3 randomPosition = new Vector3 (
			center.x + radius * Mathf.Sin (angle * Mathf.Deg2Rad),
			center.y + radius * Mathf.Cos (angle * Mathf.Deg2Rad),
			center.z);

		initEnemy.GetComponent<Enemy> ().SetStats (
			currentStats.MovementSpeed,
			currentStats.Damage,
			currentStats.Range,
			currentStats.Health,
			currentStats.Scale,
			currentStats.Color,
			false,
			false,
			angle
		);

		initEnemy.name = "Enemy " + numberOfEnemies;
		numberOfEnemies++;
		initEnemy.transform.position = randomPosition;
	}
}
