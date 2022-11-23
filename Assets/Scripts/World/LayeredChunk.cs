using System;
using System.Collections.Generic;
using MsgPack.Serialization;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace World
{
    public class LayeredChunk
    {
        public readonly Chunk GroundChunk;
        public readonly Chunk ObstacleChunk;

        public LayeredChunk()
        {
            GroundChunk = new Chunk();
            ObstacleChunk = new Chunk();
        }

        [MessagePackDeserializationConstructor]
        public LayeredChunk(Chunk groundChunk, Chunk obstacleChunk)
        {
            GroundChunk = groundChunk;
            ObstacleChunk = obstacleChunk;
        }

        public void Setup(Tilemap groundLayer, Tilemap obstacleLayer)
        {
            GroundChunk.Setup(groundLayer);
            ObstacleChunk.Setup(obstacleLayer);
        }

        public void Place(Vector3Int pos, TileBase tile, TilemapLayer layer)
        {
            switch (layer)
            {
                case TilemapLayer.Ground:
                    GroundChunk.Place(pos, tile);
                    break;
                case TilemapLayer.Obstacles:
                    ObstacleChunk.Place(pos, tile);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(layer), layer, null);
            }
        }

        public void Place(Vector3Int[] poses, TileBase[] tiles, TilemapLayer layer)
        {
            switch (layer)
            {
                case TilemapLayer.Ground:
                    GroundChunk.Place(poses, tiles);
                    break;
                case TilemapLayer.Obstacles:
                    ObstacleChunk.Place(poses, tiles);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(layer), layer, null);
            }
        }

        public void Remove(Vector3Int pos, TilemapLayer layer)
        {
            switch (layer)
            {
                case TilemapLayer.Ground:
                    GroundChunk.Remove(pos);
                    break;
                case TilemapLayer.Obstacles:
                    ObstacleChunk.Remove(pos);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(layer), layer, null);
            }
        }

        public void Remove(IEnumerable<Vector3Int> poses, TilemapLayer layer)
        {
            switch (layer)
            {
                case TilemapLayer.Ground:
                    GroundChunk.Remove(poses);
                    break;
                case TilemapLayer.Obstacles:
                    ObstacleChunk.Remove(poses);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(layer), layer, null);
            }
        }
    }
}