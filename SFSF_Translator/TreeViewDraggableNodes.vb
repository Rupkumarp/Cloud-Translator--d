Imports System.ComponentModel

Public Class TreeViewDraggableNodes
    Inherits TreeView

    Private Declare Unicode Function SetWindowTheme Lib "uxtheme.dll" (hWnd As IntPtr, pszSubAppName As String, pszSubIdList As String) As Integer

    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle()
        SetWindowTheme(Me.Handle, "Explorer", Nothing)
    End Sub

#Region "Events"

    Public Event NodeMovedByDrag As EventHandler(Of NodeMovedByDragEventArgs)

    Public Event MyNodeSelected(ByVal NodeText As String)
    Public Event SetMaster()
    Public Event UnSetMaster()

    Protected Overridable Sub OnNodeMovedByDrag(ByVal e As NodeMovedByDragEventArgs)
        RaiseEvent NodeMovedByDrag(Me, e)
    End Sub

    Public Event NodeMovingByDrag As EventHandler(Of NodeMovingByDragEventArgs)

    Protected Overridable Sub OnNodeMovingByDrag(ByVal e As NodeMovingByDragEventArgs)
        RaiseEvent NodeMovingByDrag(Me, e)
    End Sub

    Public Event NodeDraggingOver As EventHandler(Of NodeDraggingOverEventArgs)

    Protected Overridable Sub OnNodeDraggingOver(ByVal e As NodeDraggingOverEventArgs)
        RaiseEvent NodeDraggingOver(Me, e)
    End Sub

#End Region

    Sub New()
        MyBase.AllowDrop = True
        'MyBase.DrawMode = TreeViewDrawMode.OwnerDrawAll
    End Sub

    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Shadows ReadOnly Property DrawMode() As System.Windows.Forms.TreeViewDrawMode
        Get
            Return (MyBase.DrawMode)
        End Get
    End Property

    '<DefaultValue(True)> _
    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Shadows ReadOnly Property AllowDrop() As Boolean
        Get
            Return (MyBase.AllowDrop)
        End Get
    End Property

    Protected Overrides Sub OnItemDrag(ByVal e As System.Windows.Forms.ItemDragEventArgs)
        MyBase.DoDragDrop(e.Item, DragDropEffects.Move)
        MyBase.OnItemDrag(e)
    End Sub

    Protected Overrides Sub OnDragOver(ByVal drgevent As System.Windows.Forms.DragEventArgs)
        'Get the node we are currently over
        'while dragging another node
        Dim targetNode As TreeNode = MyBase.GetNodeAt(MyBase.PointToClient(New Point(drgevent.X, drgevent.Y)))

        'Get the node being dragged
        Dim dragNode As TreeNode = FindNodeInDataObject(drgevent.Data)

        Dim eaDraggingOver As New NodeDraggingOverEventArgs(dragNode, targetNode)
        OnNodeDraggingOver(eaDraggingOver)

        If eaDraggingOver.DropIsLegal = False Then
            drgevent.Effect = DragDropEffects.None
            Return
        End If


        Dim TT_DragNode As TreeNodeTag = dragNode.Tag

        If TT_DragNode.TI = TreeNodeTag.TagIndex.Root Or TT_DragNode.TI = TreeNodeTag.TagIndex.ProjectGroup Then
            drgevent.Effect = DragDropEffects.None
            Return
        End If


        'If we are not currently dragging over
        'a node...
        If targetNode Is Nothing Then
            'Let no node be selected
            MyBase.SelectedNode = Nothing

            'Allow the move because its valid
            'to drag a node over the TreeView itself
            'the drop will place the node being dragged
            'in the root

            drgevent.Effect = DragDropEffects.None

            'Get out
            Return
        End If

        'This would only be nothing if something is being
        'dragged over the TreeView that isn't a node
        If dragNode IsNot Nothing Then
            Dim TT_TargetNode As TreeNodeTag = targetNode.Tag
            'Illegal to drop nodes inside their descendants
            'Its not logical
            If targetNode Is dragNode OrElse IsNodeDescendant(targetNode, dragNode) Then
                'Prevents a drop
                drgevent.Effect = DragDropEffects.None
            Else
                'Allows a drop
                drgevent.Effect = DragDropEffects.Move

                If TT_TargetNode.TI = TreeNodeTag.TagIndex.ProjectGroup Then
                    choice = "child"
                ElseIf TT_TargetNode.TI = TreeNodeTag.TagIndex.ProjectName Then
                    choice = "sibling"
                    If TT_TargetNode.isMaster Then
                        targetNode.SelectedImageIndex = TreeNodeTag.MasterImageIndex
                    Else
                        targetNode.SelectedImageIndex = TT_TargetNode.ImageIndex
                    End If

                Else
                    drgevent.Effect = DragDropEffects.None
                End If

                MytargetNode = targetNode.Text
                MyBase.SelectedNode = targetNode

            End If

        End If


        ''This would only be nothing if something is being
        ''dragged over the TreeView that isn't a node
        'If dragNode IsNot Nothing Then

        '    'Illegal to drop nodes inside their descendants
        '    'Its not logical
        '    If targetNode Is dragNode OrElse IsNodeDescendant(targetNode, dragNode) Then
        '        'Prevents a drop
        '        drgevent.Effect = DragDropEffects.None
        '    Else
        '        'Allows a drop
        '        drgevent.Effect = DragDropEffects.Move

        '        If targetNode.Tag = 2 Then
        '            choice = "child"
        '        ElseIf targetNode.Tag = 3 Then
        '            choice = "sibling"
        '            targetNode.SelectedImageIndex = 13
        '        ElseIf targetNode.Tag = 1 Then
        '            choice = "sibling"
        '            targetNode.SelectedImageIndex = 18
        '        Else
        '            drgevent.Effect = DragDropEffects.None
        '        End If

        '        'If targetNode.ImageIndex = 0 Then
        '        '    choice = "child"
        '        'ElseIf targetNode.ImageIndex = 13 Or targetNode.ImageIndex = 12 Then
        '        '    choice = "sibling"
        '        '    targetNode.SelectedImageIndex = 13
        '        'ElseIf targetNode.ImageIndex = 18 Then
        '        '    choice = "sibling"
        '        '    targetNode.SelectedImageIndex = 18
        '        'Else
        '        '    drgevent.Effect = DragDropEffects.None
        '        'End If

        '        MytargetNode = targetNode.Text
        '        MyBase.SelectedNode = targetNode

        '    End If

        'End If

        MyBase.OnDragOver(drgevent)

    End Sub

    'Private WithEvents cms As New ContextMenuStrip
    Dim choice As String = ""
    Dim MytargetNode As String = ""

    Protected Overrides Sub OnDragDrop(ByVal drgevent As System.Windows.Forms.DragEventArgs)
        Dim dragNode As TreeNode = FindNodeInDataObject(drgevent.Data)

        Dim BUnsetMaster As Boolean = False

        If dragNode IsNot Nothing Then

            Dim prevParent As TreeNode = dragNode.Parent

            Dim parentToBe As TreeNode = If(MyBase.SelectedNode Is Nothing, Nothing, If(choice = "child", MyBase.SelectedNode, If(choice = "sibling", MyBase.SelectedNode.Parent, Nothing)))
            If parentToBe Is Nothing Then
                parentToBe = MyBase.SelectedNode
            ElseIf parentToBe.ImageIndex = 10 Then
                drgevent.Effect = DragDropEffects.None
                Exit Sub
            End If
            Dim eaNodeMoving As New NodeMovingByDragEventArgs(dragNode, prevParent, parentToBe)

            choice = ""

            OnNodeMovingByDrag(eaNodeMoving)

            Dim TT_DragNode As TreeNodeTag = dragNode.Tag

            If parentToBe Is Nothing Then
                drgevent.Effect = DragDropEffects.None
                Exit Sub
            End If

            Dim TT_parentToBe As TreeNodeTag = parentToBe.Tag


            If eaNodeMoving.CancelMove = False Then
                If TT_DragNode.TI = TreeNodeTag.TagIndex.ProjectGroup Then
                    Exit Sub
                End If
                If parentToBe IsNot Nothing Then
                    Dim bMasterAvailalbe As Boolean = False
                    If TT_DragNode.isMaster Then
                        If isMasterInProject(parentToBe) Then
                            bMasterAvailalbe = True

                            Select Case MsgBox("There is already Master project set in that Project Group!" & vbNewLine & "Are you sure you want to set the selected one as Master?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNoCancel, "?")
                                Case MsgBoxResult.Cancel
                                    Exit Sub
                                Case MsgBoxResult.No
                                    BUnsetMaster = True
                                Case MsgBoxResult.Yes
                                    BUnsetMaster = False
                            End Select
                           
                        End If
                    End If
                    dragNode.Remove()
                    parentToBe.Nodes.Add(dragNode)
                    If TT_DragNode.isMaster Then
                        dragNode.SelectedImageIndex = TreeNodeTag.MasterImageIndex
                        dragNode.ImageIndex = TreeNodeTag.MasterImageIndex
                    Else
                        dragNode.SelectedImageIndex = 12
                        dragNode.ImageIndex = 12
                    End If

                    RaiseEvent MyNodeSelected(dragNode.Text)

                    If BUnsetMaster Then
                        RaiseEvent UnSetMaster()
                    ElseIf bMasterAvailalbe Then
                        RaiseEvent SetMaster()
                    End If

                Else
                    'dragNode.Remove()
                    'MyBase.Nodes.Add(dragNode)
                    'RaiseEvent MyNodeSelected(dragNode.Text)
                End If

                OnNodeMovedByDrag(New NodeMovedByDragEventArgs(dragNode, prevParent))
            End If


        End If
        MyBase.OnDragDrop(drgevent)

    End Sub

    Public Function isMasterAlready(ByVal ParentNode As TreeNode, ByVal Projectname As String) As Boolean
        Dim TT As TreeNodeTag
        For i As Integer = 0 To ParentNode.Nodes.Count - 1
            TT = ParentNode.Nodes(i).Tag
            If TT.isMaster And ParentNode.Nodes(i).Text.ToLower = Projectname.ToLower Then
                Return True
            End If
        Next
        Return False
    End Function

    Public Function isMasterInProject(ByVal ParentNode As TreeNode) As Boolean
        Dim TT As TreeNodeTag
        For i As Integer = 0 To ParentNode.Nodes.Count - 1
            TT = ParentNode.Nodes(i).Tag
            If TT.isMaster Then
                Return True
            End If
        Next
        Return False
    End Function

    Public Function GetMasterProjectName(ByVal ParentNode As TreeNode) As String
        Dim MasterProjectName As String = ""
        Dim TT As TreeNodeTag
        For i As Integer = 0 To ParentNode.Nodes.Count - 1
            TT = ParentNode.Nodes(i).Tag
            If TT.isMaster Then
                MasterProjectName = ParentNode.Nodes(i).Text
                Exit For
            End If
        Next
        Return MasterProjectName
    End Function

    'Private Sub childClicked(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    choice = "child"
    '    OnDragDrop(DirectCast(cms.Tag, DragEventArgs))
    'End Sub

    'Private Sub siblingClicked(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    choice = "sibling"
    '    OnDragDrop(DirectCast(cms.Tag, DragEventArgs))
    'End Sub

    Private Function IsNodeDescendant(ByVal node As TreeNode, ByVal potentialElder As TreeNode) As Boolean
        Dim n As TreeNode
        If node Is Nothing OrElse potentialElder Is Nothing Then Return False
        Do
            n = node.Parent
            If n IsNot Nothing Then
                If n Is potentialElder Then
                    Return (True)
                Else
                    node = n
                End If

            End If

        Loop Until n Is Nothing

        Return (False)

    End Function

    Private Function FindNodeInDataObject(ByVal dataObject As IDataObject) As TreeNode

        For Each format As String In dataObject.GetFormats
            Dim data As Object = dataObject.GetData(format)

            If GetType(TreeNode).IsAssignableFrom(data.GetType) Then
                Return (DirectCast(data, TreeNode))
            End If
        Next

        Return (Nothing)

    End Function

End Class

Public Class NodeDraggingOverEventArgs
    Inherits EventArgs

    Private _DropLegal As Boolean
    Private _MovingNode As TreeNode
    Private _TargetNode As TreeNode

    Public Sub New(ByVal movingNode As TreeNode, ByVal targetNode As TreeNode)
        _DropLegal = True
        _MovingNode = movingNode
        _TargetNode = targetNode
    End Sub

    Public ReadOnly Property TargetNode() As TreeNode
        Get
            Return (_TargetNode)
        End Get
    End Property

    Public ReadOnly Property MovingNode() As TreeNode
        Get
            Return (_MovingNode)
        End Get
    End Property

    'Use this to disallow a drop

    Public Property DropIsLegal() As Boolean
        Get
            Return _DropLegal
        End Get
        Set(ByVal value As Boolean)
            _DropLegal = value
        End Set
    End Property

End Class

Public Class NodeMovingByDragEventArgs
    Inherits EventArgs

    Private _MovingNode As TreeNode
    Private _CurParent As TreeNode
    Private _ParentToBe As TreeNode
    Private _CancelMove As Boolean

    Public Sub New(ByVal nodeMoving As TreeNode, ByVal prevParent As TreeNode, ByVal parentToBe As TreeNode)
        _MovingNode = nodeMoving
        _CurParent = prevParent
        _ParentToBe = parentToBe
    End Sub

    Public Property CancelMove() As Boolean
        Get
            Return _CancelMove
        End Get
        Set(ByVal value As Boolean)
            _CancelMove = value
        End Set
    End Property

    Public ReadOnly Property MovingNode() As TreeNode
        Get
            Return _MovingNode
        End Get

    End Property

    Public ReadOnly Property CurrentParent() As TreeNode
        Get
            Return _CurParent
        End Get
    End Property

    Public ReadOnly Property ParentToBe() As TreeNode
        Get
            Return _ParentToBe
        End Get
    End Property

End Class

Public Class NodeMovedByDragEventArgs
    Inherits EventArgs

    Private _MovedNode As TreeNode
    Private _PreviousParent As TreeNode

    Public Sub New(ByVal nodeMoved As TreeNode, ByVal prevParent As TreeNode)
        _MovedNode = nodeMoved
        _PreviousParent = prevParent
    End Sub

    Public ReadOnly Property MovedNode() As TreeNode
        Get
            Return _MovedNode
        End Get
    End Property

    Public ReadOnly Property PreviousParent() As TreeNode
        Get
            Return _PreviousParent
        End Get
    End Property

End Class

Public Class TreeNodeTag
    Public Enum TagIndex
        Root = 1
        ProjectGroup = 2
        ProjectName = 3
    End Enum
    Public TI As TagIndex
    Public Property ImageIndex As Integer
    Public Property isMaster As Boolean
    Public Const MasterImageIndex As Integer = 18
End Class


