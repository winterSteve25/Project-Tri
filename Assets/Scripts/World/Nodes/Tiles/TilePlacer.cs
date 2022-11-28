using Tiles;
using UnityEngine;
using World.Tiles;

namespace World.Nodes.Tiles
{
    [CreateNodeMenu("Terrain Gen/Terrain/Tile Placer")]
    public class TilePlacer : TerrainGenerator
    {
        [SerializeField] private TriTile material;
        [SerializeField] private TilemapLayer layer;
        [SerializeField] private string message;
        
        [SerializeField, Input(ShowBackingValue.Never)] private Vector3Int[] positions;

        public override void Generate(int seed, int width, int height, int xOffset, int yOffset, TilemapManager tilemapManager)
        {
            var p = GetInputValue<Vector3Int[]>(nameof(positions));

            if (p == null)
            {
                Debug.LogError($"No valid position input found on {name} Node in the {graph.name} Graph");
                return;
            }

            var tiles = new TileInstance[p.Length];
            for (var i = 0; i < tiles.Length; i++)
            {
                tiles[i] = new TileInstance(material);
            }

            tilemapManager.PlaceTile(p, tiles, layer, false, false);
        }

        public override string StageMessage()
        {
            return message;
        }
    }
}