' ********************************************************************************
' '  Document    :  BaseBatchWorker.vb
' '  Description :  [type_description_here]
' '  Created     :  11/20/2012-10:45:50
' '  <copyright company="ECMG">
' '      Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
' '      Copying or reuse without permission is strictly forbidden.
' '  </copyright>
' ********************************************************************************

#Region "Imports"

Imports System.ComponentModel
Imports Documents.Utilities
Imports Operations

#End Region

Public Class BaseBatchWorker
  Inherits BackgroundWorker
  'Implements IBatchWorker

#Region "Class Variables"

  Private WithEvents mobjBatch As Batch
  Private mobjDoWorkEventArgs As DoWorkEventArgs
  Private mobjProcessResults As IProcessResults = New ProcessResults

#End Region

#Region "Public Properties"

  Public Property Batch() As Batch 'Implements IBatchWorker.Batch
    Get
      Try
        Return mobjBatch
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        '   Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(ByVal value As Batch)
      Try
        mobjBatch = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        '   Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public ReadOnly Property CurrentWorkSummary() As WorkSummary
    Get
      Try
        If Me.Batch IsNot Nothing AndAlso Me.Batch.CurrentWorkSummary IsNot Nothing Then
          Return Me.Batch.CurrentWorkSummary
        Else
          Return Nothing
        End If
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public Property DoWorkEventArgs() As DoWorkEventArgs 'Implements IBatchWorker.DoWorkEventArgs
    Get
      Try
        Return mobjDoWorkEventArgs
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        '   Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(ByVal value As DoWorkEventArgs)
      Try
        mobjDoWorkEventArgs = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        '   Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public ReadOnly Property ProcessResults As IProcessResults
    Get
      Try
        Return mobjProcessResults
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        '   Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

#End Region

#Region "Constructors"

  Public Sub New(ByVal lpBatch As Batch)

    MyBase.New()

    Try

      Me.WorkerReportsProgress = True
      Me.WorkerSupportsCancellation = True

      mobjBatch = lpBatch

    Catch ex As Exception

    End Try

  End Sub

#End Region

  'Public Shadows Sub CancelAsync() Implements IBackgroundWorker.CancelAsync
  '	Try
  '		MyBase.CancelAsync()
  '	Catch ex As Exception
  '		ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '		'   Re-throw the exception to the caller
  '		Throw
  '	End Try
  'End Sub

  'Public Shadows ReadOnly Property CancellationPending As Boolean Implements IBackgroundWorker.CancellationPending
  '	Get
  '		Try
  '			Return MyBase.CancellationPending
  '		Catch ex As Exception
  '			ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '			'   Re-throw the exception to the caller
  '			Throw
  '		End Try
  '	End Get
  'End Property

  'Public Shadows Event DoWork(sender As Object, e As DoWorkEventArgs) Implements IBackgroundWorker.DoWork

  'Public Shadows ReadOnly Property IsBusy As Boolean Implements IBackgroundWorker.IsBusy
  '	Get
  '		Try
  '			Return MyBase.IsBusy
  '		Catch ex As Exception
  '			ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '			'   Re-throw the exception to the caller
  '			Throw
  '		End Try
  '	End Get
  'End Property

  'Public Shadows Event ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Implements IBackgroundWorker.ProgressChanged

  'Public Shadows Sub ReportProgress(percentProgress As Integer) Implements IBackgroundWorker.ReportProgress
  '	Try
  '		MyBase.ReportProgress(percentProgress)
  '	Catch ex As Exception
  '		ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '		'   Re-throw the exception to the caller
  '		Throw
  '	End Try
  'End Sub

  'Public Shadows Sub ReportProgress(percentProgress As Integer, userState As Object) Implements IBackgroundWorker.ReportProgress
  '	Try
  '		MyBase.ReportProgress(percentProgress, userState)
  '	Catch ex As Exception
  '		ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '		'   Re-throw the exception to the caller
  '		Throw
  '	End Try
  'End Sub

  'Public Shadows Sub RunWorkerAsync() Implements IBackgroundWorker.RunWorkerAsync
  '	Try
  '		MyBase.RunWorkerAsync()
  '	Catch ex As Exception
  '		ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '		'   Re-throw the exception to the caller
  '		Throw
  '	End Try
  'End Sub

  ''Public Shadows Sub RunWorkerAsync(argument As Object) Implements IBackgroundWorker.RunWorkerAsync
  ''	Try
  ''		MyBase.RunWorkerAsync(argument)
  ''	Catch ex As Exception
  ''		ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  ''		'   Re-throw the exception to the caller
  ''		Throw
  ''	End Try
  ''End Sub

  'Public Shadows Event RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Implements IBackgroundWorker.RunWorkerCompleted

  'Public Shadows Property WorkerReportsProgress As Boolean Implements IBackgroundWorker.WorkerReportsProgress
  '	Get
  '		Try
  '			Return MyBase.WorkerReportsProgress
  '		Catch ex As Exception
  '			ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '			'   Re-throw the exception to the caller
  '			Throw
  '		End Try
  '	End Get
  '	Set(value As Boolean)
  '		Try
  '			MyBase.WorkerReportsProgress = value
  '		Catch ex As Exception
  '			ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '			'   Re-throw the exception to the caller
  '			Throw
  '		End Try
  '	End Set
  'End Property

  'Public Shadows Property WorkerSupportsCancellation As Boolean Implements IBackgroundWorker.WorkerSupportsCancellation
  '	Get
  '		Try
  '			Return MyBase.WorkerSupportsCancellation
  '		Catch ex As Exception
  '			ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '			'   Re-throw the exception to the caller
  '			Throw
  '		End Try
  '	End Get
  '	Set(value As Boolean)
  '		Try
  '			MyBase.WorkerSupportsCancellation = value
  '		Catch ex As Exception
  '			ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '			'   Re-throw the exception to the caller
  '			Throw
  '		End Try
  '	End Set
  'End Property

End Class
