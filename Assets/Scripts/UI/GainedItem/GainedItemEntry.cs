using Items;
using TMPro;
using UnityEngine;

namespace UI.GainedItem
{
    public class GainedItemEntry : UIItem
    {
        [SerializeField] private TextMeshProUGUI plusOrMinus;

        public void Set(ItemStack itemStack, bool remove)
        {
            Item = itemStack;
            plusOrMinus.text = remove ? "-" : "+";
        }
    }
}