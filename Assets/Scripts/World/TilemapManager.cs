using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Items;
using PlaceableTiles;
using SaveLoad;
using SaveLoad.Interfaces;
using SaveLoad.Tasks;
using Sirenix.OdinInspector;
using Tiles;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils.Data;
using Transform = UnityEngine.Transform;

namespace World
{
    /// <summary>
    /// Global instance that allows access to ground layer and obstacles layer
    /// </summary>
    public class TilemapManager : MonoBehaviour, ICustomWorldData
    {
        [SerializeField, AssetsOnly] private Tilemap groundLayerPrefab;
        [SerializeField, AssetsOnly] private Tilemap obstaclesLayerPrefab;

        [NonSerialized] public Tilemap GroundLayer;
        [NonSerialized] public Tilemap ObstacleLayer;

        [NonSerialized] public bool UpdateTiles = true;

        private LayeredChunk _chunk;

        public LayeredChunk Chunk
        {
            get => _chunk;
            set
            {
                _chunk = value;
                _chunk.Setup(GroundLayer, ObstacleLayer);
            }
        }

        public void Awake()
        {
            GroundLayer = Instantiate(groundLayerPrefab, transform);
            ObstacleLayer = Instantiate(obstaclesLayerPrefab, transform);
            Chunk = new LayeredChunk();
        }

        public bool TryPlaceTile(PlaceableTile tile, Vector3Int pos, TilemapLayer layer)
        {
            if (!tile.CanPlace(pos, GroundLayer, ObstacleLayer)) return false;
            PlaceTile(pos, tile.tileBase, layer);
            return true;
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
            
            Chunk.Place(pos, tileBase, layer);
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
            
            Chunk.Place(pos, tileBase, layer);
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
                            ItemSpawner.Current.SpawnApproximatelyAt(new Vector3(pos.x, pos.y) + ObstacleLayer.cellSize * 0.5f, item);
                        }
                    
                        tileMachine.OnBroken();
                    }
                    
                    ObstacleLayer.SetTile(pos, null);
                    UpdateNeighbors(pos, updateTiles);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(layer), layer, null);
            }
            
            Chunk.Remove(pos, layer);
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
            
            Chunk.Remove(pos, layer);
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

        public SerializationPriority Priority => SerializationPriority.Medium;

        public async Task Save()
        {
            await SaveUtilities.Save(FileLocation.CurrentWorldSave(FileNameConstants.WorldChunk), Chunk);

            var saveTask = new SaveTask();
            var machineTiles = new List<MachineTile>();

            foreach (Transform child in ObstacleLayer.transform)
            {
                if (!child.TryGetComponent<MachineTile>(out var data)) continue;
                if (data is IChainedWorldData)
                {
                    machineTiles.Add(data);
                }
            }

            await saveTask.Serialize(machineTiles.Count);

            foreach (var machineTile in machineTiles)
            {
                await saveTask.Serialize(machineTile.Pos);
                await ((IChainedWorldData)machineTile).Save(saveTask);
            }

            await saveTask.WriteToFile(FileLocation.CurrentWorldSave(FileNameConstants.MachineData));
        }

        public async Task Read(FileLocation worldFolder)
        {
            var chunk = await SaveUtilities.Load<LayeredChunk>(worldFolder.FileAtLocation(FileNameConstants.WorldChunk), null);
            Chunk = chunk;

            var loadTask = await LoadTask.ReadFromFile(FileLocation.CurrentWorldSave(FileNameConstants.MachineData));
            var machineTileCount = await loadTask.Deserialize<int>();

            for (var i = 0; i < machineTileCount; i++)
            {
                var machinePos = await loadTask.Deserialize<Vector3Int>();
                var machine = GetGameObject(machinePos, TilemapLayer.Obstacles).GetComponent<MachineTile>();
                await ((IChainedWorldData)machine).Load(loadTask);
            }

            await loadTask.DisposeAsync();
        }
    }
}