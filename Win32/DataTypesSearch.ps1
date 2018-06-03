$Global:ScriptDirectory = Split-Path -Parent $Script:MyInvocation.MyCommand.Path

$_M_IX86 = $False
$_MSC_VER = 1300
$WINVER = 0x0500
$_WIN64 = $True
$UNICODE = $True

. "$Global:ScriptDirectory\DataTypes.ps1"

While ($True)
{
    $Type = Read-Host
    $Type = $Type.Trim()

    Write-Host -NoNewline $Type
    While ($True)
    {
        $Type = $Dic[$Type]
        If ($Type -EQ $Null)
        {
            Break
        }
        Write-Host -NoNewline -ForegroundColor Red " -> "
        Write-Host -NoNewline $Type
    }
    Write-Host
}
