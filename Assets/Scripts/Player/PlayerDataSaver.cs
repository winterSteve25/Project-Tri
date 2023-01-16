using System.Threading.Tasks;
using Items;
using SaveLoad;
using SaveLoad.Interfaces;
using SaveLoad.Tasks;
using UnityEngine;
using Utils.Data;

namespace Player
{
    public class PlayerDataSaver : MonoBehaviour, ICustomWorldData
    {
        [SerializeField] private PlayerInventory inv;
        
        public SerializationPriority Priority => SerializationPriority.Low;

        public async Task Save()
        {
            var location = FileLocation.CurrentWorldSave(FileNameConstants.PlayerData);
            var saveTask = new SaveTask();
            await inv.Inv.Serialize(saveTask);
            await inv.Equipments.Serialize(saveTask);
            var transform1 = transform;
            await saveTask.Serialize((Vector2) transform1.position);
            await saveTask.Serialize(transform1.rotation);
            await saveTask.WriteToFile(location);
        }

        public async Task Read(FileLocation worldFolder)
        {
            var location = FileLocation.CurrentWorldSave(FileNameConstants.PlayerData);
            var loadTask = await LoadTask.ReadFromFile(location);
            await inv.Inv.Deserialize(loadTask);
            await inv.Equipments.Deserialize(loadTask);
            var pos = await loadTask.Deserialize<Vector2>();
            var rotation = await loadTask.Deserialize<Quaternion>();
            transform.SetPositionAndRotation(pos, rotation);
            await loadTask.DisposeAsync();
        }
    }
}