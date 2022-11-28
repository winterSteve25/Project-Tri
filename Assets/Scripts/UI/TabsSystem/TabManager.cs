using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using Utils;

namespace UI.TabsSystem
{
    public class TabManager : SerializedMonoBehaviour
    {
        [OdinSerialize] public List<TabButton> tabs { get; private set; }

        [SerializeField, FoldoutGroup("Configuration")]
        private float tabMoveDuration = 0.25f;

        [SerializeField, FoldoutGroup("Configuration")]
        private int tabMoveAmount = 100;
        
        [SerializeField, FoldoutGroup("Configuration")]
        private float tabFadeDuration = 0.1f;
        
        [SerializeField, ReadOnly] private int selectedTab;
        [SerializeField, ReadOnly] private bool moving;

        private void Start()
        {
            // disable all but first
            for (var i = 1; i < tabs.Count; i++)
            {
                var canvasGroup = tabs[i].TabObject;
                canvasGroup.alpha = 0;
                canvasGroup.gameObject.SetActive(false);
            }
        }

        public void ClickedTab(int index)
        {
            if (index == selectedTab) return;
            if (moving) return;
            StartCoroutine(RefreshContent(selectedTab, index));
        }

        private IEnumerator RefreshContent(int oldTab, int newTab)
        {
            moving = true;

            var oldT = tabs[oldTab].TabObject;
            var newT = tabs[newTab].TabObject;

            var moveAmount = -tabMoveAmount;

            // old tab is right of new tab
            if (oldTab - newTab > 0)
            {
                // move old tab right and new tab right
                moveAmount = tabMoveAmount;
            }

            // move old tab to one direction
            var oldTTransform = oldT.transform;
            var oldTOgPos = oldTTransform.position;
            oldTTransform.DOLocalMoveX(moveAmount, tabMoveDuration);
            oldT.DOFade(0, tabFadeDuration);
            
            var newTTransform = newT.transform;
            var newTOgPos = newTTransform.localPosition;

            // move tab the opposite direction so we can move it back, giving the illusion of cycling
            newTTransform.localPosition = new Vector3(newTOgPos.x - moveAmount, newTOgPos.y, newTOgPos.z);
            newT.FadeIn(tabFadeDuration);
            newTTransform.DOLocalMoveX(0, tabMoveDuration);

            yield return new WaitForSeconds(tabMoveDuration);
            
            oldT.gameObject.SetActive(false);
            // reset its position
            oldTTransform.position = oldTOgPos;
            selectedTab = newTab;
            moving = false;
        }
    }
}