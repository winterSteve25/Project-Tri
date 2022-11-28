using System.Collections.Generic;
using System.Linq;
using Registries;
using Tiles;
using UnityEngine.Tilemaps;
using World.Tiles;

namespace Utils
{
    public static class TileUtilities
    {
        public static TriTile FromTileBase(TileBase tileBase)
        {
            return tileBase == null
                ? null
                : TilesRegistry.Instance.Entries.FirstOrDefault(triTile => triTile.Key.tileBase == tileBase).Key;
        }

        public static TileBase[] FromTileInstances(IEnumerable<TileInstance> tileInstances)
        {
            return tileInstances.Select(tileInstance => tileInstance.Tile.tileBase).ToArray();
        }

        public static TileInstance[] ToTileInstances(IEnumerable<TriTile> triTiles)
        {
            return triTiles.Select(triTile => new TileInstance(triTile)).ToArray();
        }
    }
}