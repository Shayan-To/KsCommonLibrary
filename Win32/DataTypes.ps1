# In the name of God

<#
Regexes used to get from DataTypes.md to this:

MATCH CASE

^`(.*)`$
    $1

`

(?<!    )typedef


^(?!    ).*$

\r

\n{3,}
\n\n

^    


 +$


^#ifdef (.*)$
If \(\$$1\)\n{

^#if defined\((.*)$
If \(\$$1\n{

^#if !defined\((.*)$
If \(-Not \$$1\n{

^#else$
}\nElse\n{

^#endif$
}

#(?!define)|if

#define ([a-zA-Z0-9_]+)(?: ([^\n]*))?
\$Dic.Add\('$1', '$2'\)

typedef ([^;{}]+(?:\{[^{}]*\})?)(?<! ) ?((?<![a-zA-Z0-9_])[a-zA-Z0-9_]+);
\$Dic.Add\('$2', '$1'\)

define|typedef

\$(?!Dic)[a-zA-Z0-9_]+
#>

# Recommended settings:
#$_M_IX86 = $False
#$_MSC_VER = 1300
#$WINVER = 0x0500
#$_WIN64 = $True
#$UNICODE = $True

$Dic = [System.Collections.Generic.Dictionary[String, String]]::New()

$Dic.Add('APIENTRY', 'WINAPI')

$Dic.Add('ATOM', 'WORD')

$Dic.Add('BOOL', 'int')

$Dic.Add('BOOLEAN', 'BYTE')

$Dic.Add('BYTE', 'unsigned char')

$Dic.Add('CALLBACK', '__stdcall')

$Dic.Add('CCHAR', 'char')

$Dic.Add('CHAR', 'char')

$Dic.Add('COLORREF', 'DWORD')

$Dic.Add('CONST', 'const')

$Dic.Add('DWORD', 'unsigned long')

$Dic.Add('DWORDLONG', 'unsigned __int64')

$Dic.Add('DWORD_PTR', 'ULONG_PTR')

$Dic.Add('DWORD32', 'unsigned int')

$Dic.Add('DWORD64', 'unsigned __int64')

$Dic.Add('FLOAT', 'float')

$Dic.Add('HACCEL', 'HANDLE')

# This is a special type, and hopefully is never used anywhere. https://stackoverflow.com/questions/18253260/how-to-map-half-ptr-in-c-sharp
#If ($_WIN64)
#{
# $Dic.Add('HALF_PTR', 'int')
#}
#Else
#{
# $Dic.Add('HALF_PTR', 'short')
#}

$Dic.Add('HANDLE', 'PVOID')

$Dic.Add('HBITMAP', 'HANDLE')

$Dic.Add('HBRUSH', 'HANDLE')

$Dic.Add('HCOLORSPACE', 'HANDLE')

$Dic.Add('HCONV', 'HANDLE')

$Dic.Add('HCONVLIST', 'HANDLE')

$Dic.Add('HCURSOR', 'HICON')

$Dic.Add('HDC', 'HANDLE')

$Dic.Add('HDDEDATA', 'HANDLE')

$Dic.Add('HDESK', 'HANDLE')

$Dic.Add('HDROP', 'HANDLE')

$Dic.Add('HDWP', 'HANDLE')

$Dic.Add('HENHMETAFILE', 'HANDLE')

$Dic.Add('HFILE', 'int')

$Dic.Add('HFONT', 'HANDLE')

$Dic.Add('HGDIOBJ', 'HANDLE')

$Dic.Add('HGLOBAL', 'HANDLE')

$Dic.Add('HHOOK', 'HANDLE')

$Dic.Add('HICON', 'HANDLE')

$Dic.Add('HINSTANCE', 'HANDLE')

$Dic.Add('HKEY', 'HANDLE')

$Dic.Add('HKL', 'HANDLE')

$Dic.Add('HLOCAL', 'HANDLE')

$Dic.Add('HMENU', 'HANDLE')

$Dic.Add('HMETAFILE', 'HANDLE')

$Dic.Add('HMODULE', 'HINSTANCE')

If ($WINVER -GE 0x0500)
{
 $Dic.Add('HMONITOR', 'HANDLE')
}

$Dic.Add('HPALETTE', 'HANDLE')

$Dic.Add('HPEN', 'HANDLE')

$Dic.Add('HRESULT', 'LONG')

$Dic.Add('HRGN', 'HANDLE')

$Dic.Add('HRSRC', 'HANDLE')

$Dic.Add('HSZ', 'HANDLE')

$Dic.Add('WINSTA', 'HANDLE')

$Dic.Add('HWND', 'HANDLE')

$Dic.Add('INT', 'int')

#If ($_WIN64)
#{
# $Dic.Add('INT_PTR', '__int64')
#}
#Else
#{
# $Dic.Add('INT_PTR', 'int')
#}

$Dic.Add('INT8', 'signed char')

$Dic.Add('INT16', 'signed short')

$Dic.Add('INT32', 'signed int')

$Dic.Add('INT64', 'signed __int64')

$Dic.Add('LANGID', 'WORD')

$Dic.Add('LCID', 'DWORD')

$Dic.Add('LCTYPE', 'DWORD')

$Dic.Add('LGRPID', 'DWORD')

$Dic.Add('LONG', 'long')

If (-Not $_M_IX86)
{
 $Dic.Add('LONGLONG', '__int64')
}
Else
{
 $Dic.Add('LONGLONG', 'double')
}

#If ($_WIN64)
#{
# $Dic.Add('LONG_PTR', '__int64')
#}
#Else
#{
# $Dic.Add('LONG_PTR', 'long')
#}

$Dic.Add('LONG32', 'signed int')

$Dic.Add('LONG64', '__int64')

$Dic.Add('LPARAM', 'LONG_PTR')

$Dic.Add('LPBOOL', 'BOOL far *')

$Dic.Add('LPBYTE', 'BYTE far *')

$Dic.Add('LPCOLORREF', 'DWORD *')

$Dic.Add('LPCSTR', '__nullterminated CONST CHAR *')

If ($UNICODE)
{
 $Dic.Add('LPCTSTR', 'LPCWSTR')
}
Else
{
 $Dic.Add('LPCTSTR', 'LPCSTR')
}

$Dic.Add('LPCVOID', 'CONST void *')

$Dic.Add('LPCWSTR', 'CONST WCHAR *')

$Dic.Add('LPDWORD', 'DWORD *')

$Dic.Add('LPHANDLE', 'HANDLE *')

$Dic.Add('LPINT', 'int *')

$Dic.Add('LPLONG', 'long *')

$Dic.Add('LPSTR', 'CHAR *')

If ($UNICODE)
{
 $Dic.Add('LPTSTR', 'LPWSTR')
}
Else
{
 $Dic.Add('LPTSTR', 'LPSTR')
}

$Dic.Add('LPVOID', 'void *')

$Dic.Add('LPWORD', 'WORD *')

$Dic.Add('LPWSTR', 'WCHAR *')

$Dic.Add('LRESULT', 'LONG_PTR')

$Dic.Add('PBOOL', 'BOOL *')

$Dic.Add('PBOOLEAN', 'BOOLEAN *')

$Dic.Add('PBYTE', 'BYTE *')

$Dic.Add('PCHAR', 'CHAR *')

$Dic.Add('PCSTR', 'CONST CHAR *')

If ($UNICODE)
{
 $Dic.Add('PCTSTR', 'LPCWSTR')
}
Else
{
 $Dic.Add('PCTSTR', 'LPCSTR')
}

$Dic.Add('PCWSTR', 'CONST WCHAR *')

$Dic.Add('PDWORD', 'DWORD *')

$Dic.Add('PDWORDLONG', 'DWORDLONG *')

$Dic.Add('PDWORD_PTR', 'DWORD_PTR *')

$Dic.Add('PDWORD32', 'DWORD32 *')

$Dic.Add('PDWORD64', 'DWORD64 *')

$Dic.Add('PFLOAT', 'FLOAT *')

# Redundant if
If ($_WIN64)
{
 $Dic.Add('PHALF_PTR', 'HALF_PTR *')
}
Else
{
 $Dic.Add('PHALF_PTR', 'HALF_PTR *')
}

$Dic.Add('PHANDLE', 'HANDLE *')

$Dic.Add('PHKEY', 'HKEY *')

$Dic.Add('PINT', 'int *')

$Dic.Add('PINT_PTR', 'INT_PTR *')

$Dic.Add('PINT8', 'INT8 *')

$Dic.Add('PINT16', 'INT16 *')

$Dic.Add('PINT32', 'INT32 *')

$Dic.Add('PINT64', 'INT64 *')

$Dic.Add('PLCID', 'PDWORD')

$Dic.Add('PLONG', 'LONG *')

$Dic.Add('PLONGLONG', 'LONGLONG *')

$Dic.Add('PLONG_PTR', 'LONG_PTR *')

$Dic.Add('PLONG32', 'LONG32 *')

$Dic.Add('PLONG64', 'LONG64 *')

If ($_WIN64)
{
 $Dic.Add('POINTER_32', '__ptr32')
}
Else
{
 $Dic.Add('POINTER_32', '')
}

If ($_MSC_VER -GE 1300)
{
 $Dic.Add('POINTER_64', '__ptr64')
}
Else
{
 $Dic.Add('POINTER_64', '')
}

$Dic.Add('POINTER_SIGNED', '__sptr')

$Dic.Add('POINTER_UNSIGNED', '__uptr')

$Dic.Add('PSHORT', 'SHORT *')

$Dic.Add('PSIZE_T', 'SIZE_T *')

$Dic.Add('PSSIZE_T', 'SSIZE_T *')

$Dic.Add('PSTR', 'CHAR *')

$Dic.Add('PTBYTE', 'TBYTE *')

$Dic.Add('PTCHAR', 'TCHAR *')

If ($UNICODE)
{
 $Dic.Add('PTSTR', 'LPWSTR')
}
Else
{
 $Dic.Add('PTSTR', 'LPSTR')
}

$Dic.Add('PUCHAR', 'UCHAR *')

# Redundant if
If ($_WIN64)
{
 $Dic.Add('PUHALF_PTR', 'UHALF_PTR *')
}
Else
{
 $Dic.Add('PUHALF_PTR', 'UHALF_PTR *')
}

$Dic.Add('PUINT', 'UINT *')

$Dic.Add('PUINT_PTR', 'UINT_PTR *')

$Dic.Add('PUINT8', 'UINT8 *')

$Dic.Add('PUINT16', 'UINT16 *')

$Dic.Add('PUINT32', 'UINT32 *')

$Dic.Add('PUINT64', 'UINT64 *')

$Dic.Add('PULONG', 'ULONG *')

$Dic.Add('PULONGLONG', 'ULONGLONG *')

$Dic.Add('PULONG_PTR', 'ULONG_PTR *')

$Dic.Add('PULONG32', 'ULONG32 *')

$Dic.Add('PULONG64', 'ULONG64 *')

$Dic.Add('PUSHORT', 'USHORT *')

$Dic.Add('PVOID', 'void *')

$Dic.Add('PWCHAR', 'WCHAR *')

$Dic.Add('PWORD', 'WORD *')

$Dic.Add('PWSTR', 'WCHAR *')

$Dic.Add('QWORD', 'unsigned __int64')

$Dic.Add('SC_HANDLE', 'HANDLE')

$Dic.Add('SC_LOCK', 'LPVOID')

$Dic.Add('SERVICE_STATUS_HANDLE', 'HANDLE')

$Dic.Add('SHORT', 'short')

$Dic.Add('SIZE_T', 'ULONG_PTR')

$Dic.Add('SSIZE_T', 'LONG_PTR')

If ($UNICODE)
{
 $Dic.Add('TBYTE', 'WCHAR')
}
Else
{
 $Dic.Add('TBYTE', 'unsigned char')
}

If ($UNICODE)
{
 $Dic.Add('TCHAR', 'WCHAR')
}
Else
{
 $Dic.Add('TCHAR', 'char')
}

$Dic.Add('UCHAR', 'unsigned char')

#If ($_WIN64)
#{
# $Dic.Add('UHALF_PTR', 'unsigned int')
#}
#Else
#{
# $Dic.Add('UHALF_PTR', 'unsigned short')
#}

$Dic.Add('UINT', 'unsigned int')

#If ($_WIN64)
#{
# $Dic.Add('UINT_PTR', 'unsigned __int64')
#}
#Else
#{
# $Dic.Add('UINT_PTR', 'unsigned int')
#}

$Dic.Add('UINT8', 'unsigned  char')

$Dic.Add('UINT16', 'unsigned  short')

$Dic.Add('UINT32', 'unsigned int')

$Dic.Add('UINT64', 'usigned __int 64')

$Dic.Add('ULONG', 'unsigned long')

If (-Not $_M_IX86)
{
 $Dic.Add('ULONGLONG', 'unsigned __int64')
}
Else
{
 $Dic.Add('ULONGLONG', 'double')
}

#If ($_WIN64)
#{
# $Dic.Add('ULONG_PTR', 'unsigned __int64')
#}
#Else
#{
# $Dic.Add('ULONG_PTR', 'unsigned long')
#}

$Dic.Add('ULONG32', 'unsigned int')

$Dic.Add('ULONG64', 'unsigned __int64')

$Dic.Add('UNICODE_STRING', 'struct _UNICODE_STRING {
  USHORT  Length;
  USHORT  MaximumLength;
  PWSTR  Buffer;
}')
$Dic.Add('PUNICODE_STRING', 'UNICODE_STRING *')
$Dic.Add('PCUNICODE_STRING', 'const UNICODE_STRING *')

$Dic.Add('USHORT', 'unsigned short')

$Dic.Add('USN', 'LONGLONG')

$Dic.Add('VOID', 'void')

$Dic.Add('WCHAR', 'wchar_t')

$Dic.Add('WINAPI', '__stdcall')

$Dic.Add('WORD', 'unsigned short')

$Dic.Add('WPARAM', 'UINT_PTR')
