'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  OtherJobSourceEventArgs.vb
'   Description :  [type_description_here]
'   Created     :  5/13/2013 1:24:19 PM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

#Region "Imports"

Imports System.Text
Imports Documents.Utilities
Imports Operations

#End Region

Public Class OtherJobSourceEventArgs
  Implements IDBLookupSourceEventArgs

#Region "Class Variables"

  Private mstrSourceJobName As String = String.Empty
  Private mobjTargetJob As Job = Nothing
  Private mstrProcessedStatusFilter As String = String.Empty
  Private menuSourceIdType As enumSourceIdType
  Private mstrSourceConnectionString As String = String.Empty

#End Region

#Region "Public Properties"

  Public Property SourceJobName As String
    Get
      Try
        Return mstrSourceJobName
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As String)
      Try
        mstrSourceJobName = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public Property TargetJob As Job Implements IDBLookupSourceEventArgs.TargetJob
    Get
      Try
        Return mobjTargetJob
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As Job)
      Try
        mobjTargetJob = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public Property ProcessedStatusFilter As String
    Get
      Try
        Return mstrProcessedStatusFilter
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As String)
      Try
        mstrProcessedStatusFilter = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public Property SourceIdType As enumSourceIdType
    Get
      Try
        Return menuSourceIdType
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As enumSourceIdType)
      Try
        menuSourceIdType = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public ReadOnly Property SourceSQLStatement As String Implements IDBLookupSourceEventArgs.SourceSQLStatement
    Get
      Try
        Return GenerateSourceSQLStatement()
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property SourceConnectionString As String Implements IDBLookupSourceEventArgs.SourceConnectionString
    Get
      Try
        Return GenerateSourceConnectionString()
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property NativeSourceConnectionString As String Implements IDBLookupSourceEventArgs.NativeSourceConnectionString
    Get
      Try
        Return GenerateNativeSourceConnectionString()
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

#End Region

#Region "Constructors"

  Public Sub New()

  End Sub

  Public Sub New(lpTargetJob As Job)
    Try
      If lpTargetJob.Source Is Nothing Then
        Throw New InvalidOperationException("Job source is null.")
      End If
      If lpTargetJob.Source.Type <> enumSourceType.OtherJob Then
        Throw New InvalidOperationException(String.Format("Source type is '{0}'.", lpTargetJob.Source.Type.ToString))
      End If
      If String.IsNullOrEmpty(lpTargetJob.Source.SourceJob) Then
        Throw New InvalidOperationException("Source job is not set.")
      End If
      TargetJob = lpTargetJob
      SourceJobName = lpTargetJob.Source.SourceJob
      ProcessedStatusFilter = lpTargetJob.Source.ProcessedStatusFilter
      SourceIdType = lpTargetJob.Source.SourceIdType
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Sub New(lpJobSource As JobSource)
    Try
      If lpJobSource.Type <> enumSourceType.OtherJob Then
        Throw New InvalidOperationException(String.Format("Source type is '{0}'.", lpJobSource.Type.ToString))
      End If
      If String.IsNullOrEmpty(lpJobSource.SourceJob) Then
        Throw New InvalidOperationException("Source job is not set.")
      End If
      SourceJobName = lpJobSource.SourceJob
      ProcessedStatusFilter = lpJobSource.ProcessedStatusFilter
      SourceIdType = lpJobSource.SourceIdType
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Sub New(lpSourceJobName As String, lpProcessedStatusFilter As String, lpSourceIdType As enumSourceIdType)
    Try
      SourceJobName = lpSourceJobName
      ProcessedStatusFilter = lpProcessedStatusFilter
      SourceIdType = lpSourceIdType
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Sub New(lpSourceJobName As String, lpProcessedStatusFilter As OperationEnumerations.ProcessedStatus,
                 lpSourceIdType As enumSourceIdType)
    Try
      SourceJobName = lpSourceJobName
      ProcessedStatusFilter = lpProcessedStatusFilter.ToString
      SourceIdType = lpSourceIdType
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

  Private Function GenerateSourceSQLStatement() As String
    Try
      If TargetJob Is Nothing Then
        ' Throw New InvalidOperationException("Job property not set.")
        Return String.Empty
      End If

      Dim lstrSourceJobViewName As String = TargetJob.Project.Container.GetJobViewName(SourceJobName)
      Dim lobjSqlBuilder As New StringBuilder

      ' Bracket the job name if necessary
      If lstrSourceJobViewName.Contains("-") OrElse lstrSourceJobViewName.Contains(" ") Then
        lstrSourceJobViewName = String.Format("[{0}]", lstrSourceJobViewName)
      End If

      Select Case SourceIdType
        Case enumSourceIdType.SourceDocId
          lobjSqlBuilder.AppendFormat("SELECT SourceDocId FROM {0}", lstrSourceJobViewName)

        Case enumSourceIdType.DestinationDocId
          lobjSqlBuilder.AppendFormat("SELECT DestDocId FROM {0}", lstrSourceJobViewName)

        Case Else
          lobjSqlBuilder.AppendFormat("SELECT SourceDocId FROM {0}", lstrSourceJobViewName)

      End Select

      If ((Not String.IsNullOrEmpty(ProcessedStatusFilter)) AndAlso
          (Not String.Equals(ProcessedStatusFilter, Job.PROCESS_STATUS_ALL))) Then
        lobjSqlBuilder.AppendFormat(" WHERE ProcessedStatus = '{0}'", ProcessedStatusFilter)
      End If

      Return lobjSqlBuilder.ToString

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      Return String.Empty
    End Try
  End Function

  Private Function GenerateSourceConnectionString() As String
    Try
      If TargetJob Is Nothing Then
        ' Throw New InvalidOperationException("Job property not set.")
        Return String.Empty
      End If

      Return TargetJob.Project.ItemsLocation.ToOleDBConnectionString

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      Return String.Empty
    End Try
  End Function

  Private Function GenerateNativeSourceConnectionString() As String
    Try
      If TargetJob Is Nothing Then
        ' Throw New InvalidOperationException("Job property not set.")
        Return String.Empty
      End If

      Return TargetJob.Project.ItemsLocation.ToNativeConnectionString()

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      Return String.Empty
    End Try
  End Function

#End Region

End Class
