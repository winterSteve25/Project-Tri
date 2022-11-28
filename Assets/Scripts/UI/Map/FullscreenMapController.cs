using System.Linq;
using Sirenix.OdinInspector;
using UI.Utilities;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Utils.Data;
using World.Tiles;

namespace UI.Map
{
    public class FullscreenMapController : MonoBehaviour
    {
        [SerializeField, Required] private Image textureImage;
        [SerializeField, Required] private GameObject background;
        
        private LayeredChunk _world;

        private void Start()
        {
            _world = TilemapManager.Current.Chunk;
            background.SetActive(false);
        }

        private void Update()
        {
            if (GameInput.KeyboardKeyDown(KeyCode.M))
            {
                ToggleMap();
            }
        }

        private void ToggleMap()
        {
            if (!background.gameObject.activeSelf)
            {
                // enable map

                // generate map texture
                var worldSettings = GlobalData.Read(GlobalDataKeys.CurrentWorldSettings);
                
                var w = worldSettings.Width;
                var h = worldSettings.Height;
                var xOff = -w / 2;
                var yOff = -h / 2;
                
                var texture = new Texture2D(w, h);
                var colors = new Color[w][];

                // loop through all the ground tiles and grab their color
                for (var x = 0; x < w; x++)
                {
                    var column = colors[x] = new Color[h];
                    
                    for (var y = 0; y < h; y++)
                    {
                        var tile = _world.GroundChunk.GetTile(new Vector3Int(x + xOff, y + yOff));
                        column[y] = tile == null ? Color.black : tile.Tile.color;
                    }
                }
                
                // loop through all the obstacle tiles and if there is non we skip as this overrides the ground ones.
                for (var x = 0; x < w; x++)
                {
                    var column = colors[x];
                    
                    for (var y = 0; y < h; y++)
                    {
                        var tile = _world.ObstacleChunk.GetTile(new Vector3Int(x + xOff, y + yOff));
                        if (tile == null) continue;
                        column[y] = tile.Tile.color;
                    }
                }
                
                // applies color to texture
                texture.SetPixels(colors.SelectMany(x => x).ToArray());
                texture.Apply();
                ResizeTool.Resize(texture, 384, 384, filter: FilterMode.Point);

                // sets image to texture
                textureImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                textureImage.GetComponent<RectTransform>().sizeDelta = new Vector2(texture.width, texture.height);

                background.gameObject.SetActive(true);
            }
            else
            {
                // disable map
                // disable map camera and enable the normal camera
                background.gameObject.SetActive(false);
            }
        }
    }
}