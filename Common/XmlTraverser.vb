Namespace Common

    Public MustInherit Class XmlTraverser

        Public Shared Function Create(ByVal VisitDelegate As Action(Of Xml.XmlNode, VisitAction)) As DelegateXmlTraverser
            Return New DelegateXmlTraverser(VisitDelegate)
        End Function

        Protected MustOverride Sub Visit(ByVal Node As Xml.XmlNode, ByVal VisitAction As VisitAction)

        Public Sub Traverse(ByVal Node As Xml.XmlNode)
            Me._VisitAction.Reset()
            Me.Visit(Node, Me._VisitAction)

            If Me._VisitAction.TraverseChildren Then
                For Each Child As Xml.XmlNode In Node.ChildNodes
                    Me.Traverse(Child)
                Next
            End If
        End Sub

        Private ReadOnly _VisitAction As VisitAction = New VisitAction()

        Public Class VisitAction

            Public Sub New()
                Me.Reset()
            End Sub

            Public Sub Reset()
                Me.TraverseChildren = True
            End Sub

#Region "TraverseChildren Property"
            Private _TraverseChildren As Boolean

            Public Property TraverseChildren As Boolean
                Get
                    Return Me._TraverseChildren
                End Get
                Set(ByVal Value As Boolean)
                    Me._TraverseChildren = Value
                End Set
            End Property
#End Region

        End Class

    End Class

    Public Class DelegateXmlTraverser
        Inherits XmlTraverser

        Public Sub New(ByVal VisitDelegate As Action(Of Xml.XmlNode, VisitAction))
            Me.VisitDelegate = VisitDelegate
        End Sub

        Protected Overrides Sub Visit(Node As Xml.XmlNode, VisitAction As VisitAction)
            Me.VisitDelegate.Invoke(Node, VisitAction)
        End Sub

        Private ReadOnly VisitDelegate As Action(Of Xml.XmlNode, VisitAction)

    End Class

End Namespace
