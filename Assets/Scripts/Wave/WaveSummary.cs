using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSummary : MonoBehaviour {

	void Start () {

		//Summarize current wave

		//Spawn prefab for each attribute > 0

		//Set correct icon and scale of each prefab

		//Position prefabs	
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
