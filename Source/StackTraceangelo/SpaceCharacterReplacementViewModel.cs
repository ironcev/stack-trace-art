/*
 * WARNING!
 * This source file contains Intentionally Bad Code.
 * WARNING!
 */

using System;

namespace StackTraceangelo
{
    class SpaceCharacterReplacementViewModel
    {
        public char Character { get; private set; }
        public string UnicodeValue { get; private set; }
        public string CharacterName { get; private set; }

        public SpaceCharacterReplacementViewModel(char character, string characterName)
        {
            Character = character;
            UnicodeValue = string.Format(@"\u{0:X4}", Convert.ToUInt16(character));
            CharacterName = string.Format("Space replacement: {0}.", characterName);
        }
    }
}
