using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osuTK.Graphics;
using System;

namespace HeavensBeat.VisualComponents
{
    public class ColourPicker : Container
    {
        public Action<Color4>? OnCommit;
        private readonly BasicButton switchModesButton;
        private readonly FillFlowContainer<ColourPropertyDisplay> hsvContainer;
        private readonly FillFlowContainer<ColourPropertyDisplay> rgbContainer;
        private readonly Box resultBox;
        private readonly ColourPropertyDisplay hueProp;
        private readonly ColourPropertyDisplay satProp;
        private readonly ColourPropertyDisplay valProp;
        private readonly ColourPropertyDisplay redProp;
        private readonly ColourPropertyDisplay greenProp;
        private readonly ColourPropertyDisplay blueProp;
        private readonly BasicButton applyButton;
        private bool rgbMode;

        public ColourPicker()
        {
            AutoSizeAxes = Axes.Y;
            Width = 300;
            Children = new Drawable[]
            {
                new FillFlowContainer
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Direction = FillDirection.Vertical,
                    Spacing = new osuTK.Vector2(26),
                    Children = new Drawable[]
                    {
                        switchModesButton = new BasicButton
                        {
                            RelativeSizeAxes = Axes.X,
                            Height = 30,
                            Action = SwitchModes,
                        },
                        new Container
                        {
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y,
                            Children = new Drawable[]
                            {
                                hsvContainer = new FillFlowContainer<ColourPropertyDisplay>
                                {
                                    RelativeSizeAxes = Axes.X,
                                    AutoSizeAxes = Axes.Y,
                                    Direction = FillDirection.Vertical,
                                    Spacing = new osuTK.Vector2(10),
                                    Children = new ColourPropertyDisplay[]
                                    {
                                        hueProp = new ColourPropertyDisplay("Hue", b => Color4.FromHsv(new osuTK.Vector4(b/255f, 1, 1, 1))),
                                        satProp = new ColourPropertyDisplay("Saturation", b => Color4.FromHsv(new osuTK.Vector4(0.5f, b/255f, 0.5f, 1))),
                                        valProp = new ColourPropertyDisplay("Value", b => Color4.FromHsv(new osuTK.Vector4(0, 1, b/255f, 1))),
                                    }
                                },
                                rgbContainer = new FillFlowContainer<ColourPropertyDisplay>
                                {
                                    RelativeSizeAxes = Axes.X,
                                    AutoSizeAxes = Axes.Y,
                                    Direction = FillDirection.Vertical,
                                    Spacing = new osuTK.Vector2(10),
                                    Children = new ColourPropertyDisplay[]
                                    {
                                        redProp = new ColourPropertyDisplay("Red", b => new Color4(b/255f, 0, 0, 1)),
                                        greenProp = new ColourPropertyDisplay("Green", b => new Color4(0, b/255f, 0, 1)),
                                        blueProp = new ColourPropertyDisplay("Blue", b => new Color4(0, 0, b/255f, 1)),
                                    }
                                }
                            }
                        },
                        new FillFlowContainer<GridItem>
                        {
                            RelativeSizeAxes = Axes.X,
                            Height = 30,
                            Direction = FillDirection.Horizontal,
                            Spacing = new osuTK.Vector2(6),
                            Children = new GridItem[]
                            {
                                new GridItem(resultBox = new Box
                                {
                                    RelativeSizeAxes = Axes.X,
                                    Height = 30
                                }),
                                new GridItem(applyButton = new BasicButton
                                {
                                    Text = "Apply",
                                    RelativeSizeAxes = Axes.Both,
                                    Action = () => OnCommit?.Invoke(GetColour())
                                })
                            }
                        }
                    }
                }
            };
            hueProp.Bindable.BindValueChanged(HsvChanged, true);
            satProp.Bindable.BindValueChanged(HsvChanged, true);
            valProp.Bindable.BindValueChanged(HsvChanged, true);
            redProp.Bindable.BindValueChanged(RgbChanged, true);
            greenProp.Bindable.BindValueChanged(RgbChanged, true);
            blueProp.Bindable.BindValueChanged(RgbChanged, true);
            do
                SwitchModes();
            while (rgbMode);
        }

        public Color4 GetColour() => new Color4(redProp.Value / 255f, greenProp.Value / 255f, blueProp.Value / 255f, 1);
        public void SetColour(Color4 colour)
        {
            redProp.Bindable.Value = (byte)(colour.R * byte.MaxValue);
            greenProp.Bindable.Value = (byte)(colour.G * byte.MaxValue);
            blueProp.Bindable.Value = (byte)(colour.B * byte.MaxValue);
            var hsvColour = Color4.ToHsv(colour);
            hueProp.Bindable.Value = (byte)(byte.MaxValue * hsvColour.X);
            satProp.Bindable.Value = (byte)(byte.MaxValue * hsvColour.Y);
            valProp.Bindable.Value = (byte)(byte.MaxValue * hsvColour.Z);
        }

        private void HsvChanged(ValueChangedEvent<int> obj)
        {
            if (rgbMode)
                return;
            var colour = Color4.FromHsv(new osuTK.Vector4(hueProp.Value / 255f, satProp.Value / 255f, valProp.Value / 255f, 1));
            redProp.Bindable.Value = (byte)(byte.MaxValue * colour.R);
            greenProp.Bindable.Value = (byte)(byte.MaxValue * colour.G);
            blueProp.Bindable.Value = (byte)(byte.MaxValue * colour.B);
            resultBox.Colour = colour;
            applyButton.Colour = colour;
        }

        private void RgbChanged(ValueChangedEvent<int> obj)
        {
            if (!rgbMode)
                return;
            var colour = new Color4(redProp.Value / 255f, greenProp.Value / 255f, blueProp.Value / 255f, 1);
            var hsvColour = Color4.ToHsv(colour);
            hueProp.Bindable.Value = (byte)(byte.MaxValue * hsvColour.X);
            satProp.Bindable.Value = (byte)(byte.MaxValue * hsvColour.Y);
            valProp.Bindable.Value = (byte)(byte.MaxValue * hsvColour.Z);
            resultBox.Colour = colour;
            applyButton.Colour = colour;
        }

        private void SwitchModes()
        {
            rgbMode = !rgbMode;
            switchModesButton.Text = rgbMode ? "Switch to HSV" : "Switch to RGB";
            rgbContainer.Alpha = rgbMode ? 1 : 0;
            hsvContainer.Alpha = rgbMode ? 0 : 1;
        }

        private class ColourPropertyDisplay : Container
        {
            public int Value => Bindable.Value;
            public readonly BindableNumber<int> Bindable;

            private const int font_size = 16;
            private const int spacing = 4;
            private const int slider_height = 30;
            private const int height = font_size + spacing + slider_height;
            private readonly Func<int, Color4> resultFunction;
            private readonly Box resultBox;

            public ColourPropertyDisplay(string name, Func<int, Color4> resultColour)
            {
                resultFunction = resultColour;
                RelativeSizeAxes = Axes.X;
                Height = height;
                Children = new Drawable[]
                {
                    new SpriteText
                    {
                        Text = name,
                        Font = new FontUsage(null, font_size)
                    },
                    new FillFlowContainer<GridItem>
                    {
                        RelativeSizeAxes = Axes.X,
                        Height = slider_height,
                        Direction = FillDirection.Horizontal,
                        Spacing = new osuTK.Vector2(spacing),
                        Anchor = Anchor.BottomLeft,
                        Origin = Anchor.BottomLeft,
                        Children = new GridItem[]
                        {
                            new GridItem(new HBSliderBar<int>
                            {
                                RelativeSizeAxes = Axes.Both,
                                Current = Bindable = new BindableNumber<int>
                                {
                                    MinValue = 0,
                                    MaxValue = 255,
                                    Value = 127,
                                    Precision = 1
                                }
                            }),
                            new GridItem(0.2f, resultBox = new Box
                            {
                                RelativeSizeAxes = Axes.Both
                            })
                        }
                    }
                };
                Bindable.BindValueChanged(OnValueChanged, true);
            }

            private void OnValueChanged(ValueChangedEvent<int> change) => resultBox.Colour = resultFunction(change.NewValue);
        }
    }
}
