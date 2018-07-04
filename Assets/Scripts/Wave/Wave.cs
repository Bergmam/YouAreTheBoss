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

    public int Count()
    {
        return this.subWaves.Count;
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

    public float GetDuration()
    {
        return this.GetTimeStamp(this.subWaves.Count - 1);
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

}
