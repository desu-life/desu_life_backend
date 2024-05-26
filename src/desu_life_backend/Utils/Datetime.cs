namespace desu.life.Utils;

public static class Datetime
{
    public static DateTimeOffset TimeStampMilliToDateTime(int timeStamp)
    {
        return DateTimeOffset.FromUnixTimeMilliseconds(timeStamp);
    }

    public static DateTimeOffset TimeStampSecToDateTime(long timeStamp)
    {
        return DateTimeOffset.FromUnixTimeSeconds(timeStamp);
    }

    public static string Duration2String(long duration)
    {
        long day,
            hour,
            minute,
            second;
        day = duration / 86400;
        duration %= 86400;
        hour = duration / 3600;
        duration %= 3600;
        minute = duration / 60;
        second = duration % 60;
        return $"{day}d {hour}h {minute}m {second}s";
    }

    public static string Duration2StringWithoutSec(long duration)
    {
        long day,
            hour,
            minute,
            second;
        day = duration / 86400;
        duration %= 86400;
        hour = duration / 3600;
        duration %= 3600;
        minute = duration / 60;
        second = duration % 60;
        return $"{day}d {hour}h {minute}m";
    }

    public static string Duration2TimeString(long duration)
    {
        long hour,
            minute,
            second;
        hour = duration / 3600;
        duration %= 3600;
        minute = duration / 60;
        second = duration % 60;
        if (hour > 0)
            return $"{hour}:{minute:00}:{second:00}";
        return $"{minute}:{second:00}";
    }

    public static string Duration2TimeString_ForScoreV3(long duration)
    {
        long hour,
            minute,
            second;
        hour = duration / 3600;
        duration %= 3600;
        minute = duration / 60;
        second = duration % 60;
        if (hour > 0)
            return $"{hour}H,{minute:00}M,{second:00}S";
        return $"{minute}M,{second:00}S";
    }

    public static string GetTimeStamp(bool isMillisec)
    {
        TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        if (!isMillisec)
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        else
            return Convert.ToInt64(ts.TotalMilliseconds).ToString();
    }

}