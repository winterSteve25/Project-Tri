using TMPro;
using UnityEngine;

namespace UI.Utilities
{
    [RequireComponent(typeof(TMP_InputField))]
    public class ParsedInput : MonoBehaviour
    {
        public string Text { get; private set; }
        public TMP_InputField InputField { get; private set; }
        
        public int Int => int.Parse(Text);
        public float Float => float.Parse(Text);
        public bool Bool => bool.Parse(Text);

        private void Start()
        {
            InputField = GetComponent<TMP_InputField>();
            Text = InputField.text;
            InputField.onValueChanged.AddListener(str => Text = str);
        }

        public bool TryInt(out int res)
        {
            return int.TryParse(Text, out res);
        }
        
        public bool TryFloat(out float res)
        {
            return float.TryParse(Text, out res);
        }
        
        public bool TryBool(out bool res)
        {
            return bool.TryParse(Text, out res);
        }
    }
}