using System.Linq;
using Sirenix.OdinInspector;
using Tiles;
using UnityEngine;
using World.Generation.Nodes;
using World.Generation.Nodes.Tiles;
using World.Tiles;
using XNode;

namespace World.Generation.XNode
{
    [CreateNodeMenu("Biome/Terrain/Tile Placer")]
    public class TilePlacerXNode : Node
    {
        private const string PositionFunctionInput = "Position Function Input";

        [Input(ShowBackingValue.Never), SerializeField]
        private byte previousNode;
        [Output, SerializeField] private byte nextNode;

        [SerializeField, BoxGroup("Configurations"), LabelWidth(50)]
        private TriTile material;

        [SerializeField, BoxGroup("Configurations"), LabelWidth(50)]
        private TilemapLayer layer;

        [SerializeField, BoxGroup("Configurations"), LabelWidth(50)]
        private string message;

        protected override void Init()
        {
            if (DynamicInputs.All(port => port.fieldName != PositionFunctionInput))
            {
                AddDynamicInput(typeof(FunctionNode), fieldName: PositionFunctionInput);
            }
        }

        public TilePlacerNode GetNode()
        {
            return TilePlacerNode.Create(
                GetInputValue<FunctionNode>(PositionFunctionInput),
                material, 
                layer,
                message
            );
        }
    }
}