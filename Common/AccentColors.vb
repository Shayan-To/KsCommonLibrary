Imports Media = System.Windows.Media

Public NotInheritable Class AccentColors

    Private Sub New()

    End Sub

    Public Shared ReadOnly Lime As Media.Color = Utilities.HexToColor("#A4C400")
    Public Shared ReadOnly Green As Media.Color = Utilities.HexToColor("#60A917")
    Public Shared ReadOnly Emerald As Media.Color = Utilities.HexToColor("#008A00")
    Public Shared ReadOnly Teal As Media.Color = Utilities.HexToColor("#00ABA9")
    Public Shared ReadOnly Cyan As Media.Color = Utilities.HexToColor("#1BA1E2")
    Public Shared ReadOnly Cobalt As Media.Color = Utilities.HexToColor("#0050EF")
    Public Shared ReadOnly Indigo As Media.Color = Utilities.HexToColor("#6A00FF")
    Public Shared ReadOnly Violet As Media.Color = Utilities.HexToColor("#AA00FF")
    Public Shared ReadOnly Pink As Media.Color = Utilities.HexToColor("#F472D0")
    Public Shared ReadOnly Magenta As Media.Color = Utilities.HexToColor("#D80073")
    Public Shared ReadOnly Crimson As Media.Color = Utilities.HexToColor("#A20025")
    Public Shared ReadOnly Red As Media.Color = Utilities.HexToColor("#E51400")
    Public Shared ReadOnly Orange As Media.Color = Utilities.HexToColor("#FA6800")
    Public Shared ReadOnly Amber As Media.Color = Utilities.HexToColor("#F0A30A")
    Public Shared ReadOnly Yellow As Media.Color = Utilities.HexToColor("#E3C800")
    Public Shared ReadOnly Brown As Media.Color = Utilities.HexToColor("#825A2C")
    Public Shared ReadOnly Olive As Media.Color = Utilities.HexToColor("#6D8764")
    Public Shared ReadOnly Steel As Media.Color = Utilities.HexToColor("#647687")
    Public Shared ReadOnly Mauve As Media.Color = Utilities.HexToColor("#76608A")
    Public Shared ReadOnly Taupe As Media.Color = Utilities.HexToColor("#87794E")

#If VBC_VER >= 11.0 Then
    Public Shared ReadOnly Iterator Property Colors As IEnumerable(Of Media.Color)
        Get
            Yield Lime
            Yield Green
            Yield Emerald
            Yield Teal
            Yield Cyan
            Yield Cobalt
            Yield Indigo
            Yield Violet
            Yield Pink
            Yield Magenta
            Yield Crimson
            Yield Red
            Yield Orange
            Yield Amber
            Yield Yellow
            Yield Brown
            Yield Olive
            Yield Steel
            Yield Mauve
            Yield Taupe
        End Get
    End Property
#End If

End Class
