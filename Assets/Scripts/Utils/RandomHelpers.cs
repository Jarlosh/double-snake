using System.Collections.Generic;
using UnityEngine;

namespace DoubleSnake.Core
{
    public static class RandomHelpers
    {
        public static bool Probability(float normalized)
        {
            return (1 - Random.value) > normalized;
        }

        public static bool CoinFlip() => Probability(0.5f);


        public static T Pick<T>(this IList<T> list)
        {
            return list[Random.Range(0, list.Count - 1)];
        }  
        
        public static void Shuffle<T>(this IList<T> list)  
        {  
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n);
                (list[k], list[n]) = (list[n], list[k]);
            }  
        }
    }
}