using JetBrains.Annotations;
using UI.TextContents;
using UnityEngine;

namespace TileBehaviours
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class CustomTileHoverEffect : MonoBehaviour
    {
        private TextContent _textContent;
        private bool _isPointerOver;

        protected virtual void Start()
        {
            Rebuild();
        }

        private void OnMouseEnter()
        {
            if (_textContent == null) return;
            _isPointerOver = true;
            TooltipManager.Show(_textContent);
        }

        private void OnMouseExit()
        {
            _isPointerOver = false;
            TooltipManager.Hide();
        }

        protected void Rebuild()
        {
            _textContent = Build();
            if (!_isPointerOver) return;
            if (_textContent == null) return;
            TooltipManager.Show(_textContent);
        }

        [CanBeNull] protected abstract TextContent Build();

        public abstract bool CanInteract();
    }
}