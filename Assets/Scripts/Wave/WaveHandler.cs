using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveHandler : MonoBehaviour {

	private EnemySpawner enemySpawner;
	private int enemiesInWave;
	private int waveUnitsSpawned;

	void Awake()
	{
		this.enemySpawner = GetComponent<EnemySpawner>();
	}

	void Start ()
	{
		List<SubWave> wave = WaveFactory.GenerateWave (WaveNumber.waveNumber);
		enemiesInWave = 0;
		foreach(SubWave subWave in wave){
			enemiesInWave += subWave.EnemyCount();
		}
		this.waveUnitsSpawned = 0;
		this.enemySpawner.SpawnWave(wave);
	}

	void Update()
	{
		int livingEnemies = GameObject.FindObjectsOfType(typeof (Enemy)).Length;
		if (livingEnemies <= 0 && waveUnitsSpawned >= enemiesInWave) {
			WaveNumber.waveNumber++;
			SceneHandler.SwitchScene("Main Menu Scene");
		}
	}

	public void NofifyWaveUnitsSpawned()
	{
		waveUnitsSpawned++;
	}
}
