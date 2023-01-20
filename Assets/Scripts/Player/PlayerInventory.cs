using System.Threading.Tasks;
using Items;
using SaveLoad;
using SaveLoad.Interfaces;
using Systems.Inv;
using UI.Managers;
using UnityEngine;
using UnityEngine.Localization;

namespace Player
{
    /// <summary>
    /// The inventory attached to the player
    /// </summary>
    public class PlayerInventory : MonoBehaviour, ICustomWorldData
    {
        public Inventory Inv { get; private set; }
        public Inventory Equipments { get; private set; }

        [SerializeField] private ItemStack[] startingEquipments;
        [SerializeField] private ItemStack[] startingInventory;
        [SerializeField] private LocalizedString inventoryName;
        
        private void Awake()
        {
            Inv = new Inventory(inventoryName);
            Equipments = new Inventory(null, 3);
        }

        private void Start()
        {
            var pos = transform.position;

            foreach (var equipment in startingEquipments)
            {
                Equipments.Add(pos, equipment);
            }

            foreach (var item in startingInventory)
            {
                Inv.Add(pos, item);
            }
        }

        public bool TryAddItem(Vector2 position, ItemStack itemStack)
        {
            return Inv.Add(position, itemStack);
        }

        public SerializationPriority Priority => SerializationPriority.Medium;

        public Task Save()
        {
            // tries to add dragging item to inventory
            var draggingItem = ItemDragManager.Current.DraggedItem.Item;
            if (draggingItem.IsEmpty) return Task.CompletedTask;
            TryAddItem(transform.position, draggingItem);
            ItemDragManager.Current.DragItem(ItemStack.Empty);
            
            // no data are being saved here. inventory data being saved in PlayerDataSaver 
            return Task.CompletedTask;
        }

        public Task Read(FileLocation worldFolder)
        {
            return Task.CompletedTask;
        }
    }
}