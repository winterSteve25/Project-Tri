using System;
using EditorAttributes;
using Items;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public sealed class ItemSlotDrawer : OdinAttributeDrawer<ItemSlotAttribute, ItemStack>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            var rect = GUILayoutUtility.GetRect(Attribute.Width, Attribute.Height, GUILayoutOptions.ExpandWidth(false));
            
            var value = ValueEntry.SmartValue;
            var texture = value.IsEmpty ? Sprite.Create(EditorGUIUtility.whiteTexture, new Rect(0, 0, 4, 4),Vector2.zero) : value.item.sprite;
            
            var id = DragAndDropUtilities.GetDragAndDropId(rect);
            DragAndDropUtilities.DrawDropZone(rect, texture, label, id);
            value.item = DragAndDropUtilities.DropZone(rect, value.item, id);
            value.item = DragAndDropUtilities.ObjectPickerZone(rect, value.item, false, id);

            if (rect.Contains(Event.current.mousePosition) && value.item != null)
            {
                var countRect = rect.Padding(5, 5, 5, 0).AlignTop(16);
                value.count = Mathf.Clamp(EditorGUI.IntField(countRect, value.count), 1, value.item.maxStackSize);
                GUI.Label(countRect, "/ " + value.item.maxStackSize, SirenixGUIStyles.RightAlignedGreyMiniLabel);
            }

            ValueEntry.SmartValue = value;
        }

        public override bool CanDrawTypeFilter(Type type)
        {
            return type == typeof(ItemStack);
        }
    }
}