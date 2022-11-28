using System;
using System.Collections.Generic;
using MsgPack.Serialization;
using SaveLoad;

namespace Utils.Data
{
    /// <summary>
    /// Saves everything as bytes, modification of data is inefficient and should be minimized. Only use this if data NEEDS to be saved. If it does not need to be saved, use DataStorage.
    /// </summary>
    public class SerializableDataStorage
    {
        private readonly Dictionary<string, byte[]> _data;
        public Dictionary<string, byte[]> Data => _data;

        public event Action<string, object> OnValueChanged;
        public event Action<string> OnValueRemoved; 

        public SerializableDataStorage() : this(new Dictionary<string, byte[]>())
        {
        }

        [MessagePackDeserializationConstructor]
        public SerializableDataStorage(Dictionary<string, byte[]> data)
        {
            _data = data;
        }

        private void Set<T>(string key, T value)
        {
            var obj = SaveUtilities.GetSerializer<T>().PackSingleObject(value);

            OnValueChanged?.Invoke(key, value);
            
            if (_data.ContainsKey(key))
            {
                _data[key] = obj;
                return;
            }
            
            _data.Add(key, obj);
        }
        
        public void Set<T>(DataSignature<T> signature, T value)
        {
            Set(signature.Key, value);
        }

        private T Read<T>(string key)
        {
            return SaveUtilities.GetSerializer<T>().UnpackSingleObject(_data[key]);
        }

        public T Read<T>(DataSignature<T> signature)
        {
            return Read<T>(signature.Key);
        }

        private void ComputeIfPresent<T>(string key, Action<T> action)
        {
            if (_data.ContainsKey(key))
            {
                action(Read<T>(key));
            }
        }
        
        public void ComputeIfPresent<T>(DataSignature<T> signature, Action<T> action)
        {
            ComputeIfPresent(signature.Key, action);
        }

        private bool HasKey(string key)
        {
            return _data.ContainsKey(key);
        }

        public bool HasKey<T>(DataSignature<T> signature)
        {
            return HasKey(signature.Key);
        }

        private bool ExistAnd<T>(string key, Predicate<T> condition)
        {
            return HasKey(key) && condition(Read<T>(key));
        }

        public bool ExistAnd<T>(DataSignature<T> key, Predicate<T> condition)
        {
            return ExistAnd(key.Key, condition);
        }

        private bool NotExistOr<T>(string key, Predicate<T> condition)
        {
            return !HasKey(key) || condition(Read<T>(key));
        }
        
        public bool NotExistOr<T>(DataSignature<T> key, Predicate<T> condition)
        {
            return NotExistOr(key.Key, condition);
        }
        
        private void Remove(string key)
        {
            OnValueRemoved?.Invoke(key);
            _data.Remove(key);
        }

        public void Remove<T>(DataSignature<T> signature)
        {
            Remove(signature.Key);
        }

        private T GetOrDefault<T>(string key, T def)
        {
            if (HasKey(key))
            {
                return Read<T>(key);
            }

            return def;
        }
        
        public T GetOrDefault<T>(DataSignature<T> signature, T def)
        {
            return GetOrDefault(signature.Key, def);
        }
    }
}