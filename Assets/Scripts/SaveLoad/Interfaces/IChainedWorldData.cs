using System.Threading.Tasks;
using SaveLoad.Tasks;

namespace SaveLoad.Interfaces
{
    public interface IChainedWorldData
    {
        Task Save(SaveTask saveTask);

        Task Load(LoadTask loadTask);
    }
}