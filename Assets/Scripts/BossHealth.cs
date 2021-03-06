﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    ProgressBarBehaviour bossHealthBar;
    private GameObject gameOverPanel;
    private GameObject scoreLabel;
    private GameObject bossButtons;
    private GameObject itemButtons;
    private GameObject pauseButton;

    private GameObject activeAttackFireButton;
    private bool gameOver = false;
    private bool invunerable;
    private ColorModifier shieldColorModifier;
    private IEnumerator resetInvunerabilityCoroutine;
    private WaveHandler waveHandler;
    private Coroutine continueFieldShadeCoroutine;
    private CameraShake camShake;
    private AttackController attackController;

    void Awake()
    {
        this.activeAttackFireButton = GameObject.Find("ActiveAttackScreenButton");
        this.gameOverPanel = GameObject.Find("GameOverPanel");
        this.scoreLabel = GameObject.Find("ScoreLabel");
        this.bossButtons = GameObject.Find("BossButtons");
        this.itemButtons = GameObject.Find("ItemButtons");
        this.pauseButton = GameObject.Find("PauseButton");
        this.gameOverPanel.SetActive(false);
        this.scoreLabel.SetActive(true);
        this.bossButtons.SetActive(true);
        this.shieldColorModifier = this.transform.Find("Shield").gameObject.GetComponent<ColorModifier>();
        this.shieldColorModifier.SetSelectedColor(Parameters.BOSS_COLOR);
        this.waveHandler = GameObject.FindObjectOfType<WaveHandler>();
        this.camShake = GameObject.FindObjectOfType<CameraShake>();
        this.attackController = GameObject.FindObjectOfType<AttackController>();
    }

    void Start()
    {
        this.shieldColorModifier.SetDefaultColor(new Color(1.0f, 1.0f, 1.0f, 0.0f));
        bossHealthBar = GameObject.Find("BossHealthBar").GetComponent<ProgressBarBehaviour>();
        bossHealthBar.UpdateFill(BossHealthHolder.BossHealth / BossHealthHolder.BossFullHealth);
    }

    private IEnumerator continueShieldFadeAfterDamage()
    {
        yield return new WaitForSeconds(0.2f);
        this.shieldColorModifier.SetFadePaused(false);
    }

    public bool bossTakeDamage(float damage)
    {
        if (invunerable)
        {
            Color shieldColor = this.shieldColorModifier.GetColor();
            this.shieldColorModifier.SetFadePaused(true);
            if (this.continueFieldShadeCoroutine != null)
            {
                StopCoroutine(this.continueFieldShadeCoroutine);
            }
            this.shieldColorModifier.SetColor(Color.red);
            this.continueFieldShadeCoroutine = StartCoroutine(continueShieldFadeAfterDamage());
            return false;
        }

        camShake.Shake(0.05f, 0.1f);
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
            this.attackController.StopAttacking();
            this.gameOverPanel.SetActive(true);
            this.activeAttackFireButton.SetActive(false);
            this.scoreLabel.SetActive(false);
            this.bossButtons.SetActive(false);
            this.itemButtons.SetActive(false);
            this.pauseButton.SetActive(false);
            this.waveHandler.clearWave();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void HealBossPercentage(int percentage)
    {
        float healthToAdd = 0.01f * percentage * BossHealthHolder.BossFullHealth;
        BossHealthHolder.BossHealth = Mathf.Min(BossHealthHolder.BossHealth + healthToAdd, BossHealthHolder.BossFullHealth);
        bossHealthBar.UpdateFill(BossHealthHolder.BossHealth / BossHealthHolder.BossFullHealth);
    }

    public void MakeInvunerable(int seconds)
    {
        if(seconds <= 0)
        {
            return;
        }
        if (this.resetInvunerabilityCoroutine != null)
        {
            StopCoroutine(this.resetInvunerabilityCoroutine);
        }
        this.invunerable = true;
        this.shieldColorModifier.Select();
        this.shieldColorModifier.FadeToDeselected(seconds);
        this.resetInvunerabilityCoroutine = ResetInvunerabilityAfterTime(seconds);
        StartCoroutine(resetInvunerabilityCoroutine);
    }

    public IEnumerator ResetInvunerabilityAfterTime(float time) {
        yield return new WaitForSeconds(time);
        this.invunerable = false;
    }

    public void playAgain()
    {
        WaveNumber.waveNumber = 0;
        AttackLists.ResetUpgradedAttacks();
        BossItemHolder.BossItems = new ItemData[Parameters.NUMBER_OF_ITEMS];
        SceneHandler.SwitchScene("Main Menu Scene");
    }
}
