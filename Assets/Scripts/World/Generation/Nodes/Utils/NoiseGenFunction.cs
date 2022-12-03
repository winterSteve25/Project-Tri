using System;
using UnityEngine;
using Utils;

namespace World.Generation.Nodes.Utils
{
    public class NoiseGenFunction : FunctionNode
    {
        public override Type ProductType => typeof(float[,]);

        private readonly int _scale;
        private readonly int _octaves;
        private readonly float _persistence;
        private readonly float _lacunarity;

        public NoiseGenFunction(int scale = 20, int octaves = 2, float persistence = 0.5f, float lacunarity = 1.87f)
        {
            _scale = scale;
            _octaves = octaves;
            _persistence = persistence;
            _lacunarity = lacunarity;
        }

        protected override object SupplyInternal()
        {
            return NoiseHelper.GenerateNoiseMap(Width, Height, _scale, new Vector2(XOffset, YOffset), Seed, _octaves, _persistence, _lacunarity);
        }
    }
}