using JetBrains.Annotations;
using Systems.Inv;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils;
using World;

namespace Items.ItemTypes
{
    public interface IHoldBehaviourItem : IInteractableItem
    {
        void Hold(MouseButton mouseButton, [CanBeNull] TileBase tileClicked, Vector3Int pos,
            TilemapManager tilemapManager, InventoryManager inventoryManager, Vector3 playerPosition,
            Vector3 playerDistanceToClickedPoint);
    }
}