$buildId = Get-Date
$failBuildLevelSelector = "Warning"

$tempExePath = "C:\Temp"
if(!(Test-Path $inspectCodeExePath)) {
    mkdir $tempExePath
}

$solutionOrProjectPath = "$PSScriptRoot\Sfa.Tl.Matching.sln"

$sourceNugetExe = "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe"
$nugetExeLocation = "$tempExePath\nuget.exe"

if(!(Test-Path $nugetExeLocation)) {
    Write-Output "Downloading Latest Nuget.exe"
    Invoke-WebRequest $sourceNugetExe -OutFile $nugetExeLocation
    Set-Alias nuget $nugetExeLocation -Scope Global -Verbose
    Write-Output "Nuget.exe downloaded"
}

$nugetArguments = "install JetBrains.ReSharper.CommandLineTools -source https://api.nuget.org/v3/index.json -OutputDirectory $tempExePath"

Write-Output "downloading Resharper CLT $nugetExeLocation $nugetArguments"

Start-Process -FilePath "$nugetExeLocation" $nugetArguments -Wait

Write-Output "Resharper CLT downloaded"

$resharperPreInstalledDirectoryPath = [System.IO.Directory]::EnumerateDirectories($tempExePath, "*JetBrains*")[0]
$commandLineInterfacePath = [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($resharperPreInstalledDirectoryPath, "tools"));
$inspectCodeExePath =  [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($commandLineInterfacePath, "InspectCode.exe"));

if(!(Test-Path $inspectCodeExePath)) {
    Throw [System.IO.FileNotFoundException] "InspectCode.exe was not found at $inspectCodeExePath"
}

[string] $solutionOrProjectFullPath = [System.IO.Path]::GetFullPath($solutionOrProjectPath.Replace("`"",""));

if(!(Test-Path $solutionOrProjectFullPath)) {
    Throw [System.IO.FileNotFoundException] "No solution or project found at $solutionOrProjectFullPath"
}

[string] $inspectCodeResultsPath = [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($tempExePath, "Reports\CodeInspection_$buildId.xml"));

$severityLevels = @{"Hint" = 0; "Suggestion" = 1; "Warning" = 2; "Error" = 3}

Write-Verbose "Using Resharper Code Analysis found at '$inspectCodeExePath'"

Write-Output "Inspecting code for $solutionOrProjectPath"

# Run code analysis

$arguments = """$solutionOrProjectFullPath"" -o=""$inspectCodeResultsPath"" -s=WARNING --verbosity=WARN --swea --no-buildin-settings"

Write-Output "Invoking InspectCode.exe using arguments $arguments" 

Start-Process -FilePath $inspectCodeExePath -ArgumentList $arguments -Wait

# Analyse results

$xmlContent = [xml] (Get-Content "$inspectCodeResultsPath")
$issuesTypesXpath = "/Report/IssueTypes//IssueType"
$issuesTypesElements = $xmlContent | Select-Xml $issuesTypesXpath | Select-Object -Expand Node

$issuesXpath = "/Report/Issues//Issue"
$issuesElements = $xmlContent | Select-Xml $issuesXpath | Select-Object -Expand Node

$filteredElements = New-Object System.Collections.Generic.List[System.Object]

foreach($issue in $issuesElements) {
    $severity = @($issuesTypesElements | Where-Object {$_.Attributes["Id"].Value -eq $issue.Attributes["TypeId"].Value})[0].Attributes["Severity"].Value

    $severityLevel = $severityLevels[$severity]

    if($severityLevel -ge $severityLevels[$failBuildLevelSelector]) {
        $item = New-Object -TypeName PSObject -Property @{
            'Severity' = $severity
            'Message' = $issue.Attributes["Message"].Value
            'File' = $issue.Attributes["File"].Value
            'Line' = $issue.Attributes["Line"].Value
        }

        $filteredElements.Add($item)
    }
}

# Report results output
foreach ($issue in $filteredElements | Sort-Object Severity -Descending) {
    $errorType = "warning"
    if($issue.Severity -eq "Error"){
        $errorType = "error"
        Write-Warning ("issue type={0};sourcepath={1};linenumber={2};columnnumber=1] {3} to Supress \\Resharper Disable {4}" -f $errorType, $issue.File, $issue.Line, $issue.Message, $issue.TypeId)
    }
    Write-Error ("issue type={0};sourcepath={1};linenumber={2};columnnumber=1] {3} to Supress \\Resharper Disable {4}" -f $errorType, $issue.File, $issue.Line, $issue.Message, $issue.TypeId)
}

function Set-Results {
    param(
        [string]
        $summaryMessage,
        [ValidateSet("Succeeded", "Failed")]
        [string]
        $buildResult
    )
    if($buildResult -eq "Failed") {
        Write-Error ("result={0};]{1}" -f $buildResult, $summaryMessage)
    } else {
        Write-Information ("result={0};]{1}" -f $buildResult, $summaryMessage) -ForegroundColor Green
    }
}

$summaryMessage = ""

if($filteredElements.Count -eq 0) {
    Set-Results -summaryMessage "No code quality issues found" -buildResult "Succeeded"
} elseif($filteredElements.Count -eq 1) {
    Set-Results -summaryMessage "One code quality issue found" -buildResult "Failed"
} else {
    $summaryMessage = "{0} code quality issues found" -f $filteredElements.Count
    Set-Results -summaryMessage $summaryMessage -buildResult "Failed"
}

Remove-Item $tempExePath