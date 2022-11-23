using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Registries
{
    [CreateAssetMenu(fileName = "Tiles Registry", menuName = "Registries/New Tiles Registry")]
    public class TilesRegistry : Registry<TilesRegistry.TilesRegistryDictionary, TileBase>
    {
        private static TilesRegistry _instance;
        public static TilesRegistry Instance => _instance ??= Resources.Load<TilesRegistry>("Registries/REG_Tiles Registry");

        [Serializable]
        public class TilesRegistryDictionary : SerializableDictionary<TileBase, string>
        {
        }
    }
}