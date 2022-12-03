using System.Linq;
using UnityEngine;
using Utils;
using World.Generation.Nodes;
using World.Generation.Nodes.PosGenerators;

namespace World.Generation.XNode.Functions
{
    [CreateNodeMenu("Biome/Functions/Clusterize Pos")]
    public class ClusterizeFunctionXNode : FunctionXNode
    {
        private const string OriginalPosInput = "Original Positions";

        [SerializeField] private Vector2Int minSize;
        [SerializeField] private Vector2Int maxSize;
        [SerializeField] private AnimationCurve distanceEffect;
        [SerializeField] private int noiseScale = 15;
        [SerializeField] private int noiseOctave = 1;
        [SerializeField] private float noiseThreshold = 0.7f;
        [SerializeField] private ComparisonOperator comparison = ComparisonOperator.GreaterThan;

        protected override void Init()
        {
            base.Init();
            if (DynamicInputs.All(port => port.fieldName != OriginalPosInput))
            {
                AddDynamicInput(typeof(FunctionNode), fieldName: OriginalPosInput);
            }
        }

        protected override FunctionNode GetValue()
        {
            return new ClusterizeFunction(
                GetInputValue<FunctionNode>(OriginalPosInput),
                minSize,
                maxSize,
                f => distanceEffect.Evaluate(f),
                noiseScale,
                noiseOctave,
                noiseThreshold,
                comparison
            );
        }
    }
}