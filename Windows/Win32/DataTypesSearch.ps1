$Global:ScriptDirectory = Split-Path -Parent $Script:MyInvocation.MyCommand.Path

. "$Global:ScriptDirectory\DataTypes.ps1"

Function MergeValues($Value, $NewValue)
{
    If (-Not ($Value -Is [HashTable] -BOr $Value -Is [String]) -BOr
        -Not ($NewValue -Is [HashTable] -BOr $NewValue -Is [String]))
    {
        Throw 'Invalid value.'
    }

    If ($Value -Is [String])
    {
        If ($NewValue -Is [String])
        {
            Return $Value + $NewValue
        }

        $R = @{}
        ForEach ($K In $NewValue.Keys)
        {
            $R[$K] = $Value + $NewValue[$K]
        }
        Return $R
    }

    If ($NewValue -Is [String])
    {
        ForEach ($K In @($Value.Keys))
        {
            $Value[$K] += $NewValue
        }
        Return $Value
    }

    $T = [System.Linq.Enumerable]::Count([System.Linq.Enumerable]::Intersect($Value.Keys, $NewValue.Keys))

    If ($T -Eq 0)
    {
        ForEach ($K In @($Value.Keys))
        {
            $Value[$K] = MergeValues ($Value[$K]) $NewValue
        }
        Return $Value
    }

    If ($T -Eq $Value.Count -And $T -Eq $NewValue.Count)
    {
        ForEach ($K In @($Value.Keys))
        {
            $Value[$K] = MergeValues ($Value[$K]) ($NewValue[$K])
        }
        Return $Value
    }

    Throw 'Cannot merge partially intersecting dictionaries.'
}

Function NarrowType($Type)
{
    $Changed = $False
    $R = ''
    ForEach ($I In $Type -Split '(?<=[a-z0-9_])(?![a-z0-9_])|(?<![a-z0-9_])(?=[a-z0-9_])')
    {
        $T = $Dic[$I]
        If ($T -Eq $Null)
        {
            $T = $I
        }
        Else
        {
            $Changed = $True
        }
        $R = MergeValues $R $T
    }

    If (-Not $Changed)
    {
        Return $Null
    }
    Return $R
}

Function WriteDataTypes($Type, $Indent = 0)
{
    If ($Type -Is [String])
    {
        Write-Host -NoNewline $Type
        $Type = NarrowType $Type
        If ($Type -NE $Null)
        {
            Write-Host -NoNewline -ForegroundColor Red " -> "
            WriteDataTypes $Type $Indent
        }
    }
    Else
    {
        If ($Type -IsNot [HashTable])
        {
            Throw 'Invalid type.'
        }
        $Indent += 1
        Write-Host '{'
        ForEach ($Key In $Type.Keys)
        {
            Write-Host -NoNewline ('  ' * $Indent)
            Write-Host -NoNewline $Key
            Write-Host -NoNewline ' : '
            WriteDataTypes ($Type[$Key]) $Indent
            Write-Host
        }
        Write-Host -NoNewline ('  ' * ($Indent - 1))
        Write-Host -NoNewline '}'
    }
}

While ($True)
{
    $Type = Read-Host
    If ($Type -Eq '*')
    {
        ForEach ($I In $Dic.Keys)
        {
            WriteDataTypes $I
            Write-Host
        }
    }
    Else
    {
        WriteDataTypes ($Type.Trim())
        Write-Host
    }
}
