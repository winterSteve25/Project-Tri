using System;
using System.Linq;
using MsgPack.Serialization;
using Registries;
using Sirenix.OdinInspector;

namespace Liquid
{
    [Serializable]
    public struct LiquidStack
    {
        public string LiquidId => liquid == null ? string.Empty : LiquidsRegistry.Instance.Entries[liquid];
        public float volume;

        [MessagePackIgnore] 
        [AssetSelector]
        public TriLiquid liquid;

        [MessagePackIgnore]
        public bool IsEmpty => liquid == null || volume <= 0;

        public LiquidStack(TriLiquid liquid, float volume)
        {
            this.liquid = liquid;
            this.volume = volume;
        }
        
        [MessagePackDeserializationConstructor]
        public LiquidStack(string liquidId, int volume)
        {
            liquid = liquidId == string.Empty ? null : LiquidsRegistry.Instance.Entries.First(i => i.Value == liquidId).Key;
            this.volume = volume;
        }

        public static LiquidStack operator+(LiquidStack a, LiquidStack b)
        {
            if (a.liquid != b.liquid)
            {
                throw new ArgumentException("Can not add 2 liquid stacks with different liquids");
            }

            return new LiquidStack(a.liquid, a.volume + b.volume);
        }

        public static LiquidStack operator -(LiquidStack a, LiquidStack b)
        {
            if (a.liquid != b.liquid)
            {
                throw new ArgumentException("Can not subtract 2 liquid stacks with different liquids");
            }

            return new LiquidStack(a.liquid, a.volume - b.volume);
        }

        public static bool operator==(LiquidStack a, LiquidStack b)
        {
            return a.liquid == b.liquid && Math.Abs(a.volume - b.volume) < 0.001;
        }

        public static bool operator !=(LiquidStack a, LiquidStack b)
        {
            return !(a == b);
        }
        
        public bool Equals(LiquidStack other)
        {
            return volume.Equals(other.volume) && Equals(liquid, other.liquid);
        }

        public override bool Equals(object obj)
        {
            return obj is LiquidStack other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(volume, liquid);
        }
    }
}