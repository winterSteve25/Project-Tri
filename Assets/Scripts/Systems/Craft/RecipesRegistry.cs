using System.Collections.Generic;
using UnityEngine;

namespace Systems.Craft
{
    [CreateAssetMenu(fileName = "Recipes Registry", menuName = "Crafting/New Recipes Registry")]
    public class RecipesRegistry : ScriptableObject
    {
        [SerializeField] private List<CraftingRecipe> recipes;
        public List<CraftingRecipe> Recipes => recipes;
    }
}