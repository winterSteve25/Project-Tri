using System;
using System.Collections.Generic;

namespace Utils.Data
{
    public static class GlobalData
    {
        private static readonly Dictionary<string, object> Data = new();

        private static void Set(string key, object value)
        {
            if (Data.ContainsKey(key))
            {
                Data[key] = value;
                return;
            }
            Data.Add(key, value);
        }

        public static void Set<T>(GlobalDataSignature<T> signature, T value)
        {
            Set(signature.Key, value);
        }
        
        private static T Read<T>(string key)
        {
            return (T)Data[key];
        }

        public static T Read<T>(GlobalDataSignature<T> signature)
        {
            return Read<T>(signature.Key);
        }

        private static void ComputeIfPresent<T>(string key, Action<T> action)
        {
            if (Data.ContainsKey(key))
            {
                action((T) Data[key]);
            }
        }
        
        private static void ComputeIfPresent<T>(GlobalDataSignature<T> signature, Action<T> action)
        {
            ComputeIfPresent(signature.Key, action);
        }

        public static bool HasKey(string key)
        {
            return Data.ContainsKey(key);
        }

        public static bool HasKey<T>(GlobalDataSignature<T> signature)
        {
            return HasKey(signature.Key);
        }

        public static bool ExistAnd<T>(string key, Predicate<T> condition)
        {
            return HasKey(key) && condition(Read<T>(key));
        }
        
        public static void Remove(string key)
        {
            Data.Remove(key);
        }

        public static void Remove<T>(GlobalDataSignature<T> signature)
        {
            Remove(signature.Key);
        }

        private static T GetOrDefault<T>(string key, T def)
        {
            if (HasKey(key))
            {
                return (T)Data[key];
            }

            return def;
        }
        
        public static T GetOrDefault<T>(GlobalDataSignature<T> signature, T def)
        {
            return GetOrDefault(signature.Key, def);
        }
    }
}