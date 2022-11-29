using JetBrains.Annotations;
using World.Tiles;

namespace World.Sequence
{
    public abstract class GenerationNode : BaseNode
    {
        public abstract string StageMessage { get; }
        public abstract bool IsDebugNode { get; }

        private GenerationNode _nextStep;
        
        protected GenerationNode([CanBeNull] GenerationNode nextStep)
        {
            _nextStep = nextStep;
        }

        public void Generate(int seed, int width, int height, int xOffset, int yOffset, TilemapManager tilemapManager)
        {
            Seed = seed;
            Width = width;
            Height = height;
            XOffset = xOffset;
            YOffset = yOffset;

            GenerateInternal(tilemapManager);
            _nextStep?.Generate(seed, width, height, xOffset, yOffset, tilemapManager);
        }

        protected abstract void GenerateInternal(TilemapManager tilemapManager);
    }
}