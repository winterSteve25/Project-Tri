﻿using System;
using DG.Tweening;
using KevinCastejon.MoreAttributes;
using Player;
using UnityEngine;

namespace Items
{
    /// <summary>
    /// Behaviour that controlls the items on the ground
    /// </summary>
    public class GroundItemBehaviour : MonoBehaviour
    {
        [SerializeField] private float despawnTimer = 180;
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        private Action _despawnFunc;
        private ItemStack _item;
        
        [ReadOnly, SerializeField] private bool despawnAwaited;
        [ReadOnly, SerializeField] private float internalTimer;

        public float DespawnTime => internalTimer;
        public ItemStack Item => _item;
        
        public void Init(Action despawnFunc, ItemStack i, float despawnTime)
        {
            _despawnFunc = despawnFunc;
            _item = i;
            despawnAwaited = false;
            internalTimer = despawnTime < 0 ? despawnTimer : despawnTime;
            spriteRenderer.sprite = _item.item.sprite;
        }

        private void OnEnable()
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 1);
            transform.DORotate(new Vector3(0, 0, 360), 4f, RotateMode.FastBeyond360)
                .SetLoops(-1, LoopType.Incremental)
                .SetEase(Ease.Linear);
        }

        private void Update()
        {
            internalTimer -= Time.deltaTime;
            if (internalTimer <= 0)
            {
                _despawnFunc();
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (despawnAwaited) return;
            if (!col.gameObject.TryGetComponent<PlayerInventory>(out var playerInventory)) return;
            if (playerInventory.TryAddItem(transform.position, _item))
            {
                despawnAwaited = true;
                PickUp();
            }
        }

        private void PickUp()
        {
            transform.DOScale(0, 0.2f)
                .SetEase(Ease.InOutCubic)
                .OnComplete(() =>
                {
                    transform.DOKill();
                    _despawnFunc();
                });
        }
    }
}