using System.Linq;
using UnityEngine;
using Utils;
using XNode;

namespace World.Nodes.PosGenerators
{
    [CreateNodeMenu("Pos Generators/Poisson Distribution")]
    public class PoissonPosGenerator : Node
    {
        [SerializeField, Input] private int seed;
        [SerializeField, Input] private WorldGenConfiguration worldGenConfiguration;
        [SerializeField, Output] private Vector3Int[] positions;
        
        [Header("Generator Settings")]
        [SerializeField] private float radius = 1;
        [SerializeField] private int numSamplesBeforeDiscard = 15;

        public override object GetValue(NodePort port)
        {
            if (port.fieldName != nameof(positions)) return base.GetValue(port);

            var s = GetInputValue(nameof(seed), seed);
            var config = GetInputValue(nameof(worldGenConfiguration), worldGenConfiguration);
            var x = config.xOffset;
            var y = config.yOffset;
            var w = config.width;
            var h = config.height;
            
            var points = PoissonDiscSampling.GeneratePoints(radius, new Vector2(w, h), numSamplesBeforeDiscard, s);
            return points.Select(v2 => new Vector3Int((int)v2.x + x, (int)v2.y + y)).ToArray();
        }
    }
}