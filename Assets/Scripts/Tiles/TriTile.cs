using UnityEngine;
using UnityEngine.Tilemaps;

namespace Tiles
{
    public class TriTile : ScriptableObject
    {
        public TileBase tileBase;
        public Color color;
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