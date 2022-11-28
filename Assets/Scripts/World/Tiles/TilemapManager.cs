using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SaveLoad;
using SaveLoad.Interfaces;
using SaveLoad.Tasks;
using Sirenix.OdinInspector;
using TileBehaviours;
using Tiles;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils;
using Utils.Data;
using Transform = UnityEngine.Transform;

namespace World.Tiles
{
    /// <summary>
    /// Global instance that allows access to ground layer and obstacles layer
    /// </summary>
    public class TilemapManager : CurrentInstanced<TilemapManager>, ICustomWorldData
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

        protected override void Awake()
        {
            base.Awake();
            GroundLayer = Instantiate(groundLayerPrefab, transform);
            ObstacleLayer = Instantiate(obstaclesLayerPrefab, transform);
            Chunk = new LayeredChunk(GroundLayer, ObstacleLayer);
        }

        public bool TryPlaceTile(PlaceableTile tile, Vector3Int pos, TilemapLayer layer)
        {
            if (!tile.CanPlace(pos, this)) return false;
            PlaceTile(pos, tile, layer);
            return true;
        }

        public void PlaceTile(Vector3Int pos, TileInstance tileBase, TilemapLayer layer, bool updateTiles = true, bool spawnDrops = true)
        {
            Chunk.Place(pos, tileBase, layer, updateTiles, spawnDrops);
        }

        public void PlaceTile(Vector3Int[] pos, TileInstance[] tileBase, TilemapLayer layer, bool updateTiles = true, bool spawnDrops = true)
        {
            Chunk.Place(pos, tileBase, layer, updateTiles, spawnDrops);
        }
        
        public void RemoveTile(Vector3Int pos, TilemapLayer layer, bool updateTiles = true, bool spawnDrops = true)
        {
            Chunk.Remove(pos, layer, updateTiles, spawnDrops);
        }
        
        public void RemoveTile(Vector3Int[] pos, TilemapLayer layer, bool updateTiles = true, bool spawnDrops = true)
        {
            Chunk.Remove(pos, layer, updateTiles, spawnDrops);
        }

        public GameObject GetGameObject(Vector3Int pos, TilemapLayer layer)
        {
            return Chunk.GetGameObject(pos, layer);
        }

        public TileInstance GetTile(Vector3Int pos, TilemapLayer layer)
        {
            return Chunk.GetTile(pos, layer);
        }

        public Vector3 CellToWorldPosition(Vector3Int pos, TilemapLayer layer)
        {
            return Chunk.CellToWorld(pos, layer);
        }
        
        public SerializationPriority Priority => SerializationPriority.Medium;

        public async Task Save()
        {
            await SaveUtilities.Save(FileLocation.CurrentWorldSave(FileNameConstants.WorldChunk), Chunk);

            var saveTask = new SaveTask();
            var machineTiles = new List<CustomTileBehaviour>();

            foreach (Transform child in ObstacleLayer.transform)
            {
                if (!child.TryGetComponent<CustomTileBehaviour>(out var data)) continue;
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
                var machine = Chunk.ObstacleChunk.GetGameObject(machinePos).GetComponent<CustomTileBehaviour>();
                await ((IChainedWorldData)machine).Load(loadTask);
            }

            await loadTask.DisposeAsync();
        }
    }
}