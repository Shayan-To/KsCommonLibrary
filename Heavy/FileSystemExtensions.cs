using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;

namespace Ks.Common
{
    public static class FileSystemExtensions
    {
        public static bool EnsureExists(this IFileSystem FS, string Path)
        {
            if (!FS.Directory.Exists(Path))
            {
                FS.Directory.CreateDirectory(Path);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool Exists(this IFileSystem FS, string file)
        {
            return FS.File.Exists(file) || FS.Directory.Exists(file);
        }

        public static void Move(this IFileSystem FS, string file, string destination)
        {
            if (FS.Directory.Exists(file))
            {
                FS.Directory.Move(file, destination);
            }
            else
            {
                FS.File.Move(file, destination);
            }
        }

        public static void Delete(this IFileSystem FS, string file, bool recursiveIfDirectory = false)
        {
            if (FS.Directory.Exists(file))
            {
                FS.Directory.Delete(file, recursiveIfDirectory);
            }
            else
            {
                FS.File.Delete(file);
            }
        }

        private static string RotatedNameGenerator(this IFileSystem FS, string baseName, int n)
        {
            return $"{FS.Path.GetFileNameWithoutExtension(baseName)}.{n}{FS.Path.GetExtension(baseName)}";
        }

        public static void RotateFileName(this IFileSystem FS, string file, int maxCount = -1, Func<IFileSystem, string, int, string>? nameGenerator = null)
        {
            if (nameGenerator == null)
            {
                nameGenerator = RotatedNameGenerator;
            }
            if (maxCount == -1)
            {
                maxCount = int.MaxValue;
            }

            var directory = FS.Path.GetDirectoryName(file);
            var fileName = FS.Path.GetFileName(file);

            var list = Enumerable.Range(1, maxCount).Select(i =>
            {
                var name = nameGenerator.Invoke(FS, fileName, i);
                return FS.Path.Combine(directory, name);
            }).PrependElement(file).AsCachedList();

            var n = list.IndexOf(file => !FS.Exists(file));

            if (n == -1)
            {
                n = maxCount;
                FS.Delete(list[n], true);
            }

            for (var i = n - 1; i >= 0; i -= 1)
            {
                FS.Move(list[i], list[i + 1]);
            }
        }

        public static string CorrectFileName(this IFileSystem FS, string Name, char replacement = '_')
        {
            return Utilities.Text.ReplaceInvalidCharacters(Name, FS.Path.GetInvalidFileNameChars(), replacement).Trim();
        }
    }
}
