using System.Linq;
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
        public CraftingIngredient[] ingredients;

        public ItemStack[] inputs
        {
            get => ingredients.Select(ing => ing.itemStack).ToArray();
            set => ingredients = value.Select(item => (CraftingIngredient) item).ToArray();
        }
        
        public bool IsInvalid => ingredients is not { Length: > 0 } || result.IsEmpty;

        public bool CanCraft(Inventory inventory)
        {
            var containsAllIngredients = true;

            // do we have all the ingredients?
            foreach (var ingredient in ingredients)
            {
                if (inventory.Contains(ingredient)) continue;
                containsAllIngredients = false;
            }

            return containsAllIngredients;
        }
    }
}