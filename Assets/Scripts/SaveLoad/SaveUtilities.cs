using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MsgPack.Serialization;
using SaveLoad.Interfaces;
using Utils.Data;
using World.Generation;

namespace SaveLoad
{
    /// <summary>
    /// (De)serializes and saves/loads data
    /// </summary>
    public static class SaveUtilities
    {
        /// <summary>
        /// Serializes an object into compressed bytes
        /// </summary>
        /// <param name="obj">Object to be serialized</param>
        /// <typeparam name="T">Type of the object</typeparam>
        /// <returns>Data in bytes</returns>
        public static async Task<byte[]> SerializeCompress<T>(T obj)
        {
            using var memoryStream = new MemoryStream();
            
            var serializer = GetSerializer<T>();
            var data = await serializer.PackSingleObjectAsync(obj);
            var compressedData = false;
            
            // if the data is already quite small no need to compress it
            if (data.Length > 1024)
            {
                data = DataUtilities.Compress(data);
                compressedData = true;
            }
            
            // record if we compressed the data
            memoryStream.WriteByte(compressedData ? (byte)0 : (byte)1);
            await memoryStream.WriteAsync(data, 0, data.Length);
            return memoryStream.ToArray();
        }

        /// <summary>
        /// Deserializes a compressed buffer of bytes into an object
        /// </summary>
        /// <param name="data">Byte data</param>
        /// <typeparam name="T">Type of the object to deserialize into</typeparam>
        /// <returns>Deserialized object</returns>
        public static async Task<T> DeserializeCompress<T>(byte[] data)
        {
            var serializer = GetSerializer<T>();

            // is data compressed?
            if (data[0] == 0)
            {
                // if so we decompress it
                data = DataUtilities.Decompress(data[1..]);
                return await serializer.UnpackSingleObjectAsync(data);
            }
            
            return await serializer.UnpackSingleObjectAsync(data[1..]);
        }
        
        /// <summary>
        /// Saves bytes into file
        /// </summary>
        /// <param name="location">Where to save the byte</param>
        /// <param name="data">The data in bytes</param>
        public static async Task SaveBytes(FileLocation location, byte[] data)
        {
            var worldSettings = GetWorldSettings();
            var path = location.GetPath(worldSettings);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            await using var fs = File.Create(location.GetFullPath(worldSettings));
            await fs.WriteAsync(data, 0, data.Length);
        }

        /// <summary>
        /// Load bytes into memory from a file
        /// </summary>
        /// <param name="fileLocation">The location of the file</param>
        /// <returns>Data in bytes</returns>
        public static async Task<byte[]> LoadBytes(FileLocation fileLocation)
        {
            var worldSettings = GetWorldSettings();
            if (!fileLocation.Exists(worldSettings)) return null;
            return await File.ReadAllBytesAsync(fileLocation.GetFullPath(worldSettings));
        }

        /// <summary>
        /// Serializes object into binary and saves it into a file
        /// </summary>
        /// <param name="location">File location to save the binary to</param>
        /// <param name="obj">The object to serialize</param>
        /// <typeparam name="T">The type of the object</typeparam>
        public static async Task Save<T>(FileLocation location, T obj)
        {
            var data = await SerializeCompress(obj);
            await SaveBytes(location, data);
        }

        /// <summary>
        /// Loads the binary and deserializes it into an object
        /// </summary>
        /// <param name="fileLocation">The file location to load the binary from</param>
        /// <param name="def">The default value of the file could not be loaded</param>
        /// <typeparam name="T">Type of the object to deserialize into</typeparam>
        /// <returns>The deserialized value</returns>
        public static async Task<T> Load<T>(FileLocation fileLocation, T def)
        {
            var data = await LoadBytes(fileLocation);
            if (data == null) return def;
            return await DeserializeCompress<T>(data);
        }
        
        public static MessagePackSerializer<T> GetSerializer<T>()
        {
            return MessagePackSerializer.Get<T>();
        }
        
        public static MessagePackSerializer GetSerializer(Type type)
        {
            return MessagePackSerializer.Get(type);
        }

        public static WorldSettings GetWorldSettings()
        {
            if (!GlobalData.HasKey(GlobalDataKeys.CurrentWorldSettings)) return null;
            return GlobalData.Read(GlobalDataKeys.CurrentWorldSettings);
        }

        /// <summary>
        /// Runs the save function on all the CustomWorldData going from high to low priority
        /// </summary>
        /// <param name="data">Data</param>
        public static async Task Save(this List<ICustomWorldData> data)
        {
            var task = data.Where(d => d.Priority == SerializationPriority.High).Select(dataSaver => dataSaver.Save()).ToList();
            await Task.WhenAll(task);

            task = data.Where(d => d.Priority == SerializationPriority.Medium).Select(dataSaver => dataSaver.Save()).ToList();
            await Task.WhenAll(task);

            task = data.Where(d => d.Priority == SerializationPriority.Low).Select(dataSaver => dataSaver.Save()).ToList();
            await Task.WhenAll(task);
        }

        /// <summary>
        /// Runs the save function on all the CustomWorldData going from high to low priority
        /// </summary>
        /// <param name="data">Data</param>
        /// <param name="worldFolder">World folder</param>
        public static async Task Read(this List<ICustomWorldData> data, FileLocation worldFolder)
        {
            var task = data.Where(d => d.Priority == SerializationPriority.High).Select(dataReader => dataReader.Read(worldFolder)).ToList();
            await Task.WhenAll(task);

            task = data.Where(d => d.Priority == SerializationPriority.Medium).Select(dataReader => dataReader.Read(worldFolder)).ToList();
            await Task.WhenAll(task);

            task = data.Where(d => d.Priority == SerializationPriority.Low).Select(dataReader => dataReader.Read(worldFolder)).ToList();
            await Task.WhenAll(task);
        }
    }
}