using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PotentialSpaceCharacterReplacements
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            // "Unit tests" for the IsSpaceCharacterReplacement() method.
            if (IsSpaceCharacterReplacement('A'))
                MessageBox.Show("Character 'A' cannot be space character replacement.");

            if (!IsSpaceCharacterReplacement(' '))
                MessageBox.Show("Space must be replacement for itself.");

            if (!IsSpaceCharacterReplacement('\u3164'))
                MessageBox.Show("Hangul Filler must be replacement for space.");

            DataContext = string.Join(Environment.NewLine, FindPotentialSpaceCharacterReplacements());
        }

        /*  Stride aside, what's implied? 
            Badly documented, both here and in intellisense. The stride is the width of one line in the target Array in bytes (not pixels) so if you want to read a 10 x 10 region from the top-left corner of a 32bpp bitmap, using the following:
            int[] target = new int[10 * 10];

            bitmapSource.CopyPixels(0, target, 10*4, 0); 
            Then:
            pixel(0,0) is target[0];
            pixel(1,0) is target[1];
            pixel(0,1) is target[10];
            pixel(9,9) is target[99];
            Guess it is obvious once you know, but a real pain to work out. 
         */

        private static bool IsSpaceCharacterReplacement(char character)
        {
            var drawingVisual = new SingleCharacterDrawingVisual(character);
            RenderTargetBitmap targetBitmap = new RenderTargetBitmap(30, 30, 120, 96, PixelFormats.Pbgra32);
            targetBitmap.Render(drawingVisual);
            int[] target = new int[30 * 30];
            targetBitmap.CopyPixels(Int32Rect.Empty, target, 30 * 4, 0);
            return target.All(i => i == 0);
        }

        private static IEnumerable<string> FindPotentialSpaceCharacterReplacements()
        {
            return Enumerable.Range(char.MinValue, char.MaxValue)
                             .Select(Convert.ToChar)
                             .Where(IsValidCharacterForCSharpIdentifier)
                             .Where(IsSpaceCharacterReplacement)
                             .Select(x => string.Format("{0:X4}", Convert.ToUInt32(x)));
        }

        static readonly UnicodeCategory[] allowedCharacters = 
                                        {
                                            UnicodeCategory.UppercaseLetter, UnicodeCategory.LowercaseLetter,
                                            UnicodeCategory.TitlecaseLetter, UnicodeCategory.ModifierLetter,
                                            UnicodeCategory.OtherLetter, UnicodeCategory.LetterNumber,
                                            UnicodeCategory.DecimalDigitNumber,
                                            UnicodeCategory.ConnectorPunctuation,
                                            UnicodeCategory.NonSpacingMark,
                                            UnicodeCategory.Format
                                        };

        internal static bool IsValidCharacterForCSharpIdentifier(char character)
        {
            /*
             * http://notes.jschutz.net/2007/11/unicode-character-classes/
             * http://msdn.microsoft.com/en-us/library/aa664670(v=vs.71).aspx
               identifier-part-character:
                    letter-character : A Unicode character of classes Lu (Uppercase_Letter), Ll (Lowercase_Letter), Lt (Titlecase_Letter), Lm (Modifier_Letter), Lo (Other_Letter), or Nl (Letter_Number)
                    decimal-digit-character : A Unicode character of the class Nd (Decimal_Number)
                    connecting-character : A Unicode character of the class Pc (Connector_Punctuation)
                    combining-character : A Unicode character of classes Mn or Mc (Nonspacing_Mark)
                    formatting-character : A Unicode character of the class Cf (Format)
             */

            return allowedCharacters.Contains(char.GetUnicodeCategory(character));
        }
    }
}
