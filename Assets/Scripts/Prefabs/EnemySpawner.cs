using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	GameObject preInitEnemy;
	void Start () {
		preInitEnemy = Resources.Load("Prefabs/Enemy", typeof (GameObject)) as GameObject;
		instantiateEnemyPrefab();
		InvokeRepeating("instantiateEnemyPrefab", 0.3f, 0.3f);
	}

	void instantiateEnemyPrefab() {
		Instantiate(preInitEnemy);
	}
}
