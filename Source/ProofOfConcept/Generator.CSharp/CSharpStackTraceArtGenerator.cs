/*
 * WARNING!
 * This source file contains Intentionally Bad Code.
 * WARNING!
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using StackTraceangelo.ProofOfConcept.Core;

namespace StackTraceangelo.ProofOfConcept.Generator.CSharp
{
    public class CSharpStackTraceArtGenerator : StackTraceArtGenerator
    {
        public override string Name
        {
            get { return "C#"; }
        }

        public override string GenerateStackTraceArtClass(string className, string exceptionName, string exceptionMessage, string[] callStack)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("using System;");
            sb.AppendLine();

            GenerateHeaderComment(sb, className, callStack, exceptionName, exceptionMessage);

            sb.AppendLine("// ReSharper disable CheckNamespace");
            sb.AppendLine("// ReSharper disable InconsistentNaming");
            sb.AppendLine("#pragma warning disable 1709 // warning CS1709: Filename specified for preprocessor directive is empty");
            sb.AppendLine();

            GenerateClass(sb, className, callStack, exceptionName, exceptionMessage);

            if (!IsStandardException(exceptionName))
                GenerateExceptionClass(sb, exceptionName);

            sb.AppendLine("#pragma warning restore 1709 // warning CS1709: Filename specified for preprocessor directive is empty");
            sb.AppendLine("// ReSharper restore InconsistentNaming");
            sb.AppendLine("// ReSharper restore CheckNamespace");

            return sb.ToString();            
        }

        private static void GenerateExceptionClass(StringBuilder sb, string exceptionName)
        {
            sb.AppendLine();
            sb.AppendFormat("class {0} : Exception", exceptionName);
            sb.AppendLine();
            sb.AppendLine("{");
            sb.AppendFormat("    public {0}(string message) : base(message) {{}}", exceptionName);
            sb.AppendLine();
            sb.AppendLine("}");
            sb.AppendLine();
        }

        private static void GenerateHeaderComment(StringBuilder sb, string className, string[] callStack, string exceptionName, string exceptionMessage)
        {
            sb.AppendLine("/*");

            sb.AppendLine(exceptionName);

            sb.AppendLine(exceptionMessage);

            foreach (string line in callStack.Reverse())
            {
                sb.AppendLine(line);
            }

            sb.AppendLine();
            sb.AppendLine("Use the below line of code to call the stack trace art method:");
            sb.AppendFormat("new {0}().{1};", className, callStack[0]);
            sb.AppendLine();

            sb.AppendLine("*/");
            sb.AppendLine();
        }

        private static void GenerateClass(StringBuilder sb, string className, string[] callStack, string exceptionName, string exceptionMessage)
        {
            sb.AppendLine(@"#line 1 """"");
            sb.AppendFormat("public class {0}", className);
            sb.AppendLine();
            sb.AppendLine("{");

            GenerateClassMethods(sb, callStack, exceptionName, exceptionMessage);

            sb.AppendLine("}");
        }

        private static void GenerateClassMethods(StringBuilder sb, string[] callStack, string exceptionName, string exceptionMessage)
        {
            GenerateMethod(sb, MethodVisibility.Public, callStack[0], callStack[1]);

            for (int i = 1; i < callStack.Length - 1; i++)
                GenerateMethod(sb, MethodVisibility.Private, callStack[i], callStack[i + 1]);

            GenerateMethod(sb, MethodVisibility.Private, callStack.Last(), string.Format("throw new {0}(\"{1}\")", exceptionName, exceptionMessage));
        }

        enum MethodVisibility
        {
            Public,
            Private
        }

        private static void GenerateMethod(StringBuilder sb, MethodVisibility methodVisibility, string methodName, string methodBody)
        {
            sb.AppendLine(@"#line 1 """"");
            sb.AppendFormat("    {0} {1}void {2}", methodVisibility.ToString().ToLowerInvariant(), methodVisibility == MethodVisibility.Private ? "static " : "", methodName);
            sb.AppendLine();
            sb.AppendLine("    {");
            sb.AppendLine(@"#line 1 """"");
            sb.AppendFormat("        {0};", methodBody);
            sb.AppendLine();
            sb.AppendLine("    }");
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
                    .SelectMany(x => x.GetTypes().Where(type => typeof(Exception).IsAssignableFrom(type)))
                    .Select(type => type.Name).ToArray();
        }

        public override string GeneratePreview(string className, string exceptionName, string exceptionMessage, IEnumerable<string> normalizedAsciiArt)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("{0} was unhandled", exceptionName);
            sb.AppendLine();
            sb.AppendFormat("  Message={0}", exceptionMessage);
            sb.AppendLine();
            sb.AppendLine("  Source=YourApplication");
            sb.AppendLine("  StackTrace:");
            sb.Append(GetStackTracePreviewOfAsciiArt(className, normalizedAsciiArt));
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

        private static string GetStackTracePreviewOfAsciiArt(string className, IEnumerable<string> normalizedAsciiArt)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var normalizedAsciiArtLine in normalizedAsciiArt)
            {
                sb.AppendFormat("       at {0}.{1} in :line 1", className, normalizedAsciiArtLine);
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
