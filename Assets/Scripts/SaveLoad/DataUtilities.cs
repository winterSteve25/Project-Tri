using System;
using System.IO;
using System.IO.Compression;

namespace SaveLoad
{
    /// <summary>
    /// Provides byte compression utilities
    /// </summary>
    public static class DataUtilities
    {
        public static byte[] Compress(byte[] data)
        {
            byte[] compressArray = null;
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    using (var deflateStream = new DeflateStream(memoryStream, CompressionMode.Compress))
                    {
                        deflateStream.Write(data, 0, data.Length);
                    }

                    compressArray = memoryStream.ToArray();
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return compressArray;
        }

        public static byte[] Decompress(byte[] data)
        {
            byte[] decompressedArray = null;
            try
            {
                using (var decompressedStream = new MemoryStream())
                {
                    using (var compressStream = new MemoryStream(data))
                    {
                        using (var deflateStream = new DeflateStream(compressStream, CompressionMode.Decompress))
                        {
                            deflateStream.CopyTo(decompressedStream);
                        }
                    }
                    decompressedArray = decompressedStream.ToArray();
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return decompressedArray;
        }
    }
}