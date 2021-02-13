using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WaveSummary : MonoBehaviour
{

    void Start()
    {

        // Summarize current wave
        CurrentWave.wave = WaveFactory.GenerateWave(WaveNumber.waveNumber);
        Wave currentWave = CurrentWave.wave;

        Dictionary<string, int> waveSummary = Summarize(currentWave);

        // Sort summary based on value (magnitude) to make sure largets is placed first in the grid view.
        List<KeyValuePair<string, int>> waveSummaryList = waveSummary.ToList();
        waveSummaryList.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));

        // Spawn prefab for each attribute > 0
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
        foreach (EnemySettings enemySettings in wave.GetEnemies())
        {
            Dictionary<string, bool> attributes = new Dictionary<string, bool>();
            attributes.Add("strong", enemySettings.Damage >= Parameters.STRONG_ENEMY_MIN_DAMAGE);
            attributes.Add("fast", enemySettings.MovementSpeed >= Parameters.FAST_ENEMY_MIN_SPEED);
            attributes.Add("rotating", enemySettings.angularSpeed != 0 || enemySettings.circlingSpeed != 0);
            attributes.Add("ranged", RangeUtils.rangeLevelToFloatRange(enemySettings.Range) > Parameters.MELEE_RANGE);
            attributes.Add("durable", enemySettings.Health >= Parameters.DURABLE_ENEMY_MIN_HEALTH);
            attributes.Add("mele", !(RangeUtils.rangeLevelToFloatRange(enemySettings.Range) > Parameters.MELEE_RANGE));
            attributes.Add("self_destruct", enemySettings.selfDestruct);
            
            foreach (KeyValuePair<string, bool> attribute in attributes)
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

        return summary;
    }
}
