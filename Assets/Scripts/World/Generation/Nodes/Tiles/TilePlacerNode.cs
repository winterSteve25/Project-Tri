using JetBrains.Annotations;
using Tiles;
using UnityEngine;
using Utils.Data;
using World.Tiles;

namespace World.Generation.Nodes.Tiles
{
    public class TilePlacerNode : GenerationNode
    {
        private static readonly DataSignature<Vector3Int[]> PositionGenerator = new("Pos Generator");

        public override string StageMessage { get; }
        
        private TriTile _material;
        private TilemapLayer _layer;
        private Vector3Int[] _positions;

        private TilePlacerNode([CanBeNull] GenerationNode nextStep, Vector3Int[] positions, TriTile material, TilemapLayer layer, string message) : base(nextStep)
        {
            _material = material;
            _layer = layer;
            _positions = positions;
            StageMessage = message;
        }

        protected override void GenerateInternal(TilemapManager tilemapManager)
        {
            if (_positions == null)
            {
                _positions = GetProductFromUtilityNode(PositionGenerator);
            }
            
            var tiles = new TileInstance[_positions.Length];
            for (var i = 0; i < tiles.Length; i++)
            {
                tiles[i] = new TileInstance(_material);
            }

            tilemapManager.PlaceTile(_positions, tiles, _layer, false, false);
        }

        public static TilePlacerNode Create(Vector3Int[] positions, TriTile material, TilemapLayer layer, string message)
        {
            return new TilePlacerNode(null, positions, material, layer, message);
        }

        public static TilePlacerNode Create(FunctionNode positions, TriTile material, TilemapLayer layer, string message)
        {
            var placer = new TilePlacerNode(null, null, material, layer, message);
            placer.InitializeUtilityNode(PositionGenerator, positions);
            return placer;
        }
    }
}