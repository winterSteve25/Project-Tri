namespace World
{
    public readonly struct WorldSettings
    {
        public readonly int Seed;
        public readonly int Width;
        public readonly int Height;

        public WorldSettings(int seed, int width, int height)
        {
            Seed = seed;
            Width = width;
            Height = height;
        }
    }
}