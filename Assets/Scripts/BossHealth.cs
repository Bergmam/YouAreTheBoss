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
			// TODO: Remove temporary logic for reseting the game
			object[] obj = GameObject.FindObjectsOfType(typeof (GameObject));
			foreach (object o in obj)
			{
				GameObject g = (GameObject) o;
				if (g.name.Contains("Enemy")){
					Destroy(g);
				}
			}
			BossHealthVal = 100.0f;
			bossHealthBar.UpdateFill(1);
		}
	}
}
