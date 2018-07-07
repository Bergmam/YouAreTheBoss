using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class WaveTests
{
    private Wave waveA;
    private Wave waveB;
    private Wave waveC;
    private Wave expextedMergeWaveAB;
    private Wave expextedAppendWaveAB;

    [SetUp]
    public void Init()
    {
        this.waveA = new Wave();
        this.waveA.AddSubWave(GenerateTestSubWave(), 0.0f);
        this.waveA.AddSubWave(GenerateTestSubWave(), 1.0f);
        this.waveB = new Wave();
        this.waveB.AddSubWave(GenerateTestSubWave(), 0.0f);
        this.waveC = new Wave();
        this.waveC.AddSubWave(GenerateTestSubWave(), 0.0f);

        this.expextedMergeWaveAB = new Wave();
        this.expextedMergeWaveAB.AddSubWave(GenerateTestSubWave(), 0.0f);
        this.expextedMergeWaveAB.AddSubWave(GenerateTestSubWave(), 0.0f);
        this.expextedMergeWaveAB.AddSubWave(GenerateTestSubWave(), 1.0f);

        this.expextedAppendWaveAB = new Wave();
        this.expextedAppendWaveAB.AddSubWave(GenerateTestSubWave(), 0.0f);
        this.expextedAppendWaveAB.AddSubWave(GenerateTestSubWave(), 1.0f);
        this.expextedAppendWaveAB.AddSubWave(GenerateTestSubWave(), 1.0f + Wave.STANDARD_WAVE_DURATION);
    }

    [Test]
    public void TestMerge()
    {
        this.waveA.Merge(this.waveB);
        Assert.AreEqual(this.waveA, this.expextedMergeWaveAB);
    }

    [Test]
    public void TestAppend()
    {
        this.waveA.Append(this.waveB);
        Assert.AreEqual(this.waveA, this.expextedAppendWaveAB);
    }

    [Test]
    public void TestEquals()
    {
        Assert.AreEqual(waveA, waveA);
        Assert.AreEqual(waveB, waveB);
        Assert.AreEqual(waveC, waveC);
        Assert.AreEqual(waveB, waveC);
        Assert.AreNotEqual(waveA, waveB);
        Assert.AreNotEqual(waveA, waveC);
    }

    [Test]
    public void TestHashCode()
    {
        Assert.AreEqual(waveA.GetHashCode(), waveA.GetHashCode());
        Assert.AreEqual(waveB.GetHashCode(), waveB.GetHashCode());
        Assert.AreEqual(waveC.GetHashCode(), waveC.GetHashCode());
        Assert.AreEqual(waveB.GetHashCode(), waveC.GetHashCode());
        Assert.AreNotEqual(waveA.GetHashCode(), waveB.GetHashCode());
        Assert.AreNotEqual(waveA.GetHashCode(), waveC.GetHashCode());
    }

    public SubWave GenerateTestSubWave()
    {
        SubWave subWave = new SubWave();
        subWave.AddEnemy(new StatsHolder("TestEnemy", 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, Color.black));
        return subWave;
    }

}