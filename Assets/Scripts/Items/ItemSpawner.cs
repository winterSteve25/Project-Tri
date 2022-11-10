using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;
using Utils;
using Random = UnityEngine.Random;

namespace Items
{
    public class ItemSpawner : Singleton<ItemSpawner>
    {
        
        [SerializeField] private GameObject groundItemPrefab;
        private ObjectPool<ItemBehaviour> _pool;

        private void Start()
        {
            _pool = new ObjectPool<ItemBehaviour>(
                () => Instantiate(groundItemPrefab, transform).GetComponent<ItemBehaviour>(),
                ib => ib.gameObject.SetActive(true),
                ib => ib.gameObject.SetActive(false),
                ib => Destroy(ib.gameObject),
                defaultCapacity: 16,
                maxSize: 64
            );
        }

        public ItemBehaviour Spawn(Vector2 pos, ItemStack item)
        {
            if (item.count <= 0) return null;
            var itemBehaviour = _pool.Get();
            itemBehaviour.transform.position = pos;
            itemBehaviour.Init(() => Despawn(itemBehaviour), item);
            return itemBehaviour;
        }

        public void SpawnApproximatelyAt(Vector2 pos, ItemStack item)
        {
            if (item.count <= 0) return;
            var i = Spawn(pos, item);
            i.transform.DOMove(i.transform.position + (Vector3)Random.insideUnitCircle, 0.5f)
                .SetEase(Ease.OutCubic);
        }

        private void Despawn(ItemBehaviour itemBehaviour)
        {
            _pool.Release(itemBehaviour);
        }
    }
}