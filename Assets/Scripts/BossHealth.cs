using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    ProgressBarBehaviour bossHealthBar;
    private GameObject gameOverPanel;
    private GameObject scoreLabel;
    private GameObject bossButtons;

    private GameObject activeAttackFireButton;

    private bool gameOver = false;

    void Awake()
    {
        this.activeAttackFireButton = GameObject.Find("ActiveAttackScreenButton");
        this.gameOverPanel = GameObject.Find("GameOverPanel");
        this.scoreLabel = GameObject.Find("ScoreLabel");
        this.bossButtons = GameObject.Find("BossButtons");
        this.gameOverPanel.SetActive(false);
        this.scoreLabel.SetActive(true);
        this.bossButtons.SetActive(true);
    }

    void Start()
    {
        bossHealthBar = GameObject.Find("BossHealthBar").GetComponent<ProgressBarBehaviour>();
        bossHealthBar.UpdateFill(BossHealthHolder.BossHealth / BossHealthHolder.BossFullHealth);
    }

    public bool bossTakeDamage(float damage)
    {
        BossHealthHolder.BossHealth = BossHealthHolder.BossHealth - damage;
        bossHealthBar.UpdateFill(BossHealthHolder.BossHealth / BossHealthHolder.BossFullHealth);

        if (BossHealthHolder.BossHealth <= 0 && !gameOver)
        {
            gameOver = true;
            if (!WaveNumber.highScoreGenerated)
            {
                WaveNumber.highScoreGenerated = true;
            }
            if (WaveNumber.waveNumber > WaveNumber.highScore)
            {
                WaveNumber.highScore = WaveNumber.waveNumber;
            }
            this.gameOverPanel.SetActive(true);
            this.activeAttackFireButton.SetActive(false);
            this.scoreLabel.SetActive(false);
            this.bossButtons.SetActive(false);
            ((WaveHandler)GameObject.FindObjectOfType(typeof(WaveHandler))).clearWave();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void playAgain()
    {
        WaveNumber.waveNumber = 0;
        AttackLists.ResetUpgradedAttacks();
        SceneHandler.SwitchScene("Main Menu Scene");
    }
}
