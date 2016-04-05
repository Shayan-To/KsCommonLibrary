Imports Ks.Common.MVVM

Namespace Controls

    Public Class TextBlock
        Inherits Control

        Shared Sub New()
            DefaultStyleKeyProperty.OverrideMetadata(GetType(TextBlock), New FrameworkPropertyMetadata(GetType(TextBlock)))
        End Sub

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
            Dim NewValue = DirectCast(E.NewValue, KsLanguage)

            Self.TranslatedText = If(NewValue Is Nothing, Self.Text, NewValue.Translation(Self.Text))
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

#Region "TranslatedText Property"
        Private Shared ReadOnly TranslatedTextPropertyKey As DependencyPropertyKey = DependencyProperty.RegisterReadOnly("TranslatedText", GetType(String), GetType(TextBlock), New PropertyMetadata(Nothing))
        Public Shared ReadOnly TranslatedTextProperty As DependencyProperty = TranslatedTextPropertyKey.DependencyProperty

        Public Property TranslatedText As String
            Get
                Return DirectCast(Me.GetValue(TranslatedTextProperty), String)
            End Get
            Private Set(ByVal value As String)
                Me.SetValue(TranslatedTextPropertyKey, value)
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

            Dim Lang = GetKsLanguage(Self)
            Self.TranslatedText = If(Lang Is Nothing, NewValue, Lang.Translation(NewValue))
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

    End Class

End Namespace
