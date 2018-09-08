using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{

    public float BossHealthVal = 100.0f;
    ProgressBarBehaviour bossHealthBar;
    private GameObject gameOverPanel;
    private GameObject scoreLabel;

    void Awake()
    {
        this.gameOverPanel = GameObject.Find("GameOverPanel");
        this.scoreLabel = GameObject.Find("ScoreLabel");
        this.gameOverPanel.SetActive(false);
        this.scoreLabel.SetActive(true);
    }

    void Start()
    {
        bossHealthBar = GameObject.Find("BossHealthBar").GetComponent<ProgressBarBehaviour>();
    }

    public void bossTakeDamage(float damage)
    {
        BossHealthVal = BossHealthVal - damage;
        bossHealthBar.UpdateFill(BossHealthVal / 100.0f);

        if (BossHealthVal <= 0)
        {
            if (!WaveNumber.highScoreGenerated)
            {
                WaveNumber.highScoreGenerated = true;
            }
            if (WaveNumber.waveNumber > WaveNumber.highScore)
            {
                WaveNumber.highScore = WaveNumber.waveNumber;
            }
            this.gameOverPanel.SetActive(true);
            this.scoreLabel.SetActive(false);
            ((WaveHandler)GameObject.FindObjectOfType(typeof(WaveHandler))).clearWave();
        }
    }

    public void playAgain()
    {
        this.gameOverPanel.SetActive(false);
        this.scoreLabel.SetActive(true);
        WaveNumber.waveNumber = 0;
        AttackLists.ResetUpgradedAttacks();
        SceneHandler.SwitchScene("Main Menu Scene");
    }
}
