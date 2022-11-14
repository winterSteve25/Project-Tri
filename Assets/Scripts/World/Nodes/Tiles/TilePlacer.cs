using UnityEngine;
using UnityEngine.Tilemaps;

namespace World.Nodes.Tiles
{
    [CreateNodeMenu("Terrain/Tile Placer")]
    public class TilePlacer : TerrainGenerator
    {
        [SerializeField] private TileBase material;
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

            var tiles = new TileBase[p.Length];
            for (var i = 0; i < tiles.Length; i++)
            {
                tiles[i] = material;
            }

            tilemapManager.PlaceTile(p, tiles, layer);
        }

        public override string StageMessage()
        {
            return message;
        }
    }
}