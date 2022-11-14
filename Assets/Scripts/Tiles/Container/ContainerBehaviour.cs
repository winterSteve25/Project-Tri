using System;
using Player;
using Systems.Inv;
using UnityEngine;

namespace Tiles.Container
{
    /// <summary>
    /// The behaviour of a container tile
    /// </summary>
    public class ContainerBehaviour : MachineTile
    {
        [SerializeField] private float distanceBeforeAccessDenied;
        public Inventory Inventory { get; private set; }
        
        private PlayerInventory _playerInventory;
        private InventoryManager _inventoryManager;

        private void Awake()
        {
            Inventory = new Inventory(inventoryName: "Container");
            _playerInventory = FindObjectOfType<PlayerInventory>();
            _inventoryManager = InventoryManager.current;
        }

        private void Update()
        {
            if (Vector2.Distance(_playerInventory.transform.position, transform.position) > distanceBeforeAccessDenied)
            {
                ExitMenu();
            }
        }

        public override void OnInteract()
        {
            _inventoryManager.Show(Inventory);
        }

        public override void OnBroken()
        {
            ExitMenu();
        }

        private void ExitMenu()
        {
            // if currently displaying this inventory, close it when this is broken
            if (_inventoryManager.CurrentInventory == Inventory)
            {
                _inventoryManager.Show(null);
            }
        }
    }
}