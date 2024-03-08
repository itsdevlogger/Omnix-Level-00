using System;
using Omnix.Utils;


namespace Omnix.Utils
{
    public static class NumberUtils
    {

        #region Integers
        /// <summary>
        /// Converts a number from Range 1 (oldMin to oldMax) to Range 2 (newMin to newMax)
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="oldMin">Lower bond (inclusive) of old range</param>
        /// <param name="oldMax">Upper bond (exclusive) of old range</param>
        /// <param name="newMin">Lower bond (inclusive) of new range</param>
        /// <param name="newMax">Upper bond (exclusive) of new range</param>
        /// <returns>Converted number</returns>
        public static int ConvertRange(int value, int oldMin, int oldMax, int newMin, int newMax)
        {
            if (oldMin == oldMax) return value;
            return ((value - oldMin) * (newMax - newMin) / (oldMax - oldMin)) + newMin;
        }

        /// <summary>
        /// Converts a number from Range 1 (oldMin to oldMid to oldMax) to Range 2 (newMin to newMid to newMax)
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="oldMin">Lower bond (inclusive) of old range</param>
        /// <param name="oldMid">Middle bond (virtual not actual, think of it as a divider) of old range</param>
        /// <param name="oldMax">Upper bond (exclusive) of old range</param>
        /// <param name="newMin">Lower bond (inclusive) of new range</param>
        /// <param name="newMid">Middle bond (virtual not actual, think of it as a divider) of new range</param>
        /// <param name="newMax">Upper bond (exclusive) of new range</param>
        /// <returns>Converted number</returns>
        public static int ConvertRange(int value,
            int oldMin, int oldMid, int oldMax,
            int newMin, int newMid, int newMax)
        {
            if (value < oldMid) return ConvertRange(value, oldMin, oldMid, newMin, newMid);
            return ConvertRange(value, oldMid, oldMax, newMid, newMax);
        }


        /// <summary>
        /// Converts a number from Range 1 (Range specified by two elements at ith & (i+1)th index in oldMilestones array such that (i+1 element) > number > (i element) ) to respective range in newMilestones
        /// </summary>
        /// <returns>Converted number</returns>
        public static int ConvertMilestones(int value, int[] oldMilestones, int[] newMilestones)
        {
            if (oldMilestones.Length != newMilestones.Length)
            {
                throw new ArgumentException(
                    $"number of oldMilestones must be same as number of newMilestones ({oldMilestones.Length} != {newMilestones.Length})"
                );
            }

            int previous = oldMilestones[0];
            int current;
            for (int i = 1; i < oldMilestones.Length; i++)
            {
                current = oldMilestones[i];
                if (previous <= value && value < current)
                {
                    return ConvertRange(value, previous, current, newMilestones[i - 1], newMilestones[i]);
                }

                previous = current;
            }

            return value;
        }

        /// <summary>
        /// Converts a number from Range 1 (minimum to maximum) to Range 2 (maximum to minimum)
        /// </summary>
        /// <returns>Converted number</returns>
        public static int ReverseRange(int value, int minimum, int maximum) => ConvertRange(value, minimum, maximum, maximum, minimum);

        /// <returns>Closest multiple of stepSize to value</returns>
        public static int Round(int value, int stepSize)
        {
            int modulo = value % stepSize;
            if (modulo == 0)
            {
                return value;
            }

            if (modulo > (stepSize / 2))
            {
                return value + stepSize - modulo;
            }

            return value - modulo;
        }
        #endregion

        #region Floats
        /// <summary>
        /// Converts a number from Range 1 (oldMin to oldMax) to Range 2 (newMin to newMax)
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="oldMin">Lower bond (inclusive) of old range</param>
        /// <param name="oldMax">Upper bond (exclusive) of old range</param>
        /// <param name="newMin">Lower bond (inclusive) of new range</param>
        /// <param name="newMax">Upper bond (exclusive) of new range</param>
        /// <returns>Converted number</returns>
        public static float ConvertRange(float value, float oldMin, float oldMax, float newMin, float newMax)
        {
            if (Math.Abs(oldMin - oldMax) < 0.001f) return value;
            return ((value - oldMin) * (newMax - newMin) / (oldMax - oldMin)) + newMin;
        }

        /// <summary>
        /// Converts a number from Range 1 (oldMin to oldMid to oldMax) to Range 2 (newMin to newMid to newMax)
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="oldMin">Lower bond (inclusive) of old range</param>
        /// <param name="oldMid">Middle bond (virtual not actual, think of it as a divider) of old range</param>
        /// <param name="oldMax">Upper bond (exclusive) of old range</param>
        /// <param name="newMin">Lower bond (inclusive) of new range</param>
        /// <param name="newMid">Middle bond (virtual not actual, think of it as a divider) of new range</param>
        /// <param name="newMax">Upper bond (exclusive) of new range</param>
        /// <returns>Converted number</returns>
        public static float ConvertRange(float value,
            float oldMin, float oldMid, float oldMax,
            float newMin, float newMid, float newMax)
        {
            if (value < oldMid) return ConvertRange(value, oldMin, oldMid, newMin, newMid);
            return ConvertRange(value, oldMid, oldMax, newMid, newMax);
        }


        /// <summary>
        /// Converts a number from Range 1 (Range specified by two elements at ith & (i+1)th index in oldMilestones array such that (i+1 element) > number > (i element) ) to respective range in newMilestones
        /// </summary>
        /// <returns>Converted number</returns>
        public static float ConvertMilestones(float value, float[] oldMilestones, float[] newMilestones)
        {
            if (oldMilestones.Length != newMilestones.Length)
            {
                throw new ArgumentException(
                    $"number of oldMilestones must be same as number of newMilestones ({oldMilestones.Length} != {newMilestones.Length})"
                );
            }

            float previous = oldMilestones[0];
            float current;
            for (int i = 1; i < oldMilestones.Length; i++)
            {
                current = oldMilestones[i];
                if (previous <= value && value < current)
                {
                    return ConvertRange(value, previous, current, newMilestones[i - 1], newMilestones[i]);
                }

                previous = current;
            }

            return value;
        }

        /// <summary>
        /// Converts a number from Range 1 (minimum to maximum) to Range 2 (maximum to minimum)
        /// </summary>
        /// <returns>Converted number</returns>
        public static float ReverseRange(float value, float minimum, float maximum) => ConvertRange(value, minimum, maximum, maximum, minimum);

        /// <returns>Closest multiple of stepSize to value</returns>
        public static float Round(float value, float stepSize)
        {
            float modulo = value % stepSize;
            if (modulo == 0)
            {
                return value;
            }

            if (modulo > (stepSize / 2))
            {
                return value + stepSize - modulo;
            }

            return value - modulo;
        }
        #endregion

        #region Longs
        /// <summary>
        /// Converts a number from Range 1 (oldMin to oldMax) to Range 2 (newMin to newMax)
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="oldMin">Lower bond (inclusive) of old range</param>
        /// <param name="oldMax">Upper bond (exclusive) of old range</param>
        /// <param name="newMin">Lower bond (inclusive) of new range</param>
        /// <param name="newMax">Upper bond (exclusive) of new range</param>
        /// <returns>Converted number</returns>
        public static long ConvertRange(long value, long oldMin, long oldMax, long newMin, long newMax)
        {
            if (Math.Abs(oldMin - oldMax) < 0.001f) return value;
            return ((value - oldMin) * (newMax - newMin) / (oldMax - oldMin)) + newMin;
        }

        /// <summary>
        /// Converts a number from Range 1 (oldMin to oldMid to oldMax) to Range 2 (newMin to newMid to newMax)
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="oldMin">Lower bond (inclusive) of old range</param>
        /// <param name="oldMid">Middle bond (virtual not actual, think of it as a divider) of old range</param>
        /// <param name="oldMax">Upper bond (exclusive) of old range</param>
        /// <param name="newMin">Lower bond (inclusive) of new range</param>
        /// <param name="newMid">Middle bond (virtual not actual, think of it as a divider) of new range</param>
        /// <param name="newMax">Upper bond (exclusive) of new range</param>
        /// <returns>Converted number</returns>
        public static long ConvertRange(long value,
            long oldMin, long oldMid, long oldMax,
            long newMin, long newMid, long newMax)
        {
            if (value < oldMid) return ConvertRange(value, oldMin, oldMid, newMin, newMid);
            return ConvertRange(value, oldMid, oldMax, newMid, newMax);
        }


        /// <summary>
        /// Converts a number from Range 1 (Range specified by two elements at ith & (i+1)th index in oldMilestones array such that (i+1 element) > number > (i element) ) to respective range in newMilestones
        /// </summary>
        /// <returns>Converted number</returns>
        public static long ConvertMilestones(long value, long[] oldMilestones, long[] newMilestones)
        {
            if (oldMilestones.Length != newMilestones.Length)
            {
                throw new ArgumentException(
                    $"number of oldMilestones must be same as number of newMilestones ({oldMilestones.Length} != {newMilestones.Length})"
                );
            }

            long previous = oldMilestones[0];
            long current;
            for (int i = 1; i < oldMilestones.Length; i++)
            {
                current = oldMilestones[i];
                if (previous <= value && value < current)
                {
                    return ConvertRange(value, previous, current, newMilestones[i - 1], newMilestones[i]);
                }

                previous = current;
            }

            return value;
        }

        /// <summary>
        /// Converts a number from Range 1 (minimum to maximum) to Range 2 (maximum to minimum)
        /// </summary>
        /// <returns>Converted number</returns>
        public static long ReverseRange(long value, long minimum, long maximum) => ConvertRange(value, minimum, maximum, maximum, minimum);

        /// <returns>Closest multiple of stepSize to value</returns>
        public static long Round(long value, long stepSize)
        {
            long modulo = value % stepSize;
            if (modulo == 0)
            {
                return value;
            }

            if (modulo > (stepSize / 2))
            {
                return value + stepSize - modulo;
            }

            return value - modulo;
        }
        #endregion

        #region Doubles
        /// <summary>
        /// Converts a number from Range 1 (oldMin to oldMax) to Range 2 (newMin to newMax)
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="oldMin">Lower bond (inclusive) of old range</param>
        /// <param name="oldMax">Upper bond (exclusive) of old range</param>
        /// <param name="newMin">Lower bond (inclusive) of new range</param>
        /// <param name="newMax">Upper bond (exclusive) of new range</param>
        /// <returns>Converted number</returns>
        public static double ConvertRange(double value, double oldMin, double oldMax, double newMin, double newMax)
        {
            if (Math.Abs(oldMin - oldMax) < 0.001f) return value;
            return ((value - oldMin) * (newMax - newMin) / (oldMax - oldMin)) + newMin;
        }

        /// <summary>
        /// Converts a number from Range 1 (oldMin to oldMid to oldMax) to Range 2 (newMin to newMid to newMax)
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="oldMin">Lower bond (inclusive) of old range</param>
        /// <param name="oldMid">Middle bond (virtual not actual, think of it as a divider) of old range</param>
        /// <param name="oldMax">Upper bond (exclusive) of old range</param>
        /// <param name="newMin">Lower bond (inclusive) of new range</param>
        /// <param name="newMid">Middle bond (virtual not actual, think of it as a divider) of new range</param>
        /// <param name="newMax">Upper bond (exclusive) of new range</param>
        /// <returns>Converted number</returns>
        public static double ConvertRange(double value,
            double oldMin, double oldMid, double oldMax,
            double newMin, double newMid, double newMax)
        {
            if (value < oldMid) return ConvertRange(value, oldMin, oldMid, newMin, newMid);
            return ConvertRange(value, oldMid, oldMax, newMid, newMax);
        }


        /// <summary>
        /// Converts a number from Range 1 (Range specified by two elements at ith & (i+1)th index in oldMilestones array such that (i+1 element) > number > (i element) ) to respective range in newMilestones
        /// </summary>
        /// <returns>Converted number</returns>
        public static double ConvertMilestones(double value, double[] oldMilestones, double[] newMilestones)
        {
            if (oldMilestones.Length != newMilestones.Length)
            {
                throw new ArgumentException(
                    $"number of oldMilestones must be same as number of newMilestones ({oldMilestones.Length} != {newMilestones.Length})"
                );
            }

            double previous = oldMilestones[0];
            double current;
            for (int i = 1; i < oldMilestones.Length; i++)
            {
                current = oldMilestones[i];
                if (previous <= value && value < current)
                {
                    return ConvertRange(value, previous, current, newMilestones[i - 1], newMilestones[i]);
                }

                previous = current;
            }

            return value;
        }

        /// <summary>
        /// Converts a number from Range 1 (minimum to maximum) to Range 2 (maximum to minimum)
        /// </summary>
        /// <returns>Converted number</returns>
        public static double ReverseRange(double value, double minimum, double maximum) => ConvertRange(value, minimum, maximum, maximum, minimum);

        /// <returns>Closest multiple of stepSize to value</returns>
        public static double Round(double value, double stepSize)
        {
            double modulo = value % stepSize;
            if (modulo == 0)
            {
                return value;
            }

            if (modulo > (stepSize / 2))
            {
                return value + stepSize - modulo;
            }

            return value - modulo;
        }
        #endregion

    }
}


namespace Omnix.Extensions
{
    public static class NumberExtensions
    {

        #region Integers
        /// <summary>
        /// Converts a number from Range 1 (oldMin to oldMax) to Range 2 (newMin to newMax)
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="oldMin">Lower bond (inclusive) of old range</param>
        /// <param name="oldMax">Upper bond (exclusive) of old range</param>
        /// <param name="newMin">Lower bond (inclusive) of new range</param>
        /// <param name="newMax">Upper bond (exclusive) of new range</param>
        /// <returns>Converted number</returns>
        public static int ConvertRange(this int value, int oldMin, int oldMax, int newMin, int newMax) => NumberUtils.ConvertRange(value, oldMin, oldMax, newMin, newMax);

        /// <summary>
        /// Converts a number from Range 1 (oldMin to oldMid to oldMax) to Range 2 (newMin to newMid to newMax)
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="oldMin">Lower bond (inclusive) of old range</param>
        /// <param name="oldMid">Middle bond (virtual not actual, think of it as a divider) of old range</param>
        /// <param name="oldMax">Upper bond (exclusive) of old range</param>
        /// <param name="newMin">Lower bond (inclusive) of new range</param>
        /// <param name="newMid">Middle bond (virtual not actual, think of it as a divider) of new range</param>
        /// <param name="newMax">Upper bond (exclusive) of new range</param>
        /// <returns>Converted number</returns>
        public static int ConvertRange(this int value, int oldMin, int oldMid, int oldMax, int newMin, int newMid, int newMax) => NumberUtils.ConvertRange(value, oldMin, oldMid, oldMax, newMin, newMid, newMax);

        /// <summary>
        /// Converts a number from Range 1 (Range specified by two elements at ith & (i+1)th index in oldMilestones array such that (i+1 element) > number > (i element) ) to respective range in newMilestones
        /// </summary>
        /// <returns>Converted number</returns>
        public static int ConvertMilestones(this int value, int[] oldMilestones, int[] newMilestones) => NumberUtils.ConvertMilestones(value, oldMilestones, newMilestones);

        /// <summary>
        /// Converts a number from Range 1 (minimum to maximum) to Range 2 (maximum to minimum)
        /// </summary>
        /// <returns>Converted number</returns>
        public static int ReverseRange(this int value, int minimum, int maximum) => NumberUtils.ConvertRange(value, minimum, maximum, maximum, minimum);

        /// <returns>Closest multiple of stepSize to value</returns>
        public static int Round(this int value, int stepSize) => NumberUtils.Round(value, stepSize);
        #endregion

        #region Floats
        /// <summary>
        /// Converts a number from Range 1 (oldMin to oldMax) to Range 2 (newMin to newMax)
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="oldMin">Lower bond (inclusive) of old range</param>
        /// <param name="oldMax">Upper bond (exclusive) of old range</param>
        /// <param name="newMin">Lower bond (inclusive) of new range</param>
        /// <param name="newMax">Upper bond (exclusive) of new range</param>
        /// <returns>Converted number</returns>
        public static float ConvertRange(this float value, float oldMin, float oldMax, float newMin, float newMax) => NumberUtils.ConvertRange(value, oldMin, oldMax, newMin, newMax);

        /// <summary>
        /// Converts a number from Range 1 (oldMin to oldMid to oldMax) to Range 2 (newMin to newMid to newMax)
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="oldMin">Lower bond (inclusive) of old range</param>
        /// <param name="oldMid">Middle bond (virtual not actual, think of it as a divider) of old range</param>
        /// <param name="oldMax">Upper bond (exclusive) of old range</param>
        /// <param name="newMin">Lower bond (inclusive) of new range</param>
        /// <param name="newMid">Middle bond (virtual not actual, think of it as a divider) of new range</param>
        /// <param name="newMax">Upper bond (exclusive) of new range</param>
        /// <returns>Converted number</returns>
        public static float ConvertRange(this float value, float oldMin, float oldMid, float oldMax, float newMin, float newMid, float newMax) => NumberUtils.ConvertRange(value, oldMin, oldMid, oldMax, newMin, newMid, newMax);

        /// <summary>
        /// Converts a number from Range 1 (Range specified by two elements at ith & (i+1)th index in oldMilestones array such that (i+1 element) > number > (i element) ) to respective range in newMilestones
        /// </summary>
        /// <returns>Converted number</returns>
        public static float ConvertMilestones(this float value, float[] oldMilestones, float[] newMilestones) => NumberUtils.ConvertMilestones(value, oldMilestones, newMilestones);

        /// <summary>
        /// Converts a number from Range 1 (minimum to maximum) to Range 2 (maximum to minimum)
        /// </summary>
        /// <returns>Converted number</returns>
        public static float ReverseRange(this float value, float minimum, float maximum) => NumberUtils.ConvertRange(value, minimum, maximum, maximum, minimum);

        /// <returns>Closest multiple of stepSize to value</returns>
        public static float Round(this float value, float stepSize) => NumberUtils.Round(value, stepSize);
        #endregion

        #region Longs
        /// <summary>
        /// Converts a number from Range 1 (oldMin to oldMax) to Range 2 (newMin to newMax)
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="oldMin">Lower bond (inclusive) of old range</param>
        /// <param name="oldMax">Upper bond (exclusive) of old range</param>
        /// <param name="newMin">Lower bond (inclusive) of new range</param>
        /// <param name="newMax">Upper bond (exclusive) of new range</param>
        /// <returns>Converted number</returns>
        public static long ConvertRange(this long value, long oldMin, long oldMax, long newMin, long newMax) => NumberUtils.ConvertRange(value, oldMin, oldMax, newMin, newMax);

        /// <summary>
        /// Converts a number from Range 1 (oldMin to oldMid to oldMax) to Range 2 (newMin to newMid to newMax)
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="oldMin">Lower bond (inclusive) of old range</param>
        /// <param name="oldMid">Middle bond (virtual not actual, think of it as a divider) of old range</param>
        /// <param name="oldMax">Upper bond (exclusive) of old range</param>
        /// <param name="newMin">Lower bond (inclusive) of new range</param>
        /// <param name="newMid">Middle bond (virtual not actual, think of it as a divider) of new range</param>
        /// <param name="newMax">Upper bond (exclusive) of new range</param>
        /// <returns>Converted number</returns>
        public static long ConvertRange(this long value, long oldMin, long oldMid, long oldMax, long newMin, long newMid, long newMax) => NumberUtils.ConvertRange(value, oldMin, oldMid, oldMax, newMin, newMid, newMax);

        /// <summary>
        /// Converts a number from Range 1 (Range specified by two elements at ith & (i+1)th index in oldMilestones array such that (i+1 element) > number > (i element) ) to respective range in newMilestones
        /// </summary>
        /// <returns>Converted number</returns>
        public static long ConvertMilestones(this long value, long[] oldMilestones, long[] newMilestones) => NumberUtils.ConvertMilestones(value, oldMilestones, newMilestones);

        /// <summary>
        /// Converts a number from Range 1 (minimum to maximum) to Range 2 (maximum to minimum)
        /// </summary>
        /// <returns>Converted number</returns>
        public static long ReverseRange(this long value, long minimum, long maximum) => NumberUtils.ConvertRange(value, minimum, maximum, maximum, minimum);

        /// <returns>Closest multiple of stepSize to value</returns>
        public static long Round(this long value, long stepSize) => NumberUtils.Round(value, stepSize);
        #endregion

        #region Doubles
        /// <summary>
        /// Converts a number from Range 1 (oldMin to oldMax) to Range 2 (newMin to newMax)
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="oldMin">Lower bond (inclusive) of old range</param>
        /// <param name="oldMax">Upper bond (exclusive) of old range</param>
        /// <param name="newMin">Lower bond (inclusive) of new range</param>
        /// <param name="newMax">Upper bond (exclusive) of new range</param>
        /// <returns>Converted number</returns>
        public static double ConvertRange(this double value, double oldMin, double oldMax, double newMin, double newMax) => NumberUtils.ConvertRange(value, oldMin, oldMax, newMin, newMax);

        /// <summary>
        /// Converts a number from Range 1 (oldMin to oldMid to oldMax) to Range 2 (newMin to newMid to newMax)
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="oldMin">Lower bond (inclusive) of old range</param>
        /// <param name="oldMid">Middle bond (virtual not actual, think of it as a divider) of old range</param>
        /// <param name="oldMax">Upper bond (exclusive) of old range</param>
        /// <param name="newMin">Lower bond (inclusive) of new range</param>
        /// <param name="newMid">Middle bond (virtual not actual, think of it as a divider) of new range</param>
        /// <param name="newMax">Upper bond (exclusive) of new range</param>
        /// <returns>Converted number</returns>
        public static double ConvertRange(this double value, double oldMin, double oldMid, double oldMax, double newMin, double newMid, double newMax) => NumberUtils.ConvertRange(value, oldMin, oldMid, oldMax, newMin, newMid, newMax);

        /// <summary>
        /// Converts a number from Range 1 (Range specified by two elements at ith & (i+1)th index in oldMilestones array such that (i+1 element) > number > (i element) ) to respective range in newMilestones
        /// </summary>
        /// <returns>Converted number</returns>
        public static double ConvertMilestones(this double value, double[] oldMilestones, double[] newMilestones) => NumberUtils.ConvertMilestones(value, oldMilestones, newMilestones);

        /// <summary>
        /// Converts a number from Range 1 (minimum to maximum) to Range 2 (maximum to minimum)
        /// </summary>
        /// <returns>Converted number</returns>
        public static double ReverseRange(this double value, double minimum, double maximum) => NumberUtils.ConvertRange(value, minimum, maximum, maximum, minimum);

        /// <returns>Closest multiple of stepSize to value</returns>
        public static double Round(this double value, double stepSize) => NumberUtils.Round(value, stepSize);
        #endregion

    }
}