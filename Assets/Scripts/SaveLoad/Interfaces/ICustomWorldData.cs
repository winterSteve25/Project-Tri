using System.Threading.Tasks;

namespace SaveLoad.Interfaces
{
    public interface ICustomWorldData
    {
        SerializationPriority Priority { get; }
        
        Task Save();

        Task Read(FileLocation worldFolder);
    }
}