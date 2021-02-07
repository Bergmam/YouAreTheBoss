// using NUnit.Framework;
// using UnityEngine;

// public class StatsHolderTests
// {
//     [Test]
//     public void TestEquals()
//     {
//         StatsHolder statsHolderA = new StatsHolder("TestHolder", 1.0f, 1.0f, RangeLevel.MELE, 1.0f, 1.0f, Color.black);
//         StatsHolder statsHolderB = new StatsHolder("TestHolder", 1.0f, 1.0f, RangeLevel.MELE, 1.0f, 1.0f, Color.black);

//         StatsHolder[] otherStatsHolders = {
//             new StatsHolder("AnotherTestHolder", 1.0f, 1.0f, RangeLevel.MELE, 1.0f, 1.0f, Color.black),
//             new StatsHolder("TestHolder", 2.0f, 1.0f, RangeLevel.MELE, 1.0f, 1.0f, Color.black),
//             new StatsHolder("TestHolder", 1.0f, 2.0f, RangeLevel.MELE, 1.0f, 1.0f, Color.black),
//             new StatsHolder("TestHolder", 1.0f, 1.0f, RangeLevel.MID, 1.0f, 1.0f, Color.black),
//             new StatsHolder("TestHolder", 1.0f, 1.0f, RangeLevel.MELE, 2.0f, 1.0f, Color.black),
//             new StatsHolder("TestHolder", 1.0f, 1.0f, RangeLevel.MELE, 1.0f, 2.0f, Color.black),
//             new StatsHolder("TestHolder", 1.0f, 1.0f, RangeLevel.MELE, 1.0f, 1.0f, Color.red)
//         };
//         Assert.AreEqual(statsHolderA, statsHolderA);
//         Assert.AreEqual(statsHolderA, statsHolderB);
//         Assert.AreEqual(statsHolderB, statsHolderB);
//         foreach (StatsHolder otherStatsHolder in otherStatsHolders)
//         {
//             Assert.AreNotEqual(statsHolderA, otherStatsHolder);
//             Assert.AreNotEqual(statsHolderB, otherStatsHolder);
//         }
//     }

//     [Test]
//     public void TestHashCode()
//     {
//         StatsHolder statsHolderA = new StatsHolder("TestHolder", 1.0f, 1.0f, RangeLevel.MELE, 1.0f, 1.0f, Color.black);
//         StatsHolder statsHolderB = new StatsHolder("TestHolder", 1.0f, 1.0f, RangeLevel.MELE, 1.0f, 1.0f, Color.black);

//         StatsHolder[] otherStatsHolders = {
//             new StatsHolder("AnotherTestHolder", 1.0f, 1.0f, RangeLevel.MELE, 1.0f, 1.0f, Color.black),
//             new StatsHolder("TestHolder", 1.0f, 1.0f, RangeLevel.MELE, 1.0f, 1.0f, Color.red),
//             new StatsHolder("TestHolder", 2.0f, 1.0f, RangeLevel.MELE, 1.0f, 1.0f, Color.black),
//             new StatsHolder("TestHolder", 1.0f, 2.0f, RangeLevel.MELE, 1.0f, 1.0f, Color.black),
//             new StatsHolder("TestHolder", 1.0f, 1.0f, RangeLevel.MID, 1.0f, 1.0f, Color.black),
//             new StatsHolder("TestHolder", 1.0f, 1.0f, RangeLevel.MELE, 2.0f, 1.0f, Color.black),
//             new StatsHolder("TestHolder", 1.0f, 1.0f, RangeLevel.MELE, 1.0f, 2.0f, Color.black)
//         };
//         Assert.AreEqual(statsHolderA.GetHashCode(), statsHolderA.GetHashCode());
//         Assert.AreEqual(statsHolderA.GetHashCode(), statsHolderB.GetHashCode());
//         Assert.AreEqual(statsHolderB.GetHashCode(), statsHolderB.GetHashCode());
//         foreach (StatsHolder otherStatsHolder in otherStatsHolders)
//         {
//             Assert.AreNotEqual(statsHolderA.GetHashCode(), otherStatsHolder.GetHashCode());
//             Assert.AreNotEqual(statsHolderB.GetHashCode(), otherStatsHolder.GetHashCode());
//         }
//     }
// }