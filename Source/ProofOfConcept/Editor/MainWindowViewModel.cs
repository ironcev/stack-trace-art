/*
 * WARNING!
 * This source file contains Intentionally Bad Code.
 * WARNING!
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using StackTraceangelo.ProofOfConcept.Core;
using Clipboard = System.Windows.Clipboard;
using FontFamily = System.Windows.Media.FontFamily;
using MessageBox = System.Windows.MessageBox;

namespace StackTraceangelo.ProofOfConcept.Editor
{
    // TODO-IG: Think what to do with Tabs and other whitespaces?
    class MainWindowViewModel : INotifyPropertyChanged
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
            get { return StackTraceArtGenerator.GeneratePreview(SpaceCharacterReplacement.ToString(CultureInfo.InvariantCulture), ExceptionName, ExceptionMessage, NormalizeAsciiArt()); }
        }

        public IEnumerable<InvalidCharacterViewModel> InvalidCharacters
        {
            get { return GetInvalidCharacters(AsciiArt).Select(x => new InvalidCharacterViewModel(x, this)); }
        }

        public ICollectionView SpaceCharacterReplacementCharacters { get; private set; }

        public ICollectionView StackTraceArtGenerators { get; private set; }
        public StackTraceArtGenerator StackTraceArtGenerator
        {
            get { return (StackTraceArtGenerator) StackTraceArtGenerators.CurrentItem; }
        }

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

        public ICommand GenerateStackTraceArtClass { get; private set; }
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

            StackTraceArtGenerators = CollectionViewSource.GetDefaultView(GetStackTraceArtGenerators());

            GenerateStackTraceArtClass = new GenerateStackTraceArtClassCommand(this);
            SetFont = new SetFontCommand(this);
        }

        private static IEnumerable<StackTraceArtGenerator> GetStackTraceArtGenerators()
        {
            IEnumerable<Type> result = Enumerable.Empty<Type>();

            var potentialArtLibraries = Directory.EnumerateFiles(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) ?? string.Empty /* To silent ReSharper.*/, "*.dll");

            foreach (string potentialArtLibrary in potentialArtLibraries)
            {
                Assembly potentialArtLibraryAssembly = Assembly.LoadFrom(potentialArtLibrary);
                result = result.Concat(potentialArtLibraryAssembly.GetTypes().Where(type => !type.IsAbstract && typeof(StackTraceArtGenerator).IsAssignableFrom(type)));
            }

            return result.Select(Activator.CreateInstance).Cast<StackTraceArtGenerator>();
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

        private IEnumerable<string> NormalizeAsciiArt()
        {
            string rightMargineString = new string(SpaceCharacterReplacement, RightMargine);

            var asciiArtLines = AsciiArt.Split('\n').Select(x => x.TrimEnd('\r', '\n')).ToArray();
            int longestLineLength = asciiArtLines.Select(x => x.Length).Max();
            asciiArtLines = asciiArtLines.Select(line => ReplaceWhiteSpaces(line.PadRight(longestLineLength, SpaceCharacterReplacement))).ToArray();

            List<string> normalizedAsciiArtLines = new List<string>();
            foreach (var asciiArtLine in asciiArtLines)
            {
                string normalizedAsciiArtLine = string.IsNullOrEmpty(asciiArtLine) ? SpaceCharacterReplacement.ToString(CultureInfo.InvariantCulture) : SpaceCharacterReplacement + asciiArtLine;
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

        private IEnumerable<char> GetInvalidCharacters(string text)
        {
            return new HashSet<char>(text.Replace("\n", string.Empty).Replace("\r", string.Empty)).Where(c => !StackTraceArtGenerator.IsValidCharacter(c));
        }

        class GenerateStackTraceArtClassCommand : ICommand
        {
            private readonly MainWindowViewModel mainWindowViewModel;

            public GenerateStackTraceArtClassCommand(MainWindowViewModel mainWindowViewModel)
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

                string generatedClass = mainWindowViewModel.StackTraceArtGenerator.GenerateStackTraceArtClass(
                                                            mainWindowViewModel.SpaceCharacterReplacement.ToString(CultureInfo.InvariantCulture),
                                                            mainWindowViewModel.ExceptionName,
                                                            mainWindowViewModel.ExceptionMessage,
                                                            mainWindowViewModel.NormalizeAsciiArt().Reverse().ToArray());

                Clipboard.SetText(generatedClass);
            }

            public bool CanExecute(object parameter)
            {
                return true;
            }

#pragma warning disable 0067 // warning CS0067: The event is never used
            public event EventHandler CanExecuteChanged;
#pragma warning restore 0067 // warning CS0067: The event is never used
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

#pragma warning disable 0067 // warning CS0067: The event is never used
            public event EventHandler CanExecuteChanged;
#pragma warning restore 0067 // warning CS0067: The event is never used
        }
    }
}
