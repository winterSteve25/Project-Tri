using Player.Interaction;
using Sirenix.OdinInspector;
using TileBehaviours.Excavator;
using UI.BreakProgress;
using UI.Menu.InventoryMenu;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils;
using Utils.Data;
using World.Tiles;

namespace Items.ItemTypes
{
    public class MiningItem : TriItem, IHoldBehaviourItem, IReleasedBehaviourItem
    {
        private static readonly DataSignature<TileInstance> CurrentlyMiningTile = new("CurrentlyMiningTile");

        [BoxGroup(GeneralInformationBox)] [VerticalGroup(VerticalMain)] [SerializeField]
        private MiningRecipes recipes;

        [BoxGroup(GeneralInformationBox)] [VerticalGroup(VerticalMain)] [SerializeField] [MinValue(1)]
        private float miningSpeedModifier;

        public void Hold(MouseButton mouseButton, ref ItemStack itemStack, TileInstance tileClicked, Vector3 clickedPos,
            Vector3Int pos, TilemapManager tilemapManager, InventoryTabController inventoryTabController,
            EquipmentsController equipmentsController, Vector3 playerPosition)
        {
            if (mouseButton != MouseButton.Left) return;

            if (tileClicked is null)
            {
                var ore = tilemapManager.GetTile(pos, TilemapLayer.Ground);
                if (!recipes.HasRecipe(ore.Tile))
                {
                    CancelMining(ref itemStack);
                    return;
                }
                
                if (MineTile(ref itemStack, ore, pos, tilemapManager.GetTilemap(TilemapLayer.Ground)))
                {
                    var result = recipes.FindRecipe(ore.Tile).output;
                    ItemSpawner.Current.SpawnApproximatelyAt(clickedPos, result);
                }
            }
            else if (MineTile(ref itemStack, tileClicked, pos, tilemapManager.GetTilemap(TilemapLayer.Obstacles)))
            {
                tilemapManager.RemoveTile(pos, TilemapLayer.Obstacles);
            }
        }

        private static void CancelMining(ref ItemStack itemStack)
        {
            var usedToBeMiningTile = itemStack.CustomData.GetOrDefault(CurrentlyMiningTile, null);
            if (usedToBeMiningTile != null)
            {
                BreakProgressManager.Current.CancelMining(usedToBeMiningTile);
            }

            itemStack.CustomData.Remove(CurrentlyMiningTile);
        }

        public void Release(MouseButton mouseButton, ref ItemStack itemStack, TilemapManager tilemapManager,
            InventoryTabController inventoryTabController, EquipmentsController equipmentsController,
            Vector3 playerPosition)
        {
            CancelMining(ref itemStack);
        }

        public bool CanInteract(ref ItemStack itemStack, TileInstance tileAtLocation, Vector3 clickedPos,
            Vector3Int pos, TilemapManager tilemapManager, InventoryTabController inventoryTabController,
            EquipmentsController equipmentsController, Vector3 playerPosition)
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

        protected bool MineTile(ref ItemStack itemStack, TileInstance tileInstance, Vector3Int pos, Tilemap tilemap)
        {
            var usedToBeMiningTile = itemStack.CustomData.GetOrDefault(CurrentlyMiningTile, null);
            var breakProgressManager = BreakProgressManager.Current;

            if (tileInstance != usedToBeMiningTile)
            {
                // reset the old breaking progress if there is one
                if (usedToBeMiningTile != null)
                {
                    breakProgressManager.CancelMining(usedToBeMiningTile);
                }

                if (tileInstance != null)
                {
                    itemStack.CustomData.Set(CurrentlyMiningTile, tileInstance);
                }
            }

            if (breakProgressManager.MineTile(tileInstance, pos, tilemap, miningSpeedModifier))
            {
                itemStack.CustomData.Remove(CurrentlyMiningTile);
                return true;
            }

            return false;
        }
    }
}