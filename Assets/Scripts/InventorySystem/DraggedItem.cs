using UnityEngine;

namespace InventorySystem
{
    public class DraggedItem : UIItem
    {
        [SerializeField] private Canvas parentCanvas;
        
        private void Update()
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentCanvas.transform as RectTransform, Input.mousePosition,
                parentCanvas.worldCamera, out var pos);

            transform.position = parentCanvas.transform.TransformPoint(pos);
        }

        protected override void Refresh()
        {
            if (item.IsEmpty)
            {
                gameObject.SetActive(false);
                itemCount.text = "";
                return;
            }
            
            itemIcon.sprite = item.item.sprite;
            itemCount.text = $"x{item.count}";
        }
    }
}