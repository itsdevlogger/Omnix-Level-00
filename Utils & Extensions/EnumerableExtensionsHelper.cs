using System;
using UnityEngine;
using System.Collections.Generic;
using System.Text;
using Omnix.Utils;
using Random = UnityEngine.Random;

namespace Omnix.Extensions
{
    static class EEH
    {
        public static void RandomizeInPlace<T>(IEnumWrapper<T> array)
        {
            int count = array.Length;
            for (int i = 0; i < count; i++)
            {
                int randomIndex = Random.Range(0, count);
                T element = array.Get(randomIndex);
                array.Set(randomIndex, array.Get(i));
                array.Set(i, element);
            }
        }

        public static IEnumWrapper<T> Randomized<T>(IEnumWrapper<T> array)
        {
            IEnumWrapper<T> newArray = array.Copy();
            RandomizeInPlace(newArray);
            return newArray;
        }

        public static int ToLoopingIndex(int index, int count)
        {
            if (index < 0) return (count + index % count) % count;
            return index % count;
        }

        public static IEnumWrapper<T> PySubArray<T>(IEnumWrapper<T> array, int start, int end = -1, int step = 1)
        {
            int endNew = end;
            if (endNew < 0) endNew = array.Length + 1 + endNew;

            IEnumWrapper<T> newList = array.Empty((endNew - start) / step);

            Counter.Reset();
            for (int i = start; i < endNew; i += step)
            {
                newList.Set(Counter.Current, array.Get(i));
            }
            return newList;
        }

        public static Pair<T> ClosestPair<T>(IEnumWrapper<T> array, Func<T, T, float> distanceFunction)
        {
            int minOut = 0;
            int minIn = 0;
            float minDis = float.MaxValue;
            int cnt = array.Length;

            for (int co = 0; co < cnt; co++)
            {
                for (int ci = co; ci < cnt; ci++)
                {
                    float curDis = distanceFunction.Invoke(array.Get(co), array.Get(ci));
                    if (curDis < minDis)
                    {
                        minDis = curDis;
                        minOut = co;
                        minIn = ci;
                    }
                }
            }

            return new Pair<T>()
            {
                Distance = minDis,
                Index1 = minIn,
                Index2 = minOut,
                Element1 = array.Get(minIn),
                Element2 = array.Get(minOut),
            };
        }

        public static Pair<T> FurthestPair<T>(IEnumWrapper<T> array, Func<T, T, float> distanceFunction)
        {
            int maxOut = 0;
            int maxIn = 0;
            float maxDis = float.MinValue;
            int cnt = array.Length;

            for (int co = 0; co < cnt; co++)
            {
                for (int ci = co; ci < cnt; ci++)
                {
                    float curDis = distanceFunction.Invoke(array.Get(co), array.Get(ci));
                    if (curDis > maxDis)
                    {
                        maxDis = curDis;
                        maxOut = co;
                        maxIn = ci;
                    }
                }
            }

            return new Pair<T>()
            {
                Distance = maxDis,
                Index1 = maxIn,
                Index2 = maxOut,
                Element1 = array.Get(maxIn),
                Element2 = array.Get(maxOut),
            };
        }

        public static ElementInfo<T> ClosestTo<T>(IEnumerable<T> array, Func<T, float> distanceFunction)
        {
            int ind = 0;
            float minDis = float.MaxValue;
            int count = 0;
            T elem = default;
            foreach (T ele in array)
            {
                float curDis = distanceFunction.Invoke(ele);
                if (curDis >= minDis) continue;

                minDis = curDis;
                ind = count;
                elem = ele;
                count++;
            }

            return new ElementInfo<T>()
            {
                Index = ind,
                Distance = minDis,
                Item = elem
            };
        }

        public static ElementInfo<T> FurthestTo<T>(IEnumerable<T> array, Func<T, float> distanceFunction)
        {
            int ind = 0;
            int count = 0;
            float maxDist = float.MinValue;
            float curDis;
            T elem = default;


            foreach (T ele in array)
            {
                curDis = distanceFunction.Invoke(ele);
                if (curDis <= maxDist) continue;

                maxDist = curDis;
                ind = count;
                elem = ele;
                count++;
            }

            return new ElementInfo<T>()
            {
                Index = ind,
                Distance = maxDist,
                Item = elem
            };
        }

        public static int IndexOf<T>(IEnumerable<T> array, T element)
        {
            int index = 0;
            foreach (T t in array)
            {
                if (element.Equals(t))
                    return index;
            }
            return -1;
        }
    }

    interface IEnumWrapper<T>
    {
        public T Get(int index);
        public void Set(int index, T value);
        public int Length { get; }
        public IEnumWrapper<T> Copy();
        public IEnumWrapper<T> Empty(int capacity);
    }

    class ListWrapper<T> : IEnumWrapper<T>
    {
        public List<T> List;
        public ListWrapper(List<T> list) => this.List = list;
        public T Get(int index) => List[index];
        public void Set(int index, T value)
        {
            if (index >= List.Count) List.Add(value);
            else List[index] = value;
        }
        public int Length => List.Count;
        public IEnumWrapper<T> Empty(int capacity) => new ListWrapper<T>(new List<T>(capacity));

        public IEnumWrapper<T> Copy()
        {
            ListWrapper<T> wrapper = new ListWrapper<T>(new List<T>(List.Capacity));
            int index = 0;
            foreach (T item in List)
            {
                wrapper.List[index] = item;
                index++;
            }
            return wrapper;
        }
    }

    class ArrayWrapper<T> : IEnumWrapper<T>
    {
        public T[] Array;
        public ArrayWrapper(T[] array) => this.Array = array;
        public T Get(int index) => Array[index];
        public void Set(int index, T value) => Array[index] = value;
        public int Length => Array.Length;
        public IEnumWrapper<T> Empty(int capacity) => new ArrayWrapper<T>(new T[capacity]);
        public IEnumWrapper<T> Copy()
        {
            ArrayWrapper<T> wrapper = new ArrayWrapper<T>(new T[Array.Length]);
            int index = 0;
            foreach (T item in Array)
            {
                wrapper.Array[index] = item;
                index++;
            }
            return wrapper;
        }
    }

    public class Pair<T>
    {
        public float Distance;
        public int Index1;
        public int Index2;
        public T Element1;
        public T Element2;
    }

    public class ElementInfo<T>
    {
        public T Item;
        public int Index;
        public float Distance;
    }
}