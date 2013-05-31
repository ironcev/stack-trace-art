/*
 * WARNING!
 * This source file contains Intentionally Bad Code.
 * WARNING!
 */

using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;

namespace StackTraceangelo
{
    public partial class CharacterReplacementDialog
    {
        private readonly CharacterReplacementDialogViewModel viewModel;

        public CharacterReplacementDialog(char character, IEnumerable<char> availableCharacters, FontFamily font)
        {
            InitializeComponent();
            DataContext = viewModel = new CharacterReplacementDialogViewModel(character, availableCharacters, font);
        }

        private void OnButtonGotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            viewModel.ReplacementCharacter = ((Button) sender).Content.ToString()[0];
        }

        private void OnReplaceButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogResult = true;
        }

        public char ReplacementCharacter
        {
            get { return viewModel.ReplacementCharacter; }
        }
    }
}
