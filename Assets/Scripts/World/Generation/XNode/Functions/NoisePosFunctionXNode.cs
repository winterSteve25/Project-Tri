using System.Linq;
using UnityEngine;
using Utils;
using World.Generation.Nodes;
using World.Generation.Nodes.PosGenerators;
using World.Generation.Nodes.Utils;

namespace World.Generation.XNode.Functions
{
    [CreateNodeMenu("Biome/Functions/Noise Pos")]
    public class NoisePosFunctionXNode : FunctionXNode
    {
        private const string NoiseFunctionInput = "Noise Function";

        [SerializeField] private ComparisonOperator comparison;
        [SerializeField] private float noiseThreshold;

        protected override void Init()
        {
            base.Init();
            if (DynamicInputs.All(port => port.fieldName != NoiseFunctionInput))
            {
                AddDynamicInput(typeof(FunctionNode), fieldName: NoiseFunctionInput);
            }
        }

        protected override FunctionNode GetValue()
        {
            return new NoisePosFunction(
                comparison,
                noiseThreshold,
                (NoiseGenFunction)GetInputValue<FunctionNode>(NoiseFunctionInput)
            );
        }
    }
}