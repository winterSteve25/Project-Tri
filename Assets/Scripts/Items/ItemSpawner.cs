using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace Items
{
    /// <summary>
    /// Manages item spawning
    /// </summary>
    public class ItemSpawner : MonoBehaviour
    {
        public static ItemSpawner Current { get; private set; }
        
        [SerializeField, AssetsOnly] private GroundItemBehaviour groundGroundItemPrefab;
        [SerializeField] private Transform groundItemsParent;
        
        private ObjectPool<GroundItemBehaviour> _pool;

        private void Awake()
        {
            Current = this;
        }

        private void Start()
        {
            _pool = new ObjectPool<GroundItemBehaviour>(
                () => Instantiate(groundGroundItemPrefab, groundItemsParent),
                ib => ib.gameObject.SetActive(true),
                ib => ib.gameObject.SetActive(false),
                ib => Destroy(ib.gameObject),
                defaultCapacity: 16,
                maxSize: 64
            );
        }
        
        public GroundItemBehaviour Spawn(Vector2 pos, ItemStack item, float despawnTime = -1)
        {
            if (item.count <= 0) return null;
            var itemBehaviour = _pool.Get();
            itemBehaviour.transform.position = pos;
            itemBehaviour.Init(() => Despawn(itemBehaviour), item, despawnTime);
            return itemBehaviour;
        }

        public void SpawnApproximatelyAt(Vector2 pos, ItemStack item)
        {
            if (item.count <= 0) return;
            var i = Spawn(pos, item);
            i.transform.DOMove(i.transform.position + (Vector3)Random.insideUnitCircle, 0.5f)
                .SetEase(Ease.OutCubic);
        }

        private void Despawn(GroundItemBehaviour groundItemBehaviour)
        {
            _pool.Release(groundItemBehaviour);
        }
    }
}