using System;

public static class Extensions
{
}

public static class EnumHelper
{
    public static bool TryParse<TEnum>(String value, out TEnum result) where TEnum : struct
    {
        try
        {
            result = (TEnum)Enum.Parse(typeof(TEnum), value);
            return true;
        }
        catch
        {
            result = default(TEnum);
            return false;
        }
    }
}
