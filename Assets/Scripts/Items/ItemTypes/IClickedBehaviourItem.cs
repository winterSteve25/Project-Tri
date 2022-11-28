using JetBrains.Annotations;
using Tiles;
using UI.Menu.EscapeMenu;
using UnityEngine;
using Utils;
using World.Tiles;

namespace Items.ItemTypes
{
    public interface IClickedBehaviourItem : IInteractableItem
    {
        void Click(MouseButton mouseButton, ref ItemStack itemStack, [CanBeNull] TileInstance tileClicked, Vector3Int pos,
            TilemapManager tilemapManager, InventoryUIController inventoryUIController,
            EquipmentsController equipmentsController, Vector3 playerPosition,
            Vector3 playerDistanceToClickedPoint);
    }
}