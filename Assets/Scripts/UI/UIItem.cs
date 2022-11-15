using Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
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
        
        [SerializeField] protected ItemStack item;
        [SerializeField] protected Image itemIcon;
        [SerializeField] protected TextMeshProUGUI itemCount;
        [SerializeField] private bool includeName;
        
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

            if (item.count <= 0)
            {
                itemCount.text = includeName ? $"{item.item.itemName}" : string.Empty;
            }
            else
            {
                itemCount.text = includeName ? $"{item.item.itemName} x{item.count}" : $"x{item.count}";
            }
        }
    }
}