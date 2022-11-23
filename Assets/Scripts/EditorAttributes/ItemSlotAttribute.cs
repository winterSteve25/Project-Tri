using System;

namespace EditorAttributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ItemSlotAttribute : Attribute
    {
        public int Width;
        public int Height;

        public ItemSlotAttribute(int width = 100, int height = 100)
        {
            Width = width;
            Height = height;
        }
    }
}