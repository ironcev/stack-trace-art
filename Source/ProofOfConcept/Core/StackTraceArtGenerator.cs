/*
 * WARNING!
 * This source file contains Intentionally Bad Code.
 * WARNING!
 */

using System.Linq;
using System.Collections.Generic;
using System.Globalization;

namespace StackTraceangelo.ProofOfConcept.Core
{
    public abstract class StackTraceArtGenerator
    {
        public abstract string Name { get; }
        public abstract string GenerateStackTraceArtClass(string className, string exceptionName, string exceptionMessage, string[] callStack);
        public abstract string GeneratePreview(string className, string exceptionName, string exceptionMessage, IEnumerable<string> normalizedAsciiArt);

        static readonly UnicodeCategory[] validCharacters = 
                                        {
                                            UnicodeCategory.UppercaseLetter, UnicodeCategory.LowercaseLetter,
                                            UnicodeCategory.TitlecaseLetter, UnicodeCategory.ModifierLetter,
                                            UnicodeCategory.OtherLetter, UnicodeCategory.LetterNumber,
                                            UnicodeCategory.DecimalDigitNumber,
                                            UnicodeCategory.ConnectorPunctuation,
                                            UnicodeCategory.NonSpacingMark,
                                            UnicodeCategory.Format
                                        };

        public virtual bool IsValidCharacter(char character)
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

            return validCharacters.Contains(char.GetUnicodeCategory(character));
        }
    }
}
