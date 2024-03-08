using System;
using Omnix.Utils;

namespace Omnix.Utils
{
    public static class DateTimeUtils
    {
        /// <summary> Converts UnixTimeStamps to DateTime </summary>
        /// <param name="timeStamp">UnixTimeStamps</param>
        /// <param name="isMillisecond">if timestamp is in milliseconds</param>
        public static DateTime FromTimeStamp(double timeStamp, bool isMillisecond)
        {
            TimeSpan time;
            if (isMillisecond) time = TimeSpan.FromMilliseconds(timeStamp);
            else time = TimeSpan.FromSeconds(timeStamp);
            return new DateTime(1970, 1, 1).AddTicks(time.Ticks);
        }

        /// <returns>True if date dt2 comes after date dt1</returns>
        public static bool HasDateArrived(DateTime dt1, DateTime dt2) => DateTime.Compare(dt1, dt2) < 0;

        /// <returns>Differance (in Milliseconds) between two dates (can be negative)</returns>
        public static int MillisecondsBetween(DateTime from, DateTime to) => (int)(to - from).TotalMilliseconds;

        /// <returns>Differance (in Seconds) between two dates (can be negative)</returns>
        public static int SecondsBetween(DateTime from, DateTime to) => (int)(to - from).TotalSeconds;
        
        /// <returns>Differance (in Minutes) between two dates (can be negative)</returns>
        public static int MinutesBetween(DateTime from, DateTime to) => (int)(to - from).TotalMinutes;
        
        /// <returns>Differance (in Hours) between two dates (can be negative)</returns>
        public static int HoursBetween(DateTime from, DateTime to) => (int)(to - from).TotalHours;
        
        /// <returns>Differance (in Days) between two dates (can be negative)</returns>
        public static int DaysBetween(DateTime from, DateTime to) => (int)(to - from).TotalDays;

        /// <returns> Differance (in Minutes) between given time and start of the day (0H 0M 0S 0ms is considered as start of day) (can be negative)</returns>
        public static int MinutesSinceDayStart(DateTime dt) => dt.Hour * 60 + dt.Minute;

        /// <returns> Differance (in Seconds) between given time and start of the day (0H 0M 0S 0ms is considered as start of day) (can be negative)</returns>
        public static int SecondsSinceDayStart(DateTime dt) => dt.Hour * 3600 + dt.Minute * 60 + dt.Second;
        
        /// <returns> Differance (in Milliseconds) between given time and start of the day (0H 0M 0S 0ms is considered as start of day) (can be negative)</returns>
        public static int MillisecondsSinceDayStart(DateTime dt) => dt.Hour * 3600000 + dt.Minute * 60000 + dt.Second + 1000 + dt.Millisecond;

        /// <returns> Differance (in Seconds) between given time and start of the day (0H 0M 0S 0ms is considered as start of day) (can be negative)</returns>
        public static float SecondsSinceDayStart(DateTime dt, bool includeMS)
        {
            if (includeMS) return dt.Hour * 3600f + dt.Minute * 60f + dt.Second + dt.Millisecond * 0.001f;
            return dt.Hour * 3600f + dt.Minute * 60f + dt.Second;
        }
        
        /// <param name="includeSecond">if true then fractional part contains differance in Seconds</param>
        /// <param name="includeMS">if true then fractional part contains differance in MilliSeconds</param>
        /// <returns> Differance (in Minutes) between given time and start of the day (0H 0M 0S 0ms is considered as start of day) (can be negative)</returns>
        public static float MinutesSinceDayStart(DateTime dt, bool includeSecond, bool includeMS)
        {
            float minutes = dt.Hour * 60 + dt.Minute;
            if (includeSecond) minutes += dt.Second / 60f;
            if (includeMS) minutes += dt.Millisecond * 0.001f / 60f;
            return minutes;
        }
    }
}



namespace Omnix.Extensions {
    public static class DateTimeExtensions
    {
        /// <returns> Differance (in Minutes) between this time and start of the day (0H 0M 0S 0ms is considered as start of day) (can be negative)</returns>
        public static int MinutesSinceDayStart(this DateTime dt) => DateTimeUtils.MinutesSinceDayStart(dt);  
        
        /// <returns> Differance (in Seconds) between this time and start of the day (0H 0M 0S 0ms is considered as start of day) (can be negative)</returns>
        public static int SecondsSinceDayStart(this DateTime dt) => DateTimeUtils.SecondsSinceDayStart(dt);  

        /// <returns> Differance (in Milliseconds) between this time and start of the day (0H 0M 0S 0ms is considered as start of day) (can be negative)</returns>
        public static int MillisecondsSinceDayStart(this DateTime dt) => DateTimeUtils.MillisecondsSinceDayStart(dt); 

        /// <returns> Differance (in Seconds) between given time and start of the day (0H 0M 0S 0ms is considered as start of day) (can be negative)</returns>
        public static float SecondsSinceDayStart(this DateTime dt, bool includeMS) => DateTimeUtils.SecondsSinceDayStart(dt, includeMS); 
        
        /// <param name="includeSecond">if true then fractional part contains differance in Seconds</param>
        /// <param name="includeMS">if true then fractional part contains differance in MilliSeconds</param>
        /// <returns> Differance (in Minutes) between this time and start of the day (0H 0M 0S 0ms is considered as start of day) (can be negative)</returns>
        public static float MinutesSinceDayStart(this DateTime dt, bool includeSecond, bool includeMS) => DateTimeUtils.MinutesSinceDayStart(dt, includeSecond, includeMS); 
        
        /// <returns> Milliseconds left to other date from this date (can be negative) </returns>
        public static int MillisecondsTo(this DateTime from, DateTime other) => DateTimeUtils.MillisecondsBetween(from, other);
        
        /// <returns> Seconds left to other date from this date (can be negative) </returns>
        public static int SecondsTo(this DateTime from, DateTime other) => DateTimeUtils.SecondsBetween(from, other);
        
        /// <returns> Minutes left to other date from this date (can be negative) </returns>
        public static int MinutesTo(this DateTime from, DateTime other) => DateTimeUtils.MinutesBetween(from, other);
        
        /// <returns> Hours left to other date from this date (can be negative) </returns>
        public static int HoursTo(this DateTime from, DateTime other) => DateTimeUtils.HoursBetween(from, other);
        
        /// <returns> Days left to other date from this date (can be negative) </returns>
        public static int DaysTo(this DateTime from, DateTime other) => DateTimeUtils.DaysBetween(from, other);
        
        
        /// <returns> Milliseconds left to this date from- other date (can be negative) </returns>
        public static int MillisecondsFrom(this DateTime to, DateTime from) => DateTimeUtils.MillisecondsBetween(from, to);
        
        /// <returns> Seconds left to this date from other date (can be negative) </returns>
        public static int SecondsFrom(this DateTime to, DateTime from) => DateTimeUtils.SecondsBetween(from, to);
        
        /// <returns> Minutes left to this date from other date (can be negative) </returns>
        public static int MinutesFrom(this DateTime to, DateTime from) => DateTimeUtils.MinutesBetween(from, to);
        
        /// <returns> Hours left to this date from other date (can be negative) </returns>
        public static int HoursFrom(this DateTime to, DateTime from) => DateTimeUtils.HoursBetween(from, to);
        
        /// <returns> Days left to this date from other date (can be negative) </returns>
        public static int DaysFrom(this DateTime to, DateTime from) => DateTimeUtils.DaysBetween(from, to);
    }
}