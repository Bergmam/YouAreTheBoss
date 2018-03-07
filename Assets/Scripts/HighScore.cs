using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (!WaveNumber.highScoreGenerated){
			gameObject.SetActive(false);
		} else {
			Text text = GetComponent<Text>();
			text.text = "Highscore: " + WaveNumber.highScore;
		}
	}
}
