using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WaveSummary : MonoBehaviour
{

    void Start()
    {

        //Summarize current wave
        Wave currentWave = WaveFactory.GenerateWave(WaveNumber.waveNumber);
        Dictionary<string, int> waveSummary = Summarize(currentWave);

        // Sort summary based on value (magnitude) to make sure largets is placed first in the grid view.
        List<KeyValuePair<string, int>> waveSummaryList = waveSummary.ToList();
        waveSummaryList.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));

        //Spawn prefab for each attribute > 0
        foreach (KeyValuePair<string, int> waveAttribute in waveSummaryList)
        {
            string attribute = waveAttribute.Key;
            int magnitude = waveAttribute.Value;
            if (magnitude > 0)
            {

                // Place summary icons in grid view.
                GameObject preInitIcon = Resources.Load("Prefabs/WaveAttributeIcon", typeof(GameObject)) as GameObject;
                GameObject icon = Instantiate(preInitIcon);
                icon.transform.name = attribute + "Icon";
                icon.transform.SetParent(transform, false);

                // Select correct image for each summary icon panel and scale the image.
                AttributeIconHandler attributeHandler = icon.GetComponent<AttributeIconHandler>();
                attributeHandler.SetAttributeAndMagnitude(attribute, magnitude);
            }
        }
    }

    public Dictionary<string, int> Summarize(Wave wave)
    {
        Dictionary<string, int> summary = new Dictionary<string, int>();
        foreach (KeyValuePair<float, SubWave> timeStampSubWave in wave.GetSubWaves())
        {
            SubWave subWave = timeStampSubWave.Value;
            foreach (StatsHolder enemyStats in subWave.GetEnemies())
            {
                foreach (KeyValuePair<string, bool> attribute in enemyStats.GetAttributes())
                {
                    if (attribute.Value)
                    {
                        if (!summary.ContainsKey(attribute.Key))
                        {
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
