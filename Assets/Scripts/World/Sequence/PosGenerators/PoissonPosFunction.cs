using System;
using System.Linq;
using UnityEngine;
using Utils;

namespace World.Sequence.PosGenerators
{
    public class PoissonPosFunction : FunctionNode
    {
        public override Type ProductType => typeof(Vector3Int[]);

        private readonly float _radius;
        private readonly int _numSamplesBeforeDiscard;

        public PoissonPosFunction(float radius = 16, int numSamplesBeforeDiscard = 15)
        {
            this._radius = radius;
            this._numSamplesBeforeDiscard = numSamplesBeforeDiscard;
        }

        protected override object SupplyInternal()
        {
            var points = PoissonDiscSampling.GeneratePoints(_radius, new Vector2(Width, Height), _numSamplesBeforeDiscard, Seed);
            return points.Select(v2 => new Vector3Int((int)v2.x + XOffset, (int)v2.y + YOffset)).ToArray();
        }
    }
}