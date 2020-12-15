using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;

using Reflect = System.Reflection;
using SIO = System.IO;

namespace Ks.Common
{
    public class FileSystemUtilities
    {
        public static bool EnsureExists(string Path)
        {
            return DefaultCacher<FileSystem>.Value.EnsureExists(Path);
        }

        public static bool Exists(string file)
        {
            return DefaultCacher<FileSystem>.Value.Exists(file);
        }

        public static void Move(string file, string destination)
        {
            DefaultCacher<FileSystem>.Value.Move(file, destination);
        }

        public static void Delete(string file, bool recursiveIfDirectory = false)
        {
            DefaultCacher<FileSystem>.Value.Delete(file, recursiveIfDirectory);
        }

        public static void RotateFileName(string file, int maxCount = -1, Func<IFileSystem, string, int, string>? nameGenerator = null)
        {
            DefaultCacher<FileSystem>.Value.RotateFileName(file, maxCount, nameGenerator);
        }

        /// <summary>
        /// Replaces invalid file name characters with '_'.
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public static string CorrectFileName(string Name)
        {
            return DefaultCacher<FileSystem>.Value.CorrectFileName(Name);
        }
    }
}
