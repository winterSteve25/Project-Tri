using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Localization;

namespace Liquid
{
    /// <summary>
    /// Object that holds the identity of an item
    /// </summary>
    [CreateAssetMenu(fileName = "New Liquid", menuName = "Liquids/New Liquid")]
    public class TriLiquid : ScriptableObject
    {
        [BoxGroup("Basic Information")]
        public LocalizedString liquidName;
        [BoxGroup("Basic Information")]
        public Color color;
    }
}