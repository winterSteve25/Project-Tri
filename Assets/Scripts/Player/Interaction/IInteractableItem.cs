using Items;
using JetBrains.Annotations;
using UI.Menu.InventoryMenu;
using UnityEngine;
using World.Tiles;

namespace Player.Interaction
{
    public interface IInteractableItem
    {
        bool CanInteract(ref ItemStack itemStack, [CanBeNull] TileInstance tileAtLocation, Vector3 clickedPos,
            Vector3Int pos, TilemapManager tilemapManager, InventoryController inventoryController,
            EquipmentsController equipmentsController, Vector3 playerPosition);
    }
}