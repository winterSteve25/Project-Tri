using System.Collections.Generic;
using System.Threading.Tasks;
using SaveLoad;
using SaveLoad.Interfaces;
using UnityEngine;
using Utils.Data;

namespace Items
{
    public class GroundItems : MonoBehaviour, ICustomWorldData
    {
        public SerializationPriority Priority => SerializationPriority.Low;

        public async Task Save()
        {
            var itemstacks = new List<GroundItemStack>();
            
            foreach (Transform child in transform)
            {
                if (!child.gameObject.activeSelf) continue;
                var item = child.GetComponent<GroundItemBehaviour>();
                itemstacks.Add(new GroundItemStack(item.Item, item.transform.position, item.DespawnTime));
            }

            var location = FileLocation.CurrentWorldSave(FileNameConstants.GroundItems);
            await SaveUtilities.Save(location, itemstacks);
        }

        public async Task Read(FileLocation worldFolder)
        {
            var itemstacks = await SaveUtilities.Load(worldFolder.FileAtLocation(FileNameConstants.GroundItems), new List<GroundItemStack>());
            
            foreach (var item in itemstacks)
            {
                ItemSpawner.Current.Spawn(item.Position, item.Item, item.DespawnTimer);
            }
        }
    }
}