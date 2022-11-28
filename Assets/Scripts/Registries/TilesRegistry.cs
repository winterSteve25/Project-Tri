using System;
using Tiles;
using UnityEngine;

namespace Registries
{
    [CreateAssetMenu(fileName = "Tiles Registry", menuName = "Registries/New Tiles Registry")]
    public class TilesRegistry : Registry<TilesRegistry.TilesRegistryDictionary, TriTile>
    {
        private static TilesRegistry _instance;
        public static TilesRegistry Instance => _instance ??= Resources.Load<TilesRegistry>("Registries/REG_Tiles Registry");

        [Serializable]
        public class TilesRegistryDictionary : SerializableDictionary<TriTile, string>
        {
        }
    }
}