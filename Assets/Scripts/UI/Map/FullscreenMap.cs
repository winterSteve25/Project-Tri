using Tiles;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using World;

namespace UI.Map
{
    public class FullscreenMap : MonoBehaviour
    {
        [SerializeField] private Image mapImage;

        private int _width = 128;
        private int _height = 128;

        private TilemapManager _tilemapManager;
        private Tilemap _groundLayer;
        private Tilemap _obstacleLayer;

        private void Start()
        {
            _tilemapManager = FindObjectOfType<TilemapManager>();
            _groundLayer = _tilemapManager.GroundLayer;
            _obstacleLayer = _tilemapManager.ObstacleLayer;
        }

        public void GenerateMap()
        {
            var texture = new Texture2D(_width, _height);
            var colors = new Color[_width * _height];
            
            for (var i = 0; i < _width; i++)
            {
                for (var j = 0; j < _height; j++)
                {
                    var pos = new Vector3Int(i - 64, j - 64);
                    var color = Color.white;
                    var go = _tilemapManager.GetGameObject(_groundLayer.GetTile(pos), pos, _groundLayer);
                    if (go != null && go.TryGetComponent<MapTile>(out var t))
                    {
                        color = t.color;
                    }
                    
                    var go2 = _tilemapManager.GetGameObject(_obstacleLayer.GetTile(pos), pos, _obstacleLayer);
                    if (go2 != null && go.TryGetComponent<MapTile>(out var t2))
                    {
                        color = t2.color;
                    }
                    
                    colors[j * _width + i] = color;
                }
            }
            
            texture.SetPixels(colors);
            texture.Apply();
            
            mapImage.sprite = Sprite.Create(texture, new Rect(0, 0, _width, _height), Vector2.zero);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                mapImage.enabled = !mapImage.enabled;
                GenerateMap();
            }
        }
    }
}