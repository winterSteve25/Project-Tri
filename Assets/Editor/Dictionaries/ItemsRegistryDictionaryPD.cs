using Registries;
using UnityEditor;

namespace Editor.Dictionaries
{
    [CustomPropertyDrawer(typeof(ItemsRegistry.ItemsRegistryDictionary))]
    public sealed class ItemsRegistryDictionaryPD : SerializableDictionaryPropertyDrawer
    {
    }
}