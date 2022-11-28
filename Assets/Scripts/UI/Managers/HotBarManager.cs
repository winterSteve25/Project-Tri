using Player;
using Sirenix.OdinInspector;
using Systems.Inv;
using UnityEngine;

namespace UI.Managers
{
    public class HotBarManager : MonoBehaviour
    {
        [SerializeField, Required] 
        private ItemSlot outerItem;
        
        [SerializeField, Required] 
        private ItemSlot[] firstRowSlots;

        private PlayerInventory _playerInventory;
        
        private void Start()
        {
            _playerInventory = FindObjectOfType<PlayerInventory>();
            
            var playerInventoryEquipments = _playerInventory.Equipments;
            var playerInventoryInv = _playerInventory.Inv;

            playerInventoryEquipments.OnChanged += RefreshEquipments;
            playerInventoryInv.OnChanged += RefreshInventory;
            
            outerItem.Init(playerInventoryEquipments, 0, true);
            
            for (var i = 0; i < firstRowSlots.Length; i++)
            {
                firstRowSlots[i].Init(playerInventoryInv, i);
            }
        }

        private void RefreshEquipments()
        {
            outerItem.Item = _playerInventory.Equipments[0];
        }

        private void RefreshInventory()
        {
            _playerInventory.Inv.RefreshItemSlotsWithContent(firstRowSlots);
        }
    }
}