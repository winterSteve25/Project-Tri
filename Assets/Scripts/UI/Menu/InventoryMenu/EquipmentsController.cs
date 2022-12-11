using Items;
using Player;
using Systems.Inv;
using UnityEngine;
using Utils;

namespace UI.Menu.InventoryMenu
{
    public class EquipmentsController : CurrentInstanced<EquipmentsController>
    {
        [SerializeField] private ItemSlot[] slots;

        public ItemStack this[EquipmentType index]
        {
            get => Inventory[(int) index];
            set => Inventory[(int) index] = value;
        }
        public Inventory Inventory { get; private set; }

        private void Start()
        {
            Inventory = FindObjectOfType<PlayerInventory>().Equipments;
            Inventory.OnChanged += Refresh;

            for (var i = 0; i < slots.Length; i++)
            {
                slots[i].Init(Inventory, i, true);
            }
        }

        private void Refresh()
        {
            Inventory.RefreshItemSlotsWithContent(slots);
        }
    }
}