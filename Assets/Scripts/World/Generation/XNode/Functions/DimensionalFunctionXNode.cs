using World.Generation.Nodes;
using World.Generation.Nodes.PosGenerators;

namespace World.Generation.XNode.Functions
{
    [CreateNodeMenu("Biome/Functions/Dimensional Pos")]
    public class DimensionalFunctionXNode : FunctionXNode
    {
        protected override FunctionNode GetValue()
        {
            return new DimensionalPosFunction();
        }
    }
}