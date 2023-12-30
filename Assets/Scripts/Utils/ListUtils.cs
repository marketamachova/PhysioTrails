using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public static class ListUtils
    {
        public static void Shuffle<T>(List<T> list)
        {
            var random = new System.Random();
            var n = list.Count;
            
            list.Sort((x, y) => random.Next(n * 2) - n);
        }
        
        public static List<T> GetRandomSubset<T>(List<T> list, int subsetSize)
        {
            var random = new System.Random();
            var n = list.Count;
            var subset = new List<T>();
            
            for (int i = 0; i < subsetSize; i++)
            {
                var randomIndex = random.Next(n);
                subset.Add(list[randomIndex]);
                list.RemoveAt(randomIndex);
                n--;
            }

            return subset;
        }
    }
}
