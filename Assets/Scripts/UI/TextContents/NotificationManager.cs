using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UI.Utilities;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.TextContents
{
    public class NotificationManager : Singleton<NotificationManager>
    {
        [SerializeField, AssetsOnly] private GameObject notificationPrefab;
        [SerializeField, AssetsOnly] private LayoutElement notificationPlaceHolder;
        [SerializeField] private Transform realParent;

        [SerializeField, BoxGroup("Notification Locations")]
        private RectTransform topLeft;

        [SerializeField, BoxGroup("Notification Locations")]
        private RectTransform topRight;

        [SerializeField, BoxGroup("Notification Locations")]
        private RectTransform bottomLeft;

        [SerializeField, BoxGroup("Notification Locations")]
        private RectTransform bottomRight;
        
        public static void CreateNotification(TextContent textContent, NotificationPosition notificationPosition,
            float duration = 5f)
        {
            var position = notificationPosition switch
            {
                NotificationPosition.TopLeft => Instance.topLeft,
                NotificationPosition.TopRight => Instance.topRight,
                NotificationPosition.BottomLeft => Instance.bottomLeft,
                NotificationPosition.BottomRight => Instance.bottomRight,
                _ => throw new ArgumentOutOfRangeException(nameof(notificationPosition), notificationPosition, null)
            };

            var placeHolder = Instantiate(Instance.notificationPlaceHolder, position);
            var go = Instantiate(Instance.notificationPrefab, Instance.realParent);

            var notificationTransform = (RectTransform)go.transform;
            var textWrapper = go.GetComponent<TextWrapper>();
            textWrapper.TextBoxes.Clear();
            textContent.Build(notificationTransform);
            textWrapper.Refresh();
            LayoutRebuilder.ForceRebuildLayoutImmediate(notificationTransform);

            var rect = notificationTransform.rect;
            placeHolder.minWidth = rect.width;
            placeHolder.minHeight = rect.height;

            LayoutRebuilder.ForceRebuildLayoutImmediate(position);

            var placeHolderTransform = placeHolder.transform;
            var startingOffset = notificationPosition switch
            {
                NotificationPosition.TopLeft or NotificationPosition.BottomLeft => new Vector3(-100, 0),
                NotificationPosition.TopRight or NotificationPosition.BottomRight => new Vector3(100, 0),
                _ => throw new ArgumentOutOfRangeException(nameof(notificationPosition), notificationPosition, null)
            };

            go.GetComponent<FollowRectWithOffset>()
                .Follow(placeHolderTransform, startingOffset, new Vector3(0, 30));

            notificationTransform.localScale = Vector3.zero;
            notificationTransform.DOScale(1, 0.2f)
                .SetEase(Ease.InCubic);

            go.GetComponent<CanvasGroup>()
                .DOFade(0, 1f)
                .SetDelay(duration)
                .OnComplete(() =>
                {
                    Destroy(go);
                    Destroy(placeHolder.gameObject);
                });
        }

        public enum NotificationPosition
        {
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight
        }
    }
}