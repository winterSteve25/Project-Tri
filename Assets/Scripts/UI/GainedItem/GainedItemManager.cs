using DG.Tweening;
using Items;
using Player;
using Sirenix.OdinInspector;
using Systems.Inv;
using UnityEngine;
using UnityEngine.Pool;
using Utils;

namespace UI.GainedItem
{
    public class GainedItemManager : MonoBehaviour
    {
        [SerializeField, AssetsOnly] private GainedItemEntry gainedItemEntryPrefab;
        [SerializeField] private Transform gainedItemsParent;

        private Inventory _inventory;
        private ObjectPool<GainedItemEntry> _itemEntries;

        private void Start()
        {
            _inventory = FindObjectOfType<PlayerInventory>().Inv;
            _inventory.OnItemChanged += Refresh;

            _itemEntries = new ObjectPool<GainedItemEntry>(
                () =>
                {
                    var obj = Instantiate(gainedItemEntryPrefab, gainedItemsParent);
                    obj.gameObject.SetActive(false);
                    obj.GetComponent<CanvasGroup>().alpha = 0;
                    return obj;
                },
                ib => ib.gameObject.SetActive(true),
                ib => ib.gameObject.SetActive(false),
                ib => Destroy(ib.gameObject),
                defaultCapacity: 16,
                maxSize: 64
            );
        }

        private void Refresh(ItemStack itemStack, bool isAdded)
        {
            var obj = _itemEntries.Get();
            obj.Set(itemStack, !isAdded);
            var canvasGroup = obj.GetComponent<CanvasGroup>();
            canvasGroup.FadeIn(0.2f)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                    canvasGroup
                        .DOFade(0, 1f)
                        .SetDelay(3)
                        .SetEase(Ease.Linear)
                        .OnComplete(() => _itemEntries.Release(obj))
                );
        }
    }
}