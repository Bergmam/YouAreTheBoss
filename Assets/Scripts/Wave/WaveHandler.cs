using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveHandler : MonoBehaviour {

	private EnemySpawner enemySpawner;

	void Awake()
	{
		this.enemySpawner = GetComponent<EnemySpawner>();
	}

	void Start ()
	{
		List<SubWave> wave = WaveFactory.GenerateWave (WaveNumber.waveNumber);
		this.enemySpawner.SpawnWave(wave);
	}

	void Update()
	{
		if (GameObject.FindObjectsOfType(typeof (Enemy)).Length == 0) {
			WaveNumber.waveNumber++;
			SceneHandler.SwitchScene("Main Menu Scene");
		}
	}
}
