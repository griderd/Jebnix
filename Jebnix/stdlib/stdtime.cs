using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSP;

namespace Jebnix.stdlib
{
    /// <summary>
    /// Handles time-related operations
    /// </summary>
    public class stdtime
    {
        public const int MINUTE = 60;
        public const int HOUR = 3600;
        public const int DAY = 86400;
        public const int YEAR = 31536000;

        /// <summary>
        /// Represents the current time in the common epoch.
        /// </summary>
        public struct KSPTimeSpan
        {
            public int Years;
            public int Days;
            public int Hours;
            public int Minutes;
            public double Seconds;
        }

        /// <summary>
        /// Gets the number of seconds that have elapsed since the beginning of the common epoch.
        /// </summary>
        /// <returns></returns>
        public static double Now
        {
            get
            {
                return Planetarium.GetUniversalTime();
            }
        }

        /// <summary>
        /// Gets a structure containing the year, day, hour, minute, and second at this time.
        /// </summary>
        /// <returns></returns>
        public static KSPTimeSpan GetCurrentTime()
        {
            return new KSPTimeSpan
            {
                Days = GetCurrentDay(),
                Hours = GetCurrentHour(),
                Minutes = GetCurrentHour(),
                Seconds = GetCurrentHour(),
                Years = GetCurrentYear()
            };
        }

        public static int GetYearsElapsed()
        {
            int seconds = Convert.ToInt32(Math.Round(Now()));

            return seconds / YEAR;
        }

        public static int GetCurrentYear()
        {
            return GetCurrentYear() + 1;
        }

        public static int GetDaysElapsed()
        {
            int seconds = Convert.ToInt32(Math.Round(Now()));

            return seconds / DAY;
        }

        public static int GetCurrentDay()
        {
            return GetDaysElapsed() % 365;
        }

        public static int GetHoursElapsed()
        {
            int seconds = Convert.ToInt32(Math.Round(Now()));

            return seconds / HOUR;
        }

        public static int GetCurrentHour()
        {
            return GetHoursElapsed() % 24;
        }

        public static int GetMinutesElapsed()
        {
            int seconds = Convert.ToInt32(Math.Round(Now()));

            return seconds / MINUTE;
        }

        public static int GetCurrentMinute()
        {
            return GetMinutesElapsed() % 60;
        }

        public static double GetCurrentSecond()
        {
            int integralTime = GetYearsElapsed() * YEAR +
                                GetDaysElapsed() * DAY +
                                GetHoursElapsed() * HOUR +
                                GetMinutesElapsed() * MINUTE;

            return Now() - integralTime;
        }
    }
}
