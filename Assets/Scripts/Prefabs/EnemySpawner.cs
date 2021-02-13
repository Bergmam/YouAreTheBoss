using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    GameObject preInitEnemy;
    int numberOfEnemies;
    Wave currentWave;
    int nextSubWaveNumber;
    float currentTime;
    private WaveHandler waveHandler;

    void Awake()
    {
        this.waveHandler = GameObject.FindObjectOfType<WaveHandler>();
        preInitEnemy = Resources.Load("Prefabs/Enemy", typeof(GameObject)) as GameObject;
    }

    void Update()
    {
        if (currentWave != null)
        {
            if (nextSubWaveNumber >= 0 && nextSubWaveNumber < currentWave.CountSubWaves())
            {
                if (currentTime < currentWave.GetTimeStamp(nextSubWaveNumber))
                {
                    currentTime += Time.deltaTime;
                }
                else
                {
                    SubWave subWave = currentWave.GetSubWave(nextSubWaveNumber);
                    SpawnSubWave(subWave);
                    nextSubWaveNumber++;
                }
            }
        }
    }

    public void SpawnWave(Wave wave)
    {
        this.numberOfEnemies = 0;
        this.nextSubWaveNumber = 0;
        this.currentTime = 0;
        this.currentWave = wave;
    }

    // Spawn all enemies of a subwave.
    public void SpawnSubWave(SubWave subWave)
    {
        foreach (EnemySettings enemy in subWave.GetEnemies())
        {
            InstantiateEnemyPrefab(enemy);
        }
    }

    public GameObject InstantiateEnemyPrefab(EnemySettings enemySettings)
    {
        GameObject initEnemy = Instantiate(preInitEnemy);
        if (!enemySettings.predefinedPosition)
        {
            enemySettings.spawnAngle = Random.value * 360;
        }

        initEnemy.GetComponent<Enemy>().SetStats(enemySettings);

        if (enemySettings.requiredKill)
        {
            waveHandler.NofifyRequiredKillUnitSpawned();
        }

        initEnemy.name = enemySettings.Name + numberOfEnemies;
        initEnemy.transform.localScale *= Parameters.SPRITE_SCALE_FACTOR;
        numberOfEnemies++;
        return initEnemy;
    }
}
