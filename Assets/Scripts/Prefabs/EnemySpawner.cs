using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	GameObject preInitEnemy;
	int numberOfEnemies;
	List<SubWave> currentWave;
	int currentSubWaveNumber;
	float delay;

	void Awake()
	{
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
			// TODO: Comment in this to enable wave spawning. When doing so, add a WaveHandler to the Handler in the Fighting Scene.
			/*if (currentSubWaveNumber < currentWave.Count) // Spawn all subwaves of a wave, one at a time with delay between. 
			{
				SubWave subWave = currentWave [currentSubWaveNumber];
				SpawnSubWave (subWave);
				delay = subWave.GetDuration ();
				currentSubWaveNumber++;
			}*/
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
			instantiateEnemyPrefab (enemy);
		}
	}

	void instantiateEnemyPrefab(StatsHolder currentStats)
	{
		GameObject initEnemy = Instantiate (preInitEnemy);
		float angle;
		if(currentStats.predefinedPosition)
		{
			angle = currentStats.spawnAngle;
		}
		else
		{
			angle = Random.value * 360;
		}
		 
		float radius = 5f;
		Vector3 center = Vector3.zero;
        RadialPosition randomRadialPos = new RadialPosition(radius, angle);
        Vector3 randomPosition = RotationUtils.RadialPosToXY(randomRadialPos);
		initEnemy.GetComponent<Enemy> ().SetStats (
            currentStats.MovementSpeed,
            currentStats.angularSpeed,
			currentStats.Damage,
			currentStats.Range,
			currentStats.Health,
			currentStats.Scale,
			currentStats.Color,
			false,
			false,
            randomRadialPos
		);

		initEnemy.name = "Enemy " + numberOfEnemies;
		numberOfEnemies++;
		initEnemy.transform.position = randomPosition;
	}
}
