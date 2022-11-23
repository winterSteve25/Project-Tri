using Registries;
using UnityEditor;

namespace Editor.Dictionaries
{
    [CustomPropertyDrawer(typeof(TilesRegistry.TilesRegistryDictionary))]
    public sealed class TilesRegistryDictionaryPD : SerializableDictionaryPropertyDrawer
    {
    }
}