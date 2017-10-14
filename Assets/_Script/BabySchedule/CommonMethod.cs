using System;

namespace BabySchedule
{
    public static class CommonMethod
    {
        public static string TickToTimeStr(long tick)
        {
            var time = new DateTime(1970, 1, 1, 8, 0, 0).AddMilliseconds(tick);
            return time.ToString(time.Date == DateTime.Now.Date ? "HH:mm:ss" : "yyyy/MM/dd\nHH:mm:ss");
        }
    }
}
