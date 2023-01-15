using MsgPack.Serialization;
using UnityEngine;

namespace World.Generation
{
    public class WorldSettings
    {
        public static readonly WorldSettings Default = new("Test World", 0, 256, 256);
        
        public readonly string WorldName;
        public readonly int Seed;
        public readonly int Width;
        public readonly int Height;
        public Vector2 SpawnPoint;

        public WorldSettings(string worldName, int seed, int width, int height)
        {
            Seed = seed;
            Width = width;
            Height = height;
            WorldName = worldName;
            SpawnPoint = Vector2.zero;
        }

        [MessagePackDeserializationConstructor]
        public WorldSettings(string worldName, int seed, int width, int height, Vector2 spawnPoint)
        {
            WorldName = worldName;
            Seed = seed;
            Width = width;
            Height = height;
            SpawnPoint = spawnPoint;
        }

        public override string ToString()
        {
            return $"{WorldName}, Seed: {Seed}, Dimensions: {Width}x{Height}, Spawn Point; {SpawnPoint}";
        }
    }
}