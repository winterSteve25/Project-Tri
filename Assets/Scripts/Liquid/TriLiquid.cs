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
        [Header("General Information")]
        public LocalizedString liquidName;
        public Color color;
    }
}