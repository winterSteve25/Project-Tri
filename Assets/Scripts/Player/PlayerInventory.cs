using System;
using System.Threading.Tasks;
using Items;
using SaveLoad;
using SaveLoad.Interfaces;
using Systems.Inv;
using UnityEngine;
using UnityEngine.Localization;

namespace Player
{
    /// <summary>
    /// The inventory attached to the player
    /// </summary>
    public class PlayerInventory : MonoBehaviour, ICustomWorldData
    {
        [NonSerialized] public Inventory Inv;
        [SerializeField] private LocalizedString inventoryName;
        private InventoryManager _inventoryManager;

        private void Awake()
        {
            Inv = new Inventory(inventoryName);
        }

        private void Start()
        {
            _inventoryManager = InventoryManager.Current;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                _inventoryManager.Show(Inv);
            }
        }

        public bool TryAddItem(Vector2 position, ItemStack itemStack)
        {
            return Inv.Add(position, itemStack);
        }

        public SerializationPriority Priority => SerializationPriority.Medium;

        public Task Save()
        {
            var draggingItem = InventoryManager.Current.draggedItem.Item;
            if (draggingItem.IsEmpty) return Task.CompletedTask;
            TryAddItem(transform.position, draggingItem);
            InventoryManager.Current.DragItem(ItemStack.Empty);
            return Task.CompletedTask;
        }

        public Task Read(FileLocation worldFolder)
        {
            return Task.CompletedTask;
        }
    }
}