using System.Linq;
using MsgPack.Serialization;
using Registries;
using Tiles;
using Utils.Data;

namespace World.Tiles
{
    public class TileInstance
    {
        [MessagePackIgnore]
        public readonly TriTile Tile;
        [MessagePackIgnore] 
        public readonly DataStorage CustomData;
        [MessagePackIgnore] 
        public float BreakProgress;

        public string TileID => TilesRegistry.Instance.Entries[Tile];
        public readonly SerializableDataStorage SavedData;

        public TileInstance(TriTile tile)
        {
            Tile = tile;
            SavedData = new SerializableDataStorage();
            CustomData = new DataStorage();
        }

        [MessagePackDeserializationConstructor]
        public TileInstance(string tileId, SerializableDataStorage savedData)
        {
            Tile = TilesRegistry.Instance.Entries.First(tb => tb.Value == tileId).Key;
            SavedData = savedData;
            CustomData = new DataStorage();
        }

        public static implicit operator TileInstance(TriTile tile)
        {
            return new TileInstance(tile);
        }
    }
}