using osu.Framework.Extensions.Color4Extensions;
using osuTK.Graphics;

namespace HeavensBeat.VisualComponents
{
    public static class ColoursHB
    {
        public static readonly Color4 Red = new Color4(237, 18, 33, 255);
        public static readonly Color4 Pink = new Color4(255, 0, 170, 255);
        public static readonly Color4 Yellow = new Color4(255, 221, 85, 255);
        public static readonly Color4 OrangeRed = new Color4(255, 80, 0, 255);
        public static readonly Color4 Orange = new Color4(238, 170, 0, 255);
        public static readonly Color4 LimeGreen = new Color4(169, 204, 0, 255);
        public static readonly Color4 Green = new Color4(3, 178, 60, 255);
        public static readonly Color4 Cyan = new Color4(102, 204, 255, 255);
        public static readonly Color4 DimBlue = new Color4(16, 64, 186, 255);
        public static readonly Color4 Purple = new Color4(136, 102, 238, 255);

        public static readonly Color4 DimRed = Red.Darken(1);
        public static readonly Color4 DimYellow = Yellow.Darken(1);
        public static readonly Color4 DimOrange = Orange.Darken(0.5f);
        public static readonly Color4 DimLimeGreen = LimeGreen.Darken(0.5f);
        public static readonly Color4 DimGreen = Green.Darken(0.8f);
        public static readonly Color4 DimCyan = Cyan.Darken(0.5f);
        public static readonly Color4 Blue = DimBlue.Lighten(1f);
        public static readonly Color4 DimPurple = Purple.Darken(0.5f);

        public static readonly Color4 GalacticCyan = new Color4(51, 191, 255, 255);
        public static readonly Color4 IntenseGalacticCyan = new Color4(51, 255, 255, 255);
        public static readonly Color4 IntenseCyan = new Color4(0, 204, 255, 255);
        public static readonly Color4 BlackTransparent = new Color4(0, 0, 0, 122);

        public static readonly Color4 White = new Color4(1f, 1f, 1f, 1f);
        public static readonly Color4 DimWhite = new Color4(0.85f, 0.85f, 0.85f, 1f);
        public static readonly Color4 DarkWhite = new Color4(0.7f, 0.7f, 0.7f, 1f);

        public static readonly Color4 LightGray = new Color4(125, 125, 125, 255);
        public static readonly Color4 Gray = new Color4(100, 100, 100, 255);
        public static readonly Color4 DimGray = new Color4(75, 75, 75, 255);
        public static readonly Color4 DarkGray = new Color4(55, 55, 55, 255);

        public static readonly Color4 LightBlack = new Color4(40, 40, 40, 255);
        public static readonly Color4 MediumLightBlack = new Color4(30, 30, 30, 255);
        public static readonly Color4 MediumBlack = new Color4(20, 20, 20, 255);
        public static readonly Color4 DarkBlack = new Color4(8, 8, 8, 255);
        public static readonly Color4 Black = new Color4(0, 0, 0, 255);

        public static readonly Color4 Transparent = new Color4(0, 0, 0, 0);

        //public static Color4 GetColour(this NoteType noteType)
        //{
        //    return noteType switch
        //    {
        //        NoteType.Bullet => Cyan,
        //        NoteType.Enemy => Red,
        //        NoteType.Environment => Green,
        //        NoteType.Event => Purple,
        //        _ => Black,
        //    };
        //} // should be moved to an extension class
    }
}
