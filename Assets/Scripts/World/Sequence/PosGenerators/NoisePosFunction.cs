using System;
using UnityEngine;
using Utils;
using Utils.Data;
using World.Sequence.Utils;

namespace World.Sequence.PosGenerators
{
    public class NoisePosFunction : FunctionNode
    {
        public override Type ProductType => typeof(Vector3Int[]);

        private static readonly DataSignature<float[,]> Noise = new("Noise");

        private ComparisonOperator _comparison;
        private float _threshold;
        
        public NoisePosFunction(ComparisonOperator comparison, float threshold, NoiseGenFunction noiseGenFunction)
        {
            _comparison = comparison;
            _threshold = threshold;
            InitializeUtilityNode(Noise, noiseGenFunction);
        }

        protected override object SupplyInternal()
        {
            var noise = GetProductFromUtilityNode(Noise);

            if (Width == 0 || Height == 0)
            {
                Debug.LogWarning("Dimensions should not be 0!");
                return new Vector3Int[] {};
            }

            var pos = new Vector3Int[Width * Height];
            var n = 0;
            
            for (var i = 0; i < Width; i++)
            {
                for (var j = 0; j < Height; j++)
                {
                    if (_comparison.Compare(noise[i, j], _threshold))
                    {
                        pos[n] = new Vector3Int(i + XOffset, j + YOffset);
                    }
                    
                    n++;
                }
            }

            return pos;
        }
    }
}