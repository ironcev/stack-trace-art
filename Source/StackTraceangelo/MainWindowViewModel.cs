using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using Clipboard = System.Windows.Clipboard;
using FontFamily = System.Windows.Media.FontFamily;
using MessageBox = System.Windows.MessageBox;

namespace StackTraceangelo
{
    // TODO-IG: What to do with Tabs and other whitespaces?
    class MainWindowViewModel : INotifyPropertyChanged // NOTE-IG: This is an intentionally simplified implementation on a prototype level. Real production code comming soon. Stay tuned :)
    {
        private const char HangulFiller = '\u3164';

        private string exceptionName;
        public string ExceptionName
        {
            get { return exceptionName; }
            set { exceptionName = value; FirePropertyChanged("Preview"); }
        }

        private string exceptionMessage;
        public string ExceptionMessage
        {
            get { return exceptionMessage; }
            set { exceptionMessage = value; FirePropertyChanged("Preview"); }
        }

        private string asciiArt;
        public string AsciiArt
        {
            get { return asciiArt; }
            set { asciiArt = value.Replace(' ', SpaceCharacterReplacement); FirePropertyChanged("AsciiArt"); FirePropertyChanged("Preview"); FirePropertyChanged("InvalidCharacters"); }
        }

        private int rightMargine;
        public int RightMargine
        {
            get { return rightMargine; }
            set { rightMargine = value; FirePropertyChanged("Preview"); }
        }

        private FontFamily font;
        public FontFamily Font
        {
            get { return font; }
            set { font = value; FirePropertyChanged("Font"); }
        }

        public string Preview
        {
            get { return CreatePreview(); }
        }

        public IEnumerable<InvalidCharacterViewModel> InvalidCharacters
        {
            get { return GetInvalidCharacters(AsciiArt).Select(x => new InvalidCharacterViewModel(x, this)); }
        }

        public ICollectionView SpaceCharacterReplacementCharacters { get; private set; }

        private char spaceCharacterReplacement;
        public char SpaceCharacterReplacement
        {
            get { return spaceCharacterReplacement; }
            set
            {
                AsciiArt = AsciiArt.Replace(spaceCharacterReplacement, value);
                spaceCharacterReplacement = value;
                FirePropertyChanged("SpaceCharacterReplacement");
            }
        }

        public ICommand GenerateCSharpCode { get; private set; }
        public ICommand SetFont { get; private set; }

        public MainWindowViewModel()
        {
            exceptionName = "SpaceInvadersException";
            exceptionMessage = "Your program has been attacked by Space Invaders.";
            asciiArt = Resources.DefaultAsciiArt;
            rightMargine = 5;
            font = new FontFamily("Courier New");
            spaceCharacterReplacement = HangulFiller;
            SpaceCharacterReplacementCharacters = CollectionViewSource.GetDefaultView
                    (
                        new[]
                            {
                               new SpaceCharacterReplacementViewModel('\u115F', "Hangul choseong filler"),
                               new SpaceCharacterReplacementViewModel('\u1160', "Hangul jungseong filler"),
                               new SpaceCharacterReplacementViewModel('\u3164', "Hangul filler"),
                               new SpaceCharacterReplacementViewModel('\uFFA0', "Halfwidth hangul filler")
                            }
                    );

            GenerateCSharpCode = new GenerateCSharpCodeCommand(this);
            SetFont = new SetFontCommand(this);
        }

        private string CreatePreview()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("{0} was unhandled", ExceptionName);
            sb.AppendLine();
            sb.AppendFormat("  Message={0}", ExceptionMessage);
            sb.AppendLine();
            sb.AppendLine("  Source=YourApplication");
            sb.AppendLine("  StackTrace:");
            sb.Append(GetStackTracePreviewOfAsciiArt());
            sb.AppendLine(@"       at YourApplication.SomeClassInYourApplication.SomeMethodThatCallsTheArtMethod(string someArgument) in D:\Projects\YourApplication\Source\SomeClassInYourApplication.cs:line 57");
            sb.AppendLine(@"       at YourApplication.Program.Main() in D:\Projects\YourApplication\Source\Program.cs:line 9");
            sb.AppendLine(@"       at System.AppDomain._nExecuteAssembly(RuntimeAssembly assembly, String[] args)");
            sb.AppendLine(@"       at System.AppDomain.ExecuteAssembly(String assemblyFile, Evidence assemblySecurity, String[] args)");
            sb.AppendLine(@"       at Microsoft.VisualStudio.HostingProcess.HostProc.RunUsersAssembly()");
            sb.AppendLine(@"       at System.Threading.ThreadHelper.ThreadStart_Context(Object state)");
            sb.AppendLine(@"       at System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean ignoreSyncCtx)");
            sb.AppendLine(@"       at System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state)");
            sb.AppendLine(@"       at System.Threading.ThreadHelper.ThreadStart()");
            sb.AppendLine(@"  InnerException:");
            return sb.ToString();
        }

        public void ReplaceCharacter(char oldCharacter, char newCharacter)
        {
            AsciiArt = AsciiArt.Replace(oldCharacter, newCharacter);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void FirePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private string GetStackTracePreviewOfAsciiArt()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var normalizedAsciiArtLine in NormalizeAsciiArt())
            {
                sb.AppendFormat("       at {0}.{1} in :line 1", SpaceCharacterReplacement, normalizedAsciiArtLine);
                sb.AppendLine();
            }

            return sb.ToString();
        }

        private IEnumerable<string> NormalizeAsciiArt()
        {
            string rightMargineString = new string(SpaceCharacterReplacement, RightMargine);

            var asciiArtLines = AsciiArt.Split('\n').Select(x => x.TrimEnd('\r', '\n')).ToArray();
            int longestLineLength = asciiArtLines.Select(x => x.Length).Max();
            asciiArtLines = asciiArtLines.Select(line => ReplaceWhiteSpaces(line.PadRight(longestLineLength, SpaceCharacterReplacement))).ToArray();

            List<string> normalizedAsciiArtLines = new List<string>();
            foreach (var asciiArtLine in asciiArtLines)
            {
                string normalizedAsciiArtLine = string.IsNullOrEmpty(asciiArtLine) ? SpaceCharacterReplacement.ToString() : SpaceCharacterReplacement + asciiArtLine;
                normalizedAsciiArtLine += rightMargineString;
                while (normalizedAsciiArtLines.Contains(normalizedAsciiArtLine))
                    normalizedAsciiArtLine += SpaceCharacterReplacement;
                normalizedAsciiArtLines.Add(normalizedAsciiArtLine);
            }

            return normalizedAsciiArtLines.Select(x => string.Format("{0}{1}()", SpaceCharacterReplacement, x));
        }

        private string ReplaceWhiteSpaces(string text)
        {
            StringBuilder sb = new StringBuilder(text);
            for (int i = 0; i < sb.Length; i++)
                if (char.IsWhiteSpace(sb[i])) sb[i] = SpaceCharacterReplacement;

            return sb.ToString();
        }

        private static IEnumerable<char> GetInvalidCharacters(string text)
        {
            return new HashSet<char>(text.Replace("\n", string.Empty).Replace("\r", string.Empty)).Where(c => !IsValidCharacterForCSharpIdentifier(c));
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

        public string GenerateCode()
        {
            string[] callStack = NormalizeAsciiArt().Reverse().ToArray();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("// Use the line below to call the stack trace art method from your code.");
            sb.AppendFormat("//new {0}().{1};", SpaceCharacterReplacement, callStack[0]);
            sb.AppendLine();
            sb.AppendLine(@"#line 1 """"");
            sb.AppendFormat(@"public class {0} {{{1}}}", SpaceCharacterReplacement, GenerateClassMethods(callStack, ExceptionName, ExceptionMessage));

            if (!IsStandardException(exceptionName))
              sb.AppendFormat("class {0}:Exception{{public {0}(string m):base(m){{}}}}", exceptionName);

            return sb.ToString();
        }

        private static string GenerateClassMethods(string[] callStack, string exceptionName, string exceptionMessage)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("public void {0}{{{1};}}", callStack[0], callStack[1]);

            for (int i = 1; i < callStack.Length - 1; i++)
                sb.AppendFormat("private void {0}{{{1};}}", callStack[i], callStack[i + 1]);

            sb.AppendFormat("private void {0}{{throw new {1}(\"{2}\");}}", callStack.Last(), exceptionName, exceptionMessage);

            sb.AppendLine();

            return sb.ToString();
        }

        private static string[] standardExceptions;
        private static bool IsStandardException(string exceptionName)
        {
            EnsureStandardEsceptions();
            return standardExceptions.Contains(exceptionName);
        }

        private static void EnsureStandardEsceptions()
        {
            if (standardExceptions != null) return;
            standardExceptions =
                new[]
                    {
                        Assembly.GetAssembly(typeof (Exception)), Assembly.GetAssembly(typeof (NullReferenceException)),
                        Assembly.GetAssembly(typeof (ArgumentException))
                    }
                    .SelectMany(x => x.GetTypes().Where(type => typeof (Exception).IsAssignableFrom(type)))
                    .Select(type => type.Name).ToArray();
        }

        class GenerateCSharpCodeCommand : ICommand
        {
            private readonly MainWindowViewModel mainWindowViewModel;

            public GenerateCSharpCodeCommand(MainWindowViewModel mainWindowViewModel)
            {
                this.mainWindowViewModel = mainWindowViewModel;
            }

            public void Execute(object parameter)
            {
                if (mainWindowViewModel.AsciiArt.Split('\n').Length < 2)
                {
                    MessageBox.Show("So far, the art has to have at least two lines.");
                    return;
                }

                Clipboard.SetText(mainWindowViewModel.GenerateCode());

                MessageBox.Show("Stack trace art class has been copied to the clipboard. Paste it to your project and call the art method to throw the exception.");
            }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public event EventHandler CanExecuteChanged;
        }

        class SetFontCommand : ICommand
        {
            private readonly MainWindowViewModel mainWindowViewModel;

            public SetFontCommand(MainWindowViewModel mainWindowViewModel)
            {
                this.mainWindowViewModel = mainWindowViewModel;
            }

            public void Execute(object parameter)
            {
                FontDialog dialog = new FontDialog();
                dialog.Font = new Font(mainWindowViewModel.Font.Source, 12);
                dialog.ShowEffects = false;
                dialog.AllowScriptChange = false;

                if (dialog.ShowDialog() != DialogResult.OK) return;

                mainWindowViewModel.Font = new FontFamily(dialog.Font.FontFamily.Name);
            }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public event EventHandler CanExecuteChanged;
        }
    }
}
