using System.Linq;
using World.Generation.Nodes;
using XNode;

namespace World.Generation.XNode.Functions
{
    [CreateNodeMenu("")]
    public abstract class FunctionXNode : Node
    {
        private const string OutputPortName = "Function Output";

        protected override void Init()
        {
            if (DynamicOutputs.All(port => port.fieldName != OutputPortName))
            {
                AddDynamicOutput(typeof(FunctionNode), fieldName: OutputPortName);
            }
        }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName != OutputPortName) return base.GetValue(port);
            return GetValue();
        }

        protected abstract FunctionNode GetValue();
    }
}