using System.Collections.Generic;
using UnityEngine;

namespace Omnix.Flow
{
    public static class Extensions
    {
        public static string GetPath(this Transform target)
        {
            var path = new Stack<string>();
            path.Push(target.name);
            while (target.parent != null)
            {
                target = target.parent;
                path.Push(target.name);
            }
            return string.Join("\\", path);
        }

        public static string GetPath<T>(this T target) where T : MonoBehaviour
        {
            return GetPath(target.transform);
        }
    }
}