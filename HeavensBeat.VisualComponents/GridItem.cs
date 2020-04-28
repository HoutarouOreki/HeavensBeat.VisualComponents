using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Layout;

namespace HeavensBeat.VisualComponents
{
    public class GridItem : Container
    {
        private float? targetSize;

        private bool relativeSize;

        private bool shouldValidate;

        public float? TargetSize
        {
            get => targetSize;
            set
            {
                targetSize = value;
                Invalidate(Invalidation.DrawSize);
            }
        }

        public bool RelativeSize
        {
            get => relativeSize;
            set
            {
                relativeSize = value;
                Invalidate(Invalidation.DrawSize);
            }
        }

        public GridItem(Drawable? drawable = null)
        {
            if (drawable != null)
                Child = drawable;
        }

        public GridItem(float targetSize, bool relativeSize = true, Drawable? drawable = null) : this(drawable)
        {
            TargetSize = targetSize;
            RelativeSize = relativeSize;
        }

        public GridItem(float targetSize, Drawable drawable) : this(targetSize, true, drawable) { }

        protected override bool OnInvalidate(Invalidation invalidation, InvalidationSource source)
        {
            base.OnInvalidate(invalidation, source);
            if (source == InvalidationSource.Parent && (invalidation.HasFlag(Invalidation.DrawInfo) || invalidation.HasFlag(Invalidation.MiscGeometry) || invalidation.HasFlag(Invalidation.DrawSize)))
            {
                shouldValidate = true;
                return true;
            }
            return false;
        }

        protected override void Update()
        {
            base.Update();
            if (shouldValidate)
            {
                Validate();
                shouldValidate = false;
            }
        }

        private void Validate()
        {
            if (Parent == null)
                return;
            if (!(Parent is FillFlowContainer<GridItem> parent))
                throw new System.Exception($"Parent is a {Parent.GetType()} (should be {typeof(GridItemsContainer)}).");
            if (parent.Direction == FillDirection.Full)
                throw new System.Exception($"Parent has a Full fill direction");

            var newRSA = parent.Direction == FillDirection.Horizontal ? Axes.Y : Axes.X;
            if (!AutoSizeAxes.HasFlag(newRSA))
                RelativeSizeAxes = newRSA;
            else
                RelativeSizeAxes = Axes.None;

            var direction = parent.Direction;
            var spacing = parent.Direction == FillDirection.Horizontal ? parent.Spacing.X : parent.Spacing.Y;
            var fullSpace = (direction == FillDirection.Horizontal ? parent.ChildSize.X : parent.ChildSize.Y) - (spacing * (parent.Children.Count - 1));
            float size;

            if (TargetSize.HasValue)
                size = (RelativeSize ? fullSpace : 1) * TargetSize.Value;
            else
            {
                var autoSizeChildrenCount = parent.Children.Count;
                var autoSizeSpace = fullSpace;

                foreach (var neighbour in parent.Children)
                {
                    if (!neighbour.TargetSize.HasValue)
                        continue;
                    autoSizeChildrenCount--;
                    autoSizeSpace -= neighbour.RelativeSize ? neighbour.TargetSize.Value * fullSpace : neighbour.TargetSize.Value;
                }

                size = autoSizeSpace / autoSizeChildrenCount;
            }
            if (direction == FillDirection.Horizontal)
                Width = size;
            else if (direction == FillDirection.Vertical)
                Height = size;
        }
    }
}
