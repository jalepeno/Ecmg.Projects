'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  DetailedJobInfo.vb
'   Description :  [type_description_here]
'   Created     :  2/5/2014 9:41:57 AM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

#Region "Imports"

Imports Documents.Utilities
Imports Operations

#End Region

Public Class DetailedJobInfo
  Inherits JobInfo
  Implements IDetailedJobInfo

#Region "Class Variables"

  Private mobjBatches As IBatchInfoCollection = New BatchInfoCollection

#End Region

#Region "IDetailedJobInfo Implementation"

  Public ReadOnly Property Batches As IBatchInfoCollection Implements IDetailedJobInfo.Batches
    Get
      Try
        Return mobjBatches
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

#End Region

#Region "Constructors"

  Public Sub New(lpJobInfo As IJobInfo, lpBatches As IBatchInfoCollection)
    MyBase.New(lpJobInfo.Id, _
              lpJobInfo.Name, _
              lpJobInfo.DisplayName, _
              lpJobInfo.ProjectName, _
              lpJobInfo.Description, _
              lpJobInfo.BatchSize, _
              lpJobInfo.IsCancelled, _
              lpJobInfo.IsCompleted, _
              lpJobInfo.IsInitialized, _
              lpJobInfo.IsRunning, _
              lpJobInfo.BatchThreadsRunning, _
              lpJobInfo.CancellationReason, _
              lpJobInfo.ItemsProcessed, _
              lpJobInfo.Operation, _
              lpJobInfo.CreateDate, _
              lpJobInfo.Process, _
              lpJobInfo.WorkSummary)
    Try
      mobjBatches = lpBatches
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Sub New(lpId As String, _
               lpName As String, _
               lpDisplayName As String, _
               lpProjectName As String, _
               lpDescription As String, _
               lpBatchSize As Integer, _
               lpIsCancelled As Boolean, _
               lpIsCompleted As Boolean, _
               lpIsInitialized As Boolean, _
               lpIsRunning As Boolean, _
               lpBatchThreadsRunning As Integer, _
               lpCancellationReason As String, _
               lpItemsProcessed As Long, _
               lpOperation As String, _
               lpCreateDate As DateTime, _
               lpProcess As IProcess, _
               lpWorkSummary As IWorkSummary, _
               lpBatches As IBatchInfoCollection)

    MyBase.New(lpId, _
               lpName, _
               lpDisplayName, _
               lpProjectName, _
               lpDescription, _
               lpBatchSize, _
               lpIsCancelled, _
               lpIsCompleted, _
               lpIsInitialized, _
               lpIsRunning, _
               lpBatchThreadsRunning, _
               lpCancellationReason, _
               lpItemsProcessed, _
               lpOperation, _
               lpCreateDate, _
               lpProcess, _
               lpWorkSummary)

    Try
      mobjBatches = lpBatches
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

End Class
