using System;

namespace World.Generation.Nodes
{
    public abstract class FunctionNode : BaseNode
    {
        public abstract Type ProductType { get; }

        public object Supply(int seed, int width, int height, int xOffset, int yOffset)
        {
            Seed = seed;
            Width = width;
            Height = height;
            XOffset = xOffset;
            YOffset = yOffset;
            return SupplyInternal();
        }

        protected abstract object SupplyInternal();
    }
}