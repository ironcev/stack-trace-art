/*
 * WARNING!
 * This source file contains Intentionally Bad Code.
 * WARNING!
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;

namespace StackTraceangelo.ProofOfConcept
{
    class CharacterReplacementDialogViewModel : INotifyPropertyChanged
    {
        public CharacterReplacementDialogViewModel(char character, IEnumerable<char> availableCharacters, FontFamily font)
        {
            Character = character;
            CharacterGroups = CollectionViewSource.GetDefaultView(CreateCharacterGroups(availableCharacters));
            Font = font;
        }

        private static IEnumerable<CharacterGroupViewModel> CreateCharacterGroups(IEnumerable<char> availableCharacters)
        {
            const int groupSize = 500;
            var characters = availableCharacters.ToArray();
            while(characters.Length > 0)
            {
                yield return new CharacterGroupViewModel(characters.Take(groupSize).ToArray());
                characters = characters.Skip(groupSize).ToArray();
            }
        }

        public char Character { get; private set; }

        private char replacementCharacter;
        public char ReplacementCharacter
        {
            get { return replacementCharacter; }
            set { replacementCharacter = value; FirePropertyChanged("ReplacementCharacter"); FirePropertyChanged("ReplacementCharacterCode"); }
        }

        public string ReplacementCharacterCode
        {
            get { return string.Format(@"\u{0:X4}", Convert.ToUInt16(replacementCharacter)); }
        }

        public ICollectionView CharacterGroups { get; private set; }
        public FontFamily Font { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void FirePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    class CharacterGroupViewModel
    {
        public string GroupName { get; private set; }
        public ICollectionView Characters { get; private set; }

        public CharacterGroupViewModel(ICollection<char> charactersInGroup)
        {
            GroupName = string.Format("{0} .. {1}", charactersInGroup.First(), charactersInGroup.Last());
            Characters = CollectionViewSource.GetDefaultView(charactersInGroup);
        }
    }
}
