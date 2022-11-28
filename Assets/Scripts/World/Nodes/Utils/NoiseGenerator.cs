using System.Linq;
using UnityEngine;
using Utils;
using World.Generation;
using XNode;

namespace World.Nodes.Utils
{
    [CreateNodeMenu("Terrain Gen/Utils/Noise Generator")]
    public class NoiseGenerator : Node
    {
        public const string FieldName = "noise";
    
        [SerializeField, Input] private int seed;
        [SerializeField, Input] private WorldGenConfiguration worldGenConfiguration;

        [SerializeField] private int scale = 20;
        [SerializeField] private int octaves = 2;
        [SerializeField] private float persistence = 0.5f;
        [SerializeField] private float lacunarity = 1.87f;

        protected override void Init()
        {
            if (DynamicOutputs.All(port => port.fieldName != FieldName))
            {
                AddDynamicOutput(typeof(float[,]), fieldName: FieldName);
            }
        }

        public override object GetValue(NodePort port)
        {
            var s = GetInputValue<int>(nameof(seed));
            var config = GetInputValue(nameof(worldGenConfiguration), worldGenConfiguration);
            var x = config.xOffset;
            var y = config.yOffset;
            var w = config.width;
            var h = config.height;
            
            if (port.fieldName == "noise")
            {
                return NoiseHelper.GenerateNoiseMap(w, h, scale, new Vector2(x, y), s, octaves, persistence, lacunarity);
            }
            
            return base.GetValue(port);
        }
    }
}