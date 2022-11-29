﻿using Items;
using JetBrains.Annotations;
using UI.Menu.EscapeMenu;
using UnityEngine;
using Utils;
using World.Tiles;

namespace Player.Interaction
{
    public interface IHoldBehaviourItem : IInteractableItem
    {
        void Hold(MouseButton mouseButton, ref ItemStack itemStack, [CanBeNull] TileInstance tileClicked, Vector3 clickedPos,
            Vector3Int pos, TilemapManager tilemapManager, InventoryUIController inventoryUIController,
            EquipmentsController equipmentsController, Vector3 playerPosition,
            Vector3 playerDistanceToClickedPoint);
    }
}