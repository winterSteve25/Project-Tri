using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Localization;

namespace Items
{
    
    /// <summary>
    /// Object that holds the identity of an item
    /// </summary>
    [CreateAssetMenu(fileName = "New Item", menuName = "Items/New Item")]
    public class Item : ScriptableObject
    {
        #region Group Names

        protected const string HorizontalMain = "Main";
        protected const string SpriteBox = HorizontalMain + "/Sprite";
        protected const string GeneralInformationBox = HorizontalMain + "/General Info";
        protected const string VerticalMain = GeneralInformationBox + "/Main";
        
        #endregion

        [BoxGroup(SpriteBox)]
        [HorizontalGroup(HorizontalMain, width: 80)]
        [PreviewField(ObjectFieldAlignment.Center, Height = 80), HideLabel]
        public Sprite sprite;
        
        [BoxGroup(GeneralInformationBox)]
        [VerticalGroup(VerticalMain)]
        public LocalizedString itemName;

        [BoxGroup(GeneralInformationBox)]
        [VerticalGroup(VerticalMain)]
        public LocalizedString itemDescription;

        [BoxGroup(GeneralInformationBox)]
        [HorizontalGroup(VerticalMain + "Max Stack")]
        [MinValue(1)]
        public int maxStackSize = 1;

        [HorizontalGroup(VerticalMain + "Max Stack")]
        [Button(5, Name = "-16", DirtyOnClick = true)]
        private void DecrementStackSize()
        {
            maxStackSize -= 16;
            maxStackSize = Mathf.Clamp(maxStackSize, 1, int.MaxValue);
        }

        [HorizontalGroup(VerticalMain + "Max Stack")]
        [Button(5, Name = "+16", DirtyOnClick = true)]
        private void IncrementStackSize()
        {
            maxStackSize += 16;
        }
    }
}