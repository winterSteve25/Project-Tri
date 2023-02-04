using System.Collections.Generic;
using System.Linq;
using Items;
using UnityEngine;

namespace TileBehaviours.Melter
{
    /// <summary>
    /// A single instance scriptable object used across all excavator related scripts that defines all the valid recipes of an excavator
    /// </summary>
    [CreateAssetMenu(fileName = "Melter Recipes", menuName = "Machines/Melter/New Recipes")]
    public class MelterRecipes : ScriptableObject
    {
        [SerializeField] private List<MelterRecipe> recipes;

        public bool HasRecipe(ItemStack input)
        {
            return recipes.Any(r => r.input.item == input.item && r.input.count <= input.count);
        }

        public MelterRecipe FindRecipe(ItemStack input)
        {
            return recipes.Find(r => r.input.item == input.item && r.input.count <= input.count);
        }
    }
}