public class RangeUtils
{
    public static float rangeLevelToFloatRange(RangeLevel inputRange)
    {
        switch (inputRange)
        {
            case RangeLevel.SELF_DESTRUCT:
                return Parameters.SELF_DESTRUCT_RANGE;
            case RangeLevel.MELE:
                return Parameters.MELEE_RANGE;
            case RangeLevel.MID:
                return Parameters.MID_RANGE;
            case RangeLevel.LONG:
                return Parameters.LONG_RANGE;
            default:
                return 0.0f;
        }
    }

    public static RangeLevel floatRangeToRangeLevel(float inputRange)
    {
        if (inputRange == Parameters.SELF_DESTRUCT_RANGE)
        {
            return RangeLevel.SELF_DESTRUCT;
        }
        else if (inputRange == Parameters.MID_RANGE)
        {
            return RangeLevel.MID;
        }
        else if (inputRange == Parameters.LONG_RANGE)
        {
            return RangeLevel.LONG;
        }
        else
        {
            return RangeLevel.MELE;
        }
    }
}