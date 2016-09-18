Imports System.ComponentModel
Imports System.Runtime.CompilerServices

Namespace MVVM

    Public MustInherit Class AutoStoreObject
        Implements INotifyPropertyChanged

        Protected Sub New()

        End Sub

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Protected Overridable Function GetStoreKey(ByVal PropertyName As String) As String
            Return PropertyName
        End Function

        Public Sub Initialize(ByVal Dictionary As IDictionary(Of String, String))
            Verify.True(Me._StoreDictionary Is Nothing, "Object has already been initialized.")
            Me._StoreDictionary = Dictionary

            Me.OnBeforeInitialize()

            For Each T In Me.GetType().GetBaseTypes()
                For Each KV In MetadataDic.Item(T)
                    Dim V As String = Nothing
                    If Dictionary.TryGetValue(Me.GetStoreKey(KV.Key), V) Then
                        KV.Value.SetCallback.Invoke(Me, V)
                    End If
                Next
            Next

            Me.OnInitialize()

            Me._IsInitialized = True
        End Sub

        Protected Overridable Sub OnInitialize()

        End Sub

        Protected Overridable Sub OnBeforeInitialize()

        End Sub

        Protected Function SetProperty(Of T)(ByRef Source As T, ByVal Value As T, <CallerMemberName()> Optional ByVal PropertyName As String = Nothing) As Boolean
            Verify.True(Me.IsInitialized, "Object is not initialized.")

            If Not Object.Equals(Source, Value) Then
                Source = Value
                Me.NotifyPropertyChanged(PropertyName)

                For Each T In Me.GetType().GetBaseTypes()
                    Dim M As PropertyMetadata = Nothing
                    If MetadataDic.Item(T).TryGetValue(PropertyName, M) Then
                        Dim Str = M.ToStringCallback.Invoke(Source)
                        StoreDictionary.Item(Me.GetStoreKey(PropertyName)) = Str
                        Exit For
                    End If
                Next

                Return True
            End If

            Return False
        End Function

        Protected Overridable Sub OnPropertyChanged(ByVal E As PropertyChangedEventArgs)
            RaiseEvent PropertyChanged(Me, E)
        End Sub

        Protected Sub NotifyPropertyChanged(<CallerMemberName()> Optional ByVal PropertyName As String = Nothing)
            Me.OnPropertyChanged(New PropertyChangedEventArgs(PropertyName))
        End Sub

        Protected Shared Function RegisterProperty(ByVal OwnerType As Type, ByVal Name As String, Optional ByVal SetCallback As Action(Of AutoStoreObject, String) = Nothing, Optional ByVal ToStringCallBack As Func(Of Object, String) = Nothing) As Object
            For Each T In OwnerType.GetBaseTypes()
                Verify.False(MetadataDic.Item(T).ContainsKey(Name), "Name already eists.")
            Next

            If SetCallback Is Nothing Then
                SetCallback = Sub(M, O)
                                  M.GetType().GetProperty(Name).SetValue(M, O) ' ToDo Invoke CType on O.
                              End Sub
            End If
            If ToStringCallBack Is Nothing Then
                ToStringCallBack = Function(O) String.Format(Globalization.CultureInfo.InvariantCulture, "{0}", O)
            End If
            MetadataDic.Item(OwnerType).Add(Name, New PropertyMetadata(SetCallback, ToStringCallBack))
            Return Nothing
        End Function

#Region "IsInitialized Property"
        Private _IsInitialized As Boolean

        Public ReadOnly Property IsInitialized As Boolean
            Get
                Return Me._IsInitialized
            End Get
        End Property
#End Region

#Region "StoreDictionary Property"
        Private _StoreDictionary As IDictionary(Of String, String)

        Protected ReadOnly Property StoreDictionary As IDictionary(Of String, String)
            Get
                Return Me._StoreDictionary
            End Get
        End Property
#End Region

        Private Shared ReadOnly MetadataDic As CreateInstanceDictionary(Of Type, Dictionary(Of String, PropertyMetadata)) = New CreateInstanceDictionary(Of Type, Dictionary(Of String, PropertyMetadata))()

        Private Structure PropertyMetadata

            Public Sub New(ByVal SetCallback As Action(Of AutoStoreObject, String), ByVal ToStringCallback As Func(Of Object, String))
                Me._SetCallback = SetCallback
                Me._ToStringCallback = ToStringCallback
            End Sub

#Region "SetCallback Property"
            Private ReadOnly _SetCallback As Action(Of AutoStoreObject, String)

            Public ReadOnly Property SetCallback As Action(Of AutoStoreObject, String)
                Get
                    Return Me._SetCallback
                End Get
            End Property
#End Region

#Region "ToStringCallback Property"
            Private ReadOnly _ToStringCallback As Func(Of Object, String)

            Public ReadOnly Property ToStringCallback As Func(Of Object, String)
                Get
                    Return Me._ToStringCallback
                End Get
            End Property
#End Region

        End Structure

    End Class

End Namespace
