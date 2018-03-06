using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	GameObject preInitEnemy;

	Dictionary<int, StatsHolder> enemyTypesDict = new Dictionary<int, StatsHolder>();

	int numberOfEnemies = 0;

	void Start () {

		preInitEnemy = Resources.Load("Prefabs/Enemy", typeof (GameObject)) as GameObject;

		List<SubWave> wave1 = new List<SubWave> ();
		List<StatsHolder> wave1Enemies = new List<StatsHolder> ();
		wave1Enemies.Add (new StatsHolder ("StandardEnemy", 1.0f, 2.5f, 1.0f, 100.0f, 1.0f, Color.white));
		wave1Enemies.Add (new StatsHolder ("StandardEnemy", 1.0f, 2.5f, 1.0f, 100.0f, 1.0f, Color.white));
		wave1Enemies.Add (new StatsHolder ("StandardEnemy", 1.0f, 2.5f, 1.0f, 100.0f, 1.0f, Color.white));
		enemyTypesDict.Add(2, new StatsHolder("FastEnemy", 3.0f, 1.0f, 2.0f, 50.0f, 0.5f, Color.yellow));
		enemyTypesDict.Add(3, new StatsHolder("SlowEnemy", 0.3f, 6.0f, 1.0f, 300.0f, 2.0f, Color.black));
		SubWave wave1SubWave1 = new SubWave (wave1Enemies, 5.0f);
		wave1.Add (wave1SubWave1);
		float duration = instantiateSubWave (wave1SubWave1);
		//TODO: WAIT FOR duration SECONDS
		duration = instantiateSubWave (wave1SubWave1);
	}

	float instantiateSubWave(SubWave subWave)
	{
		float duration = subWave.GetDuration ();
		foreach (StatsHolder enemy in subWave.GetEnemies())
		{
			instantiateEnemyPrefab (enemy);
		}
		return duration;
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
