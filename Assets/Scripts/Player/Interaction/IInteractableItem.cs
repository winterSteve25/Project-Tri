using Items;
using JetBrains.Annotations;
using UI.Menu.EscapeMenu;
using UnityEngine;
using World.Tiles;

namespace Player.Interaction
{
    public interface IInteractableItem
    {
        bool CanInteract(ref ItemStack itemStack, [CanBeNull] TileInstance tileAtLocation, Vector3 clickedPos,
            Vector3Int pos, TilemapManager tilemapManager, InventoryUIController inventoryUIController,
            EquipmentsController equipmentsController, Vector3 playerPosition,
            Vector3 playerDistanceToClickedPoint);
    }
}