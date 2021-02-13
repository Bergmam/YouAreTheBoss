using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Wave
{
    private SortedDictionary<float, SubWave> subWaves;

    public Wave()
    {
        this.subWaves = new SortedDictionary<float, SubWave>();
    }

    public SortedDictionary<float, SubWave> GetSubWaves()
    {
        return this.subWaves;
    }

    public void SetSubwaves(SortedDictionary<float, SubWave> subWaves)
    {
        this.subWaves = subWaves;
    }

    public SubWave GetSubWave(int index)
    {
        return this.subWaves.ToList()[index].Value;
    }

    public float GetTimeStamp(int index)
    {
        return this.subWaves.ToList()[index].Key;
    }

    public int CountSubWaves()
    {
        return this.subWaves.Count;
    }

    public int CountEnemies()
    {
        int nbrOfEnemies = 0;
        foreach (KeyValuePair<float, SubWave> timeStampAndSubWave in this.subWaves)
        {
            nbrOfEnemies += timeStampAndSubWave.Value.GetEnemies().Count;
        }
        return nbrOfEnemies;
    }

    public List<EnemySettings> GetEnemies()
    {
        List<EnemySettings> enemies = new List<EnemySettings>();
        foreach (KeyValuePair<float, SubWave> timeStampAndSubWave in this.subWaves)
        {
            enemies.AddRange(timeStampAndSubWave.Value.GetEnemies());
        }
        return enemies;
    }


    public void AddSubWave(SubWave subWave, float timeStamp)
    {
        if (this.subWaves.ContainsKey(timeStamp))
        {
            this.subWaves[timeStamp].Merge(subWave);
        }
        else
        {
            this.subWaves[timeStamp] = subWave;
        }
    }

    public void SwapSubWaves(int i, int j)
    {
        float timeStampI = GetTimeStamp(i);
        float timeStampJ = GetTimeStamp(j);
        SubWave tmp = this.subWaves[timeStampI];
        this.subWaves[timeStampI] = this.subWaves[timeStampJ];
        this.subWaves[timeStampJ] = tmp;
    }

    internal void Shuffle()
    {
        System.Random rng = new System.Random((int)DateTime.Now.Ticks);
        for (int i = 0; i < CountSubWaves(); i++)
        {
            int j = (int)Mathf.Round(UnityEngine.Random.Range(0, CountSubWaves()));
            SwapSubWaves(i, j);
        }
    }

    public float GetDuration()
    {
        if (this.subWaves.Count == 0)
        {
            return 0.0f;
        }
        return this.GetTimeStamp(this.subWaves.Count - 1) + Parameters.STANDARD_WAVE_DURATION;
    }

    public void Append(Wave other)
    {
        this.Merge(other, this.GetDuration());
    }

    public void Merge(Wave other)
    {
        this.Merge(other, 0f);
    }

    public void Merge(Wave other, float offset)
    {
        foreach (KeyValuePair<float, SubWave> timeStampAndSubWave in other.subWaves)
        {
            float timeStamp = timeStampAndSubWave.Key;
            SubWave subWave = timeStampAndSubWave.Value;
            this.AddSubWave(subWave, timeStamp + offset);
        }
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        Wave other = (Wave)obj;

        if (other.subWaves.Count != this.subWaves.Count)
        {
            return false;
        }

        return this.subWaves.ToList().SequenceEqual(other.subWaves.ToList());
    }

    public override int GetHashCode()
    {
        int hashCode = 0;
        foreach (KeyValuePair<float, SubWave> timeStampAndSubWave in this.subWaves)
        {
            hashCode += (int)(timeStampAndSubWave.Key * timeStampAndSubWave.Value.GetHashCode());
        }
        return hashCode;
    }

}
