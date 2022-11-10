using System;
using InventorySystem;
using Items;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// The inventory attached to the player
    /// </summary>
    public class PlayerInventory : MonoBehaviour
    {
        [NonSerialized] public Inventory Inv;

        private void Start()
        {
            Inv = new Inventory();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                InventoryManager.Instance.Show(Inv);
            }
        }

        public bool TryAddItem(Vector2 position, ItemStack itemStack)
        {
            return Inv.Add(position, itemStack);
        }
    }
}