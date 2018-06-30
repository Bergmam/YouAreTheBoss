using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class containing general util methods for calculating rotations in the game.
/// </summary>
public class RotationUtils
{
	/// <summary>
	/// Returns true if the angle is between low and high. The values are always counted in a clockwise manner.
	/// The "low"-angle is always the one with the lowest angle, and the "high" is the one with the second highest angle,
	/// regardless of which is the "from"-angle and which is the "to"-angle.
	/// </summary>
	/// <param name="angle">The angle to be checked.</param>
	/// <param name="low">The low value for angle to be compared with.</param>
	/// <param name="high">The high value for angle to be compared with.</param>
	public static bool InCounterClockwiseLimits(float angle, float low, float high)
	{
        angle = MakePositiveAngle(angle);
        low = MakePositiveAngle(low);
        high = MakePositiveAngle(high);

		bool zeroInLimits = high < low;
		bool inLimitsAroundZero = zeroInLimits && (angle < high || angle > low);
		bool inLimitsWithoutZero = !zeroInLimits && (angle > low && angle < high);
        bool lowEqualsHigh = Mathf.Abs(high-low) < 0.0001f;
        if(Mathf.Abs(high-low) > 0.0001f && Mathf.Abs(high-low) < 0.05f){
            throw new System.Exception("An angle of size " + Mathf.Abs(high-low) + " was found!");
        }
		return inLimitsAroundZero || inLimitsWithoutZero || lowEqualsHigh;
	}	

	/// <summary>
	/// The method returns midpoint between two angles low and high. Angles from 0 to 360.
	/// </summary>
	/// <param name="low">The low angle.</param>
	/// <param name="high">The High angle.</param>
	public static float MiddleOfRotations(float low, float high)
	{
        low = MakePositiveAngle(low);
        high = MakePositiveAngle(high);

		//High has to be highter than low. Need to check if high is equal to or has passed zero.
		if (InCounterClockwiseLimits (0.0f, low, high) || high == 0)
		{
			high += 360;
		}
        return ((high+low)/2) % 360;
	}

    /// <summary>
    /// Calls InSmallerLimit with Zero as the angle parameter. 
    /// Checks if zero is between the two limits.
    /// </summary>
    /// <param name="limit1">The "from"-limit to test<param>
    /// <param name="limit2">The "to"-limit to test</param>
    /// <returns></returns>
    public static bool ZeroInSmallerLimit(float limit1, float limit2)
    {
        return InSmallerLimit(0, limit1, limit2);
    }

    /// <summary>
    /// Checks if a specified angle is between the smaller gap in the rotation circle
    /// between the two limits.
    /// </summary>
    /// <param name="angle">The angle to test against</param>
    /// <param name="limit1">The "from"-limit to test</param>
    /// <param name="limit2">The "to"-limit to test</param>
    /// <returns></returns>
    public static bool InSmallerLimit(float angle, float limit1, float limit2)
    {
        if(CounterClockwiseDistance(limit1,limit2) < CounterClockwiseDistance(limit2, limit1))
        {
            return InCounterClockwiseLimits(angle, limit1, limit2);
        } else
        {
            return InCounterClockwiseLimits(angle, limit2, limit1);
        }
    }

    /// <summary>
    /// Help function to InSmallerLimit. 
    /// Returns the counterclockwise distance between the two angles.
    /// </summary>
    /// <param name="angle1"></param>
    /// <param name="angle2"></param>
    /// <returns></returns>
    public static float CounterClockwiseDistance(float angle1, float angle2)
    {
        angle1 = MakePositiveAngle(angle1);
        angle2 = MakePositiveAngle(angle2);
        if(angle1 > angle2)
        {
            angle2 += 360;
        }
        return (Mathf.Abs(angle2  - angle1));
    }

    /// <summary>
    /// Makes an Angle positive by adding 360 to it until it is positive.
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    public static float MakePositiveAngle(float angle)
    {
        while(angle < 0)
        {
            angle += 360;
        }
        return angle % 360;
    }

	public static Vector3 RadialPosToXY(RadialPosition radialPosition){
        float angle = radialPosition.GetAngle ();
        angle = MakePositiveAngle(angle);
		float radius = radialPosition.GetRadius ();
		float x = Mathf.Cos (Mathf.Deg2Rad * angle) * radius;
        float y = Mathf.Sin (Mathf.Deg2Rad * angle) * radius;
        if (Mathf.Abs(x) < 0.001)
        {
            x = 0;
        }
        if (Mathf.Abs(y) < 0.001)
        {
            y = 0;
        }
		return new Vector3 (x, y);
	}

	public static RadialPosition XYToRadialPos (Vector3 position){
        float angle = Mathf.Rad2Deg * Mathf.Atan2(position.y,position.x);
        float radius = new Vector2(position.x, position.y).magnitude;
        angle = MakePositiveAngle (angle);
		return new RadialPosition (radius, angle);
	}
}
