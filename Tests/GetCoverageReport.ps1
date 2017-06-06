$MyDir = Split-Path $Script:MyInvocation.MyCommand.Path -Parent

$OpenCover = Resolve-Path "$MyDir\..\packages\OpenCover.4.6.519\tools\OpenCover.Console.exe"
$ReportGenerator = Resolve-Path "$MyDir\..\packages\ReportGenerator.2.5.8\tools\ReportGenerator.exe"
$xUnitConsole = Resolve-Path "$MyDir\..\packages\xunit.runner.console.2.2.0\tools\xunit.console.exe"

$TestsAssemblyDirectory = Resolve-Path "$MyDir\bin\Debug"
$TestsAssemblyName = "Tests.dll"
$TestsAssembly = Resolve-Path "$TestsAssemblyDirectory\$TestsAssemblyName"

$OutputDirectory = "$MyDir\TestResults"

If ((Test-Path $OutputDirectory))
{
	Remove-Item -Recurse -Force $OutputDirectory
}
New-Item -ItemType Directory -Path $OutputDirectory | Out-Null

$OutputXmlFile = "$OutputDirectory\TestsCoverageOutput.xml"
$OutputReportsDirectory = "$OutputDirectory\TestsCoverageReports"

$JunctionDirectory = "D:\Temp\XUnitTemp"
New-Item -ItemType Junction -Path $JunctionDirectory -Value $TestsAssemblyDirectory | Out-Null
$TestsAssemblyDirectory = $JunctionDirectory
$TestsAssembly = Resolve-Path "$TestsAssemblyDirectory\$TestsAssemblyName"

& $OpenCover `
	-register:user `
	-target:"$xUnitConsole" `
	"-targetargs:""$TestsAssembly"" -noshadow" `
	-filter:"+[*]* -[Tests]Ks.Tests.* -[FsCheck*]*" `
	-mergebyhash `
	-skipautoprops `
	-output:"""$OutputXmlFile"""

Remove-Item -Force -Recurse $JunctionDirectory

& $ReportGenerator `
	-reports:"$OutputXmlFile" `
	-targetdir:"$OutputReportsDirectory"

& "$OutputReportsDirectory\index.htm"
