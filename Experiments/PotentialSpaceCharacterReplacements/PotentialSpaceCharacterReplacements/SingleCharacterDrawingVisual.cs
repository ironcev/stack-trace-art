using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace PotentialSpaceCharacterReplacements
{
    class SingleCharacterDrawingVisual : DrawingVisual
    {
        public SingleCharacterDrawingVisual(char character)
        {
            using (DrawingContext dc = RenderOpen())
            {
                FormattedText ft = new FormattedText(character.ToString(),
                    CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    new Typeface(new FontFamily("Courier New"), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal), 10, Brushes.Black);
                dc.DrawText(ft, new Point(0.0, 0.0));
            }
        }
    }
}
