using System;
using Items;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Systems.Craft
{
    [Serializable]
    public struct CraftingIngredient
    {
        [HideInInspector]
        public ItemStack itemStack;

        [HorizontalGroup("Horizontal Main", width: 35)]
        [ShowInInspector, ReadOnly, HideLabel, PreviewField(35, ObjectFieldAlignment.Center)]
        private Sprite Sprite => itemStack.item == null ? null : itemStack.item.sprite;
            
        [VerticalGroup("Horizontal Main/Attributes")]
        [LabelWidth(40), ShowInInspector]
        private TriItem Item
        {
            get => itemStack.item;
            set => itemStack.item = value;
        }
            
        [VerticalGroup("Horizontal Main/Attributes")]
        [LabelWidth(40), ShowInInspector, MinValue(1)]
        private int Count
        {
            get => itemStack.count;
            set => itemStack.count = value;
        }
            
        public CraftingIngredient(ItemStack itemStack)
        {
            this.itemStack = itemStack;
        }

        public static implicit operator CraftingIngredient(ItemStack itemStack)
        {
            return new CraftingIngredient(itemStack);
        }

        public static implicit operator ItemStack(CraftingIngredient craftingIngredient)
        {
            return craftingIngredient.itemStack;
        }
    }
}