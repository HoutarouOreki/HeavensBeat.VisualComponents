using osu.Framework.Graphics;
using osu.Framework.Graphics.UserInterface;

namespace HeavensBeat.VisualComponents
{
    public class HBTextBox : BasicTextBox
    {
        public HBTextBox()
        {
            TextFlow.Padding = new MarginPadding(1);
            TextContainer.Padding = new MarginPadding(1);
            BackgroundCommit = ColoursHB.IntenseGalacticCyan;
            BackgroundFocused = ColoursHB.DarkGray;
            BackgroundUnfocused = ColoursHB.MediumBlack;
            CornerRadius = 0;
        }
    }
}
