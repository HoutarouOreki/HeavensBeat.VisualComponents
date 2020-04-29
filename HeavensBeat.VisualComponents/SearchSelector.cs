using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using System;
using System.Collections.Generic;

namespace HeavensBeat.VisualComponents
{
    public class SearchSelector : FillFlowContainer<GridItem>
    {
        public Bindable<string> Current = new Bindable<string>();

        public const float BASE_SIZE = MODULE_SIZE + spacing;
        public const int MODULE_SIZE = 30;
        private const int spacing = 16;

        public IEnumerable<string> Options
        {
            get => options;
            set
            {
                options = value;
                foreach (var optionDisplay in optionsFlow)
                    optionDisplay.Selected -= OnOptionSelected;
                if (deselectOption)
                {
                    var optionDisplay = new OptionDisplay(null);
                    optionsFlow.Add(optionDisplay);
                    optionDisplay.Selected += OnOptionSelected;
                }
                var amountOfOptions = deselectOption ? 1 : 0;
                foreach (var option in options)
                {
                    var optionDisplay = new OptionDisplay(option);
                    optionsFlow.Add(optionDisplay);
                    optionDisplay.Selected += OnOptionSelected;
                    amountOfOptions++;
                }
                if (amountOfOptions > optionsWithoutSearch)
                    searchBoxGridItem.Show();
                else
                    searchBoxGridItem.Hide();
                OnOptionSelected(Current.Value);
            }
        }

        private readonly Bindable<string> searchCurrent = new Bindable<string>();

        private IEnumerable<string> options = new List<string>();
        private readonly FillFlowContainer<OptionDisplay> optionsFlow;
        private readonly int optionsWithoutSearch;
        private readonly bool deselectOption;
        private readonly GridItem searchBoxGridItem;

        /// <param name="optionsWithoutSearch">
        /// Search textbox doesn't appear if the amount of options is up to this number.
        /// </param>
        /// <param name="deselectOption"></param>
        public SearchSelector(int optionsWithoutSearch = 4, bool deselectOption = false)
        {
            Direction = FillDirection.Vertical;
            Spacing = new osuTK.Vector2(spacing);
            Children = new GridItem[]
            {
                searchBoxGridItem = new GridItem(MODULE_SIZE, false, new HBTextBox
                {
                    RelativeSizeAxes = Axes.Both,
                    Current = searchCurrent,
                    PlaceholderText = "Search..."
                }),
                new GridItem(new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = ColoursHB.MediumBlack
                        },
                        new BasicScrollContainer<FillFlowContainer<OptionDisplay>>
                        {
                            RelativeSizeAxes = Axes.Both,
                            Child = optionsFlow = new FillFlowContainer<OptionDisplay>
                            {
                                RelativeSizeAxes = Axes.X,
                                AutoSizeAxes = Axes.Y
                            }
                        }
                    }
                })
            };
            searchCurrent.BindValueChanged(SearchUpdated, true);
            this.optionsWithoutSearch = optionsWithoutSearch;
            this.deselectOption = deselectOption;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            Current.BindValueChanged(CurrentChanged, true);
        }

        private void CurrentChanged(ValueChangedEvent<string> vc)
        {
            foreach (var optionDisplay in optionsFlow)
                optionDisplay.IsActive = optionDisplay.Value == vc.NewValue;
        }

        private void SearchUpdated(ValueChangedEvent<string> searchValueChange)
        {
            foreach (var optionDisplay in optionsFlow)
                if (optionDisplay.Value == null)
                    continue;
                else if (optionDisplay.Value.Contains(searchValueChange.NewValue) == true)
                    optionDisplay.Show();
                else
                    optionDisplay.Hide();
        }

        private void OnOptionSelected(string value) => Current.Value = value;

        private class OptionDisplay : Container
        {
            public bool IsActive
            {
                set
                {
                    isActive = value;
                    UpdateLayout();
                }
                get => isActive;
            }

            public readonly string Value;

            private readonly Box background;
            private bool isActive;

            public event Action<string>? Selected;

            public OptionDisplay(string? value)
            {
                Value = value ?? "";
                Height = 30;
                RelativeSizeAxes = Axes.X;
                Children = new Drawable[]
                {
                    background = new Box { RelativeSizeAxes = Axes.Both },
                    new Container
                    {
                        RelativeSizeAxes = Axes.Both,
                        Padding = new MarginPadding { Vertical = 6, Left = 11 },
                        Child = new SpriteText
                        {
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft,
                            Text = value ?? "<None>",
                            Font = new FontUsage("", 18)
                        }
                    }
                };
            }

            protected override bool OnClick(ClickEvent e)
            {
                Selected?.Invoke(Value);
                background.FlashColour(ColoursHB.White, 300, Easing.OutQuint);
                return true;
            }

            protected override bool OnHover(HoverEvent e)
            {
                UpdateLayout();
                return base.OnHover(e);
            }

            protected override void OnHoverLost(HoverLostEvent e)
            {
                UpdateLayout();
                base.OnHoverLost(e);
            }

            private void UpdateLayout()
            {
                if (isActive)
                    background.Colour = ColoursHB.DimGray;
                else
                    background.Colour = IsHovered ? ColoursHB.DarkGray : ColoursHB.LightBlack;
            }
        }
    }
}
