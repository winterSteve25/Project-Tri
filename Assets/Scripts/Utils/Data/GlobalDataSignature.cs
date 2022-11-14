namespace Utils.Data
{
    public readonly struct GlobalDataSignature<T>
    {
        public readonly string Key;

        public GlobalDataSignature(string key)
        {
            Key = key;
        }
    }
}