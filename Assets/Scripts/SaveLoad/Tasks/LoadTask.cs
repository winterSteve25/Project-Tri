using System;
using System.IO;
using System.Threading.Tasks;

namespace SaveLoad.Tasks
{
    public class LoadTask : IAsyncDisposable, IDisposable
    {
        private MemoryStream _memoryStream;
        private bool _decompressed;
        
        public LoadTask(MemoryStream dataBuffer)
        {
            _memoryStream = dataBuffer;
        }

        public LoadTask(byte[] data) : this(new MemoryStream(data))
        {
        }

        public async Task<T> Deserialize<T>()
        {
            if (!_decompressed) Decompress();
            var serializer = SaveUtilities.GetSerializer<T>();
            return await serializer.UnpackAsync(_memoryStream);
        }

        private void Decompress()
        {
            // did not compress data just proceed
            if (_memoryStream.ReadByte() == 1)
            {
                _decompressed = true;
                return;
            }
            
            var data = _memoryStream.ToArray();
            _memoryStream = new MemoryStream(DataUtilities.Decompress(data[1..]));
            _decompressed = true;
        }

        public static async Task<LoadTask> ReadFromFile(FileLocation fileLocation)
        {
            var data = await SaveUtilities.LoadBytes(fileLocation);
            return new LoadTask(data);
        }

        public async ValueTask DisposeAsync()
        {
            await _memoryStream.DisposeAsync();
        }

        public void Dispose()
        {
            _memoryStream?.Dispose();
        }
    }
}