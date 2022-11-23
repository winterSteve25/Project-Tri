using System;
using Systems.Craft;
using UnityEngine;

namespace Registries
{
    [CreateAssetMenu(fileName = "Recipes Registry", menuName = "Registries/New Recipes Registry")]
    public class RecipesRegistry : Registry<RecipesRegistry.RecipesRegistryDictionary, CraftingRecipe>
    {
        private static RecipesRegistry _instance;
        public static RecipesRegistry Instance => _instance ??= Resources.Load<RecipesRegistry>("Registries/REG_Recipes Registry");

        [Serializable]
        public class RecipesRegistryDictionary : SerializableDictionary<CraftingRecipe, string>
        {
        }
    }
}