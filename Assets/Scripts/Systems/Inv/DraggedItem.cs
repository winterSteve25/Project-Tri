using UI;

namespace Systems.Inv
{
    public class DraggedItem : UIItem
    {
        protected override void PostItemChanged()
        {
            if (Item.IsEmpty)
            {
                gameObject.SetActive(false);
                return;
            }

            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }
            
            base.PostItemChanged();
        }
    }
}