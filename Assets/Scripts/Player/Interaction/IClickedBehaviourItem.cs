using Items;
using JetBrains.Annotations;
using UI.Menu.InventoryMenu;
using UnityEngine;
using Utils;
using World.Tiles;

namespace Player.Interaction
{
    public interface IClickedBehaviourItem : IInteractableItem
    {
        void Click(MouseButton mouseButton, ref ItemStack itemStack, [CanBeNull] TileInstance tileClicked, Vector3 clickedPos,
            Vector3Int pos, TilemapManager tilemapManager, InventoryTabController inventoryTabController,
            EquipmentsController equipmentsController, Vector3 playerPosition);
    }
}