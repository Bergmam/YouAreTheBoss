using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	GameObject preInitEnemy;

	Dictionary<int, StatsHolder> enemyTypesDict = new Dictionary<int, StatsHolder>();

	int numberOfEnemies = 0;

	void Start () {
		enemyTypesDict.Add(1, new StatsHolder("StandardEnemy", 1.0f, 2.5f, 1.0f, 100.0f, 1.0f, Color.white));
		enemyTypesDict.Add(2, new StatsHolder("FastEnemy", 3.0f, 1.0f, 1.0f, 50.0f, 0.5f, Color.yellow));
		enemyTypesDict.Add(3, new StatsHolder("SlowEnemy", 0.3f, 6.0f, 1.0f, 300.0f, 2.0f, Color.black));

	//	preInitEnemy = Resources.Load("Prefabs/Enemy", typeof (GameObject)) as GameObject;
		preInitEnemy = Resources.Load("Prefabs/Enemy", typeof (GameObject)) as GameObject;
		InvokeRepeating("instantiateEnemyPrefab", 0, 2.0f);
	}

	void instantiateEnemyPrefab() {
		GameObject initEnemy = Instantiate(preInitEnemy);

		int dictLength = enemyTypesDict.Count;
		System.Random rand = new System.Random();
		int keyNumber = rand.Next(1, dictLength+1);
		StatsHolder currentStats = enemyTypesDict[keyNumber];

		initEnemy.GetComponent<Enemy>().SetStats(
			currentStats.MovementSpeed,
			currentStats.Damage,
			currentStats.Range,
			currentStats.Health,
			currentStats.Scale,
			currentStats.Color
		);

		initEnemy.name = "Enemy " + numberOfEnemies;
		numberOfEnemies++;
	}

	private class StatsHolder {
		public string Name;
		public float MovementSpeed;
		public float Damage;
		public float Range;
		public float Health;
		public float Scale;
		public Color Color;

		public StatsHolder(string name, 
			float MovementSpeed, 
			float Damage, 
			float Range, 
			float Health, 
			float Scale,
			Color Color) {
				this.Name = name;
				this.MovementSpeed = MovementSpeed;
				this.Damage = Damage;
				this.Range = Range;
				this.Health = Health;
				this.Scale = Scale;
				this.Color = Color;
		}
	}
}
