using TimeZoneConverter;

namespace Harmony.Common;

public class Server
{
    public static DateTime GetDate()
    {
        TimeZoneInfo tzi = TZConvert.GetTimeZoneInfo("America/Santo_Domingo");
        return TimeZoneInfo.ConvertTime(DateTime.Now, tzi);
    }
}