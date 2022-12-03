using UnityEngine;
using World.Generation.Nodes;
using World.Generation.Nodes.Utils;

namespace World.Generation.XNode.Functions
{
    [CreateNodeMenu("Biome/Functions/Noise Generator")]
    public class NoiseGenFunctionXNode : FunctionXNode
    {
        [SerializeField] private int scale;
        [SerializeField] private int octaves;
        [SerializeField] private float persistence;
        [SerializeField] private float lacunarity;

        protected override FunctionNode GetValue()
        {
            return new NoiseGenFunction(
                scale,
                octaves,
                persistence,
                lacunarity
            );
        }
    }
}