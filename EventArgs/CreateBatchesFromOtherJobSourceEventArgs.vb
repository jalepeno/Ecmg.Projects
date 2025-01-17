'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  CreateBatchesFromOtherJobSourceEventArgs.vb
'   Description :  [type_description_here]
'   Created     :  5/13/2013 1:56:42 PM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

#Region "Imports"

Imports System.ComponentModel
Imports Documents.Utilities
Imports Operations

#End Region

Public Class CreateBatchesFromOtherJobSourceEventArgs
  Inherits OtherJobSourceEventArgs

#Region "Class Variables"

  Private mobjOldSavedBatches As Batches
  Private mobjBackgroundWorker As BackgroundWorker
  Private mobjBackgroundWorkerEventArgs As DoWorkEventArgs

#End Region

#Region "Public Properties"

  Public Property OldSavedBatches As Batches
    Get
      Try
        Return mobjOldSavedBatches
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As Batches)
      Try
        mobjOldSavedBatches = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public Property BackgroundWorker As BackgroundWorker
    Get
      Try
        Return mobjBackgroundWorker
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As BackgroundWorker)
      Try
        mobjBackgroundWorker = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public Property BackgroundWorkerEventArgs As DoWorkEventArgs
    Get
      Try
        Return mobjBackgroundWorkerEventArgs
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As DoWorkEventArgs)
      Try
        mobjBackgroundWorkerEventArgs = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property


#End Region
#Region "Constructors"
  Public Sub New()

  End Sub

  Public Sub New(lpTargetJob As Job,
             lpOldSavedBatches As Batches,
             lpBackgroundWorker As BackgroundWorker,
             lpBackgroundWorkerEventArgs As DoWorkEventArgs)
    MyBase.New(lpTargetJob)
    Try
      OldSavedBatches = lpOldSavedBatches
      BackgroundWorker = lpBackgroundWorker
      BackgroundWorkerEventArgs = lpBackgroundWorkerEventArgs
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Sub New(lpJobSource As JobSource,
               lpOldSavedBatches As Batches,
               lpBackgroundWorker As BackgroundWorker,
               lpBackgroundWorkerEventArgs As DoWorkEventArgs)
    MyBase.New(lpJobSource)
    Try
      OldSavedBatches = lpOldSavedBatches
      BackgroundWorker = lpBackgroundWorker
      BackgroundWorkerEventArgs = lpBackgroundWorkerEventArgs
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Sub New(lpSourceJobName As String,
                 lpProcessedStatusFilter As String,
                 lpSourceIdType As enumSourceIdType,
                 lpOldSavedBatches As Batches,
                 lpBackgroundWorker As BackgroundWorker,
                 lpBackgroundWorkerEventArgs As DoWorkEventArgs)
    MyBase.New(lpSourceJobName, lpProcessedStatusFilter, lpSourceIdType)
    Try
      OldSavedBatches = lpOldSavedBatches
      BackgroundWorker = lpBackgroundWorker
      BackgroundWorkerEventArgs = lpBackgroundWorkerEventArgs
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Sub New(lpSourceJobName As String,
               lpProcessedStatusFilter As OperationEnumerations.ProcessedStatus,
               lpSourceIdType As enumSourceIdType,
               lpOldSavedBatches As Batches,
               lpBackgroundWorker As BackgroundWorker,
               lpBackgroundWorkerEventArgs As DoWorkEventArgs)
    MyBase.New(lpSourceJobName, lpProcessedStatusFilter, lpSourceIdType)
    Try
      OldSavedBatches = lpOldSavedBatches
      BackgroundWorker = lpBackgroundWorker
      BackgroundWorkerEventArgs = lpBackgroundWorkerEventArgs
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

#Region "Public Methods"

#End Region

#Region "Private Methods"

#End Region

End Class
