using System.Collections.Generic;
using System.Linq;
using MsgPack.Serialization;
using Registries;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace World
{
    public class Chunk
    {
        [MessagePackIgnore] public readonly Dictionary<Vector3Int, TileBase> Tiles;
        
        public Vector3Int[] Positions => Tiles.Keys.ToArray();
        public string[] TileIDs => Tiles.Values.Select(tileBase => TilesRegistry.Instance.Entries[tileBase]).ToArray();

        public Chunk()
        {
            Tiles = new Dictionary<Vector3Int, TileBase>();
        }

        [MessagePackDeserializationConstructor]
        public Chunk(Vector3Int[] positions, string[] tileIds)
        {
            if (positions.Length != tileIds.Length)
            {
                Debug.LogError("Chunk failed to deserialize as positions count do not match tiles count");
            }

            Tiles = new Dictionary<Vector3Int, TileBase>();
            var entries = TilesRegistry.Instance.Entries;
            
            for (var i = 0; i < positions.Length; i++)
            {
                Tiles.Add(positions[i], entries.First(tb => tb.Value == tileIds[i]).Key);
            }
        }

        public void Setup(Tilemap tilemap)
        {
            var entries = TilesRegistry.Instance.Entries;
            tilemap.SetTiles(Positions, TileIDs.Select(id => entries.First(tb => tb.Value == id).Key).ToArray());
        }
        
        public void Place(Vector3Int pos, TileBase tile)
        {
            if (Tiles.ContainsKey(pos)) Remove(pos);
            Tiles.Add(pos, tile);
        }

        public void Place(Vector3Int[] poses, TileBase[] tiles)
        {
            for (var i = 0; i < poses.Length; i++)
            {
                Place(poses[i], tiles[i]);
            }
        }

        public void Remove(Vector3Int pos)
        {
            Tiles.Remove(pos);
        }
        
        public void Remove(IEnumerable<Vector3Int> poses)
        {
            foreach (var pos in poses)
            {
                Remove(pos);
            }
        }
    }
}