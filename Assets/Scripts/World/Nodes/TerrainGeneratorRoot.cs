using UnityEngine;
using World.Generation;
using XNode;

namespace World.Nodes
{
    [CreateNodeMenu("Terrain Gen/Terrain Generator Root")]
    public class TerrainGeneratorRoot : Node
    {
        [SerializeField, Output] private byte nextStage;
        
        [Output] public int seed;
        [Output] public WorldGenConfiguration configuration;
        [Output] public int xOffset;
        [Output] public int yOffset;
        [Output] public int width;
        [Output] public int height;

        public override object GetValue(NodePort port)
        {
            return port.fieldName switch
            {
                nameof(seed) => seed,
                nameof(configuration) => new WorldGenConfiguration(xOffset, yOffset, width, height),
                nameof(xOffset) => xOffset,
                nameof(yOffset) => yOffset,
                nameof(width) => width,
                nameof(height) => height,
                nameof(nextStage) => 0,
                _ => base.GetValue(port)
            };
        }
    }
}