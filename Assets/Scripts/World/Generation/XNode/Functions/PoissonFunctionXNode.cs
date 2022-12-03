using UnityEngine;
using World.Generation.Nodes;
using World.Generation.Nodes.PosGenerators;

namespace World.Generation.XNode.Functions
{
    [CreateNodeMenu("Biome/Functions/Poisson Pos")]
    public class PoissonFunctionXNode : FunctionXNode
    {
        [SerializeField] private float radius = 16f;
        [SerializeField] private int samplesBeforeDiscard = 15;
        
        protected override FunctionNode GetValue()
        {
            return new PoissonPosFunction(radius, samplesBeforeDiscard);
        }
    }
}