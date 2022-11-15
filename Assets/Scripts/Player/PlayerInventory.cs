using System;
using Items;
using Systems.Inv;
using UnityEngine;
using UnityEngine.Localization;

namespace Player
{
    /// <summary>
    /// The inventory attached to the player
    /// </summary>
    public class PlayerInventory : MonoBehaviour
    {
        [NonSerialized] public Inventory Inv;
        [SerializeField] private LocalizedString inventoryName;
        private InventoryManager _inventoryManager;

        private void Awake()
        {
            Inv = new Inventory(inventoryName.GetLocalizedString());
        }

        private void Start()
        {
            _inventoryManager = InventoryManager.current;
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
    }
}