' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------
'  Document    :  WorkEventArgs.vb
'  Description :  [type_description_here]
'  Created     :  8/1/2012 3:20:16 PM
'  <copyright company="ECMG">
'      Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'      Copying or reuse without permission is strictly forbidden.
'  </copyright>
' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------

#Region "Imports"

Imports Documents.Utilities

#End Region

Public Class WorkEventArgs

#Region "Class Variables"
  Private mstrMessage As String = String.Empty
  Private mobjJob As IJobInfo = Nothing
  Private mobjOriginatingBatch As Batch = Nothing

#End Region

#Region "Public Properties"

  Public ReadOnly Property Job As IJobInfo
    Get
      Return mobjJob
    End Get
  End Property

  Public ReadOnly Property Message As String
    Get
      Return mstrMessage
    End Get
  End Property

  Public ReadOnly Property OriginatingBatch As Batch
    Get
      Return mobjOriginatingBatch
    End Get
  End Property

#End Region

#Region "Constructors"

  Public Sub New(lpJob As IJobInfo)
    Try
      mobjJob = lpJob
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Sub New(lpOriginatingBatch As Batch)
    Try
      mobjOriginatingBatch = lpOriginatingBatch
      If lpOriginatingBatch.Job IsNot Nothing Then
        mobjJob = lpOriginatingBatch.Job
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Sub New(lpJob As IJobInfo, lpOriginatingBatch As Batch)
    Try
      mobjJob = lpJob
      mobjOriginatingBatch = lpOriginatingBatch
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Sub New(lpJob As IJobInfo, lpMessage As String)
    Try
      mobjJob = lpJob
      mstrMessage = lpMessage
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

End Class
