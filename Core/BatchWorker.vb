'---------------------------------------------------------------------------------
' <copyright company="ECMG">
'     Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'     Copying or reuse without permission is strictly forbidden.
' </copyright>
'---------------------------------------------------------------------------------

#Region "Imports"

#End Region

Public Class BatchWorker
  Inherits BaseBatchWorker

#Region "Class Variables"

  Private mobjTreeNode As Object
  Private mobjParentTreeNode As Object

#End Region

  Public Sub New(ByVal lpBatch As Batch,
                 ByVal lpTreeNode As Object,
                 ByVal lpParentTreeNode As Object)

    MyBase.New(lpBatch)

    Try

      mobjTreeNode = lpTreeNode
      mobjParentTreeNode = lpParentTreeNode

    Catch ex As Exception

    End Try

  End Sub

  Public ReadOnly Property TreeNode() As Object
    Get
      Return mobjTreeNode
    End Get
  End Property

  Public ReadOnly Property ParentTreeNode() As Object
    Get
      Return mobjParentTreeNode
    End Get
  End Property

End Class
