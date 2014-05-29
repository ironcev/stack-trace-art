/*
 * WARNING!
 * This source file contains Intentionally Bad Code.
 * WARNING!
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace StackTraceangelo.ProofOfConcept.Core
{
    public static class StackTraceArtClassWrapperHelper // Well, it is static and has Helper in name. It cannot be worse :-)
    {
        public static Type[] FindStackTraceArtClassWrappersInDirectory(DirectoryInfo directory)
        {
            IEnumerable<Type> result = Enumerable.Empty<Type>();

            IEnumerable<FileInfo> potentialArtLibraries = directory.EnumerateFiles("*.dll");

            foreach (FileInfo potentialArtLibrary in potentialArtLibraries)
            {
                Assembly potentialArtLibraryAssembly = Assembly.LoadFrom(potentialArtLibrary.FullName);
                result = result.Concat(potentialArtLibraryAssembly.GetTypes().Where(type => type.IsClass && type.IsPublic && type.IsAbstract && type.IsSealed && HasPaintMethod(type)));
            }

            return result.ToArray();
        }

        public static void Paint(Type stackTraceArtClassWrapperType)
        {
            stackTraceArtClassWrapperType.GetMethod("Paint").Invoke(null, null);
        }

        private static bool HasPaintMethod(Type type)
        {
            var paintMethod = type.GetMethod("Paint", BindingFlags.Static | BindingFlags.Public);
            if (paintMethod == null) return false;

            return paintMethod.GetParameters().Length == 0;
        }
    }
}
