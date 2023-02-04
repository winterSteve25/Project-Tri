using System;
using EditorAttributes;
using Items;
using Liquid;

namespace TileBehaviours.Melter
{
    [Serializable]
    public struct MelterRecipe
    {
        [ItemSlot(75, 75)]
        public ItemStack input;
        public LiquidStack output;
        public float duration;
    }
}