﻿using System.Collections.Generic;
using System;
using System.Linq;
using System.Collections;

namespace HK.GL.Extensions
{
    /// <summary>
    /// <see cref="Array{T}"/>の拡張クラス
    /// </summary>
    public static partial class Extensions
    {
        public static void ForEach<T>(this IEnumerable<T> self, Action<T> action)
        {
            foreach(var t in self)
            {
                action(t);
            }
        }

        public static T Find<T>(this T[] self, Predicate<T> match)
        {
            return Array.Find(self, match);
        }

        public static T[] FindAll<T>(this T[] self, Predicate<T> match)
        {
            return Array.FindAll(self, match);
        }

        public static int FindIndex<T>(this T[] self, Predicate<T> match)
        {
            return Array.FindIndex(self, match);
        }

        public static void Sort<T>(this T[] self, IComparer<T> comparer)
        {
            Array.Sort(self, comparer);
        }
    }
}
