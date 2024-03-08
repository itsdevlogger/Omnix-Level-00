using System.Collections.Generic;
using Omnix.Extensions;


namespace Omnix.Utils
{
    public static class Python
    {
        /// <summary> Enumerate over first with index and element. </summary>
        /// <remarks>Use element.Index to get index & element.Item for element</remarks>
        public static IEnumerable<Element<T>> Enumerate<T>(this IEnumerable<T> first, int increment = 1)
        {
            int counter = 0;
            foreach (T ele in first)
            {
                yield return new Element<T>(counter, ele);
                counter += increment;
            }
        }

        /// <summary> Zip two Enumerators in one dictionary </summary>
        /// <returns>Dictionary</returns>
        public static Dictionary<TKey, TValue> ZipDict<TKey, TValue>(IEnumerable<TKey> keys, IEnumerable<TValue> values)
        {
            Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();
            IEnumerator<TKey> Keys = keys.GetEnumerator();
            IEnumerator<TValue> Values = values.GetEnumerator();
            while (Keys.MoveNext() && Values.MoveNext())
            {
                dict.Add(Keys.Current, Values.Current);
            }
            return dict;
        }

        /// <summary> Enumerate over two Enumerables simultaneously with index and element. </summary>
        /// <remarks>Use element.Index to get index & element.Item for element</remarks>
        public static IEnumerable<Bundle<T1, T2>> Zip<T1, T2>(IEnumerable<T1> first, IEnumerable<T2> second)
        {
            int counter = 0;
            IEnumerator<T1> First = first.GetEnumerator();
            IEnumerator<T2> Second = second.GetEnumerator();
            while (true)
            {
                if (First.MoveNext() && Second.MoveNext())
                {
                    yield return new Bundle<T1, T2>(counter, First.Current, Second.Current);
                }
                else
                {
                    break;
                }
                counter++;
            }
        }

        /// <summary> Enumerate over three Enumerables simultaneously with index and element. </summary>
        /// <remarks>Use element.Index to get index & element.Item for element</remarks>
        public static IEnumerable<Bundle<T1, T2, T3>> Zip<T1, T2, T3>(IEnumerable<T1> first, IEnumerable<T2> second, IEnumerable<T3> third)
        {
            int counter = 0;
            IEnumerator<T1> First = first.GetEnumerator();
            IEnumerator<T2> Second = second.GetEnumerator();
            IEnumerator<T3> Third = third.GetEnumerator();
            while (true)
            {
                if (First.MoveNext() && Second.MoveNext() && Third.MoveNext())
                {
                    yield return new Bundle<T1, T2, T3>(counter, First.Current, Second.Current, Third.Current);
                }
                else
                {
                    break;
                }
                counter++;
            }

            yield break;
        }

        /// <summary> Enumerate over four Enumerables simultaneously with index and element. </summary>
        /// <remarks>Use element.Index to get index & element.Item for element</remarks>
        public static IEnumerable<Bundle<T1, T2, T3, T4>> Zip<T1, T2, T3, T4>(IEnumerable<T1> first, IEnumerable<T2> second, IEnumerable<T3> third, IEnumerable<T4> forth)
        {
            int counter = 0;
            IEnumerator<T1> First = first.GetEnumerator();
            IEnumerator<T2> Second = second.GetEnumerator();
            IEnumerator<T3> Third = third.GetEnumerator();
            IEnumerator<T4> Forth = forth.GetEnumerator();
            while (true)
            {
                if (First.MoveNext() && Second.MoveNext() && Third.MoveNext() && Forth.MoveNext())
                {
                    yield return new Bundle<T1, T2, T3, T4>(counter, First.Current, Second.Current, Third.Current, Forth.Current);
                }
                else
                {
                    break;
                }
                counter++;
            }

            yield break;
        }

        /// <summary> Enumerate over five Enumerables simultaneously with index and element. </summary>
        /// <remarks>Use element.Index to get index & element.Item for element</remarks>
        public static IEnumerable<Bundle<T1, T2, T3, T4, T5>> Zip<T1, T2, T3, T4, T5>(IEnumerable<T1> first, IEnumerable<T2> second, IEnumerable<T3> third, IEnumerable<T4> forth, IEnumerable<T5> fifth)
        {
            int counter = 0;
            IEnumerator<T1> First = first.GetEnumerator();
            IEnumerator<T2> Second = second.GetEnumerator();
            IEnumerator<T3> Third = third.GetEnumerator();
            IEnumerator<T4> Forth = forth.GetEnumerator();
            IEnumerator<T5> Fifth = fifth.GetEnumerator();
            while (true)
            {
                if (First.MoveNext() && Second.MoveNext() && Third.MoveNext() && Forth.MoveNext())
                {
                    yield return new Bundle<T1, T2, T3, T4, T5>(counter, First.Current, Second.Current, Third.Current, Forth.Current, Fifth.Current);
                }
                else
                {
                    break;
                }
                counter++;
            }

            yield break;
        }

        public static IEnumerable<T> SubEnumerator<T>(this IEnumerable<T> combinations, int enumLength, int start, int end, int step)
        {
            int startIndex = PyToCsIndex(enumLength, start);
            int endIndex = PyToCsIndex(enumLength, end);
            int currentIndex = 0;
            int _stp = (step >= 1) ? step : 1;

            foreach (T element in combinations)
            {
                if (currentIndex >= endIndex) yield break;
                if ((currentIndex - startIndex) % _stp == 0)
                    yield return element;
                currentIndex++;
            }
        }



        /// <remarks> Enumerate over a part of this Dictionary </remarks>
        public static IEnumerable<KeyValuePair<TKey, TVal>> SubEnumerator<TKey, TVal>(this Dictionary<TKey, TVal> combinations, int start, int end, int step = 1) => SubEnumerator(combinations, combinations.Count, start, end, step);

        /// <remarks> Enumerate over a part of this List </remarks>
        public static IEnumerable<T> SubEnumerator<T>(this List<T> combinations, int start, int end, int step = 1) => SubEnumerator(combinations, combinations.Count, start, end, step);
        
        /// <remarks> Enumerate over a part of this Array </remarks>
        public static IEnumerable<T> SubEnumerator<T>(this T[] combinations, int start, int end, int step = 1) => SubEnumerator(combinations, combinations.Length, start, end, step);

        /// <summary> Converts python index to cs index, python supports -ve index. </summary>
        /// <param name="count">Length of array</param>
        /// <param name="index">python index</param>
        /// <param name="clamp">If false then user can get IndexOutOfRangeError</param>
        /// <returns>CS index</returns>
        public static int PyToCsIndex(int count, int index, bool clamp = true)
        {
            if (!clamp) return EEH.ToLoopingIndex(index, count);
            
            if (index < 0) return count + index;
            return index;
        }
    }

    public struct Element<T>
    {
        public int Index;
        public T Item;

        public Element(int index, T item)
        {
            this.Index = index;
            this.Item = item;
        }
    }


    public struct Bundle<T1, T2>
    {
        public int index;
        public T1 Item1;
        public T2 Item2;

        public Bundle(int index, T1 item1, T2 item2)
        {
            this.index = index;
            this.Item1 = item1;
            this.Item2 = item2;
        }
    }


    public struct Bundle<T1, T2, T3>
    {
        public int index;
        public T1 Item1;
        public T2 Item2;
        public T3 Item3;

        public Bundle(int index, T1 item1, T2 item2, T3 item3)
        {
            this.index = index;
            this.Item1 = item1;
            this.Item2 = item2;
            this.Item3 = item3;
        }
    }


    public struct Bundle<T1, T2, T3, T4>
    {
        public int index;
        public T1 Item1;
        public T2 Item2;
        public T3 Item3;
        public T4 Item4;

        public Bundle(int index, T1 item1, T2 item2, T3 item3, T4 item4)
        {
            this.index = index;
            this.Item1 = item1;
            this.Item2 = item2;
            this.Item3 = item3;
            this.Item4 = item4;
        }
    }


    public struct Bundle<T1, T2, T3, T4, T5>
    {
        public int index;
        public T1 Item1;
        public T2 Item2;
        public T3 Item3;
        public T4 Item4;
        public T5 Item5;

        public Bundle(int index, T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
        {
            this.index = index;
            this.Item1 = item1;
            this.Item2 = item2;
            this.Item3 = item3;
            this.Item4 = item4;
            this.Item5 = item5;
        }
    }
}