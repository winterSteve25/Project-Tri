using TileBehaviours.Excavator;
using UnityEngine;
using World.Tiles;

namespace Tiles
{
    /// <summary>
    /// Scriptable object that holds information about the excavator placeable tile
    /// </summary>
    [CreateAssetMenu(fileName = "Excavator Tile", menuName = "Machines/Excavator/New Excavator Tile")]
    public class ExcavatorTile : PlaceableTile
    {
        [SerializeField] private MiningRecipes recipes;
        
        public override bool CanPlace(Vector3Int pos, TilemapManager tilemapManager)
        {
            return recipes.HasRecipe(tilemapManager.GetTile(pos, TilemapLayer.Ground).Tile) && base.CanPlace(pos, tilemapManager);
        }
    }
}