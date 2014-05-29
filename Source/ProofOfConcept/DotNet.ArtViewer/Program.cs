/*
 * WARNING!
 * This source file contains Intentionally Bad Code.
 * WARNING!
 */

using System;
using System.IO;
using System.Reflection;
using StackTraceangelo.ProofOfConcept.Core;

namespace StackTraceangelo.ProofOfConcept.DotNet.ArtViewer
{
    class Program
    {
        static void Main()
        {
            var stackTraceArtClassWrappers = StackTraceArtClassWrapperHelper.FindStackTraceArtClassWrappersInDirectory(new DirectoryInfo(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) ?? string.Empty /* To silent ReSharper.*/));

            for (int i = 0; i < stackTraceArtClassWrappers.Length; i++)
                Console.WriteLine("{0}. {1}", i + 1, stackTraceArtClassWrappers[i].Name);

            Console.Write("Select art: ");

            int artIndex = int.Parse(Console.ReadLine() ?? "1") - 1;

            StackTraceArtClassWrapperHelper.Paint(stackTraceArtClassWrappers[artIndex]);
        }
    }
}
