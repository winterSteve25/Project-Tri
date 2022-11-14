using UnityEngine;
using Utils;

namespace World.Nodes.Utils
{
    [CreateNodeMenu("Utils/Noise Texture Generator")]
    public class OutputNoiseTexture : TerrainGenerator
    {
        [SerializeField] private string nameOfTexture;
        
        protected override void Init()
        {
            AddNoiseInput();
        }

        public override void Generate(int seed, int width, int height, int xOffset, int yOffset, TilemapManager tilemapManager)
        {
            var noise = GetNoiseInput();
            NoiseHelper.SaveNoiseTextureToLocal(noise.GetLength(0), noise.GetLength(1), noise, nameOfTexture);
        }

        public override string StageMessage()
        {
            return $"Generating {nameOfTexture} texture`...";
        }

        public override bool IsDebugNode()
        {
            return true;
        }
    }
}