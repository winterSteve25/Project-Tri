using System.Collections.Generic;
using System.Linq;
using Items;
using MsgPack.Serialization;
using TileBehaviours;
using Tiles;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils;

namespace World.Tiles
{
    public class Chunk
    {
        [MessagePackIgnore] public readonly Dictionary<Vector3Int, TileInstance> Tiles;
        [MessagePackIgnore] private Tilemap _tileMap;

        public Vector3Int[] Positions => Tiles.Keys.ToArray();
        public TileInstance[] TileInstances => Tiles.Values.ToArray();

        public Chunk(Tilemap tilemap)
        {
            Tiles = new Dictionary<Vector3Int, TileInstance>();
            _tileMap = tilemap;
        }

        [MessagePackDeserializationConstructor]
        public Chunk(Vector3Int[] positions, TileInstance[] tileInstances)
        {
            if (positions.Length != tileInstances.Length)
            {
                Debug.LogError("Chunk failed to deserialize as positions count do not match tiles count");
            }

            Tiles = new Dictionary<Vector3Int, TileInstance>();
            
            for (var i = 0; i < positions.Length; i++)
            {
                Tiles.Add(positions[i], tileInstances[i]);
            }
        }

        public void Setup(Tilemap tilemap)
        {
            _tileMap = tilemap;
            _tileMap.SetTiles(Positions, TileUtilities.FromTileInstances(TileInstances));
        }
        
        public void Place(Vector3Int pos, TileInstance tile, bool updateTiles, bool spawnDrops)
        {
            if (Tiles.ContainsKey(pos)) Remove(pos, updateTiles, spawnDrops);
            Tiles.Add(pos, tile);
            _tileMap.SetTile(pos, tile.Tile.tileBase);
            UpdateNeighbors(pos, updateTiles);
        }

        public void Place(Vector3Int[] poses, TileInstance[] tiles, bool updateTiles, bool spawnDrops)
        {
            for (var i = 0; i < poses.Length; i++)
            {
                var pos = poses[i];
                if (Tiles.ContainsKey(pos)) Remove(pos, updateTiles, spawnDrops);
                Tiles.Add(pos, tiles[i]);
                UpdateNeighbors(pos, updateTiles);
            }
            
            _tileMap.SetTiles(poses, TileUtilities.FromTileInstances(tiles));
        }

        public void Remove(Vector3Int pos, bool updateTiles, bool spawnDrops)
        {
            if (!Tiles.ContainsKey(pos)) return;

            if (spawnDrops)
            {
                var tile = GetTile(pos);
                if (tile.Tile is PlaceableTile placeableTile)
                {
                    foreach (var item in placeableTile.drops)
                    {
                        ItemSpawner.Current.SpawnApproximatelyAt(CellToWorld(pos), item.Get());
                    }
                }
            }

            var go = GetGameObject(pos);
            if (go != null)
            {
                if (go.TryGetComponent<CustomTileBehaviour>(out var behaviour))
                {
                    behaviour.OnBroken();
                }
            }
            
            Tiles.Remove(pos);
            _tileMap.SetTile(pos, null);
            
            UpdateNeighbors(pos, updateTiles);
        }
        
        public void Remove(IEnumerable<Vector3Int> poses, bool updateTiles, bool spawnDrops)
        {
            foreach (var pos in poses)
            {
                Remove(pos, updateTiles, spawnDrops);
            }
        }
        
        public GameObject GetGameObject(Vector3Int pos)
        {
            return GetGameObject(GetTile(pos), pos);
        }

        private GameObject GetGameObject(TileInstance tile, Vector3Int pos)
        {
            if (tile == null) return null;
            return tile.Tile.tileBase switch
            {
                Tile => _tileMap.GetInstantiatedObject(pos),
                RuleTile rt => rt.m_DefaultGameObject,
                _ => null
            };
        }
        
        private void UpdateNeighbors(Vector3Int pos, bool updateTiles)
        {
            if (!TilemapManager.Current.UpdateTiles || !updateTiles) return;
            
            for (var i = -1; i <= 1; i++)
            {
                for (var j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;
                    if (Mathf.Abs(i) + Mathf.Abs(j) > 1) continue;
                    var p = new Vector3Int(i, j) + pos;
                    var tile = GetGameObject(p);
                    if (tile is null) continue;
                    if (tile.TryGetComponent<CustomTileBehaviour>(out var machineTile))
                    {
                        machineTile.OnNeighborsChanged();
                    }
                }
            }
        }
        
        public TileInstance GetTile(Vector3Int pos)
        {
            if (!Tiles.ContainsKey(pos)) return null;
            return Tiles[pos];
        }

        public Vector3 CellToWorld(Vector3Int pos)
        {
            return new Vector3(pos.x, pos.y) + _tileMap.cellSize * 0.5f;
        }
    }
}