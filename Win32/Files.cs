using System;
using System.Runtime.InteropServices;

namespace Ks.Common.Win32
{
        public static class Files
        {

            public static class Unsafe
            {

                /// BOOL WINAPI CreateHardLink(
            /// _In_       LPCTSTR               lpFileName,
            /// _In_       LPCTSTR               lpExistingFileName,
            /// _Reserved_ LPSECURITY_ATTRIBUTES lpSecurityAttributes
            /// );
            /// <summary>
            /// Establishes a hard link between an existing file and a new file. This function is only supported on the NTFS file system, and only for files, not directories.
            /// To perform this operation as a transacted operation, use the CreateHardLinkTransacted function.
            /// </summary>
            /// <param name="lpFileName">
            /// lpFileName [in]
            /// The name of the new file.
            /// This parameter may include the path but cannot specify the name of a directory.
            /// In the ANSI version of this function, the name is limited to MAX_PATH characters. To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path. For more information, see Naming a File.
            /// Tip  Starting with Windows 10, version 1607, for the unicode version of this function (CreateHardLinkW), you can opt-in to remove the MAX_PATH limitation without prepending "\\?\". See the "Maximum Path Length Limitation" section of Naming Files, Paths, and Namespaces for details.
            /// </param>
            /// <param name="lpExistingFileName">
            /// lpExistingFileName [in]
            /// The name of the existing file.
            /// This parameter may include the path cannot specify the name of a directory.
            /// In the ANSI version of this function, the name is limited to MAX_PATH characters. To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path. For more information, see Naming a File.
            /// Tip  Starting with Windows 10, version 1607, for the unicode version of this function (CopyFileW), you can opt-in to remove the MAX_PATH limitation without prepending "\\?\". See the "Maximum Path Length Limitation" section of Naming Files, Paths, and Namespaces for details.
            /// </param>
            /// <param name="lpSecurityAttributes">
            /// lpSecurityAttributes
            /// Reserved; must be NULL.
            /// </param>
            /// <returns>
            /// If the function succeeds, the return value is nonzero.
            /// If the function fails, the return value is zero (0). To get extended error information, call GetLastError.
            /// The maximum number of hard links that can be created with this function is 1023 per file. If more than 1023 links are created for a file, an error results.
            /// </returns>
            /// <remarks>
            /// Any directory entry for a file that is created with CreateFile or CreateHardLink is a hard link to an associated file. An additional hard link that is created with the CreateHardLink function allows you to have multiple directory entries for a file, that is, multiple hard links to the same file, which can be different names in the same directory, or the same or different names in different directories. However, all hard links to a file must be on the same volume.
            /// Because hard links are only directory entries for a file, many changes to that file are instantly visible to applications that access it through the hard links that reference it. However, the directory entry size and attribute information is updated only for the link through which the change was made.
            /// The security descriptor belongs to the file to which a hard link points. The link itself is only a directory entry, and does not have a security descriptor. Therefore, when you change the security descriptor of a hard link, you a change the security descriptor of the underlying file, and all hard links that point to the file allow the newly specified access. You cannot give a file different security descriptors on a per-hard-link basis.
            /// This function does not modify the security descriptor of the file to be linked to, even if security descriptor information is passed in the lpSecurityAttributes parameter.
            /// Use DeleteFile to delete hard links. You can delete them in any order regardless of the order in which they are created.
            /// Flags, attributes, access, and sharing that are specified in CreateFile operate on a per-file basis. That is, if you open a file that does not allow sharing, another application cannot share the file by creating a new hard link to the file.
            /// When you create a hard link on the NTFS file system, the file attribute information in the directory entry is refreshed only when the file is opened, or when GetFileInformationByHandle is called with the handle of a specific file.
            /// Symbolic link behavior—If the path points to a symbolic link, the function creates a hard link to the target.
            /// In Windows 8 and Windows Server 2012, this function is supported by the following technologies.
            /// </remarks>
                [DllImport("Kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
                public static extern bool CreateHardLinkW(string lpFileName, string lpExistingFileName, IntPtr lpSecurityAttributes);
            }

            public static void CreateHardLink(string FileName, string ExistingFileName)
            {
                var R = Unsafe.CreateHardLinkW(FileName, ExistingFileName, IntPtr.Zero);
                if (!R)
                    Common.ThrowError();
            }
        }
    }
