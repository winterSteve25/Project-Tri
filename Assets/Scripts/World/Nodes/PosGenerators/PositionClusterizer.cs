using System.Collections.Generic;
using UnityEngine;
using Utils;
using World.Generation;
using XNode;

namespace World.Nodes.PosGenerators
{
    [CreateNodeMenu("Terrain Gen/Pos Generators/Clusterizer")]
    public class PositionClusterizer : Node
    {
        [SerializeField, Input] private int seed;
        [SerializeField, Input] private WorldGenConfiguration worldGenConfiguration;
        [SerializeField, Input(ShowBackingValue.Never)] private Vector3Int[] positions;
        [SerializeField, Output] private Vector3Int[] clusterizedPositions;

        [Header("Generator Settings")] 
        [SerializeField] private Vector2Int minSize;
        [SerializeField] private Vector2Int maxSize;
        [SerializeField] private AnimationCurve distanceEffect;
        [SerializeField] private int noiseScale = 15;
        [SerializeField] private int noiseOctaves = 1;
        [SerializeField] private float noiseThreshold = 0.7f;
        [SerializeField] private ComparisonOperator comparison = ComparisonOperator.GreaterThan;
        
        public override object GetValue(NodePort port)
        {
            if (port.fieldName != nameof(clusterizedPositions)) return base.GetValue(port);
            var poses = new List<Vector3Int>();
            var s = GetInputValue(nameof(seed), seed);
            var p = GetInputValue<Vector3Int[]>(nameof(positions));
            var config = GetInputValue(nameof(worldGenConfiguration), worldGenConfiguration);
            var rand = new System.Random(s);
            
            foreach (var center in p)
            {
                var size = new Vector2Int(rand.Next(minSize.x, maxSize.x + 1), rand.Next(minSize.y, maxSize.y + 1));
                
                var centreOfNoise = new Vector3Int((int)(size.x * 0.5f), (int)(size.y * 0.5f));
                var noise = NoiseHelper.GenerateNoiseMap(size.x, size.y, noiseScale, offset: new Vector2(center.x, center.y), octave: noiseOctaves, seed: s);
                
                for (var i = 0; i < size.x; i++)
                {
                    for (var j = 0; j < size.y; j++)
                    {
                        var pos = new Vector3Int(i, j);
                        var noiseValue = distanceEffect.Evaluate(MathHelper.MapTo0_1(centreOfNoise.x, (pos - centreOfNoise).magnitude)) * noise[i, j];
                        if (!comparison.Compare(noiseThreshold, noiseValue)) continue;
                        var finalPos = pos + center - centreOfNoise;
                        if (finalPos.x >= config.xOffset && finalPos.x < config.width + config.xOffset && finalPos.y >= config.yOffset && finalPos.y < config.height + config.yOffset)
                        {
                            poses.Add(finalPos);
                        }
                    }
                }
            }

            return poses.ToArray();
        }
    }
}