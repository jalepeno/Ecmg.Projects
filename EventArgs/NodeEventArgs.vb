'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  NodeEvent.vb
'   Description :  [type_description_here]
'   Created     :  1/10/2014 4:17:04 PM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

#Region "Imports"

Imports Documents.Utilities

#End Region

#Region "Event Delegates"

Public Delegate Sub NodeEventHandler(ByVal sender As Object, ByRef e As NodeEventArgs)

#End Region

Public Class NodeEventArgs

#Region "Class Variables"

  Private mobjNode As INodeInfo = Nothing

#End Region

#Region "Public Properties"

  Public ReadOnly Property Node As INodeInfo
    Get
      Try
        Return mobjNode
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

#End Region

#Region "Constructors"

  Public Sub New(node As INodeInfo)
    Try
      mobjNode = node
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

End Class
