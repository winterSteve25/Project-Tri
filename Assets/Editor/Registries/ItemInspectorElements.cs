using Items;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;

namespace Editor.Registries
{
    public sealed class ItemTableEntry : BaseTableEntry<Item>
    {
        [TableColumnWidth(50, false)]
        [ShowInInspector, PreviewField(50, ObjectFieldAlignment.Center)]
        public Sprite Sprite
        {
            get => Object.sprite;
            set
            {
                Object.sprite = value;
                EditorUtility.SetDirty(Object);
            }
        }

        [ShowInInspector]
        public LocalizedString Name
        {
            get => Object.itemName;
            set
            {
                Object.itemName = value;
                EditorUtility.SetDirty(Object);
            }
        }

        [ShowInInspector]
        public LocalizedString Description
        {
            get => Object.itemDescription;
            set
            {
                Object.itemDescription = value;
                EditorUtility.SetDirty(Object);
            }
        }

        [ShowInInspector]
        public int MaxObjectStacks
        {
            get => Object.maxStackSize;
            set
            {
                Object.maxStackSize = value;
                EditorUtility.SetDirty(Object);
            }
        }

        public ItemTableEntry(Item o) : base(o)
        {
        }
    }
}