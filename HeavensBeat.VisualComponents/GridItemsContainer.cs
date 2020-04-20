using osu.Framework.Graphics.Containers;

namespace HeavensBeat.VisualComponents
{
    public class GridItemsContainer : FillFlowContainer<GridItem>
    {
        public GridItemsContainer(GridDirection gridDirection) => Direction = (FillDirection)gridDirection;
    }

    public enum GridDirection
    {
        Horizontal = 1,
        Vertical = 2
    }
}
