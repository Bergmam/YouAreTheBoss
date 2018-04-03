using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveUnitsText : MonoBehaviour {

	// Use this for initialization
	Dictionary<string, int> waveSummary = new Dictionary<string, int>();
	void Start () {

		Text text = GetComponent<Text>();

		waveSummary = Summarize(WaveFactory.GenerateWave(WaveNumber.waveNumber));

		string enemyInfoText = "";
		List<string> nameList = new List<string>();

		foreach(KeyValuePair<string, int> entry in waveSummary){
			nameList.Add(entry.Key + ": " + entry.Value + "\n");
		}

		nameList.Sort();
		foreach(string name in nameList){
			enemyInfoText += name;
		}
		text.text = enemyInfoText.ToUpper();
		
	}

	public Dictionary<string, int> Summarize(List<SubWave> wave)
	{
		Dictionary<string, int> summary = new Dictionary<string, int>();
		foreach(SubWave subWave in wave)
		{
			foreach(StatsHolder enemyStats in subWave.GetEnemies())
			{
				foreach(KeyValuePair<string, bool> attribute in enemyStats.GetAttributes()){
					if(attribute.Value){
						if(!summary.ContainsKey(attribute.Key)){
							summary.Add(attribute.Key, 1);
						}
						else
						{
							summary[attribute.Key]++;
						}
					}
				}
			}
		}
		
		return summary;
	}
}
