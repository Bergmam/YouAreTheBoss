using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour {

	public float BossHealthVal = 100.0f;
	ProgressBarBehaviour bossHealthBar;

	// Use this for initialization
	void Start () {
		bossHealthBar = GameObject.Find("BossHealthBar").GetComponent<ProgressBarBehaviour>();
	}
	
	public void bossTakeDamage(float damage){
		BossHealthVal = BossHealthVal - damage;
		bossHealthBar.UpdateFill(BossHealthVal / 100.0f);

		if (BossHealthVal <= 0) {
			if (!WaveNumber.highScoreGenerated){
				WaveNumber.highScoreGenerated = true;
			}
			if (WaveNumber.waveNumber > WaveNumber.highScore) {
				WaveNumber.highScore = WaveNumber.waveNumber;
			}
			WaveNumber.waveNumber = 0;
			SceneHandler.SwitchScene("Main Menu Scene");
		}
	}
}
