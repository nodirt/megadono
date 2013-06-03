using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegadonoTest
{
    static class Util
    {
        public static void Shuffle<T>(this T[] items)
        {
            var rand = new Random();
            var keys = new double[items.Length];
            for (int i = 0; i < items.Length; i++)
                keys[i] = rand.NextDouble();
            Array.Sort(keys, items);
        }
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> items)
        {
            var array = items.ToArray();
            array.Shuffle();
            return array;
        }
    }
}
