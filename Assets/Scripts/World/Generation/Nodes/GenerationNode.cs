using JetBrains.Annotations;
using World.Tiles;

namespace World.Generation.Nodes
{
    public abstract class GenerationNode : BaseNode
    {
        public abstract string StageMessage { get; }

        public GenerationNode NextStep { get; protected set; }
        public GenerationNode PreviousStep { get; protected set; }
        
        protected GenerationNode([CanBeNull] GenerationNode nextStep)
        {
            NextStep = nextStep;
        }

        public void Generate(int seed, int width, int height, int xOffset, int yOffset, TilemapManager tilemapManager)
        {
            Seed = seed;
            Width = width;
            Height = height;
            XOffset = xOffset;
            YOffset = yOffset;

            GenerateInternal(tilemapManager);
        }

        public int GetDepth(int starting = 1)
        {
            starting++;
            return NextStep?.GetDepth(starting) ?? starting;
        }

        public GenerationNode Next(GenerationNode nextStep)
        {
            NextStep = nextStep;
            NextStep.PreviousStep = this;
            return NextStep;
        }

        public GenerationNode Build()
        {
            return PreviousStep != null ? PreviousStep.Build() : this;
        }

        protected abstract void GenerateInternal(TilemapManager tilemapManager);
    }
}