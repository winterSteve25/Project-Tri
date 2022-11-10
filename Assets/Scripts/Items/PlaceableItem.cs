using PlaceableTiles;
using UnityEngine;

namespace Items
{
    /// <summary>
    /// A type of item that can be placed as a tile when using
    /// </summary>
    [CreateAssetMenu(fileName = "New Placeable Item", menuName = "Items/New Placeable Item")]
    public class PlaceableItem : Item
    {
        public PlaceableTile placeableTile;
    }
}