using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Items;
using TMPro;
using UnityEngine;
using Utils;

namespace Systems.Inv
{
    /// <summary>
    /// Manages the Inventory UI
    /// </summary>
    public class InventoryManager : MonoBehaviour
    {
        #region Animation Constants

        private const float SlotFadeDuration = 0.15f;
        private const float SlotMoveAmount = 10f;
        private const float SlotMoveDuration = 0.25f;
        private const float DelayBetweenSlots = 0.01f;
        private const float TitleMoveAmount = 30f;
        private const float TitleMoveDuration = 0.25f;
        private const float TitleFadeDuration = 0.25f;
        private const float DiscardRequestTimer = 0.5f;

        #endregion

        #region Serialized Fields

        [SerializeField] private GameObject inventory;
        [SerializeField] private TextMeshProUGUI inventoryText;

        public DraggedItem draggedItem;

        [SerializeField] private Transform slotsParent;
        [SerializeField] private ItemSlotRow longRowPrefab;
        [SerializeField] private ItemSlotRow shortRowPrefab;

        #endregion

        public static InventoryManager current { get; private set; }
        
        [NonSerialized] public Inventory CurrentInventory;
        [NonSerialized] public List<ItemSlot> Slots;

        private bool _changing;
        private int _lastRowCount;
        private CanvasGroup _inventoryTextCanvasGroup;

        private void Awake()
        {
            current = this;
        }

        private void Start()
        {
            Slots = new List<ItemSlot>();
            _inventoryTextCanvasGroup = inventoryText.GetComponent<CanvasGroup>();
        }

        private void Update()
        {
            // if user presses escape we close the inventory
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Show(null);
            }
        }

        public void Show(Inventory inv)
        {
            // already active and is the same inventory being opened
            if (inv == null || (CurrentInventory == inv && inventory.activeSelf))
            {
                if (CurrentInventory != null)
                {
                    CurrentInventory.OnChanged -= Refresh;
                    CurrentInventory = null;
                }

                // close inventory
                StartCoroutine(CloseInventory());
                return;
            }

            if (CurrentInventory != inv && inventory.activeSelf)
            {
                StartCoroutine(SwitchInventory(inv));
                return;
            }

            // not already active
            CurrentInventory = inv;
            CurrentInventory.OnChanged += Refresh;
            inventoryText.text = CurrentInventory.InventoryName;
            Refresh();

            StartCoroutine(ShowInventory());
        }

        private IEnumerator ShowInventory()
        {
            // if currently closing wait until finished
            if (_changing)
            {
                var waitingTime = 0f;

                yield return new WaitUntil(() =>
                {
                    waitingTime += Time.deltaTime;
                    return waitingTime > DiscardRequestTimer || !_changing;
                });

                // if clicked so many times that it needs to wait too long to finish we dont do this
                if (waitingTime > DiscardRequestTimer)
                {
                    yield break;
                }
            }

            _changing = true;

            // enable panel
            inventory.SetActive(true);

            // title text moves from left 
            _inventoryTextCanvasGroup.DOFade(1, TitleFadeDuration);
            var inventoryTextTransform = inventoryText.transform;
            inventoryTextTransform.DOLocalMoveX(-TitleMoveAmount, 0)
                .OnComplete(() =>
                {
                    inventoryTextTransform.DOLocalMoveX(0, TitleMoveDuration)
                        .SetEase(Ease.OutCubic);
                });

            // all the slots comes down from above
            foreach (var slot in Slots)
            {
                slot.canvasGroup.DOFade(1, SlotFadeDuration);

                var slotIconTransform = slot.slotIcon.transform;
                slotIconTransform.DOLocalMoveY(SlotMoveAmount, 0f)
                    .OnComplete(() =>
                    {
                        slotIconTransform.DOLocalMoveY(0, SlotMoveDuration)
                            .SetEase(Ease.OutCubic);
                    });

                yield return new WaitForSeconds(DelayBetweenSlots);
            }

            yield return new WaitForSeconds(DelayBetweenSlots * Slots.Count);
            _changing = false;
        }

        private IEnumerator CloseInventory()
        {
            if (_changing)
            {
                var waitingTime = 0f;

                yield return new WaitUntil(() =>
                {
                    waitingTime += Time.deltaTime;
                    return waitingTime > DiscardRequestTimer || !_changing;
                });

                // if clicked so many times that it needs to wait for too long to finish we dont do this
                if (waitingTime > DiscardRequestTimer)
                {
                    yield break;
                }
            }

            _changing = true;

            foreach (var slot in ((IEnumerable<ItemSlot>)Slots).Reverse())
            {
                slot.canvasGroup.DOFade(0, SlotFadeDuration);

                var slotIconTransform = slot.slotIcon.transform;
                slotIconTransform.DOLocalMoveY(SlotMoveAmount, SlotMoveDuration)
                    .SetEase(Ease.OutCubic);

                yield return new WaitForSeconds(DelayBetweenSlots);
            }

            _inventoryTextCanvasGroup.DOFade(0, TitleFadeDuration);
            var tween = inventoryText.transform.DOLocalMoveX(-TitleMoveAmount, TitleMoveDuration)
                .SetEase(Ease.OutCubic);

            yield return new WaitUntil(() => !tween.IsActive());

            // disable panel
            inventory.SetActive(false);
            _changing = false;
        }

        private IEnumerator SwitchInventory(Inventory newInventory)
        {
            if (CurrentInventory != null)
            {
                CurrentInventory.OnChanged -= Refresh;
                CurrentInventory = null;
            }

            // close inventory
            yield return StartCoroutine(CloseInventory());

            // start new inventory
            CurrentInventory = newInventory;
            CurrentInventory.OnChanged += Refresh;
            inventoryText.text = CurrentInventory.InventoryName;
            Refresh();

            StartCoroutine(ShowInventory());
        }

        public void DragItem(ItemStack itemStack)
        {
            if (itemStack.IsEmpty)
            {
                draggedItem.Item = ItemStack.Empty;
                draggedItem.gameObject.SetActive(false);
                return;
            }

            if (!draggedItem.gameObject.activeSelf)
            {
                draggedItem.gameObject.SetActive(true);
            }

            draggedItem.Item = itemStack;
        }

        private void Refresh()
        {
            if (CurrentInventory == null) return;
            
            // if has different amount of rows
            var rowCount = CurrentInventory.RowCount;
            if (_lastRowCount != rowCount)
            {
                // delete old rows
                foreach (Transform child in slotsParent)
                {
                    Destroy(child.gameObject);
                }

                Slots.Clear();

                // instantiate rows
                for (var i = 0; i < rowCount; i++)
                {
                    // because it starts from 0 this is would be a odd number if starting from 1
                    var row = Instantiate(i % 2 == 0 ? longRowPrefab : shortRowPrefab, slotsParent);
                    Slots.AddRange(row.slots);
                }

                _lastRowCount = rowCount;
            }

            for (var i = 0; i < Slots.Count; i++)
            {
                Slots[i].Item = CurrentInventory != null ? CurrentInventory[i] : ItemStack.Empty;
            }
        }
    }
}