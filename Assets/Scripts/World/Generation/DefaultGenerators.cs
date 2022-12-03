using Registries;
using UnityEngine;
using Utils;
using World.Generation.Nodes;
using World.Generation.Nodes.PosGenerators;
using World.Generation.Nodes.Tiles;
using World.Generation.Nodes.Utils;
using World.Tiles;

namespace World.Generation
{
    public static class DefaultGenerators
    {
        public static readonly GenerationNode Biome1;

        static DefaultGenerators()
        {
            var tilesRegistry = TilesRegistry.Instance;
            var linearCurve = AnimationCurve.Linear(0, 0, 0, 1);

            Biome1 = TilePlacerNode.Create(
                    new DimensionalPosFunction(),
                    tilesRegistry.GetObjectWithID("Type_1_Rock_Tile"),
                    TilemapLayer.Ground,
                    "Placing a floor...")
                .Next(TilePlacerNode.Create(
                    new NoisePosFunction(ComparisonOperator.GreaterThan, 0.6f, new NoiseGenFunction()),
                    tilesRegistry.GetObjectWithID("Type_2_Rock_Tile"), TilemapLayer.Obstacles, "Placing obstacles..."))
                .Next(TilePlacerNode.Create(
                    new ClusterizeFunction(
                        new PoissonPosFunction(64), new Vector2Int(32, 16), new Vector2Int(48, 64),
                        f => linearCurve.Evaluate(f),
                        noiseOctaves: 4,
                        noiseThreshold: 0.5f,
                        comparison: ComparisonOperator.LessThan),
                    tilesRegistry.GetObjectWithID("Kiterite_Ore"),
                    TilemapLayer.Ground, "Placing ores with normal names..."))
                .Next(TilePlacerNode.Create(
                    new ClusterizeFunction(
                        new PoissonPosFunction(32), new Vector2Int(4, 4), new Vector2Int(8, 8),
                        f => linearCurve.Evaluate(f),
                        noiseOctaves: 3,
                        noiseThreshold: 0.3f,
                        comparison: ComparisonOperator.LessThan),
                    tilesRegistry.GetObjectWithID("Sqoil_Ore"),
                    TilemapLayer.Ground, "Placing ores with funny names..."))
                .Build();
        }
    }
}