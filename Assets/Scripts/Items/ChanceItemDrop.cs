using System;
using UnityEngine;
using Utils;

namespace Items
{
    [Serializable]
    public struct ChanceItemDrop
    {
        [SerializeField] private ItemStack itemStack;
        [Range(0, 1), SerializeField] public float chance;

        public ItemStack Get()
        {
            return MathHelper.Chance(chance) ? itemStack : ItemStack.Empty;
        }
    }
}