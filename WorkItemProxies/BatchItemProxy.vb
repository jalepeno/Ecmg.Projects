' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------
'  Document    :  BatchItemProxy.vb
'  Description :  [type_description_here]
'  Created     :  8/14/2012 8:11:55 AM
'  <copyright company="ECMG">
'      Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'      Copying or reuse without permission is strictly forbidden.
'  </copyright>
' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------

#Region "Imports"

Imports Documents.Utilities
Imports Operations

#End Region

Public Class BatchItemProxy
  Inherits WorkItemProxy
  Implements IBatchItemProxy

#Region "Class Variables"

  Private mobjBatch As Batch = Nothing

#End Region

#Region "Public Properties"

  Public ReadOnly Property Batch As Batch
    Get
      Try
        Return mobjBatch
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

#End Region

#Region "Constructors"

  Public Sub New(lpBatch As Batch)
    MyBase.New(lpBatch.Id)
    Try
      mobjBatch = lpBatch
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

#Region "IBatchItemProxy Implementation"

  Public ReadOnly Property JobName As String Implements IBatchItemProxy.JobName
    Get
      Try
        If Me.Batch IsNot Nothing AndAlso Me.Batch.Job IsNot Nothing Then
          Return Me.Batch.Job.Name
        Else
          Return String.Empty
        End If
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property ProjectName As String Implements IBatchItemProxy.ProjectName
    Get
      Try
        If Me.Batch IsNot Nothing AndAlso Me.Batch.Job IsNot Nothing AndAlso Me.Batch.Job.Project IsNot Nothing Then
          Return Me.Batch.Job.Project.Name
        Else
          Return String.Empty
        End If
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

#End Region

End Class
