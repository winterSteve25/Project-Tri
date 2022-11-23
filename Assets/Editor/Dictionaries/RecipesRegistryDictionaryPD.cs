using Registries;
using UnityEditor;

namespace Editor.Dictionaries
{
    [CustomPropertyDrawer(typeof(RecipesRegistry.RecipesRegistryDictionary))]
    public sealed class RecipesRegistryDictionaryPD : SerializableDictionaryPropertyDrawer
    {
    }
}