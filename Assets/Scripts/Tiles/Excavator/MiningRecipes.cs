using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Tiles.Excavator
{    
    /// <summary>
    /// A single instance scriptable object used across all excavator related scripts that defines all the valid recipes of an excavator
    /// </summary>
    [CreateAssetMenu(fileName = "Excavator Recipes", menuName = "Machines/Excavator/New Recipes")]
    public class MiningRecipes : ScriptableObject
    {
        [SerializeField] private List<MiningRecipe> recipes;

        public bool HasRecipe(TileBase ore)
        {
            return recipes.Any(r => r.ore == ore);
        }

        public MiningRecipe FindRecipe(TileBase ore)
        {
            return recipes.Find(r => r.ore == ore);
        }
    }
}