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
            if (nextSubWaveNumber >= 0 && nextSubWaveNumber < currentWave.Count())
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
        foreach (StatsHolder enemy in subWave.GetEnemies())
        {
            InstantiateEnemyPrefab(enemy);
        }
    }

    public void InstantiateEnemyPrefab(StatsHolder stats)
    {
        GameObject initEnemy = Instantiate(preInitEnemy);
        if (!stats.predefinedPosition)
        {
            stats.spawnAngle = Random.value * 360;
        }

        initEnemy.GetComponent<Enemy>().SetStats(
            stats
        );

        if (stats.requiredKill)
        {
            waveHandler.NofifyRequiredKillUnitSpawned();
        }

        initEnemy.name = stats.Name + numberOfEnemies;
        numberOfEnemies++;
    }
}
