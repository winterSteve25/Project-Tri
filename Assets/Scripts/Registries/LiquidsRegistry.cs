using System;
using Liquid;
using UnityEngine;

namespace Registries
{
    [CreateAssetMenu(fileName = "REG_Liquids Registry", menuName = "Registries/New Liquids Registry")]
    public class LiquidsRegistry : Registry<LiquidsRegistry.LiquidRegistryDictionary, TriLiquid>
    {
        private static LiquidsRegistry _instance;
        public static LiquidsRegistry Instance => _instance ??= Resources.Load<LiquidsRegistry>("Registries/REG_Liquids Registry");

        [Serializable]
        public class LiquidRegistryDictionary : SerializableDictionary<TriLiquid, string>
        {
        }
    }
}