using System;
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
            layoutElement.enabled = text is { Count: > 0 } && text.Any(t => t.text.Length > characterLimit);
        }
    }
}