using EditorAttributes;
using Items;
using Sirenix.OdinInspector;
using Systems.Inv;
using UnityEngine;

namespace Systems.Craft
{
    public class CraftingRecipe : ScriptableObject
    {
        [ItemSlot, HideLabel]
        [HorizontalGroup("Main", Width = 100)]
        [BoxGroup("Main/Result")]
        public ItemStack result;
        
        [HorizontalGroup("Main")]
        [BoxGroup("Main/Input")]
        public ItemStack[] inputs;

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