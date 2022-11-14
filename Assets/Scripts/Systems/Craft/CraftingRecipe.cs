using System;
using Items;
using Systems.Inv;

namespace Systems.Craft
{
    [Serializable]
    public struct CraftingRecipe
    {
        public ItemStack[] inputs;
        public ItemStack result;
        
        public bool IsInvalid => inputs is not { Length: > 0 } || result.IsEmpty;

        public bool CanCraft(Inventory inventory)
        {
            var containsAllIngredients = true;

            // do we have all the ingredients?
            foreach (var ingredient in inputs)
            {
                if (inventory.Contains(ingredient)) continue;
                containsAllIngredients = false;
            }

            return containsAllIngredients;
        }
    }
}