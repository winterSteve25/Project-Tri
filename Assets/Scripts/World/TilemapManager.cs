using System;
using Items;
using Tiles;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace World
{
    /// <summary>
    /// Global instance that allows access to ground layer and obstacles layer
    /// </summary>
    public class TilemapManager : MonoBehaviour
    {
        [SerializeField] private Tilemap groundLayerPrefab;
        [SerializeField] private Tilemap obstaclesLayerPrefab;

        [NonSerialized] public Tilemap GroundLayer;
        [NonSerialized] public Tilemap ObstacleLayer;

        [NonSerialized] public bool UpdateTiles = true;
        
        public void Awake()
        {
            GroundLayer = Instantiate(groundLayerPrefab, transform);
            ObstacleLayer = Instantiate(obstaclesLayerPrefab, transform);
        }

        public void PlaceTile(Vector3Int pos, TileBase tileBase, TilemapLayer layer, bool updateTiles = true)
        {
            switch (layer)
            {
                case TilemapLayer.Ground:
                    GroundLayer.SetTile(pos, tileBase);
                    break;
                case TilemapLayer.Obstacles:
                    ObstacleLayer.SetTile(pos, tileBase);
                    UpdateNeighbors(pos, updateTiles);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(layer), layer, null);
            }
        }

        public void PlaceTile(Vector3Int[] pos, TileBase[] tileBase, TilemapLayer layer, bool updateTiles = true)
        {
            switch (layer)
            {
                case TilemapLayer.Ground:
                    GroundLayer.SetTiles(pos, tileBase);
                    break;
                case TilemapLayer.Obstacles:
                    ObstacleLayer.SetTiles(pos, tileBase);
                    foreach (var p in pos)
                    {
                        UpdateNeighbors(p, updateTiles);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(layer), layer, null);
            }
        }
        
        public void RemoveTile(Vector3Int pos, TilemapLayer layer, bool updateTiles = true)
        {
            switch (layer)
            {
                case TilemapLayer.Ground:
                    GroundLayer.SetTile(pos, null);
                    break;
                case TilemapLayer.Obstacles:
                    var go = GetGameObject(pos, TilemapLayer.Obstacles);
                    if (go == null) return;
                    if (go.TryGetComponent<MachineTile>(out var tileMachine))
                    {
                        foreach (var item in tileMachine.GetDrops())
                        {
                            ItemSpawner.Instance.SpawnApproximatelyAt(new Vector3(pos.x, pos.y) + ObstacleLayer.cellSize * 0.5f, item);
                        }
                    
                        tileMachine.OnBroken();
                    }
                    
                    ObstacleLayer.SetTile(pos, null);
                    UpdateNeighbors(pos, updateTiles);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(layer), layer, null);
            }
        }
        
        public void RemoveTile(Vector3Int[] pos, TilemapLayer layer, bool updateTiles = true)
        {
            var tiles = new TileBase[pos.Length];
            
            switch (layer)
            {
                case TilemapLayer.Ground:
                    GroundLayer.SetTiles(pos, tiles);
                    break;
                case TilemapLayer.Obstacles:
                    ObstacleLayer.SetTiles(pos, tiles);
                    foreach (var p in pos)
                    {
                        UpdateNeighbors(p, updateTiles);
                    }
                    
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(layer), layer, null);
            }
        }

        public TileBase GetTile(Vector3Int pos, TilemapLayer layer)
        {
            return layer switch
            {
                TilemapLayer.Ground => GroundLayer.GetTile(pos),
                TilemapLayer.Obstacles => ObstacleLayer.GetTile(pos),
                _ => throw new ArgumentOutOfRangeException(nameof(layer), layer, null)
            };
        }
        
        public GameObject GetGameObject(TileBase tile, Vector3Int pos, Tilemap tilemap)
        {
            return tile switch
            {
                Tile => tilemap.GetInstantiatedObject(pos),
                RuleTile rt => rt.m_DefaultGameObject,
                _ => null
            };
        }
        
        public GameObject GetGameObject(Vector3Int pos, TilemapLayer layer)
        {
            return layer switch
            {
                TilemapLayer.Ground => GetGameObject(GroundLayer.GetTile(pos), pos, GroundLayer),
                TilemapLayer.Obstacles => GetGameObject(ObstacleLayer.GetTile(pos), pos, ObstacleLayer),
                _ => throw new ArgumentOutOfRangeException(nameof(layer), layer, null)
            };
        }

        private void UpdateNeighbors(Vector3Int pos, bool updateTiles)
        {
            if (!UpdateTiles && !updateTiles) return;
            
            for (var i = -1; i <= 1; i++)
            {
                for (var j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;
                    var p = new Vector3Int(i, j) + pos;
                    var tile = GetGameObject(p, TilemapLayer.Obstacles);
                    if (tile is null) continue;
                    if (tile.TryGetComponent<MachineTile>(out var machineTile))
                    {
                        machineTile.OnNeighborsChanged();
                    }
                }
            }
        }
    }
}