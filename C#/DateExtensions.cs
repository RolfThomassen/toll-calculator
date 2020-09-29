using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace TollFeeCalculator
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Get the next weekday from a date
        /// </summary>
        /// <param name="d">StartDate</param>
        /// <param name="WeekDay">Search WeekDay</param>
        /// <returns>Date of WeekDay</returns>
        [Extension]
        public static DateTime GetNextDay(this DateTime d, DayOfWeek WeekDay)
        {
            DateTime nextDay = d.AddDays(1);
            while (nextDay.DayOfWeek != WeekDay)
                nextDay = nextDay.AddDays(1);
            return nextDay;
        }

        /// <summary>
        /// Get the Prev Weekday from a date
        /// </summary>
        /// <param name="d">StartDate</param>
        /// <param name="WeekDay">Search Weekday</param>
        /// <returns>Date of Weekday</returns>
        [Extension]
        public DateTime GetPrevDay(this DateTime d, DayOfWeek WeekDay)
        {
            DateTime prevDay = d.AddDays(-1);
            while (prevDay.DayOfWeek != WeekDay)
                prevDay = prevDay.AddDays(-1);
            return prevDay;
        }

        /// <summary>
        /// Is Time between to periods
        /// </summary>
        /// <param name="datetime"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [Extension]
        public bool isTimeBetween(this DateTime datetime, TimeSpan start, TimeSpan end)
        {
            // convert datetime to a TimeSpan
            TimeSpan now = datetime.TimeOfDay;
            // see if start comes before end
            if (start < end)
                return start <= now && now <= end;
            // start is after end, so do the inverse comparison
            return !(end < now && now < start);
        }

        /// <summary>
        /// Calculate date of EasterSunday
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        public static void EasterSunday(int year, ref int month, ref int day)
        {
            int g = year % 19;
            int c = year / 100;
            int h = h = (c - (int)(c / 4) - (int)((8 * c + 13) / 25) + 19 * g + 15) % 30;
            int i = h - (int)(h / 28) * (1 - (int)(h / 28) * (int)(29 / (h + 1)) * (int)((21 - g) / 11));
            day = i - ((year + (int)(year / 4) + i + 2 - c + (int)(c / 4)) % 7) + 28;
            month = 3;
            if (day > 31) { month++; day -= 31; }
        }

        /// <summary>
        /// Get Eastersunday
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static DateTime EasterSunday(int year)
        {
            int month = 0;
            int day = 0;
            EasterSunday(year, out month, out day);
            return new DateTime(year, month, day);
        }

        /// <summary>
        /// Get EasterSunday
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        [Extension]
        public static DateTime EasterSunday(this DateTime d)
        {
            return EasterSunday(d.Year);
        }


        /// <summary>
        /// Get First Sunday of Advent
        /// </summary>
        /// <param name="year"></param>
        /// <returns>Date of First Sundag of Advent</returns>
        public static DateTime FirstSundayOfAdvent(int year)
        {
            return new DateTime(year, 12, 25).AddDays(-(4 * 7)).GetPrevDay(DayOfWeek.Sunday);
        }

        /// <summary>
        /// Get First Sunday of Advent
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        [Extension]
        public static DateTime FirstSundayOfAdvent(this DateTime d)
        {
            return FirstSundayOfAdvent(d.Year);
        }


        /// <summary>
        /// Get Whit Sunday
        /// </summary>
        /// <param name="year"></param>
        /// <returns>Date of WhitSunday</returns>
        public static DateTime WhitSunday(int year)
        {
            return EasterSunday(year).AddDays(49);
        }

        /// <summary>
        /// Get WhitSunday
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        [Extension]
        public static DateTime WhitSunday(this DateTime d)
        {
            return WhitSunday(d.Year);
        }

        /// <summary>
        /// Get AscensionDay
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static DateTime AscensionDay(int year)
        {
            return EasterSunday(year).AddDays(39);
        }

        /// <summary>
        /// Get AscensionDay
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        [Extension]
        public static DateTime AscensionDay(this DateTime d)
        {
            return AscensionDay(d.Year);
        }

    }
}
