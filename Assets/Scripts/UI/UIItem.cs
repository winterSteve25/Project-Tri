﻿using Items;
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
                PreItemChanged();
                item = value;
                PostItemChanged();
            }
        }
        
        [SerializeField] protected ItemStack item;
        [SerializeField] protected Image itemIcon;
        [SerializeField] protected TextMeshProUGUI itemCount;
        [SerializeField] private bool includeName;
        
        [SerializeField] private GameObject disabledCover;
        private bool _interactable = true;
        public bool Interactable
        {
            get => _interactable;
            set
            {
                _interactable = value;
                if (disabledCover == null) return;
                disabledCover.SetActive(!_interactable);
            }
        }
        
        protected virtual void Start()
        {
            PreItemChanged();
            PostItemChanged();
        }

        protected virtual void PostItemChanged()
        {
            if (item.IsEmpty)
            {
                itemIcon.gameObject.SetActive(false);
                if (itemCount == null) return;
                itemCount.text = "";
                return;
            }
            
            itemIcon.gameObject.SetActive(true);
            itemIcon.sprite = item.item.sprite;

            if (includeName)
            {
                item.item.itemName.StringChanged += UpdateName;
            }
            else
            {
                if (itemCount == null) return;
                itemCount.text = item.count <= 0 ? string.Empty : $"x{item.count}";
            }
        }

        protected virtual void PreItemChanged()
        {
            if (item.IsEmpty) return;
            if (!includeName) return;
            item.item.itemName.StringChanged -= UpdateName;
        }

        private void UpdateName(string itemName)
        {
            if (itemCount == null) return;
            itemCount.text = item.count <= 0 ? item.item.itemName.GetLocalizedString() : $"{item.item.itemName.GetLocalizedString()} x{item.count}";
        }
    }
}