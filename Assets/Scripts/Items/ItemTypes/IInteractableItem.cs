using JetBrains.Annotations;
using Tiles;
using UI.Menu.EscapeMenu;
using UnityEngine;
using World.Tiles;

namespace Items.ItemTypes
{
    public interface IInteractableItem
    {
        bool CanInteract(ref ItemStack itemStack, [CanBeNull] TileInstance tileAtLocation, Vector3Int pos,
            TilemapManager tilemapManager, InventoryUIController inventoryUIController,
            EquipmentsController equipmentsController, Vector3 playerPosition,
            Vector3 playerDistanceToClickedPoint);
    }
}