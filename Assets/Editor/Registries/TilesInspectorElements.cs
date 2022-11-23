using System.Linq;
using Items.ItemTypes;
using PlaceableTiles;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Editor.Registries
{
    public sealed class TileTableEntry : BaseTableEntry<TileBase>
    {
        public TileTableEntry(TileBase o) : base(o)
        {
        }

        [TableColumnWidth(50, false)]
        [ShowInInspector, PreviewField(50, ObjectFieldAlignment.Center)]
        public Sprite Sprite
        {
            get
            {
                return Object switch
                {
                    Tile tile => tile.sprite,
                    RuleTile ruleTile => ruleTile.m_DefaultSprite,
                    _ => null
                };
            }
            set
            {
                switch (Object)
                {
                    case Tile tile:
                        tile.sprite = value;
                        break;
                    case RuleTile ruleTile:
                        ruleTile.m_DefaultSprite = value;
                        break;
                }

                EditorUtility.SetDirty(Object);
            }
        }

        [ShowInInspector, ReadOnly] public string Name => Object.name;

        [ShowInInspector, AssetsOnly]
        public GameObject Prefab
        {
            get
            {
                return Object switch
                {
                    Tile tile => tile.gameObject,
                    RuleTile ruleTile => ruleTile.m_DefaultGameObject,
                    _ => null
                };
            }
            set
            {
                switch (Object)
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

        private PlaceableTile _placeableTile;

        [ShowInInspector, EnableIf("@_placeableTile != null")]
        [HorizontalGroup("Placeable Tile")]
        [HideLabel]
        public PlaceableTile PlaceableTile
        {
            get
            {
                if (_placeableTile != null) return _placeableTile;
                
                var p = Resources
                    .FindObjectsOfTypeAll<PlaceableTile>()
                    .FirstOrDefault(t => t.tileBase == Object);

                if (p != null)
                {
                    _placeableTile = p;
                }

                return p;
            }
            set
            {
                _placeableTile = value;
                if (_placeableTile == null) return;
                _placeableTile.tileBase = Object;
                EditorUtility.SetDirty(_placeableTile);
            }
        }

        [HorizontalGroup("Placeable Tile")]
        [Button("+"), EnableIf("@_placeableTile == null")]
        [PropertyTooltip("Makes a placeable tile and placeable item asset for this tile")]
        private void MakePlaceable()
        {
            ScriptableObjectCreator.ShowDialog<PlaceableTile>("Assets/Resources/Tiles/", x =>
            {
                x.tileBase = Object;
                EditorUtility.SetDirty(x);
                
                ScriptableObjectCreator.ShowDialog<PlaceableItem>("Assets/Resources/Items/", i =>
                {
                    i.placeableTile = x;
                    EditorUtility.SetDirty(i);
                });
            });
        }
    }
}