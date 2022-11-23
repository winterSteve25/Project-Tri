using JetBrains.Annotations;
using Systems.Inv;
using UnityEngine;
using UnityEngine.Tilemaps;
using World;

namespace Items.ItemTypes
{
    public interface IInteractableItem
    {
        bool CanInteract([CanBeNull] TileBase tileAtLocation, Vector3Int pos,
            TilemapManager tilemapManager, InventoryManager inventoryManager, Vector3 playerPosition,
            Vector3 playerDistanceToClickedPoint);
    }
}