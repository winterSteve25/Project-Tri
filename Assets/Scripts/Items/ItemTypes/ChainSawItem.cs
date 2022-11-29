using Player.Interaction;
using UnityEngine;
using World.Tiles;

namespace Items.ItemTypes
{
    public class ChainSawItem : TriItem, IUpdateBehaviourItem
    {
        public void UpdateBehaviour(Vector3 playerPosition)
        {
            var tilemapManager = TilemapManager.Current;
            var obstacleTilemap = tilemapManager.ObstacleLayer;
            var center = obstacleTilemap.WorldToCell(playerPosition);

            for (var i = -1; i <= 1; i++)
            {
                for (var j = -1; j <= 1; j++)
                {
                    var pos = new Vector3Int(center.x + i, center.y + j);
                    tilemapManager.RemoveTile(pos, TilemapLayer.Obstacles);
                }
            }
        }
    }
}