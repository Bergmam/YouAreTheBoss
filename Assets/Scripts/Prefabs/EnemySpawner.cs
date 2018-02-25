using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	GameObject preInitEnemy;

	int numberOfEnemies = 0;

	void Start () {
		preInitEnemy = Resources.Load("Prefabs/Enemy", typeof (GameObject)) as GameObject;
		InvokeRepeating("instantiateEnemyPrefab", 0, 0.1f);
	}

	void instantiateEnemyPrefab() {
		GameObject initEnemy = Instantiate(preInitEnemy);
		initEnemy.name = initEnemy.name + numberOfEnemies;
		numberOfEnemies++;
	}
}
