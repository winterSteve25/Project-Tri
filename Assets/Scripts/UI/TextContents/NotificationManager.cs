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
        
        [SerializeField]
        private RectTransform topRight;

        public static void CreateNotification(TextContent textContent, float duration = 5f)
        {
            var placeHolder = Instantiate(Instance.notificationPlaceHolder, Instance.topRight);
            var go = Instantiate(Instance.notificationPrefab, Instance.realParent);

            var notificationTransform = (RectTransform)go.transform;
            var textWrapper = go.GetComponent<TextWrapper>();
            textWrapper.TextBoxes.Clear();
            textContent.Build(null, null, notificationTransform);
            textWrapper.Refresh();
            LayoutRebuilder.ForceRebuildLayoutImmediate(notificationTransform);

            var rect = notificationTransform.rect;
            placeHolder.minWidth = rect.width;
            placeHolder.minHeight = rect.height;

            LayoutRebuilder.ForceRebuildLayoutImmediate(Instance.topRight);

            var placeHolderTransform = placeHolder.transform;
            var startingOffset = new Vector3(100, 0);

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
    }
}