using UI;

namespace Systems.Inv
{
    public class DraggedItem : UIItem
    {
        protected override void PostItemChanged()
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