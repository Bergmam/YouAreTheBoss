using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	GameObject preInitEnemy;

	Dictionary<int, StatsHolder> enemyTypesDict = new Dictionary<int, StatsHolder>();

	int numberOfEnemies;
	List<SubWave> currentWave;
	int currentSubWaveNumber;
	float delay;

	void Start ()
	{
		preInitEnemy = Resources.Load ("Prefabs/Enemy", typeof(GameObject)) as GameObject;
		numberOfEnemies = 0;
		currentSubWaveNumber = 0;
		delay = 0;
		currentWave = WaveFactory.GenerateWave (WaveNumber.waveNumber);
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
			} else if (GameObject.FindObjectsOfType(typeof (Enemy)).Length == 0) {
				WaveNumber.waveNumber++;
				SceneHandler.SwitchScene("Main Menu Scene");
			}
		}
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

		float angle = Random.value * 360;
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
