using System;
using System.Collections.Generic;
using System.Linq;
using MsgPack.Serialization;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace World.Tiles
{
    public class LayeredChunk
    {
        [MessagePackIgnore] private Tilemap _groundLayer;
        [MessagePackIgnore] private Tilemap _obstacleLayer;
        
        public readonly Chunk GroundChunk;
        public readonly Chunk ObstacleChunk;

        public LayeredChunk(Tilemap groundLayer, Tilemap obstacleLayer)
        {
            _groundLayer = groundLayer;
            _obstacleLayer = obstacleLayer;
            
            GroundChunk = new Chunk(_groundLayer);
            ObstacleChunk = new Chunk(_obstacleLayer);
        }

        [MessagePackDeserializationConstructor]
        public LayeredChunk(Chunk groundChunk, Chunk obstacleChunk)
        {
            GroundChunk = groundChunk;
            ObstacleChunk = obstacleChunk;
        }

        public void Setup(Tilemap groundLayer, Tilemap obstacleLayer)
        {
            _groundLayer ??= groundLayer;
            _obstacleLayer ??= obstacleLayer;
            
            GroundChunk.Setup(_groundLayer);
            ObstacleChunk.Setup(_obstacleLayer);
        }

        public void Place(Vector3Int pos, TileInstance tile, TilemapLayer layer, bool updateTiles, bool spawnDrops)
        {
            switch (layer)
            {
                case TilemapLayer.Ground:
                    GroundChunk.Place(pos, tile, false, spawnDrops);
                    break;
                case TilemapLayer.Obstacles:
                    ObstacleChunk.Place(pos, tile, updateTiles, spawnDrops);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(layer), layer, null);
            }
        }

        public void Place(Vector3Int[] poses, IEnumerable<TileInstance> tiles, TilemapLayer layer, bool updateTiles, bool spawnDrops)
        {
            switch (layer)
            {
                case TilemapLayer.Ground:
                    GroundChunk.Place(poses, tiles.ToArray(), false, spawnDrops);
                    break;
                case TilemapLayer.Obstacles:
                    ObstacleChunk.Place(poses, tiles.ToArray(), updateTiles, spawnDrops);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(layer), layer, null);
            }
        }

        public void Remove(Vector3Int pos, TilemapLayer layer, bool updateTiles, bool spawnDrops)
        {
            switch (layer)
            {
                case TilemapLayer.Ground:
                    GroundChunk.Remove(pos, false, spawnDrops);
                    break;
                case TilemapLayer.Obstacles:
                    ObstacleChunk.Remove(pos, updateTiles, spawnDrops);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(layer), layer, null);
            }
        }

        public void Remove(IEnumerable<Vector3Int> poses, TilemapLayer layer, bool updateTiles, bool spawnDrops)
        {
            switch (layer)
            {
                case TilemapLayer.Ground:
                    GroundChunk.Remove(poses, false, spawnDrops);
                    break;
                case TilemapLayer.Obstacles:
                    ObstacleChunk.Remove(poses, updateTiles, spawnDrops);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(layer), layer, null);
            }
        }

        public GameObject GetGameObject(Vector3Int pos, TilemapLayer layer)
        {
            return layer switch
            {
                TilemapLayer.Ground => GroundChunk.GetGameObject(pos),
                TilemapLayer.Obstacles => ObstacleChunk.GetGameObject(pos),
                _ => throw new ArgumentOutOfRangeException(nameof(layer), layer, null)
            };
        }

        public TileInstance GetTile(Vector3Int pos, TilemapLayer layer)
        {
            return layer switch
            {
                TilemapLayer.Ground => GroundChunk.GetTile(pos),
                TilemapLayer.Obstacles => ObstacleChunk.GetTile(pos),
                _ => throw new ArgumentOutOfRangeException(nameof(layer), layer, null)
            };
        }

        public Vector3 CellToWorld(Vector3Int pos, TilemapLayer layer)
        {
            return layer switch
            {
                TilemapLayer.Ground => GroundChunk.CellToWorld(pos),
                TilemapLayer.Obstacles => ObstacleChunk.CellToWorld(pos),
                _ => throw new ArgumentOutOfRangeException(nameof(layer), layer, null)
            };
        }
    }
}