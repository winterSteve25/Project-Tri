using System;

namespace World.Generation
{
    [Serializable]
    public struct WorldGenConfiguration
    {
        public int xOffset;
        public int yOffset;
        public int width;
        public int height;

        public WorldGenConfiguration(int xOffset, int yOffset, int width, int height)
        {
            this.xOffset = xOffset;
            this.yOffset = yOffset;
            this.width = width;
            this.height = height;
        }
    }
}