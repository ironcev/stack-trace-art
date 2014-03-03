/*B
 * WARNING!
 * This source file contains Intentionally Bad Code.
 * WARNING!
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace StackTraceangelo.ProofOfConcept.DotNet.ArtViewer
{
    class Program
    {
        static void Main()
        {
            Type[] stackTraceArtClassWrappers = FindStackTraceArtClassWrappers();

            for (int i = 0; i < stackTraceArtClassWrappers.Length; i++)
                Console.WriteLine("{0}. {1}", i + 1, stackTraceArtClassWrappers[i].Name);

            Console.Write("Select art: ");

            int artIndex = int.Parse(Console.ReadLine() ?? "1") - 1;

            stackTraceArtClassWrappers[artIndex].GetMethod("Paint").Invoke(null, null);
        }

        private static Type[] FindStackTraceArtClassWrappers()
        {
            IEnumerable<Type> result = Enumerable.Empty<Type>();

            var potentialArtLibraries = Directory.EnumerateFiles(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) ?? string.Empty /* To silent ReSharper.*/, "*.dll");

            foreach(string potentialArtLibrary in potentialArtLibraries)
            {
                Assembly potentialArtLibraryAssembly = Assembly.LoadFrom(potentialArtLibrary);
                result = result.Concat(potentialArtLibraryAssembly.GetTypes().Where(type => type.IsClass && type.IsPublic && type.IsAbstract && type.IsSealed && HasPaintMethod(type)));
            }

            return result.ToArray();
        }

        private static bool HasPaintMethod(Type type)
        {
            var paintMethod = type.GetMethod("Paint", BindingFlags.Static | BindingFlags.Public);
            if (paintMethod == null) return false;

            return paintMethod.GetParameters().Length == 0;
        }
    }
}
