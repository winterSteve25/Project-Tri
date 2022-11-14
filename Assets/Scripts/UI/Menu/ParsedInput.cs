using TMPro;
using UnityEngine;

namespace UI.Menu
{
    [RequireComponent(typeof(TMP_InputField))]
    public class ParsedInput : MonoBehaviour
    {
        public string Text { get; private set; }
        public int Int => int.Parse(Text);
        public float Float => float.Parse(Text);
        public bool Bool => bool.Parse(Text);
        
        private void Start()
        {
            var tmpInputField = GetComponent<TMP_InputField>();
            Text = tmpInputField.text;
            tmpInputField.onValueChanged.AddListener(str => Text = str);
        }
    }
}