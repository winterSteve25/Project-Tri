using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Registries
{
    /// <summary>
    /// Holds all the objects of this type 
    /// </summary>
    /// <typeparam name="TDictionary">Serialized Dictionary Type</typeparam>
    /// <typeparam name="T">Object type that this registry stores</typeparam>
    public abstract class Registry<TDictionary, T> : ScriptableObject where TDictionary : SerializableDictionary<T, string> where T : Object
    {
        [SerializeField] protected TDictionary entries;
        public TDictionary Entries => entries;

#if UNITY_EDITOR
        [Button(ButtonSizes.Medium)]
        public void UpdateEntries()
        {
            var objects = Resources.FindObjectsOfTypeAll<T>();
            entries.Clear();
            foreach (var obj in objects)
            {
                entries.Add(obj, obj.name.Replace(" ", "_")[2..]);
            }

            EditorUtility.SetDirty(this);
        }
#endif
    }
}