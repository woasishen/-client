using System;
using System.Linq;
using TcpConnect;

namespace BabySchedule
{
    public static class CommonMethod
    {
        public static string TickToTimeStr(long tick)
        {
            var time = TickToDateTime(tick);
            return time.ToString(time.Date == DateTime.Now.Date ? "HH:mm:ss" : "yyyy/MM/dd\nHH:mm:ss");
        }

        public static DateTime TickToDateTime(long tick)
        {
            return new DateTime(1970, 1, 1, 8, 0, 0).AddMilliseconds(tick);
        }

        public static DateTime? GetLastAddItemTime()
        {
            if (!StaticData.Eats.Any() && !StaticData.Diapers.Any())
            {
                return null;
            }
            long ticks = 0;
            if (StaticData.Eats.Any())
            {
                ticks = StaticData.Eats.Peek().Time;
            }
            if (StaticData.Diapers.Any())
            {
                ticks = Math.Max(StaticData.Diapers.Peek().Time, ticks);
            }
            return TickToDateTime(ticks);
        }
    }
}
