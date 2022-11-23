using Items.ItemTypes;
using Systems.Inv;
using Tiles;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using Utils;
using World;

namespace Player
{
    /// <summary>
    /// Allows player to edit terrain (place, mine)
    /// </summary>
    public class PlayerTerrainInteraction : MonoBehaviour
    {
        private InventoryManager _inventoryManager;
        private TilemapManager _tilemapManager;
        private Tilemap _groundLayer;
        private Tilemap _obstacleLayer;
        private Transform _transform;
        
        [SerializeField] private Camera mainCamera;

        private void Start()
        {
            _inventoryManager = InventoryManager.Current;
            _tilemapManager = FindObjectOfType<TilemapManager>();
            _groundLayer = _tilemapManager.GroundLayer;
            _obstacleLayer = _tilemapManager.ObstacleLayer;
            _transform = transform;
        }

        private void Update()
        {
            var point = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            var pos = _obstacleLayer.WorldToCell(point);
            var tileAtPos = _obstacleLayer.GetTile(pos);
            var isEmpty = tileAtPos is null;
            var overUI = EventSystem.current.IsPointerOverGameObject();
            var playerPosition = _transform.position;
            var playerDistance = playerPosition - point;

            if (overUI) return;

            var draggingItem = _inventoryManager.draggedItem.Item;

            if (draggingItem.item is IHoldBehaviourItem pressBehaviourItem)
            {
                if (Input.GetMouseButton(0))
                {
                    pressBehaviourItem.Hold(MouseButton.Left, tileAtPos, pos, _tilemapManager, _inventoryManager, playerPosition, playerDistance);
                }

                if (Input.GetMouseButton(1))
                {
                    pressBehaviourItem.Hold(MouseButton.Right, tileAtPos, pos, _tilemapManager, _inventoryManager, playerPosition, playerDistance);
                }
            }

            if (draggingItem.item is IClickedBehaviourItem clickedBehaviourItem)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    clickedBehaviourItem.Click(MouseButton.Left, tileAtPos, pos, _tilemapManager, _inventoryManager, playerPosition, playerDistance);
                }

                if (Input.GetMouseButtonDown(1))
                {
                    clickedBehaviourItem.Click(MouseButton.Right, tileAtPos, pos, _tilemapManager, _inventoryManager, playerPosition, playerDistance);
                }
            }

            if (Input.GetMouseButtonDown(1) && !isEmpty)
            {
                var go = _tilemapManager.GetGameObject(tileAtPos, pos, _obstacleLayer);
                if (go == null) return;
                if (go.TryGetComponent<MachineTile>(out var tileMachine))
                {
                    tileMachine.OnInteract();
                }
            }
        }
    }
}