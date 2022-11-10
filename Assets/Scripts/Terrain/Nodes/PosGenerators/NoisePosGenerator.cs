using System.Linq;
using Terrain.Nodes.Utils;
using UnityEngine;
using Utils;
using XNode;

namespace Terrain.Nodes.PosGenerators
{
    [CreateNodeMenu("Pos Generators/Noise")]
    public class NoisePosGenerator : Node
    {
        [SerializeField, Input] private WorldGenConfiguration worldGenConfiguration;
        [SerializeField, Output] private Vector3Int[] positions;
        
        [Header("Generator Settings")]
        [SerializeField] private float threshold = 0.5f;
        [SerializeField] private ComparisonOperator comparison = ComparisonOperator.GreaterThan;
        
        protected override void Init()
        {
            if (DynamicInputs.All(port => port.fieldName != NoiseGenerator.FieldName))
            {
                AddDynamicInput(typeof(float[,]), fieldName: NoiseGenerator.FieldName);
            }
        }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName != nameof(positions)) return base.GetValue(port);
            
            var noise = GetInputValue<float[,]>(NoiseGenerator.FieldName);

            if (noise == null)
            {
                Debug.LogError($"No valid noise input found on {name} Node in the {graph.name} Graph");
                return base.GetValue(port);
            }

            var config = GetInputValue(nameof(worldGenConfiguration), worldGenConfiguration);
            var w = config.width;
            var h = config.height;
            var x = config.xOffset;
            var y = config.yOffset;

            if (w == 0 || h == 0)
            {
                Debug.LogWarning("Dimensions should not be 0!");
                return new Vector3Int[] {};
            }

            var pos = new Vector3Int[w * h];
            var n = 0;
            
            for (var i = 0; i < w; i++)
            {
                for (var j = 0; j < h; j++)
                {
                    if (comparison.Compare(noise[i, j], threshold))
                    {
                        pos[n] = new Vector3Int(i + x, j + y);
                    }
                    
                    n++;
                }
            }

            return pos;

        }
    }
}