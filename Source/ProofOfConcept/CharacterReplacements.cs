/*
 * WARNING!
 * This source file contains Intentionally Bad Code.
 * WARNING!
 */

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace StackTraceangelo.ProofOfConcept
{
    static class CharacterReplacements // NOTE-IG: Direct dataflow pattern, my favorite :)
    {
        private static Dictionary<char, HashSet<char>> replacements;

        public static IEnumerable<char> GetReplacementsFor(char character)
        {
            EnsureReplacements();
            return replacements.ContainsKey(character) ? replacements[character] : Enumerable.Empty<char>();
        }

        public static bool HasReplacementsFor(char character)
        {
            EnsureReplacements();
            return replacements.ContainsKey(character);
        }

        private static void EnsureReplacements()
        {
            if (replacements != null) return;

            replacements = new Dictionary<char, HashSet<char>>();

            string fileName = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) ?? string.Empty, "CharacterReplacements.txt");
            foreach(string line in File.ReadAllLines(fileName))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                char[] characters = line.Split('\t').Select(s => s[0]).ToArray();

                if (!replacements.ContainsKey(characters[0]))
                    replacements.Add(characters[0], new HashSet<char>());

                HashSet<char> replacementsForCharacter = replacements[characters[0]];
                for (int i = 1; i < characters.Length; i++)
                    replacementsForCharacter.Add(characters[i]);
            }
        }
    }
}
