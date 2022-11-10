using Items;
using KevinCastejon.MoreAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InventorySystem
{
    public class UIItem : MonoBehaviour
    {
        public ItemStack Item
        {
            get => item;
            set
            {
                item = value;
                Refresh();
            }
        }
        
        [ReadOnly, SerializeField] protected ItemStack item;
        [SerializeField] protected Image itemIcon;
        [SerializeField] protected TextMeshProUGUI itemCount;

        protected virtual void Start()
        {
            Refresh();
        }

        protected virtual void Refresh()
        {
            if (item.IsEmpty)
            {
                itemIcon.gameObject.SetActive(false);
                itemCount.text = "";
                return;
            }
            
            itemIcon.gameObject.SetActive(true);
            itemIcon.sprite = item.item.sprite;
            itemCount.text = $"x{item.count}";
        }
    }
}