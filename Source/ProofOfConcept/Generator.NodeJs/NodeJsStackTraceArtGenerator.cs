/*
 * WARNING!
 * This source file contains Intentionally Bad Code.
 * WARNING!
 */

using StackTraceangelo.ProofOfConcept.Core;

namespace StackTraceangelo.ProofOfConcept.Generator.NodeJs
{
    public class NodeJsStackTraceArtGenerator : StackTraceArtGenerator
    {
        public override string Name
        {
            get { return "NodeJS"; }
        }

        public override string GenerateStackTraceArtClass(string className, string exceptionName, string exceptionMessage, string[] callStack)
        {
            throw new System.NotImplementedException();
        }
    }
}
