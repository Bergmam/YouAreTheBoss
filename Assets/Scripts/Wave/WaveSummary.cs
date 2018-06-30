using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSummary : MonoBehaviour {

	void Start () {

		//Summarize current wave
		List<SubWave> currentWave = WaveFactory.GenerateWave (WaveNumber.waveNumber);
		Dictionary<string, int> waveSummary = Summarize(currentWave);

		float nextPositionX = 0.1f;
		float nextPositionY = 1.0f;

		//Spawn prefab for each attribute > 0
		foreach(KeyValuePair<string, int> waveAttribute in waveSummary)
		{
			print(waveAttribute.Key + ": " + waveAttribute.Value);
			string attribute = waveAttribute.Key;
			int magnitude = waveAttribute.Value;
			if(magnitude > 0){
				GameObject preInitIcon = Resources.Load ("Prefabs/WaveAttributeIcon", typeof(GameObject)) as GameObject;
				GameObject icon = Instantiate (preInitIcon, transform);
				icon.transform.name = attribute + "Icon";

				RectTransform iconRectTransform = icon.GetComponent<RectTransform>();
				iconRectTransform.anchorMin = new Vector2(nextPositionX, nextPositionY);
				iconRectTransform.anchorMax = new Vector2(nextPositionX, nextPositionY);
				iconRectTransform.offsetMin = new Vector2(0f, 0f);
				iconRectTransform.offsetMax = new Vector2(0f, 0f);
				iconRectTransform.sizeDelta = new Vector2(100f, 100f);

				AttributeIconHandler attributeHandler = icon.GetComponent<AttributeIconHandler>();
				attributeHandler.SetAttributeAndMagnitude(attribute, magnitude);

				// Position prefabs	
			}
			nextPositionX += 0.1f;
			if(nextPositionX >= 0.9f){
				nextPositionY -= 0.1f;
				nextPositionX = 0.1f;
			}
		}
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
