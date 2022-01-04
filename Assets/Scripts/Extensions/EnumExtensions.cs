using System;
using System.Collections.Generic;

namespace Extensions
{
    internal static class EnumExtensions
    {
        public static IEnumerator<T> LoopedIncludedValues<T>(this T[] includeTypes) where T : Enum
        {
            while (true)
            {
                foreach (T enumValue in includeTypes)
                {
                    yield return enumValue;
                }
            }
        }
    }
}