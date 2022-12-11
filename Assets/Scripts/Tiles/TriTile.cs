using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Tiles
{
    public class TriTile : ScriptableObject
    {
        [PropertyTooltip("The tile asset used in tilemaps")]
        public TileBase tileBase;
        
        [PropertyTooltip("The color of the tile, will be used in the construction of maps")]
        public Color color;
        
        [PropertyTooltip("Hardness of the tile, determines the time (seconds) it takes to mine this tile. Set to 0 for instant break, Set to below 0 for unbreakable")]
        public float hardness;
        
        public Sprite Sprite
        {
            get
            {
                return tileBase switch
                {
                    RuleTile ruleTile when ruleTile.m_DefaultSprite != null => ruleTile.m_DefaultSprite,
                    RuleTile ruleTile => ruleTile.m_DefaultGameObject == null
                        ? null
                        :
                        ruleTile.m_DefaultGameObject.TryGetComponent<SpriteRenderer>(out var spriteRenderer)
                            ?
                            spriteRenderer.sprite
                            : null,
                    Tile tile when tile.sprite != null => tile.sprite,
                    Tile tile => tile.gameObject.TryGetComponent<SpriteRenderer>(out var spriteRenderer)
                        ? spriteRenderer.sprite
                        : null,
                    _ => null
                };
            }
        }
    }
}