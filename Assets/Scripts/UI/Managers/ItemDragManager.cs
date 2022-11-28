using Items;
using Systems.Inv;
using UnityEngine;
using Utils;

namespace UI.Managers
{
    public class ItemDragManager : CurrentInstanced<ItemDragManager>
    {
        [SerializeField]
        private DraggedItem draggedItem;
        public DraggedItem DraggedItem => draggedItem;
        
        public void DragItem(ItemStack itemStack)
        {
            draggedItem.Item = itemStack;
        }
    }
}