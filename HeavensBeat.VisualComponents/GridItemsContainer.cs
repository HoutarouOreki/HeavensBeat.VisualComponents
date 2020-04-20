using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Layout;
using osuTK;

namespace HeavensBeat.VisualComponents
{
    public class GridItemsContainer : FillFlowContainer<GridItem>
    {
        public GridItemsContainer(GridDirection gridDirection) => Direction = (FillDirection)gridDirection;

        protected override void LoadComplete() => base.LoadComplete();

        public new Vector2 Spacing
        {
            get => base.Spacing;
            set
            {
                base.Spacing = value;
                Invalidate(Invalidation.MiscGeometry, InvalidationSource.Self);
            }
        }
    }

    public enum GridDirection
    {
        Horizontal = 1,
        Vertical = 2
    }
}
