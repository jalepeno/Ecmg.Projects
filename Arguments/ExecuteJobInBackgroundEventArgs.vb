'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  ExecuteJobInBackgroundEventArgs.vb
'   Description :  [type_description_here]
'   Created     :  6/12/2014 3:48:43 PM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

#Region "Imports"

Imports System.ComponentModel
Imports Documents.Utilities

#End Region

Public Class ExecuteJobInBackgroundEventArgs
  Inherits DoWorkEventArgs

#Region "Class Variables"

  Private mobjTargetNodeNames As New List(Of String)

#End Region

#Region "Public Properties"

  Public ReadOnly Property TargetNodes As List(Of String)
    Get
      Try
        Return mobjTargetNodeNames
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

#End Region

#Region "Constructors"

  Public Sub New(ParamArray lpNodeNames() As String)
    MyBase.New(lpNodeNames)
    Try
      mobjTargetNodeNames.AddRange(lpNodeNames)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

End Class
