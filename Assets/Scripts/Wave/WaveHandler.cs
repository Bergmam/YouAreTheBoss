using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveHandler : MonoBehaviour
{
    public GameObject ContinueButton;
    public GameObject WaveClearedText;

    private EnemySpawner enemySpawner;
    private int requiredKillEnemiesInWave;
    private int requireKillUnitsSpawned;
    private bool gameOver = false;
    private int activeItemPickups;

    void Awake()
    {
        this.enemySpawner = GetComponent<EnemySpawner>();
    }

    void Start()
    {
        this.ContinueButton.SetActive(false);
        this.WaveClearedText.SetActive(false);

        Wave wave = CurrentWave.wave;
        requiredKillEnemiesInWave = 0;
        foreach (KeyValuePair<float, SubWave> timeStampSubWave in wave.GetSubWaves())
        {
            SubWave subWave = timeStampSubWave.Value;
            requiredKillEnemiesInWave += subWave.RequiredKillEnemyCount();
        }
        this.requireKillUnitsSpawned = 0;
        this.enemySpawner.SpawnWave(wave);
    }

    void Update()
    {
        int livingEnemies = GameObject.FindObjectsOfType(typeof(Enemy)).Length;
        if (livingEnemies <= 0
            && requireKillUnitsSpawned >= requiredKillEnemiesInWave
            && !gameOver)
        {
            gameOver = true;
            WaveNumber.waveNumber++;
            StartCoroutine(waitAndShowContinueButton());
        }
    }

    public void NofifyRequiredKillUnitSpawned()
    {
        requireKillUnitsSpawned++;
    }

    private IEnumerator waitAndShowContinueButton()
    {
        yield return new WaitForSeconds(0.5f);
        this.ContinueButton.SetActive(true);
        this.WaveClearedText.SetActive(true);
    }

    public void GoToMainMenu()
    {
        SceneHandler.SwitchScene("Main Menu Scene");
    }

    public void clearWave()
    {
        this.gameOver = true;
        this.enemySpawner.SpawnWave(null);
        foreach (Enemy enemy in GameObject.FindObjectsOfType(typeof(Enemy)))
        {
            GameObject.Destroy(enemy.gameObject);
        }
    }

    public void ItemAdded()
    {
        this.activeItemPickups++;
    }

    public void ItemRemoved()
    {
        this.activeItemPickups--;
    }
}
