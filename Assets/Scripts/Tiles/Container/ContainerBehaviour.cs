using System;
using InventorySystem;
using Player;
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

        private void Awake()
        {
            Inventory = new Inventory(inventoryName: "Container");
            _playerInventory = FindObjectOfType<PlayerInventory>();
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
            InventoryManager.Instance.Show(Inventory);
        }

        public override void OnBroken()
        {
            ExitMenu();
        }

        private void ExitMenu()
        {
            // if currently displaying this inventory, close it when this is broken
            var inventoryManager = InventoryManager.Instance;
            if (inventoryManager.CurrentInventory == Inventory)
            {
                inventoryManager.Show(null);
            }
        }
    }
}