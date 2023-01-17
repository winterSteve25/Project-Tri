using UI.TextContents;
using UnityEngine;

namespace TileBehaviours.GeothermalNode
{
    public class GeothermalNodeHoverEffect : CustomTileHoverEffect
    {
        [SerializeField] private GeothermalNodeBehaviour nodeBehaviour;
        
        protected override TextContent Build()
        {
            return TextContent.Titled("Geothermal Energy Node")
                .AddText($"Strength: {nodeBehaviour.Strength:F}")
                .AddText($"Diminish Rate: {nodeBehaviour.DiminishRate:F}");
        }

        public override bool CanInteract()
        {
            return true;
        }
    }
}