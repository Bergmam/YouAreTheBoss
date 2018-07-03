using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveHandler : MonoBehaviour
{

    private EnemySpawner enemySpawner;
    private int requiredKillEnemiesInWave;
    private int requireKillUnitsSpawned;
    private bool gameOver = false;

    void Awake()
    {
        this.enemySpawner = GetComponent<EnemySpawner>();
    }

    void Start()
    {
        Wave wave = WaveFactory.GenerateWave(WaveNumber.waveNumber);
        requiredKillEnemiesInWave = 0;
        foreach (SubWave subWave in wave.GetSubWaves())
        {
            requiredKillEnemiesInWave += subWave.RequiredKillEnemyCount();
        }
        this.requireKillUnitsSpawned = 0;
        this.enemySpawner.SpawnWave(wave);
    }

    void Update()
    {
        int livingEnemies = GameObject.FindObjectsOfType(typeof(Enemy)).Length;
        if (livingEnemies <= 0 && requireKillUnitsSpawned >= requiredKillEnemiesInWave && !gameOver)
        {
            gameOver = true;
            WaveNumber.waveNumber++;
            StartCoroutine(waitAndGoBack());
        }
    }

    public void NofifyRequiredKillUnitSpawned()
    {
        requireKillUnitsSpawned++;
    }

    IEnumerator waitAndGoBack()
    {
        yield return new WaitForSeconds(0.8f);
        SceneHandler.SwitchScene("Main Menu Scene");
    }
}
