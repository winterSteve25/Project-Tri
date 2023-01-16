using Liquid;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;

namespace Editor.Registries
{
    public class LiquidTableEntry : BaseTableEntry<TriLiquid>
    {
        [ShowInInspector]
        public LocalizedString Name
        {
            get => Object.liquidName;
            set
            {
                Object.liquidName = value;
                EditorUtility.SetDirty(Object);
            }
        }

        [ShowInInspector]
        public Color Color
        {
            get => Object.color;
            set
            {
                Object.color = value;
                EditorUtility.SetDirty(Object);
            }
        }
        
        public LiquidTableEntry(TriLiquid o) : base(o)
        {
        }
    }
}