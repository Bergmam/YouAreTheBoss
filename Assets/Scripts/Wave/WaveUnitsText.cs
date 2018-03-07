using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveUnitsText : MonoBehaviour {

	// Use this for initialization
	Dictionary<string, int> enemyTypeToAmount = new Dictionary<string, int>();
	void Start () {

		Text text = GetComponent<Text>();

		foreach(SubWave subWave in WaveFactory.GenerateWave(WaveNumber.waveNumber)){
			foreach(StatsHolder statsHolder in subWave.GetEnemies()){
				if (!enemyTypeToAmount.ContainsKey(statsHolder.Name)){
					enemyTypeToAmount.Add(statsHolder.Name, 1);
				} else {
					enemyTypeToAmount[statsHolder.Name]++;// enemyTypeToAmount[statsHolder.Name] + 1);
				}
			}
		}

		string enemyInfoText = "";
		List<string> nameList = new List<string>();

		foreach(KeyValuePair<string, int> entry in enemyTypeToAmount){
			nameList.Add(entry.Key + ": " + entry.Value + "\n");
		}

		nameList.Sort();
		foreach(string name in nameList){
			enemyInfoText += name;
		}
		text.text = enemyInfoText.ToUpper();
		
	}
}
