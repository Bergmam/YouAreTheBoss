using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

public class UtilTests {

	[Test]
	public void AllanglesInCounterClockwiseLimitsOfZeroAndThreeSixty(){
		float angle = -720f;
		while(angle < 720f){
			Assert.That(RotationUtils.InCounterClockwiseLimits(angle,0,360));
			angle += 0.5f;
		}
	}

	[Test]
	public void ZeroIsNotInCounterClockwiseLimitsOfOneToThreeFiftyNine(){
		float angle = 1f;
		while(angle < 360f){
			Assert.False(RotationUtils.InCounterClockwiseLimits(0,angle,360));
			angle += 0.5f;
		}
	}

	[Test]
	public void MiddleOfRotationsBetweenZeroAndThreeSixtyIsHalf(){
		float angle = 0f;
		while(angle < 360f){
			Assert.AreEqual(RotationUtils.MiddleOfRotations(angle,360),angle + (360 - angle) / 2);
			angle += 0.5f;
		}
	}

	// TODO: Add tests for the remaining methods:
	/*
	public static float MiddleOfRotations(float low, float high)
	public static bool ZeroInSmallerLimit(float limit1, float limit2)
	public static bool InSmallerLimit(float angle, float limit1, float limit2)
	public static float CounterClockwiseDistance(float angle1, float angle2)
	public static float MakePositiveAngle(float angle)
	public static Vector3 RadialPosToXY(RadialPosition radialPosition)
	public static RadialPosition XYToRadialPos (Vector3 position)
 	*/
}
