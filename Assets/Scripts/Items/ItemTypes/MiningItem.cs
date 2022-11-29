using Player.Interaction;
using Sirenix.OdinInspector;
using TileBehaviours.Excavator;
using UI.Menu.EscapeMenu;
using UnityEngine;
using Utils;
using Utils.Data;
using World.Tiles;

namespace Items.ItemTypes
{
    public class MiningItem : TriItem, IHoldBehaviourItem, IReleasedBehaviourItem
    {
        public static readonly DataSignature<TileInstance> CurrentlyMiningTile = new("CurrentlyMiningTile");

        [BoxGroup(GeneralInformationBox)] [VerticalGroup(VerticalMain)] [SerializeField]
        private MiningRecipes recipes;

        [BoxGroup(GeneralInformationBox)] [VerticalGroup(VerticalMain)] [SerializeField] [MinValue(1)]
        private float miningSpeedModifier;

        public void Hold(MouseButton mouseButton, ref ItemStack itemStack, TileInstance tileClicked, Vector3 clickedPos, 
            Vector3Int pos, TilemapManager tilemapManager, InventoryUIController inventoryUIController,
            EquipmentsController equipmentsController, Vector3 playerPosition,
            Vector3 playerDistanceToClickedPoint)
        {
            if (mouseButton != MouseButton.Left) return;

            if (tileClicked is null)
            {
                var ore = tilemapManager.GetTile(pos, TilemapLayer.Ground);
                if (!recipes.HasRecipe(ore.Tile)) return;
                if (MineTile(ref itemStack, ore))
                {
                    var result = recipes.FindRecipe(ore.Tile).output;
                    ItemSpawner.Current.SpawnApproximatelyAt(clickedPos, result);
                }
            }
            else if (MineTile(ref itemStack, tileClicked))
            {
                tilemapManager.RemoveTile(pos, TilemapLayer.Obstacles);
            }
        }

        public void Release(MouseButton mouseButton, ref ItemStack itemStack, TilemapManager tilemapManager,
            InventoryUIController inventoryUIController, EquipmentsController equipmentsController,
            Vector3 playerPosition)
        {
            var usedToBeMiningTile = itemStack.CustomData.GetOrDefault(CurrentlyMiningTile, null);
            if (usedToBeMiningTile != null)
            {
                usedToBeMiningTile.BreakProgress = 0;
            }

            itemStack.CustomData.Remove(CurrentlyMiningTile);
        }

        public bool CanInteract(ref ItemStack itemStack, TileInstance tileAtLocation, Vector3 clickedPos,
            Vector3Int pos, TilemapManager tilemapManager, InventoryUIController inventoryUIController,
            EquipmentsController equipmentsController, Vector3 playerPosition,
            Vector3 playerDistanceToClickedPoint)
        {
            if (tileAtLocation is not null) return true;

            var ore = tilemapManager.GetTile(pos, TilemapLayer.Ground);
            if (ore != null)
            {
                if (recipes.HasRecipe(ore.Tile))
                {
                    return true;
                }
            }

            return false;
        }

        protected bool MineTile(ref ItemStack itemStack, TileInstance tileInstance)
        {
            var usedToBeMiningTile = itemStack.CustomData.GetOrDefault(CurrentlyMiningTile, null);
            if (tileInstance != usedToBeMiningTile)
            {
                // reset the old breaking progress if there is one
                if (usedToBeMiningTile != null)
                {
                    usedToBeMiningTile.BreakProgress = 0;
                }

                if (tileInstance != null)
                {
                    itemStack.CustomData.Set(CurrentlyMiningTile, tileInstance);
                }
            }

            if (tileInstance == null) return false;

            tileInstance.BreakProgress += Time.deltaTime * miningSpeedModifier;

            if (tileInstance.BreakProgress >= tileInstance.Tile.hardness)
            {
                tileInstance.BreakProgress = 0;
                itemStack.CustomData.Remove(CurrentlyMiningTile);
                return true;
            }

            return false;
        }
    }
}