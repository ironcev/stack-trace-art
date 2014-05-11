/*
 * WARNING!
 * This source file contains Intentionally Bad Code.
 * WARNING!
 */

using System.Collections.Generic;

namespace StackTraceangelo.ProofOfConcept.Core
{
    public abstract class StackTraceArtGenerator
    {
        public abstract string Name { get; }
        public abstract string GenerateStackTraceArtClass(string className, string exceptionName, string exceptionMessage, string[] callStack);
        public abstract string GeneratePreview(string className, string exceptionName, string exceptionMessage, IEnumerable<string> normalizedAsciiArt);
    }
}
