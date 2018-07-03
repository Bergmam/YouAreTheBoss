using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

public class UtilTests {

	[Test]
	public void AllAnglesInCounterClockwiseLimitsOfZeroAndThreeSixty()
	{
		float angle = -720f;
		while(angle < 720f)
		{
			Assert.That(RotationUtils.InCounterClockwiseLimits(angle,0,360));
			angle += 0.5f;
		}
	}

	[Test]
	public void ZeroIsNotInCounterClockwiseLimitsOfOneToThreeFiftyNine()
	{
		float angle = 1f;
		while(angle < 360f)
		{
			Assert.False(RotationUtils.InCounterClockwiseLimits(0,angle,360));
			angle += 0.5f;
		}
	}

	[Test]
	public void MiddleOfRotationsTest(){
		float negativeStart = 720;
		float angle = -negativeStart;
		while(angle < 720f)
		{
			float normalizedAngle = (negativeStart + angle) % 360;
			Assert.AreEqual(RotationUtils.MiddleOfRotations(angle,360),normalizedAngle + (360 - normalizedAngle) / 2);
			angle += 0.5f;
		}
	}

	[Test]
	public void ZeroInSmallerLimitsTest(){
		float negativeStart = 720f;
		float angle1 = -negativeStart;
		float angle2 = -negativeStart;
		while(angle1 < 720f)
		{
			float normalizedAngle1 = (negativeStart + angle1) % 360f;
			while(angle2 < 720f)
			{
				float normalizedAngle2 = (negativeStart + angle2) % 360f;
				if(normalizedAngle1 < normalizedAngle2)
				{
					AssertZeroInLimits(angle1, angle2);
				}
				else if (normalizedAngle1 < normalizedAngle2)
				{
					AssertZeroInLimits(angle2, angle1);
				}
				else if(normalizedAngle1 == 0 && normalizedAngle2 == 0)
				{
					Assert.That(RotationUtils.ZeroInSmallerLimit(angle1,angle2));
				}
				else
				{
					Assert.False(RotationUtils.ZeroInSmallerLimit(angle1,angle2));
				}
				angle2 += 0.5f;
			}
			angle1 += 0.5f;
		}
	}

	public void AssertZeroInLimits(float lowAngle, float hightAngle)
	{
		if(lowAngle >= 270f && hightAngle <= 90f)
		{
			Assert.That(RotationUtils.ZeroInSmallerLimit(lowAngle,hightAngle));
		}
		else
		{
			Assert.False(RotationUtils.ZeroInSmallerLimit(lowAngle,hightAngle));
		}
	}

	[Test]
	public void InSmallerLimitTest()
	{
		float negativeStart = 720f;
		float angle = -negativeStart;
		while(angle < 720f)
		{
			float normalizedAngle = (negativeStart + angle) % 360;
			Assert.That(RotationUtils.InSmallerLimit(angle, angle-89.9f, angle+90f));
			Assert.False(RotationUtils.InSmallerLimit(angle, angle-90f, angle+90.1f));
			angle += 0.5f;
		}
	}

	[Test]
	public void MakePositiveAngleTest()
	{
		float angle = -720f;
		while(angle < 720f)
		{
			Assert.AreEqual(RotationUtils.MakePositiveAngle(angle), (1440 + angle) % 360);
			angle += 0.5f;
		}
	}

	[Test]
	public void CounterClockwiseDistanceTest()
	{
		float negativeStart = 720f;
		float angle = -negativeStart;
		while(angle < 720f)
		{
			float normalizedAngle = (negativeStart + angle) % 360;
			Assert.AreEqual(RotationUtils.CounterClockwiseDistance(angle, angle), 0f);
			Assert.AreEqual(RotationUtils.CounterClockwiseDistance(0, angle), normalizedAngle);
			Assert.AreEqual(RotationUtils.CounterClockwiseDistance(angle, 360), (360 - normalizedAngle) % 360);
			angle += 0.5f;
		}
	}

	[Test]
	public void RadialXYConversionTest()
	{
		for(int i = 0; i < 10000; i++)
		{
			float x = Random.Range(-1000.0f, 1000.0f);
			float y = Random.Range(-1000.0f, 1000.0f);
			Vector3 position = new Vector3(x, y);
			RadialPosition radialPosition = RotationUtils.XYToRadialPos(position);
			Vector3 newPosition = RotationUtils.RadialPosToXY(radialPosition);
			Assert.AreEqual(position.x, newPosition.x, 0.001f);
			Assert.AreEqual(position.y, newPosition.y, 0.001f);
			Assert.AreEqual(position.z, 0f);
			Assert.AreEqual(newPosition.z, 0f);
		}
	}
}
