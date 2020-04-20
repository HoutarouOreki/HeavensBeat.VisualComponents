using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;
using System;

namespace HeavensBeat.VisualComponents.PropertyManagement
{
    public abstract class PropertyEditor<T> : Container
    {
        private readonly Container mainContentContainer;
        private readonly SpriteText propertyStateText;
        private readonly Bindable<PropertyStateInfo> state = new Bindable<PropertyStateInfo>();
        private readonly TextFlowContainer descriptionFlow;
        private readonly SpriteText titleText;
        private readonly FillFlowContainer outerFlow;
        private readonly FillFlowContainer innerFlow;
        private readonly Box background;
        private Vector2 spacing;

        public new MarginPadding Padding { get => outerFlow.Padding; set => outerFlow.Padding = value; }

        public ColourInfo BackgroundColour { get => background.Colour; set => background.Colour = value; }
        public FontUsage TitleFont { get => titleText.Font; set => titleText.Font = value; }
        public FontUsage DescriptionFont { set => RegenerateDescriptionWithFont(value); }
        public Vector2 Spacing { get => spacing; set
            {
                spacing = value;
                OnSpacingChangedInternal(value);
                OnSpacingChanged(value);
            }
        }

        public bool ReadyToSave => state.Value.State == PropertyState.ReadyToSave;

        protected override Container<Drawable> Content => mainContentContainer;
        protected Bindable<T> Current { get; }
        protected Property<T> Property { get; }

        public PropertyEditor(Property<T> property)
        {
            Property = property;
            Current = CreateCurrent();
            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;
            var mainContent = CreateMainContent();
            InternalChildren = new Drawable[]
            {
                background = new Box { BypassAutoSizeAxes = Axes.Y, RelativeSizeAxes = Axes.Both, Colour = FrameworkColour.BlueGreenDark },
                outerFlow = new FillFlowContainer
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Direction = FillDirection.Vertical,
                    Children = new Drawable[]
                    {
                        innerFlow = new FillFlowContainer
                        {
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y,
                            Direction = FillDirection.Vertical,
                            Children = new Drawable[]
                            {
                                titleText = new SpriteText { Text = property.Name, Font = new FontUsage(null, 20, "Bold") },
                                descriptionFlow = new TextFlowContainer(t => t.Font = new FontUsage(null, 16))
                                {
                                    Text = property.Description,
                                    RelativeSizeAxes = Axes.X,
                                    AutoSizeAxes = Axes.Y
                                }
                            }
                        },
                        mainContentContainer = new Container
                        {
                            RelativeSizeAxes = Axes.X,
                            Height = 30,
                            Child = mainContent
                        },
                        propertyStateText = new SpriteText()
                    }
                }
            };
            Current.BindValueChanged(OnCurrentChangedInternal, true);
            state.BindValueChanged(OnStateChanged, true);
        }

        public bool Save()
        {
            if (!ReadyToSave)
                return false;
            Property.Setter(Current.Value);
            return true;
        }

        protected virtual void OnSpacingChanged(Vector2 value) { }

        protected abstract Bindable<T> CreateCurrent();

        protected abstract Drawable CreateMainContent();

        protected void TriggerError(string? errorMessage) => state.Value = new PropertyStateInfo(PropertyState.Error, errorMessage);

        protected virtual void OnCurrentChanged(ValueChangedEvent<T> change) { }

        private void OnSpacingChangedInternal(Vector2 value) => outerFlow.Spacing = innerFlow.Spacing = value;

        private void OnCurrentChangedInternal(ValueChangedEvent<T> change)
        {
            if (Property.Validator == null)
            {
                state.Value = new PropertyStateInfo(PropertyState.Unknown, null);
                return;
            }
            var validation = Property.Validator(change.NewValue);
            state.Value = new PropertyStateInfo(ValidationToPropertyState(validation.Result), validation.Message);
            OnCurrentChanged(change);
        }
        private void OnStateChanged(ValueChangedEvent<PropertyEditor<T>.PropertyStateInfo> obj) => UpdateStateText();

        private void UpdateStateText()
        {
            propertyStateText.Colour = GetStateColour(state.Value.State);
            propertyStateText.Text = state.Value.Text ?? GetDefaultStateText();
        }

        private ColourInfo GetStateColour(PropertyState propertyState) => propertyState switch
        {
            PropertyState.Saved => ColoursHB.Cyan,
            PropertyState.ReadyToSave => ColoursHB.Green,
            PropertyState.Warning => ColoursHB.Yellow,
            PropertyState.Error => ColoursHB.Red,
            _ => ColoursHB.Purple,
        };

        private string GetDefaultStateText() => state.Value.State switch
        {
            PropertyState.Saved => "Saved!",
            PropertyState.ReadyToSave => "Ready to be saved",
            PropertyState.Warning => "Why does this warning have no message?",
            PropertyState.Error => "Invalid input",
            _ => "Unknown state"
        };

        private PropertyState ValidationToPropertyState(ValidationResult validationResult) => validationResult switch
        {
            ValidationResult.Ok => PropertyState.ReadyToSave,
            ValidationResult.Warning => PropertyState.Warning,
            ValidationResult.Error => PropertyState.Error,
            _ => PropertyState.Unknown
        };

        private void RegenerateDescriptionWithFont(FontUsage font)
        {
            descriptionFlow.Text = "";
            descriptionFlow.AddText(Property.Description, t => t.Font = font);
        }

        private struct PropertyStateInfo
        {
            public readonly PropertyState State;
            public readonly string? Text;

            public PropertyStateInfo(PropertyState state, string? text)
            {
                State = state;
                Text = text;
            }
        }
    }
}
