// using NUnit.Framework;
// using UnityEngine;

// public class SubWaveTests
// {
//     private SubWave subWaveA;
//     private SubWave subWaveB;
//     private SubWave otherSubWave;

//     [SetUp]
//     public void Init()
//     {

//         StatsHolder statsHolderA = new StatsHolder("TestHolder", 1.0f, 1.0f, RangeLevel.MELE, 1.0f, 1.0f, Color.black);
//         subWaveA = new SubWave();
//         subWaveA.AddEnemy(statsHolderA);
//         StatsHolder statsHolderB = new StatsHolder("TestHolder", 1.0f, 1.0f, RangeLevel.MELE, 1.0f, 1.0f, Color.black);
//         subWaveB = new SubWave();
//         subWaveB.AddEnemy(statsHolderA);
//         StatsHolder otherStatsHolder = new StatsHolder("AnotherTestHolder", 1.0f, 1.0f, RangeLevel.MELE, 1.0f, 1.0f, Color.black);
//         otherSubWave = new SubWave();
//         otherSubWave.AddEnemy(otherStatsHolder);
//     }

//     [Test]
//     public void TestEquals()
//     {
//         Assert.AreEqual(subWaveA, subWaveA);
//         Assert.AreEqual(subWaveA, subWaveB);
//         Assert.AreEqual(subWaveB, subWaveB);
//         Assert.AreNotEqual(subWaveA, otherSubWave);
//         Assert.AreNotEqual(subWaveB, otherSubWave);
//     }

//     [Test]
//     public void TestHashCode()
//     {
//         Assert.AreEqual(subWaveA.GetHashCode(), subWaveA.GetHashCode());
//         Assert.AreEqual(subWaveA.GetHashCode(), subWaveB.GetHashCode());
//         Assert.AreEqual(subWaveB.GetHashCode(), subWaveB.GetHashCode());
//         Assert.AreNotEqual(subWaveA.GetHashCode(), otherSubWave.GetHashCode());
//         Assert.AreNotEqual(subWaveB.GetHashCode(), otherSubWave.GetHashCode());
//     }
// }