using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveHandler : MonoBehaviour {

	private EnemySpawner enemySpawner;
	private int requiredKillEnemiesInWave;
	private int requireKillUnitsSpawned;

	void Awake()
	{
		this.enemySpawner = GetComponent<EnemySpawner>();
	}

	void Start ()
	{
		List<SubWave> wave = WaveFactory.GenerateWave (WaveNumber.waveNumber);
		requiredKillEnemiesInWave = 0;
		foreach(SubWave subWave in wave){
			requiredKillEnemiesInWave += subWave.RequiredKillEnemyCount();
		}
		this.requireKillUnitsSpawned = 0;
		this.enemySpawner.SpawnWave(wave);
	}

	void Update()
	{
		int livingEnemies = GameObject.FindObjectsOfType(typeof (Enemy)).Length;
		if (livingEnemies <= 0 && requireKillUnitsSpawned >= requiredKillEnemiesInWave) {
			WaveNumber.waveNumber++;
			SceneHandler.SwitchScene("Main Menu Scene");
		}
	}

	public void NofifyRequiredKillUnitSpawned()
	{
		requireKillUnitsSpawned++;
	}
}
