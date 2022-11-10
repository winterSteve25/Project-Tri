using Tiles.Excavator;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PlaceableTiles
{
    /// <summary>
    /// Scriptable object that holds information about the excavator placeable tile
    /// </summary>
    [CreateAssetMenu(fileName = "Excavator Tile", menuName = "Machines/Excavator/New Excavator Tile")]
    public class ExcavatorTile : PlaceableTile
    {
        [SerializeField] private ExcavatorRecipes recipes;
        
        public override bool CanPlace(Vector3Int pos, Tilemap groundLayer, Tilemap obstaclesLayer)
        {
            return recipes.HasRecipe(groundLayer.GetTile(pos)) && base.CanPlace(pos, groundLayer, obstaclesLayer);
        }
    }
}