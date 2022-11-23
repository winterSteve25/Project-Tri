using Sirenix.OdinInspector;
using Systems.Inv;
using Tiles.Excavator;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils;
using World;

namespace Items.ItemTypes
{
    public class MiningItem : Item, IHoldBehaviourItem, IClickedBehaviourItem
    {
        [BoxGroup(GeneralInformationBox)]
        [VerticalGroup(VerticalMain)]
        [SerializeField] 
        private MiningRecipes recipes;

        public void Hold(MouseButton mouseButton, TileBase tileClicked, Vector3Int pos, TilemapManager tilemapManager,
            InventoryManager inventoryManager, Vector3 playerPosition, Vector3 playerDistanceToClickedPoint)
        {
            if (mouseButton != MouseButton.Left) return;
            if (tileClicked is null) return;
            tilemapManager.RemoveTile(pos, TilemapLayer.Obstacles);
        }

        public void Click(MouseButton mouseButton, TileBase tileClicked, Vector3Int pos, TilemapManager tilemapManager,
            InventoryManager inventoryManager, Vector3 playerPosition, Vector3 playerDistanceToClickedPoint)
        {
            if (mouseButton != MouseButton.Left) return;
            var ore = tilemapManager.GetTile(pos, TilemapLayer.Ground);
            if (recipes.HasRecipe(ore))
            {
                var inv = inventoryManager.CurrentInventory;
                var itemStack = recipes.FindRecipe(ore).output;
                
                if (inv == null)
                {
                    ItemSpawner.Current.SpawnApproximatelyAt(playerPosition, itemStack);
                }
                else
                {
                    inv.Add(playerPosition, itemStack);
                }
            }
        }

        public bool CanInteract(TileBase tileAtLocation, Vector3Int pos, TilemapManager tilemapManager,
            InventoryManager inventoryManager, Vector3 playerPosition, Vector3 playerDistanceToClickedPoint)
        {
            if (tileAtLocation is not null) return true;
            
            var ore = tilemapManager.GetTile(pos, TilemapLayer.Ground);
            if (recipes.HasRecipe(ore))
            {
                return true;
            }

            return false;
        }
    }
}