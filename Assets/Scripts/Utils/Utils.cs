using UnityEngine;

namespace Utils
{
    public static class Utils
    {
        public static Texture2D ColoredTexture(int width, int height, Color color)
        {
            var texture = new Texture2D(width, height);

            var pixels = new Color[width * height];
            for (var i = 0; i < pixels.Length; i++)
            {
                pixels[i] = color;
            }

            texture.SetPixels(pixels);
            texture.Apply();
            return texture;
        }
    }
}