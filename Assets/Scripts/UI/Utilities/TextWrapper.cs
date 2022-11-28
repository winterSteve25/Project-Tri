using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Utilities
{
    [ExecuteInEditMode]
    public class TextWrapper : MonoBehaviour
    {
        [SerializeField] private LayoutElement layoutElement;
        [SerializeField] private int characterLimit;
        [SerializeField] private List<TMP_Text> text;

        public List<TMP_Text> TextBoxes => text;

        private void Update()
        {
            Refresh();
        }

        public void Refresh()
        {
            if (text == null) return;
            layoutElement.enabled = text.Count > 0 && text.Where(t => t != null).Any(t => t.text.Length > characterLimit);
        }
    }
}