using UnityEngine;

namespace UI.Map
{
    public class MapIcon : MonoBehaviour
    {
        [SerializeField]
        private Sprite icon;
        public Sprite Icon => icon;
    }
}