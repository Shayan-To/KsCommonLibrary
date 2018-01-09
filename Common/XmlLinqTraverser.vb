Namespace Common

    Public MustInherit Class XmlLinqTraverser

        Public Shared Function Create(ByVal VisitDelegate As Action(Of XElement, VisitAction)) As DelegateXmlLinqTraverser
            Return New DelegateXmlLinqTraverser(VisitDelegate)
        End Function

        Protected MustOverride Sub Visit(ByVal Node As XElement, ByVal VisitAction As VisitAction)

        Public Function Traverse(ByVal Node As XElement) As XElement
            Dim NodeReplaced = False

            Me._VisitAction.Reset(Node)
            Me.Visit(Node, Me._VisitAction)
            If Me._VisitAction.ReplaceWithElement IsNot Node Then
                Node.ReplaceWith(Me._VisitAction.ReplaceWithElement)
                Node = Me._VisitAction.ReplaceWithElement
                NodeReplaced = True
            End If

            If Me._VisitAction.TraverseChildren Then
                Dim ChangedChild As XElement = Nothing

                For Each Child In Node.Elements()
                    ChangedChild = Me.Traverse(Child)
                    If ChangedChild IsNot Nothing Then
                        Exit For
                    End If
                Next

                Do Until ChangedChild Is Nothing
                    Dim T = ChangedChild
                    ChangedChild = Nothing
                    For Each Child In T.ElementsAfterSelf()
                        ChangedChild = Me.Traverse(Child)
                        If ChangedChild IsNot Nothing Then
                            Exit For
                        End If
                    Next
                Loop
            End If

            If NodeReplaced Then
                Return Node
            End If

            Return Nothing
        End Function

        Private ReadOnly _VisitAction As VisitAction = New VisitAction()

        Public Class VisitAction

            Public Sub New()
                Me.Reset(Nothing)
            End Sub

            Public Sub Reset(ByVal Element As XElement)
                Me.TraverseChildren = True
                Me.ReplaceWithElement = Element
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

#Region "ReplaceWithElement Property"
            Private _ReplaceWithElement As XElement

            Public Property ReplaceWithElement As XElement
                Get
                    Return Me._ReplaceWithElement
                End Get
                Set(ByVal Value As XElement)
                    Me._ReplaceWithElement = Value
                End Set
            End Property
#End Region

        End Class

    End Class

    Public Class DelegateXmlLinqTraverser
        Inherits XmlLinqTraverser

        Public Sub New(ByVal VisitDelegate As Action(Of XElement, VisitAction))
            Me.VisitDelegate = VisitDelegate
        End Sub

        Protected Overrides Sub Visit(Node As XElement, VisitAction As VisitAction)
            Me.VisitDelegate.Invoke(Node, VisitAction)
        End Sub

        Private ReadOnly VisitDelegate As Action(Of XElement, VisitAction)

    End Class

End Namespace
