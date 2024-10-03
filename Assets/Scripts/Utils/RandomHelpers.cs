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
    }
}