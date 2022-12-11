using System.Collections.Generic;
using System.Linq;
using Items;
using Systems.Inv;
using TileBehaviours;
using UI.Menu.InventoryMenu;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using Utils;
using World.Tiles;

namespace Player.Interaction
{
    public class PlayerInteractionHandler : MonoBehaviour
    {
        private InventoryController _inventoryController;
        private EquipmentsController _equipmentsController;
        private TilemapManager _tilemapManager;
        private Tilemap _obstacleLayer;
        private Transform _transform;

        private List<IUpdateBehaviourItem> _updateableEquipments;
        private Inventory _equipmentsInventory;

        private bool _wasLeftClickDown;
        private bool _wasRightClickDown;

        [SerializeField] private Camera mainCamera;

        private void Start()
        {
            _updateableEquipments = new List<IUpdateBehaviourItem>();
            _inventoryController = InventoryController.Current;
            _equipmentsController = EquipmentsController.Current;
            _tilemapManager = TilemapManager.Current;
            _obstacleLayer = _tilemapManager.ObstacleLayer;
            _transform = transform;

            _equipmentsInventory = _equipmentsController.Inventory;
            _equipmentsInventory.OnChanged += UpdateEquipments;
        }

        private void Update()
        {
            var playerPosition = _transform.position;
            var item = _equipmentsController[EquipmentType.Outer];

            #region Update Item Behaviours

            foreach (var updateable in _updateableEquipments)
            {
                updateable.UpdateBehaviour(ref item, playerPosition);
            }

            #endregion

            // if currently over ui non of the interaction will happen so we return
            var overUI = EventSystem.current.IsPointerOverGameObject();

            var isLeftClickHeld = GameInput.LeftClickButton();
            var isRightClickHeld = GameInput.RightClickButton();
            var isLeftClickDown = GameInput.LeftClickButtonDown();
            var isRightClickDown = GameInput.RightClickButtonDown();

            if (!overUI)
            {
                var point = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                point.z = 0;
                var pos = _obstacleLayer.WorldToCell(point);
                var tileAtPos = _tilemapManager.GetTile(pos, TilemapLayer.Obstacles);
                var isEmpty = tileAtPos is null;

                if (item.item is IHoldBehaviourItem pressBehaviourItem)
                {
                    if (isLeftClickHeld)
                    {
                        pressBehaviourItem.Hold(MouseButton.Left, ref item, tileAtPos, point, pos, _tilemapManager,
                            _inventoryController, _equipmentsController, playerPosition);
                    }

                    if (isRightClickHeld)
                    {
                        pressBehaviourItem.Hold(MouseButton.Right, ref item, tileAtPos, point, pos, _tilemapManager,
                            _inventoryController, _equipmentsController, playerPosition);
                    }
                }

                if (item.item is IClickedBehaviourItem clickedBehaviourItem)
                {
                    if (isLeftClickDown)
                    {
                        clickedBehaviourItem.Click(MouseButton.Left, ref item, tileAtPos, point, pos, _tilemapManager,
                            _inventoryController, _equipmentsController, playerPosition);
                    }

                    if (isRightClickDown)
                    {
                        clickedBehaviourItem.Click(MouseButton.Right, ref item, tileAtPos, point, pos, _tilemapManager,
                            _inventoryController, _equipmentsController, playerPosition);
                    }
                }

                if (isRightClickDown && !isEmpty)
                {
                    var go = _tilemapManager.GetGameObject(pos, TilemapLayer.Obstacles);
                    if (go == null) return;
                    if (go.TryGetComponent<CustomTileBehaviour>(out var tileMachine))
                    {
                        tileMachine.OnInteract();
                    }
                }
            }
            
            if (item.item is IReleasedBehaviourItem releasedBehaviourItem)
            {
                if (_wasLeftClickDown && !isLeftClickHeld)
                {
                    releasedBehaviourItem.Release(MouseButton.Left, ref item, _tilemapManager,
                        _inventoryController, _equipmentsController, playerPosition);
                }

                if (_wasRightClickDown && !isRightClickHeld)
                {
                    releasedBehaviourItem.Release(MouseButton.Right, ref item, _tilemapManager,
                        _inventoryController, _equipmentsController, playerPosition);
                }
            }

            _wasLeftClickDown = isLeftClickHeld;
            _wasRightClickDown = isRightClickHeld;
        }

        private void UpdateEquipments()
        {
            _updateableEquipments.Clear();
            _updateableEquipments = _equipmentsInventory.ItemStacks
                .Where(i => !i.IsEmpty && i.item is IUpdateBehaviourItem)
                .Select(i => (IUpdateBehaviourItem)i.item)
                .ToList();
        }
    }
}