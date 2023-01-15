using UnityEngine;
using Random = System.Random;

namespace Utils
{
    public static class MathHelper
    {
        public static float MapTo0_1(float max, float value)
        {
            return Mathf.Max(-value + max, 0) / max;
        }

        public static float NextFloat(this Random random, float min, float max)
        {
            return (float) random.NextDouble() * (max - min) + min;
        }

        private static readonly Random Rand = new ();

        public static bool Chance(this Random random, float chance)
        {
            return ChanceGivenRandom(random, chance);
        }

        public static bool Chance(float chance)
        {
            return ChanceGivenRandom(Rand, chance);
        }
        
        public static bool ChanceGivenRandom(Random random, float chance)
        {
            if (chance is > 1 or < 0)
            {
                Debug.LogWarning("Tried to get chance higher than 1 or below 0");
                return false;
            }

            return random.NextDouble() <= chance;
        }
    }
}