$Global:ScriptDirectory = Split-Path -Parent $Script:MyInvocation.MyCommand.Path

$Data = Get-Content -Encoding UTF8 -Raw "$Global:ScriptDirectory\DataTypes.md"
$Data = $Data -Split '(?<=[\r\n])(?=## )'
$DataDic = @{}
$Data | select -Skip 1 | % {$DataDic[$_ -Replace '^## (.*?)[\r\n]+[\s\S]*$', '$1'] = $_}

While ($True)
{
    $Type = Read-Host
    Write-Host ($DataDic[$Type.Trim()])
}
