using UnityEngine;
using UnityEngine.Tilemaps;

namespace PlaceableTiles
{
    /// <summary>
    /// Scriptable object for placeable tiles which contains a predicate to determine whether the tile can be placed or not
    /// </summary>
    [CreateAssetMenu(fileName = "New Placeable Tile", menuName = "Machines/New Placeable Tile")]
    public class PlaceableTile : ScriptableObject
    {
        public TileBase tileBase;

        public virtual bool CanPlace(Vector3Int pos, Tilemap groundLayer, Tilemap obstacleLayer)
        {
            return obstacleLayer.GetTile(pos) is null;
        }
    }
}