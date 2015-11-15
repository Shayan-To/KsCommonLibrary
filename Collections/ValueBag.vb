Imports System.ComponentModel
Imports Ks.Common.MVVM

<TypeDescriptionProvider(GetType(ValueBag(Of)))>
Public Class ValueBag(Of T)
    Inherits BindableBase
    Implements IDictionary(Of String, T),
               IFormattable

    Public ReadOnly Property Count As Integer Implements ICollection(Of KeyValuePair(Of String, T)).Count
        Get
            Return Me.Dic.Count
        End Get
    End Property

    Private ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of KeyValuePair(Of String, T)).IsReadOnly
        Get
            Return DirectCast(Me.Dic, ICollection(Of KeyValuePair(Of String, T))).IsReadOnly
        End Get
    End Property

    Default Public Property Item(ByVal Key As String) As T Implements IDictionary(Of String, T).Item
        Get
            Return Me.Dic.Item(Key)
        End Get
        Set(ByVal Value As T)
            Me.Dic.Item(Key) = Value
            Me.NotifyPropertyChanged(Key)
        End Set
    End Property

    Private ReadOnly Property Int_Keys As ICollection(Of String) Implements IDictionary(Of String, T).Keys
        Get
            Return Me.Dic.Keys
        End Get
    End Property

    Public ReadOnly Property Keys As Dictionary(Of String, T).KeyCollection
        Get
            Return Me.Dic.Keys
        End Get
    End Property

    Private ReadOnly Property Int_Values As ICollection(Of T) Implements IDictionary(Of String, T).Values
        Get
            Return Me.Dic.Values
        End Get
    End Property

    Public ReadOnly Property Values As Dictionary(Of String, T).ValueCollection
        Get
            Return Me.Dic.Values
        End Get
    End Property

    Public Sub Add(ByVal Item As KeyValuePair(Of String, T)) Implements ICollection(Of KeyValuePair(Of String, T)).Add
        Me.Dic.Add(Item.Key, Item.Value)
        Me.NotifyPropertyChanged(Item.Key)
    End Sub

    Public Sub Add(ByVal Key As String, ByVal Value As T) Implements IDictionary(Of String, T).Add
        Me.Dic.Add(Key, Value)
        Me.NotifyPropertyChanged(Key)
    End Sub

    Public Sub Clear() Implements ICollection(Of KeyValuePair(Of String, T)).Clear
        Me.Dic.Clear()
    End Sub

    Public Sub CopyTo(array() As KeyValuePair(Of String, T), arrayIndex As Integer) Implements ICollection(Of KeyValuePair(Of String, T)).CopyTo
        DirectCast(Me.Dic, ICollection(Of KeyValuePair(Of String, T))).CopyTo(array, arrayIndex)
    End Sub

    Public Function Contains(item As KeyValuePair(Of String, T)) As Boolean Implements ICollection(Of KeyValuePair(Of String, T)).Contains
        Return DirectCast(Me.Dic, ICollection(Of KeyValuePair(Of String, T))).Contains(item)
    End Function

    Public Function ContainsKey(key As String) As Boolean Implements IDictionary(Of String, T).ContainsKey
        Return Me.Dic.ContainsKey(key)
    End Function

    Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return Me.Dic.GetEnumerator()
    End Function

    Private Function Int_GetEnumerator() As IEnumerator(Of KeyValuePair(Of String, T)) Implements IEnumerable(Of KeyValuePair(Of String, T)).GetEnumerator
        Return Me.Dic.GetEnumerator()
    End Function

    Public Function GetEnumerator() As Dictionary(Of String, T).Enumerator
        Return Me.Dic.GetEnumerator()
    End Function

    Public Function Remove(item As KeyValuePair(Of String, T)) As Boolean Implements ICollection(Of KeyValuePair(Of String, T)).Remove
        Return DirectCast(Me.Dic, ICollection(Of KeyValuePair(Of String, T))).Remove(item)
    End Function

    Public Function Remove(key As String) As Boolean Implements IDictionary(Of String, T).Remove
        Return Me.Dic.Remove(key)
    End Function

    Public Function TryGetValue(key As String, ByRef value As T) As Boolean Implements IDictionary(Of String, T).TryGetValue
        Return Me.Dic.TryGetValue(key, value)
    End Function

    Public Overrides Function ToString() As String
        Return Me.ToString("", Utilities.Text.CurruntFormatProvider)
    End Function

    Public Overloads Function ToString(ByVal format As String, ByVal formatProvider As IFormatProvider) As String Implements System.IFormattable.ToString
        Dim R = New Text.StringBuilder("{"c)
        Dim Bl = True
        For Each KV In Me
            If Bl Then
                Bl = False
            Else
                R.Append(", ")
            End If

            R.Append(KV.Key).Append(" : ").Append(String.Format(formatProvider, "{0:" & format & "}", KV.Value))
        Next

        Return R.Append("}"c).ToString()
    End Function

    Private ReadOnly Dic As Dictionary(Of String, T) = New Dictionary(Of String, T)()

End Class

Friend Class ValueBagTypeDescriptionProvider
    Inherits TypeDescriptionProvider

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal Type As Type)
        MyBase.New(TypeDescriptor.GetProvider(GetType(ValueBag(Of)).MakeGenericType(Type)))
    End Sub

    Public Sub New(ByVal Parent As TypeDescriptionProvider)
        MyBase.New(Parent)
    End Sub

    Public Overrides Function GetTypeDescriptor(ByVal ObjectType As Type, ByVal Instance As Object) As ICustomTypeDescriptor
        Return New ValueBagTypeDescriptor(MyBase.GetTypeDescriptor(ObjectType, Instance), TryCast(Instance, IDictionary))
    End Function

End Class

Friend Class ValueBagTypeDescriptor
    Inherits CustomTypeDescriptor

    Public Sub New(ByVal Descriptor As ICustomTypeDescriptor, ByVal Bag As IDictionary)
        MyBase.New(Descriptor)
        Me.Bag = Bag
        Dim BagType = Bag?.GetType()
        If Me.Bag Is Nothing Or BagType.GetGenericTypeDefinition() <> GetType(ValueBag(Of)) Then
            Throw New ArgumentException()
        End If
        Me.Type = BagType.GetGenericArguments()(0)
    End Sub

    Public Overrides Function GetProperties() As PropertyDescriptorCollection
        Dim BaseProps = MyBase.GetProperties()
        Dim R = New PropertyDescriptor(BaseProps.Count + Me.Bag.Count - 1) {}
        Dim I = 0
        For Each P As PropertyDescriptor In BaseProps
            R(I) = P
            I += 1
        Next
        For Each K In Me.Bag.Keys
            R(I) = New ValueBagPropertyDescriptor(DirectCast(K, String), Me.Type)
            I += 1
        Next
        Return New PropertyDescriptorCollection(R)
    End Function

    Private ReadOnly Bag As IDictionary
    Private ReadOnly Type As Type

End Class

Friend Class ValueBagPropertyDescriptor
    Inherits PropertyDescriptor

    Public Sub New(ByVal Name As String, ByVal Type As Type)
        MyBase.New(Name, Nothing)
        Me.PropName = Name
        Me.PropType = Type
    End Sub

    Public Overrides ReadOnly Property ComponentType As Type
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Public Overrides ReadOnly Property IsReadOnly As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides ReadOnly Property PropertyType As Type
        Get
            Return Me.PropType
        End Get
    End Property

    Public Overrides Sub ResetValue(component As Object)
        Throw New NotSupportedException()
    End Sub

    Public Overrides Sub SetValue(component As Object, value As Object)
        Throw New NotSupportedException()
    End Sub

    Public Overrides Function CanResetValue(component As Object) As Boolean
        Return False
    End Function

    Public Overrides Function GetValue(component As Object) As Object
        Return DirectCast(component, IDictionary).Item(Me.PropName)
    End Function

    Public Overrides Function ShouldSerializeValue(component As Object) As Boolean
        Return False
    End Function

    Private ReadOnly PropName As String
    Private ReadOnly PropType As Type

End Class
