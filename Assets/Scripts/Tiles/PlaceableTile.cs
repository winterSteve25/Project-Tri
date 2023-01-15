using System.Collections.Generic;
using Items;
using UnityEngine;
using World.Tiles;

namespace Tiles
{
    /// <summary>
    /// Scriptable object for placeable tiles which contains a predicate to determine whether the tile can be placed or not
    /// </summary>
    [CreateAssetMenu(fileName = "New Placeable Tile", menuName = "Machines/New Placeable Tile")]
    public class PlaceableTile : TriTile
    {
        public List<ChanceItemDrop> drops;
        public bool hasCollider;

        public virtual bool CanPlace(Vector3Int pos, TilemapManager tilemapManager)
        {
            return tilemapManager.GetTile(pos, TilemapLayer.Obstacles) is null;
        }
    }
}