using UI.TextContents;

namespace TileBehaviours
{
    public class EmptyHoverEffect : CustomTileHoverEffect
    {
        protected override TextContent Build()
        {
            return null;
        }

        public override bool CanInteract()
        {
            return true;
        }
    }
}