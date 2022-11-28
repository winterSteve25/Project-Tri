using Sirenix.OdinInspector;
using Tiles;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Editor.Registries
{
    public sealed class TileTableEntry : BaseTableEntry<TriTile>
    {
        public TileTableEntry(TriTile o) : base(o)
        {
        }

        [TableColumnWidth(50, false)]
        [ShowInInspector, ReadOnly, PreviewField(50, ObjectFieldAlignment.Center)]
        public Sprite Sprite => Object.Sprite;

        [ShowInInspector, ReadOnly] public string Name => Object.name;

        [ShowInInspector, AssetsOnly]
        public GameObject Prefab
        {
            get
            {
                return Object.tileBase switch
                {
                    Tile tile => tile.gameObject,
                    RuleTile ruleTile => ruleTile.m_DefaultGameObject,
                    _ => null
                };
            }
            set
            {
                switch (Object.tileBase)
                {
                    case Tile tile:
                        tile.gameObject = value;
                        break;
                    case RuleTile ruleTile:
                        ruleTile.m_DefaultGameObject = value;
                        break;
                }

                EditorUtility.SetDirty(Object);
            }
        }
    }
}