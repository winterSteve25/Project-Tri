using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SaveLoad.Tasks
{
    public class SaveTask
    {
        private readonly MemoryStream _memoryStream;

        public SaveTask()
        {
            _memoryStream = new MemoryStream();
        }

        public async Task Serialize<T>(T obj)
        {
            var serializer = SaveUtilities.GetSerializer<T>();
            await serializer.PackAsync(_memoryStream, obj);
        }

        public byte[] CompressAndBuild()
        {
            using var memStream = new MemoryStream();
            var data = _memoryStream.ToArray();
            var compressedData = false;

            // if the data is already quite small no need to compress it
            if (data.Length > 1024)
            {
                data = DataUtilities.Compress(data);
                compressedData = true;
            }

            var output = new byte[1];
            output[0] = compressedData ? (byte)0 : (byte)1;
            return output.Concat(data).ToArray();
        }

        public async Task WriteToFile(FileLocation fileLocation)
        {
            await SaveUtilities.SaveBytes(fileLocation, CompressAndBuild());
            await _memoryStream.DisposeAsync();
        }
    }
}