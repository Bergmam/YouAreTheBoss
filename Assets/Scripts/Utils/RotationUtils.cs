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
		bool zeroInLimits = high < low;
		bool inLimitsAroundZero = zeroInLimits && (angle < high || angle > low);
		bool inLimitsWithoutZero = !zeroInLimits && (angle > low && angle < high);
		return inLimitsAroundZero || inLimitsWithoutZero;
	}	

	/// <summary>
	/// The method returns midpoint between two angles low and high. Angles from 0 to 360.
	/// </summary>
	/// <param name="low">The low angle.</param>
	/// <param name="high">The High angle.</param>
	public static float MiddleOfRotations(float low, float high)
	{
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

    // public static float CoordinateToRotPos(Vector2 position)
    // {
    //     float x = position.x;
    //     float y = position.y;
    //     float angle;

    //     if (x >= 0 && y < 0) {
    //         angle = Mathf.Atan2(x, -y);
    //     } else if (x >= 0 && y >= 0) {
    //         angle = Mathf.PI / 2 + Mathf.Atan2(y, x);
    //     } else if (x < 0 && y >= 0) {
    //         angle = Mathf.PI + Mathf.Atan2(-x, y);
    //     } else {
    //         angle = (3 * Mathf.PI) / 2 + Mathf.Atan2(-y, -x);
    //     }
    // } 
}
