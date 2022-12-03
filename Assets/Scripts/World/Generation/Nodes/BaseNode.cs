using System.Collections.Generic;
using Utils.Data;

namespace World.Generation.Nodes
{
    public abstract class BaseNode
    {
        private Dictionary<string, FunctionNode> _functionNodes;

        protected int Seed { get; set; }
        protected int Width { get; set; }
        protected int Height { get; set; }
        protected int XOffset { get; set; }
        protected int YOffset { get; set; }

        protected BaseNode()
        {
            _functionNodes = new Dictionary<string, FunctionNode>();
        }

        protected void InitializeUtilityNode<T>(DataSignature<T> signature, FunctionNode node)
        {
            _functionNodes.Add(signature.Key, node);
        }

        protected T GetProductFromUtilityNode<T>(DataSignature<T> signature)
        {
            return (T)_functionNodes[signature.Key].Supply(Seed, Width, Height, XOffset, YOffset);
        }
    }
}