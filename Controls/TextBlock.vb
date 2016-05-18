Imports System.Text.RegularExpressions
Imports Ks.Common.MVVM

Namespace Controls

    Public Class TextBlock
        Inherits Control

        Shared Sub New()
            DefaultStyleKeyProperty.OverrideMetadata(GetType(TextBlock), New FrameworkPropertyMetadata(GetType(TextBlock)))
        End Sub

        Friend Sub ReportObjGotIn(ByVal Obj As Obj)
            Me.AddLogicalChild(Obj)
        End Sub

        Friend Sub ReportObjWentOut(ByVal Obj As Obj)
            Me.RemoveLogicalChild(Obj)
        End Sub

        Friend Sub ReportObjChanged()
            Verify.True(Me.Text Is Nothing, "Cannot use objs with Text. FText must be used.")
            Verify.True(Me.Obj Is Nothing Or Me.Objs.Count = 0, "Cannot use both Obj abd Objs.")
            Me.UpdateText(Me.FText)
        End Sub

        Private Sub UpdateText()
            Dim Text = Me.Text
            If Text IsNot Nothing Then
                Me.OutText = If(GetKsLanguage(Me)?.Translation(Text), Text)
                Exit Sub
            End If

            Me.UpdateText(Me.FText)
        End Sub

        Private Sub UpdateText(ByVal S As String)
            If S Is Nothing Then
                S = "{0}"
            End If

            Verify.False(Regex.IsMatch(S, "`[^`\[\]\{\}]"), "Invalid escape sequence.")

            S = Regex.Replace(S, "`[\[\]\{\}]", Function(M) "`" & "[]{}".IndexOf(M.Value.Chars(1)).ToString())
            Dim UnEscape = Function(T As String) Regex.Replace(T, "`([0123])", Function(M) "[]{}".Chars(Integer.Parse(M.Groups.Item(1).Value)).ToString())

            If S.StartsWith("{}") Then
                S = S.Substring(2)
            End If

            Dim Lang = GetKsLanguage(Me)

            Dim Objs As IList(Of Obj)
            Dim Obj = Me.Obj
            If Obj IsNot Nothing Then
                Objs = Me.Obj1Array
                Objs.Item(0).Obj = Obj
            Else
                Objs = Me.Objs
            End If

            S = Regex.Replace(S, "\{\{(\d+)((?:,[+-]?\d+)?(?::[^\{\}]*)?)\}\}",
                              Function(M)
                                  Dim I = Integer.Parse(M.Groups.Item(1).Value)
                                  If I >= Objs.Count Then
                                      Return ""
                                  End If

                                  Dim T = String.Format(Globalization.CultureInfo.InvariantCulture, "{0" & UnEscape.Invoke(M.Groups(2).Value) & "}", Objs.Item(I).Obj)
                                  Return T
                              End Function)
            S = Regex.Replace(S, "\{(\d+)((?:,[+-]?\d+)?(?::[^\{\}]*)?)\}",
                              Function(M)
                                  Dim I = Integer.Parse(M.Groups.Item(1).Value)
                                  If I >= Objs.Count Then
                                      Return ""
                                  End If

                                  Dim T = String.Format(Globalization.CultureInfo.InvariantCulture, "{0" & UnEscape.Invoke(M.Groups(2).Value) & "}", Objs.Item(I).Obj)
                                  Return Me.CorrectString(T, Lang)
                              End Function)

            Verify.False(S.Contains("{") Or S.Contains("}"), "Invalid format string.")

            S = Regex.Replace(S, "\[\[([^\[\]]*)\]\]",
                              Function(M)
                                  Dim T = UnEscape.Invoke(M.Groups.Item(1).Value)
                                  Return Me.CorrectString(T, Lang)
                              End Function)
            S = Regex.Replace(S, "\[([^\[\]]*)\]",
                              Function(M)
                                  Dim T = UnEscape.Invoke(M.Groups.Item(1).Value)
                                  Return If(Lang?.Translation(T), T)
                              End Function)

            Verify.False(S.Contains("[") Or S.Contains("]"), "Invalid format string.")

            S = UnEscape.Invoke(S)
            Me.OutText = S
        End Sub

        Private Function CorrectString(ByVal S As String, ByVal Lang As KsLanguage) As String
            If Lang Is Nothing Then
                Return S
            End If

            If Lang.Id.ToLowerInvariant() = "pes" Then
                Dim OldDigits = "0123456789"
                Dim NewDigits = "۰۱۲۳۴۵۶۷۸۹"
                For I As Integer = 0 To 9
                    S = S.Replace(OldDigits.Chars(I), NewDigits.Chars(I))
                Next
                Return S
            End If

            Return S
        End Function

        Protected Overrides ReadOnly Property LogicalChildren As IEnumerator
            Get
                Return Me.Objs.GetEnumerator()
            End Get
        End Property

#Region "KsLanguage Property"
        Public Shared ReadOnly KsLanguageProperty As DependencyProperty = DependencyProperty.RegisterAttached("KsLanguage", GetType(KsLanguage), GetType(TextBlock), New FrameworkPropertyMetadata(Nothing, FrameworkPropertyMetadataOptions.Inherits, AddressOf KsLanguage_Changed, AddressOf KsLanguage_Coerce))

        Private Shared Function KsLanguage_Coerce(ByVal D As DependencyObject, ByVal BaseValue As Object) As Object
            'Dim Self = TryCast(D, UIElement)
            'If Self Is Nothing Then
            '    Return KsLanguageProperty.DefaultMetadata.DefaultValue
            'End If

            'Dim Value = DirectCast(BaseValue, KsLanguage)

            Return BaseValue
        End Function

        Private Shared Sub KsLanguage_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
            Dim Self = TryCast(D, TextBlock)

            If Self Is Nothing Then
                Exit Sub
            End If

            'Dim OldValue = DirectCast(E.OldValue, KsLanguage)
            'Dim NewValue = DirectCast(E.NewValue, KsLanguage)

            Self.UpdateText()
        End Sub

        Public Shared Function GetKsLanguage(ByVal Element As DependencyObject) As KsLanguage
            Verify.NonNullArg(Element, NameOf(Element))
            Return DirectCast(Element.GetValue(KsLanguageProperty), KsLanguage)
        End Function

        Public Shared Sub SetKsLanguage(ByVal Element As DependencyObject, ByVal Value As KsLanguage)
            Verify.NonNullArg(Element, NameOf(Element))
            Element.SetValue(KsLanguageProperty, Value)
        End Sub
#End Region

#Region "OutText Property"
        Private Shared ReadOnly OutTextPropertyKey As DependencyPropertyKey = DependencyProperty.RegisterReadOnly("OutText", GetType(String), GetType(TextBlock), New PropertyMetadata(Nothing))
        Public Shared ReadOnly OutTextProperty As DependencyProperty = OutTextPropertyKey.DependencyProperty

        Public Property OutText As String
            Get
                Return DirectCast(Me.GetValue(OutTextProperty), String)
            End Get
            Private Set(ByVal value As String)
                Me.SetValue(OutTextPropertyKey, value)
            End Set
        End Property
#End Region

#Region "Text Property"
        Public Shared ReadOnly TextProperty As DependencyProperty = DependencyProperty.Register("Text", GetType(String), GetType(TextBlock), New PropertyMetadata(Nothing, AddressOf Text_Changed, AddressOf Text_Coerce))

        Private Shared Function Text_Coerce(ByVal D As DependencyObject, ByVal BaseValue As Object) As Object
            'Dim Self = DirectCast(D, TextBlock)

            'Dim Value = DirectCast(BaseValue, String)

            Return BaseValue
        End Function

        Private Shared Sub Text_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
            Dim Self = DirectCast(D, TextBlock)

            'Dim OldValue = DirectCast(E.OldValue, String)
            Dim NewValue = DirectCast(E.NewValue, String)

            Verify.True(Self.FText Is Nothing, "Cannot set both Text and FText.")

            Dim Lang = GetKsLanguage(Self)
            Self.OutText = If(Lang Is Nothing, NewValue, Lang.Translation(NewValue))
        End Sub

        Public Property Text As String
            Get
                Return DirectCast(Me.GetValue(TextProperty), String)
            End Get
            Set(ByVal value As String)
                Me.SetValue(TextProperty, value)
            End Set
        End Property
#End Region

#Region "FText Property"
        Public Shared ReadOnly FTextProperty As DependencyProperty = DependencyProperty.Register("FText", GetType(String), GetType(TextBlock), New PropertyMetadata(Nothing, AddressOf FText_Changed, AddressOf FText_Coerce))

        Private Shared Function FText_Coerce(ByVal D As DependencyObject, ByVal BaseValue As Object) As Object
            Dim Self = DirectCast(D, TextBlock)

            Dim Value = DirectCast(BaseValue, String)

            Return BaseValue
        End Function

        Private Shared Sub FText_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
            Dim Self = DirectCast(D, TextBlock)

            'Dim OldValue = DirectCast(E.OldValue, String)
            Dim NewValue = DirectCast(E.NewValue, String)

            Verify.True(Self.Text Is Nothing, "Cannot set both Text and FText.")

            Self.UpdateText(NewValue)
        End Sub

        Public Property FText As String
            Get
                Return DirectCast(Me.GetValue(FTextProperty), String)
            End Get
            Set(ByVal value As String)
                Me.SetValue(FTextProperty, value)
            End Set
        End Property
#End Region

#Region "Obj Property"
        Public Shared ReadOnly ObjProperty As DependencyProperty = DependencyProperty.Register("Obj", GetType(Object), GetType(TextBlock), New PropertyMetadata(Nothing, AddressOf Obj_Changed, AddressOf Obj_Coerce))

        Private Shared Function Obj_Coerce(ByVal D As DependencyObject, ByVal BaseValue As Object) As Object
            'Dim Self = DirectCast(D, TextBlock)

            'Dim Value = DirectCast(BaseValue, Object)

            Return BaseValue
        End Function

        Private Shared Sub Obj_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
            Dim Self = DirectCast(D, TextBlock)

            'Dim OldValue = DirectCast(E.OldValue, Object)
            'Dim NewValue = DirectCast(E.NewValue, Object)

            Self.ReportObjChanged()
        End Sub

        Public Property Obj As Object
            Get
                Return DirectCast(Me.GetValue(ObjProperty), Object)
            End Get
            Set(ByVal value As Object)
                Me.SetValue(ObjProperty, value)
            End Set
        End Property
#End Region

#Region "Objs Property"
        Private ReadOnly _Objs As ObjList = New ObjList(Me)

        Public ReadOnly Property Objs As ObjList
            Get
                Return Me._Objs
            End Get
        End Property
#End Region

#Region "TextAlignment Property"
        Public Shared ReadOnly TextAlignmentProperty As DependencyProperty = DependencyProperty.Register("TextAlignment", GetType(TextAlignment), GetType(TextBlock), New PropertyMetadata(TextAlignment.Left, AddressOf TextAlignment_Changed, AddressOf TextAlignment_Coerce))

        Private Shared Function TextAlignment_Coerce(ByVal D As DependencyObject, ByVal BaseValue As Object) As Object
            'Dim Self = DirectCast(D, TextBlock)

            'Dim Value = DirectCast(BaseValue, TextAlignment)

            Return BaseValue
        End Function

        Private Shared Sub TextAlignment_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
            'Dim Self = DirectCast(D, TextBlock)

            'Dim OldValue = DirectCast(E.OldValue, TextAlignment)
            'Dim NewValue = DirectCast(E.NewValue, TextAlignment)
        End Sub

        Public Property TextAlignment As TextAlignment
            Get
                Return DirectCast(Me.GetValue(TextAlignmentProperty), TextAlignment)
            End Get
            Set(ByVal value As TextAlignment)
                Me.SetValue(TextAlignmentProperty, value)
            End Set
        End Property
#End Region

#Region "TextWrapping Property"
        Public Shared ReadOnly TextWrappingProperty As DependencyProperty = DependencyProperty.Register("TextWrapping", GetType(TextWrapping), GetType(TextBlock), New PropertyMetadata(TextWrapping.NoWrap, AddressOf TextWrapping_Changed, AddressOf TextWrapping_Coerce))

        Private Shared Function TextWrapping_Coerce(ByVal D As DependencyObject, ByVal BaseValue As Object) As Object
            'Dim Self = DirectCast(D, TextBlock)

            'Dim Value = DirectCast(BaseValue, TextWrapping)

            Return BaseValue
        End Function

        Private Shared Sub TextWrapping_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
            'Dim Self = DirectCast(D, TextBlock)

            'Dim OldValue = DirectCast(E.OldValue, TextWrapping)
            'Dim NewValue = DirectCast(E.NewValue, TextWrapping)
        End Sub

        Public Property TextWrapping As TextWrapping
            Get
                Return DirectCast(Me.GetValue(TextWrappingProperty), TextWrapping)
            End Get
            Set(ByVal value As TextWrapping)
                Me.SetValue(TextWrappingProperty, value)
            End Set
        End Property
#End Region

        Private ReadOnly Obj1Array As Obj() = New Obj(0) {New Obj()}

    End Class

End Namespace
