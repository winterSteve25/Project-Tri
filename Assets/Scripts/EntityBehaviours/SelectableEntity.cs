using DG.Tweening;
using Sirenix.OdinInspector;
using UI.TextContents;
using UnityEngine;
using Utils;

namespace EntityBehaviours
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class SelectableEntity : MonoBehaviour
    {
        [SerializeField, FoldoutGroup("Selection Color")] 
        private Color selectedColor = Color.cyan;
        
        [SerializeField, FoldoutGroup("Selection Color")]
        private Color normalColor = Color.white;

        [SerializeField] 
        protected SpriteRenderer[] spriteRenderers;

        private bool _isMouseOver;
        
        private void OnMouseEnter()
        {
            _isMouseOver = true;
            foreach (var sprite in spriteRenderers)
            {
                sprite.DOColor(selectedColor, 0.35f);
            }
        }

        private void OnMouseExit()
        {
            _isMouseOver = false;
            foreach (var sprite in spriteRenderers)
            {
                sprite.DOColor(normalColor, 0.35f);
            }
        }

        protected virtual void Update()
        {
            if (!_isMouseOver || !GameInput.LeftClickButtonDown()) return;
            EntityPanelManager.Show(BuildEntityPanel());
        }

        protected abstract TextContent BuildEntityPanel();
    }
}