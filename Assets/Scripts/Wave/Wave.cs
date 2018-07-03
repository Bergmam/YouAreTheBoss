using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave
{

    public Wave()
    {
        this.subWaves = new List<SubWave>();
    }

    private List<SubWave> subWaves;

    public List<SubWave> GetSubWaves()
    {
        return this.subWaves;
    }

    public void SetSubwaves(List<SubWave> subWaves)
    {
        this.subWaves = subWaves;
    }

    public SubWave GetSubWave(int index)
    {
        return this.subWaves[index];
    }

    public int Count()
    {
        return this.subWaves.Count;
    }

    public void AddSubWave(SubWave subWave)
    {
        this.subWaves.Add(subWave);
    }

    public void SwapSubWaves(int i, int j)
    {
        SubWave tmp = this.subWaves[i];
        this.subWaves[i] = this.subWaves[j];
        this.subWaves[j] = tmp;
    }

}
