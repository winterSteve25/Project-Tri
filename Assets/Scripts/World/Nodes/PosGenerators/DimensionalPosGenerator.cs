using UnityEngine;
using XNode;

namespace World.Nodes.PosGenerators
{
    [CreateNodeMenu("Pos Generators/Dimensional")]
    public class DimensionalPosGenerator : Node
    {
        [SerializeField, Input] private WorldGenConfiguration worldGenConfiguration;
        [SerializeField, Output] private Vector3Int[] positions;

        public override object GetValue(NodePort port)
        {
            if (port.fieldName != nameof(positions)) return base.GetValue(port);
            
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
                    pos[n] = new Vector3Int(i + x, j + y);
                    n++;
                }
            }
                
            return pos;
        }
    }
}