using UnityEngine;

namespace Utils
{
    public static class MathHelper
    {
        public static float MapTo0_1(float max, float value)
        {
            return Mathf.Max(-value + max, 0) / max;
        }

        public static float NextFloat(this System.Random random, float min, float max)
        {
            return (float) random.NextDouble() * (max - min) + min;
        }
    }
}