/*
 * WARNING!
 * This source file contains Intentionally Bad Code.
 * WARNING!
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace StackTraceangelo.ProofOfConcept.Editor
{
    internal class InvalidCharacterViewModel // TODO-IG: One they when you start implementing this properly, use flyweight pattern.
    {
        private readonly MainWindowViewModel mainWindowViewModel;

        public InvalidCharacterViewModel(char character, MainWindowViewModel mainWindowViewModel)
        {
            Character = character;
            this.mainWindowViewModel = mainWindowViewModel;
            ButtonForeground = CharacterReplacements.HasReplacementsFor(character) ? Brushes.Green : Brushes.Red;
        }

        public char Character { get; private set; }
        public Brush ButtonForeground { get; private set; } //  NOTE-IG: I'm really good in using all these worst practices in proof-of-concept development :)

        public IEnumerable<IInvalidCharacterReplacementViewModel> StandardReplacements
        {
            get
            {
                IEnumerable<char> replacements = CharacterReplacements.GetReplacementsFor(Character);

                return replacements
                        .Select(replacement => new PredefinedCharacterReplacementViewModel(Character, replacement, mainWindowViewModel))
                        .Concat<IInvalidCharacterReplacementViewModel>(new[] { new UserDefinedCharacterReplacementViewModel(Character, mainWindowViewModel) });
            }
        }
    }

    interface IInvalidCharacterReplacementViewModel
    {
        string Text { get; }
        ICommand ReplaceCharacter { get; }
    }

    class PredefinedCharacterReplacementViewModel : IInvalidCharacterReplacementViewModel
    {
        public PredefinedCharacterReplacementViewModel(char oldCharacter, char newCharacter, MainWindowViewModel mainWindowViewModel)
        {
            Text = newCharacter.ToString(CultureInfo.InvariantCulture);
            ReplaceCharacter = new ReplacePredefinedCharacterCommand(oldCharacter, newCharacter, mainWindowViewModel);
        }

        public string Text { get; private set; }
        public ICommand ReplaceCharacter { get; private set; }

        class ReplacePredefinedCharacterCommand : ICommand
        {
            private readonly char oldCharacter;
            private readonly char newCharacter;
            private readonly MainWindowViewModel mainWindowViewModel;

            public ReplacePredefinedCharacterCommand(char oldCharacter, char newCharacter, MainWindowViewModel mainWindowViewModel)
            {
                this.oldCharacter = oldCharacter;
                this.newCharacter = newCharacter;
                this.mainWindowViewModel = mainWindowViewModel;
            }

            public void Execute(object parameter)
            {
                mainWindowViewModel.ReplaceCharacter(oldCharacter, newCharacter);
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

    class UserDefinedCharacterReplacementViewModel : IInvalidCharacterReplacementViewModel
    {
        public UserDefinedCharacterReplacementViewModel(char oldCharacter, MainWindowViewModel mainWindowViewModel)
        {
            Text = "Select character...";
            ReplaceCharacter = new ReplaceUserDefinedCharacterCommand(oldCharacter, mainWindowViewModel);
        }

        public string Text { get; private set; }
        public ICommand ReplaceCharacter { get; private set; }

        class ReplaceUserDefinedCharacterCommand : ICommand
        {
            private readonly char oldCharacter;
            private readonly MainWindowViewModel mainWindowViewModel;

            private IEnumerable<char> availableCharacters;
            private IEnumerable<char> AvailableCharacters
            {
                get { return availableCharacters ?? (availableCharacters = Enumerable.Range(char.MinValue, char.MaxValue)
                                                                                     .Select(Convert.ToChar)
                                                                                     .Where(mainWindowViewModel.StackTraceArtGenerator.IsValidCharacter)); }
            }

            public ReplaceUserDefinedCharacterCommand(char oldCharacter, MainWindowViewModel mainWindowViewModel)
            {
                this.oldCharacter = oldCharacter;
                this.mainWindowViewModel = mainWindowViewModel;
            }

            public void Execute(object parameter)
            {
                CharacterReplacementDialog dialog = new CharacterReplacementDialog(oldCharacter, AvailableCharacters, mainWindowViewModel.Font);
                dialog.Owner = Application.Current.MainWindow;
                
                if (dialog.ShowDialog() != true) return;

                mainWindowViewModel.ReplaceCharacter(oldCharacter, dialog.ReplacementCharacter);
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
