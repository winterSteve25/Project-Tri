using System.Linq;
using Player.Interaction;
using Sirenix.OdinInspector;
using UI.BreakProgress;
using UnityEngine;
using Utils.Data;
using World.Tiles;

namespace Items.ItemTypes
{
    public class ChainSawItem : TriItem, IUpdateBehaviourItem
    {
        private static readonly DataSignature<Vector3Int[]> MiningTiles = new("MiningTiles");

        [BoxGroup(GeneralInformationBox)] [VerticalGroup(VerticalMain)]
        [SerializeField] private float miningSpeedModifier = 2f;

        public void UpdateBehaviour(ref ItemStack itemStack, Vector3 playerPosition)
        {
            var tilemapManager = TilemapManager.Current;
            var obstacleTilemap = tilemapManager.ObstacleLayer;
            var center = obstacleTilemap.WorldToCell(playerPosition);
            var breakManager = BreakProgressManager.Current;

            var positions = new Vector3Int[9];
            var n = 0;
            
            for (var i = -1; i <= 1; i++)
            {
                for (var j = -1; j <= 1; j++)
                {
                    var pos = new Vector3Int(center.x + i, center.y + j);
                    positions[n] = pos;
                    
                    if (breakManager.MineTile(
                            tilemapManager.GetTile(pos, TilemapLayer.Obstacles),
                            pos,
                            obstacleTilemap,
                            miningSpeedModifier
                        ))
                    {
                        tilemapManager.RemoveTile(pos, TilemapLayer.Obstacles);
                    }

                    n++;
                }
            }

            itemStack.SavedData.ComputeIfPresent(MiningTiles, oldPositions =>
            {
                foreach (var pos in oldPositions.Where(p => !positions.Contains(p)))
                {
                    breakManager.CancelMining(tilemapManager.GetTile(pos, TilemapLayer.Obstacles));
                }
            });
            
            itemStack.SavedData.Set(MiningTiles, positions);
        }
    }
}