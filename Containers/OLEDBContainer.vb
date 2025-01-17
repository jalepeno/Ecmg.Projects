'---------------------------------------------------------------------------------
' <copyright company="ECMG">
'     Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'     Copying or reuse without permission is strictly forbidden.
' </copyright>
'---------------------------------------------------------------------------------

#Region "Imports"

Imports Documents.Utilities
Imports System.Reflection
Imports System.Data
Imports System.Data.OleDb
Imports Microsoft.Data.SqlClient
Imports Documents.SerializationUtilities
Imports System.Data.Common
Imports System.IO
Imports Operations.OperationEnumerations
Imports System.Text
Imports Operations
Imports Ecmg.Cts.Projects

#End Region

Public Class OLEDBContainer
  Inherits Container

#Region "Class Variables"

  Private mobjConnection As OleDbConnection = Nothing
  Private Const TABLE_BATCH_ITEM_NAME As String = "tblBatchItems"
  Private Const TABLE_BATCH_NAME As String = "tblBatch"
  Private Const TABLE_JOB_NAME As String = "tblJob"
  Private Const TABLE_PROJECTJOBREL_NAME As String = "tblProjectJobRel"
  Private Const TABLE_JOBBATCHREL_NAME As String = "tblJobBatchRel"
  Private Const TABLE_PROJECT_NAME As String = "tblProject"
  Private Const TABLE_BATCHLOCK_NAME As String = "tblBatchLock"
  Private Const TABLE_AUDIT_NAME As String = "tblAudit"
  Private Const DATABASE_SCHEMA_VERSION As String = "1.0"
  Private Const DB_CONNECTION_STRING As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=<SOURCE>;Persist Security Info=False"
  Private mstrFullDatabasePath As String = String.Empty

#End Region

#Region "Constructors"

  Public Sub New()

  End Sub

  Public Sub New(ByVal lpItemsLocation As ItemsLocation)
    Me.ItemsLocation = lpItemsLocation

    'Build Location (Connection String)
    If (lpItemsLocation.Location = String.Empty) Then
      Me.ItemsLocation.Location = DB_CONNECTION_STRING

      If (Not Me.ItemsLocation.DatabasePath.EndsWith("\")) Then
        Me.ItemsLocation.DatabasePath &= "\"
      End If

      mstrFullDatabasePath = Me.ItemsLocation.DatabasePath & Me.ItemsLocation.DatabaseName & ".accdb"
      Me.ItemsLocation.Location = Me.ItemsLocation.Location.Replace("<SOURCE>", mstrFullDatabasePath)

    Else
      mstrFullDatabasePath = lpItemsLocation.DatabasePath & lpItemsLocation.DatabaseName & ".accdb"
      Me.ItemsLocation.Location = lpItemsLocation.Location
    End If

  End Sub

#End Region

#Region "Public Methods"

  Friend Overrides Sub ClearWorkSummary(lpWorkParent As Object)

  End Sub

  Public Overrides Function GetBatchLockCount(lpJobName As String) As Integer
    Try
      Return 0
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetAllBatchLocks() As IBatchLocks
    Try
      Throw New NotImplementedException
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function
  Public Overrides Function GetBatches(lpJob As Job) As Batches
    Try
      Throw New NotImplementedException
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetBatchIds(lpJobId As String) As IList(Of String)
    Try
      Throw New NotImplementedException
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetBatchLocks(lpJobName As String) As IBatchLocks
    Try
      Throw New NotImplementedException
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function ProjectExists(lpProjectId As String) As Boolean
    Return False
  End Function

  Public Overrides Function GetOrphanBatchCount() As Integer
    Try
      Throw New NotImplementedException
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetOrphanBatchItemCount() As Integer
    Try
      Throw New NotImplementedException
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetProjectInfo() As IProjectInfo

    Try
      Return Nothing

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      Throw
    End Try

  End Function
  Public Overrides Function GetBatchInfoCollection(lpJob As IJobInfo) As IBatchInfoCollection
    Try
      Throw New NotImplementedException
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetJobInfo(lpJobId As String) As IJobInfo
    Try
      Throw New NotImplementedException
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetDetailedJobInfo(lpJobId As String) As IDetailedJobInfo
    Try
      Throw New NotImplementedException
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetJobInfoCollection() As IJobInfoCollection
    Try
      Throw New NotImplementedException
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetJobRelationships(lpJobId As String) As JobRelationships
    Try
      Throw New NotImplementedException
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetProjectSummaryCounts(ByVal lpProject As Project) As WorkSummaries

    Try
      Return Nothing

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      Throw
    End Try

  End Function

  Public Overrides Function GetProjectAvgProcessingTime(lpProject As Project) As Single

    Try
      Return 0

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function
  
  Public Overrides Function GetProjectDbFileInfo() As DbFilesInfo

    Try
      Return Nothing

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      Throw
    End Try
  End Function

  Public Overrides Function FileExists(ByVal lpJob As Job, ByVal lpFileName As String) As Boolean

    Try
      Throw New NotImplementedException()

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  Public Overrides Function GetFileList(ByVal lpJob As Job) As IList(Of String)

    Try
      Throw New NotImplementedException()

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  Public Overloads Overrides Function RetrieveFile(lpJob As Job, lpFileName As String) As IO.Stream

    Try
      Throw New NotImplementedException()

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  Public Overloads Overrides Function RetrieveFile(lpFileId As String, lpFileName As String) As IO.Stream

    Try
      Throw New NotImplementedException()

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  Public Overrides Sub StoreFile(ByVal lpJob As Job, ByVal lpFilePath As String)

    Try
      Throw New NotImplementedException()

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Sub

  Public Overrides Sub StoreFile(lpJob As Job, lpFileName As String, lpFileData As IO.Stream)

    Try
      Throw New NotImplementedException()

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Sub

  Public Overrides Sub UpdateProcess(lpJob As Job)

    Try
      Throw New NotImplementedException()

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      Throw
    End Try

  End Sub

  Public Overrides Sub UpdateTransformations(ByVal lpJob As Job)

    Try
      Throw New NotImplementedException()

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      Throw
    End Try

  End Sub

  Public Overrides Sub UpdateTransformations(ByVal lpBatch As Batch)

    Try
      Throw New NotImplementedException()

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      Throw
    End Try

  End Sub

  Public Overrides Sub UpdateJobSource(lpJob As Job)

    Try
      Throw New NotImplementedException()

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      Throw
    End Try

  End Sub

  Public Overrides Function GetUpdateTable(lpJob As Job, _
                                           lpUpdateItems As DataTable) As DataTable

    Try
      Return Nothing

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      Throw
    End Try

  End Function

  Public Overrides Sub RefreshTransform(ByVal lpJob As Job)

    Try
      Throw New NotImplementedException()

    Catch ex As Exception
      Throw ex
    End Try

  End Sub

  Public Overrides Function IsAvailableForProcessing(ByVal lpBatchId As String) As Boolean

    Try
      Return IsAvailableForProcessingDB(lpBatchId)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      Return False
    End Try

  End Function

  Public Overrides Function GetItemsById(ByVal lpBatch As Batch, _
                                         ByVal lpIdArrayList As ArrayList, _
                                         ByVal lpIncludeProcessResults As Boolean) As BatchItems

    Try
      Return GetItemsByIdDB(lpBatch, lpIdArrayList)

    Catch ex As Exception
      Throw ex
    End Try

  End Function

  Public Overrides Function GetItemsById(lpJob As Job, lpIdTable As DataTable) As BatchItems
    Try
      Throw New NotImplementedException
    Catch Ex As Exception
      ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function
  
  Public Overrides Function GetBatchItemById(lpId As String) As IBatchItem
    Try
      Throw New NotImplementedException
    Catch Ex As Exception
      ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetItemById(lpJobName As String, lpId As String, lpScope As OperationScope) As IBatchItem
    Try
      Throw New NotImplementedException
    Catch Ex As Exception
      ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Sub ResetItemsToNotProcessed(ByVal lpIdArrayList As ArrayList)

    Try
      ResetItemsToNotProcessedDB(lpIdArrayList)

    Catch ex As Exception
      Throw ex
    End Try

  End Sub

  Public Overrides Function ResetFailedItemsByProcessedMessage(lpJob As Job, lpProcessedMessage As String) As Integer
    Try
      Throw New NotImplementedException
    Catch Ex As Exception
      ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Sub ResetFailedItemsToNotProcessed(ByVal lpBatch As Batch)

    Try
      ResetFailedItemsToNotProcessedDB(lpBatch)

    Catch ex As Exception
      Throw ex
    End Try

  End Sub

  Friend Overrides Sub ResetItemsToNotProcessed(lpBatch As Batch, lpCurrentStatus As ProcessedStatus)
    Try
      Throw New NotImplementedException
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Friend Overrides Sub ResetItemsToNotProcessed(lpJob As Job, lpCurrentStatus As ProcessedStatus)
    Try
      Throw New NotImplementedException
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overrides Sub LockBatch(ByVal lpBatchId As String, _
                                 ByVal lpLockedBy As String)

    Try
      LockBatchDB(lpBatchId, lpLockedBy)

    Catch ex As Exception
      Throw New BatchLockedException(ex.Message)
    End Try

  End Sub

  Public Overrides Sub UnLockBatch(ByVal lpBatchId As String, _
                                   ByVal lpUnLockedBy As String)

    Try
      UnLockBatchDB(lpBatchId, lpUnLockedBy)

    Catch ex As Exception
      Throw ex
    End Try

  End Sub

  Public Overrides Sub DeleteAll()

    Try
      CleanEntireDB()

    Catch ex As Exception
      Throw ex
    End Try

  End Sub

  Public Overrides Sub DeleteBatches(ByVal lpBatches As Batches)

    Try
      DeleteBatchesDB(lpBatches)

    Catch ex As Exception
      Throw ex
    End Try

  End Sub

  Public Overrides Sub DeleteOrphanBatches(lpProject As Project)
    Try
      Throw New NotImplementedException
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overrides Sub DeleteJob(ByVal lpJob As Job)

    Try
      DeleteJobDB(lpJob)

    Catch ex As Exception
      Throw ex
    End Try

  End Sub

  Public Overrides Function GetJobById(lpJobId As String) As Job
    Try
      Throw New NotImplementedException
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetJobConfigurationByName(lpJobName As String) As String
    Try
      Throw New NotImplementedException
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetJobIdentifiers() As IJobIdentifiers
    Try
      Throw New NotImplementedException
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Sub SaveJobConfiguration(lpJobConfiguration As Configuration.JobConfiguration)
    Try
      Throw New NotImplementedException
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overrides Sub DeleteProject(ByVal lpProject As Project)

    Try
      DeleteProjectDB(lpProject)

    Catch ex As Exception
      Throw ex
    End Try

  End Sub

  Public Overrides Function AddItem(ByVal lpBatchItem As BatchItem) As Boolean

    Try

      Dim lblnReturn As Boolean = False

      AddItemToDB(lpBatchItem)

      Return lblnReturn

    Catch ex As Exception
      Throw ex
    End Try

  End Function

  Public Overrides Sub CommitBatchItems()
    Try
      Throw New NotImplementedException
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      '   Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overrides Sub CreateListBatches(lpArgs As IDBLookupSourceEventArgs)
    Try
      Throw New NotImplementedException
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      '   Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overrides Sub SaveProject(ByVal lpProject As Project)

    Try
      MyBase.SaveProject(lpProject)
      SaveProjectToDB(lpProject)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Sub
  Public Overrides Sub InitializeCachedRepositories(lpProject As Project)
    Try
      Throw New NotImplementedException
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      '   Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overrides Function OpenProject(ByRef lpErrorMessage As String) As Project

    Try
      Return OpenProjectDB(lpErrorMessage)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  Public Overrides Sub SaveBatch(lpBatch As Batch)
    Try
      Throw New NotImplementedException
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      '   Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overrides Sub SaveBatches(lpJob As Job)
    Try
      Throw New NotImplementedException
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      '   Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overrides Sub SaveJob(ByRef lpJob As Job)

    Try
      SaveJobToDB(lpJob)
      SaveBatchesToDB(lpJob.Batches)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Sub

  Public Overloads Overrides Sub SaveJob(ByVal lpProjectId As String, lpJobConfiguration As Configuration.JobConfiguration)
    Try
      Throw New NotImplementedException
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overrides Sub SaveJobRelationship(lpJobRelationship As JobRelationship)
    Try
      Throw New NotImplementedException
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      '   Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  ''' <summary>
  ''' TODO: Implement Paging
  ''' </summary>
  ''' <param name="lpBatch"></param>
  ''' <param name="lpStart"></param>
  ''' <param name="lpItemsToGet"></param>
  ''' <param name="lpSortColumn"></param>
  ''' <param name="lpAscending"></param>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Overrides Function GetItems(ByVal lpBatch As Batch, _
                                     ByVal lpStart As Integer, _
                                     ByVal lpItemsToGet As Integer, _
                                     ByVal lpSortColumn As String, _
                                     ByVal lpAscending As Boolean, _
                                     ByVal lpProcessedStatusFilter As String) As BatchItems

    Try
      Return GetAllItemsFromDB(lpBatch)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  ''' <summary>
  ''' TODO: Implement paging
  ''' </summary>
  ''' <param name="lpBatch"></param>
  ''' <param name="lpStart"></param>
  ''' <param name="lpItemsToGet"></param>
  ''' <param name="lpSortColumn"></param>
  ''' <param name="lpAscending"></param>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Overrides Function GetItemsToDataTable(ByVal lpBatch As Batch, _
                                                ByVal lpStart As Integer, _
                                                ByVal lpItemsToGet As Integer, _
                                                ByVal lpSortColumn As String, _
                                                ByVal lpAscending As Boolean, _
                                                ByVal lpProcessedStatusFilter As String) As System.Data.DataTable

    Try
      Return GetAllItemsFromDBToDataTable(lpBatch, lpProcessedStatusFilter)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  Public Overrides Function GetFilteredItemsToDataTable(lpItemFilter As ItemFilter) As DataTable
    Dim lobjDataTable As DataTable = Nothing

    Try

      Return lobjDataTable

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetAllItems(ByVal lpBatch As Batch, _
                                        ByVal lpProcessedStatusFilter As String) As BatchItems

    Try
      Return GetAllItemsFromDB(lpBatch)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  Public Overrides Function GetAllItemsToDataTable(ByVal lpObject As Object, _
                                                   ByVal lpProcessedStatusFilter As String) As DataTable

    Try
      Return GetAllItemsFromDBToDataTable(lpObject, lpProcessedStatusFilter)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

    Public Overrides Function GetAllProcessedByNodeNames(lpJob As Job) As IList(Of String)
    Try
      Throw New NotImplementedException()
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      '  Re-throw the exception to the caller
      Throw
    End Try    
  End Function

  Public Overrides Function GetAllUnprocessedItems(ByVal lpBatch As Batch) As BatchItems

    Try
      Return GetAllUnprocessedItemsFromDB(lpBatch)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  Public Overrides Function GetItemCount(ByVal lpBatch As Batch) As Integer
    Try
      Throw New NotImplementedException
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetItemCount(ByVal lpJob As Job) As Long
    Try
      Throw New NotImplementedException
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetItemCount(ByVal lpProject As Project) As Long
    Try
      Throw New NotImplementedException
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  ''' <summary>
  ''' Called when a batch item has started to be migrated,exported,etc
  ''' </summary>
  ''' <param name="lpProcessedItemEventArgs"></param>
  ''' <remarks></remarks>
  Public Overrides Sub BeginProcessItem(ByVal lpProcessedItemEventArgs As BatchItemProcessEventArgs)

    Try
      'ApplicationLogging.WriteLogEntry(String.Format("BEGIN: BatchId='{0}'  BatchContainerId='{1}'  CurrentThread='{2}'", lpProcessedItemEventArgs.BatchId, Me.Id, Threading.Thread.CurrentThread.GetHashCode.ToString), TraceEventType.Information, 0)
      BeginItemProcessDB(lpProcessedItemEventArgs)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      'Throw
    End Try

  End Sub

  ''' <summary>
  ''' Called when a batch item has been migrated,exported,etc
  ''' </summary>
  ''' <param name="lpProcessedItemEventArgs"></param>
  ''' <remarks></remarks>
  Public Overrides Sub EndProcessItem(ByVal lpProcessedItemEventArgs As BatchItemProcessEventArgs)

    Try
      'ApplicationLogging.WriteLogEntry(String.Format("END: BatchId='{0}'  BatchContainerId='{1}'  CurrentThread='{2}'", lpProcessedItemEventArgs.BatchId, Me.Id, Threading.Thread.CurrentThread.GetHashCode.ToString), TraceEventType.Information, 0)
      EndItemProcessDB(lpProcessedItemEventArgs)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      'Throw
    End Try

  End Sub

  Public Overrides Function GetWorkSummaryCounts(ByVal lpBatch As Batch) As WorkSummary

    Try
      Return GetBatchSummaryCountsDB(lpBatch)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  Public Overrides Function GetWorkSummaryCounts(ByVal lpJob As Job) As WorkSummary

    Try
      'Return GetBatchSummaryCountsDB(lpJob)
      Throw New NotImplementedException
    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  Public Overrides Function GetWorkSummaryCounts(ByVal lpProject As Project) As WorkSummary

    Try
      Return Nothing

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  Public Overrides Function GetCachedWorkSummaryCounts(lpWorkParent As Object) As IWorkSummary
    Try
      Return Nothing

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetFailureItemIdsByProcessedMessage(lpJob As Job, lpProcessedMessage As String) As IList(Of String)

    Try
      Return Nothing

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  Public Overrides Function GetFailureSummaries(ByVal lpProject As Project) As FailureSummaries

    Try
      Return Nothing

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  Public Overrides Function GetFailureSummaries(ByVal lpJob As Job) As FailureSummaries

    Try
      Return Nothing

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  Public Overrides Function GetFailureSummaries(ByVal lpBatch As Batch) As FailureSummaries

    Try
      Return Nothing

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  Public Overrides Function GetProcessedItemsCount(ByVal lpProject As Project) As Long
    Try
      Throw New NotImplementedException
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetProcessedItemsCount(ByVal lpJob As Job) As Long
    Try
      Throw New NotImplementedException
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function
  
  Friend Overrides Function GetProcessResults(lpBatch As Batch) As IProcessResults
    Try
      Throw New NotImplementedException
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Friend Overrides Function GetProcessResults(lpJob As Job) As IProcessResults
    Try
      Throw New NotImplementedException
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Friend Overrides Sub SetProcessedItemsCount(ByVal lpProject As Project, lpProcessedCount As Long)
    Try
      Throw New NotImplementedException
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Friend Overrides Sub SetProcessedItemsCount(ByVal lpJob As Job, lpProcessedCount As Long)
    Try
      Throw New NotImplementedException
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

#Region "Private Methods"

#Region "Get Methods"

  ''' <summary>
  ''' Assumes only 1 project exists in the DB
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Private Function OpenProjectDB(ByRef lpErrorMessage As String) As Project

    Dim lobjProject As Project = Nothing
    Dim lobjConnection As OleDbConnection = Nothing
    Dim lobjDataReader As OleDbDataReader = Nothing

    Try

      lobjConnection = New OleDbConnection(Me.ItemsLocation.Location)

      Dim lstrSQL As String = String.Format("SELECT TOP 1 * from {0}", TABLE_PROJECT_NAME)
      Dim lobjCommand As New OleDbCommand(lstrSQL, lobjConnection)
      Helper.HandleConnection(lobjConnection)

      lobjDataReader = lobjCommand.ExecuteReader

      If (lobjDataReader.HasRows = True) Then

        While lobjDataReader.Read
          lobjProject = New Project(lobjDataReader("ProjectName").ToString, _
                                    lobjDataReader("Description").ToString(), _
                                    DeserializeString(lobjDataReader("ItemsLocation").ToString(), _
                                                      ItemsLocation.GetType, New ItemsLocation()),
                                    Convert.ToInt32(lobjDataReader("BatchSize").ToString), _
                                    lobjDataReader("ProjectId").ToString(), _
                                    lobjDataReader("CreateDate"))
        End While

      Else
        Throw New Exception(String.Format("Unable to find a project in '{0}' table at location '{1}'", TABLE_PROJECT_NAME, Me.ItemsLocation.Location))
      End If

      If (lobjDataReader IsNot Nothing) Then
        lobjDataReader.Close()
      End If

      'Get all the associated Jobs and batches
      lobjProject.Jobs = GetJobsDB(lobjProject, lpErrorMessage)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw

    Finally

      If (lobjDataReader IsNot Nothing) Then
        lobjDataReader.Close()
      End If

      If (lobjConnection.State = ConnectionState.Open) Then
        lobjConnection.Close()
      End If

    End Try

    Return lobjProject

  End Function

  ''' <summary>
  ''' Given a project, retreive all the jobs
  ''' </summary>
  ''' <param name="lpProject"></param>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Private Function GetJobsDB(ByVal lpProject As Project, ByRef lpErrorMessage As String) As Jobs

    Dim lobjJobs As New Jobs(lpProject)
    Dim lobjConnection As OleDbConnection = Nothing
    Dim lobjDataReader As OleDbDataReader = Nothing
    Dim lintJobErrorCount As Integer = 0
    Dim lstrJobErrorMessage As String = Nothing
    Dim lstrErrorMessageBuilder As New StringBuilder("Failed to load process for job ")

    Try

      lobjConnection = New OleDbConnection(Me.ItemsLocation.Location)

      ''''JOBS''''
      Dim lstrSQL As String = String.Format("SELECT  JobId from {0} WHERE ProjectId = '{1}'", TABLE_PROJECTJOBREL_NAME, lpProject.Id)

      Dim lobjCommand As New OleDbCommand(lstrSQL, lobjConnection)
      Helper.HandleConnection(lobjConnection)

      lobjDataReader = lobjCommand.ExecuteReader

      If (lobjDataReader.HasRows = True) Then

        While lobjDataReader.Read

          lstrSQL = String.Format("SELECT  * from {0} WHERE JobId = '{1}'", TABLE_JOB_NAME, lobjDataReader("JobId").ToString)
          lobjCommand = New OleDbCommand(lstrSQL, lobjConnection)
          Helper.HandleConnection(lobjConnection)

          Dim lobjJobDataReader As OleDbDataReader = lobjCommand.ExecuteReader

          If (lobjJobDataReader.HasRows = True) Then

            While lobjJobDataReader.Read

              'Dim lobjJob As New Job(lobjJobDataReader("JobId").ToString, lobjJobDataReader("JobName").ToString, lobjJobDataReader("Description").ToString(), StringToEnum(lobjJobDataReader("Operation").ToString(), GetType(OperationType)), Convert.ToInt32(lobjJobDataReader("BatchSize").ToString()), DeserializeString(lobjJobDataReader("JobSource").ToString(), GetType(JobSource), New JobSource()), DeserializeString(lobjJobDataReader("ItemsLocation").ToString(), ItemsLocation.GetType, New ItemsLocation()), lobjJobDataReader("DestinationConnectionString").ToString(), StringToEnum(lobjJobDataReader("ContentStorageType").ToString(), GetType(Core.Content.StorageTypeEnum)), lobjJobDataReader("DeclareAsRecordOnImport").ToString(), DeserializeString(lobjJobDataReader("DeclareRecordConfiguration").ToString(), GetType(DeclareRecordConfiguration), New DeclareRecordConfiguration()), DeserializeString(lobjJobDataReader("Transformations").ToString(), GetType(Transformations.TransformationCollection), New Transformations.TransformationCollection()), StringToEnum(lobjJobDataReader("DocumentFilingMode").ToString(), GetType(Core.FilingMode)), lobjJobDataReader("LeadingDelimiter").ToString(), StringToEnum(lobjJobDataReader("BasePathLocation").ToString(), GetType(Migrations.ePathLocation)), lobjJobDataReader("FolderDelimiter").ToString(), lobjJobDataReader("TransformationSourcePath").ToString())
              Dim lobjJob As Job = GetJobFromDataReader(lobjJobDataReader, lpProject, lpErrorMessage)

              If Not String.IsNullOrEmpty(lstrJobErrorMessage) Then
                lintJobErrorCount += 1
                lstrErrorMessageBuilder.AppendFormat("'{0}': {1}, ", lobjJob.Name, lstrJobErrorMessage)
              End If

              Dim lobjBatches As Batches = GetBatchesDB(lobjJob)

              For Each lobjBatch As Batch In lobjBatches
                lobjJob.Batches.Add(lobjBatch)
              Next

              lobjJobs.Add(lobjJob)

            End While

            If (lobjJobDataReader IsNot Nothing) Then
              lobjJobDataReader.Close()
            End If

          End If

        End While

      End If

      If (lobjDataReader IsNot Nothing) Then
        lobjDataReader.Close()
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw

    Finally

      If (lobjDataReader IsNot Nothing) Then
        lobjDataReader.Close()
      End If

      If (lobjConnection.State = ConnectionState.Open) Then
        lobjConnection.Close()
      End If

      If lintJobErrorCount = 1 Then
        lpErrorMessage = lstrErrorMessageBuilder.Remove(lstrErrorMessageBuilder.Length - 2, 2).ToString
      ElseIf lintJobErrorCount > 1 Then
        lpErrorMessage = lstrErrorMessageBuilder.Remove(lstrErrorMessageBuilder.Length - 2, 2).Replace("job ", "jobs ").ToString
      End If

    End Try

    Return lobjJobs

  End Function

  Private Function GetBatchesDB(ByVal lpJob As Job) As Batches

    Dim lobjBatches As New Batches(lpJob)
    Dim lobjConnection As OleDbConnection = Nothing
    Dim lobjDataReader As OleDbDataReader = Nothing

    Try

      lobjConnection = New OleDbConnection(Me.ItemsLocation.Location)

      ''''Batches''''
      'select j.BatchId, b.BatchName from tblJobBatchRel j join tblBatch b on j.batchid = b.batchid order by b.BatchName
      Dim lstrSQL As String = String.Format("SELECT j.BatchId, b.BatchName FROM {0} j INNER JOIN {1} b on j.batchid = b.batchid WHERE JobId = '{2}' ORDER BY b.BatchName", TABLE_JOBBATCHREL_NAME, TABLE_BATCH_NAME, lpJob.Id)
      'String.Format("SELECT  BatchId from {0} WHERE JobId = '{1}'", TABLE_JOBBATCHREL_NAME, lpJob.Id)

      Dim lobjCommand As New OleDbCommand(lstrSQL, lobjConnection)
      Helper.HandleConnection(lobjConnection)

      lobjDataReader = lobjCommand.ExecuteReader

      If (lobjDataReader.HasRows = True) Then

        While lobjDataReader.Read

          lstrSQL = String.Format("SELECT  * from {0} WHERE BatchId = '{1}'", TABLE_BATCH_NAME, lobjDataReader("BatchId").ToString)
          lobjCommand = New OleDbCommand(lstrSQL, lobjConnection)
          Helper.HandleConnection(lobjConnection)

          Dim lobjBatchDataReader As OleDbDataReader = lobjCommand.ExecuteReader

          If (lobjBatchDataReader.HasRows = True) Then

            While lobjBatchDataReader.Read

              'Dim lobjBatch As New Batch(lobjBatchDataReader("BatchId").ToString, lobjBatchDataReader("BatchName").ToString, lobjBatchDataReader("Description").ToString(), StringToEnum(lobjBatchDataReader("Operation").ToString(), GetType(OperationType)), lobjBatchDataReader("AssignedTo").ToString(), lobjBatchDataReader("ExportPath").ToString(), DeserializeString(lobjBatchDataReader("ItemsLocation").ToString(), ItemsLocation.GetType, New ItemsLocation()), lobjBatchDataReader("DestinationConnectionString").ToString(), lobjBatchDataReader("SourceConnectionString").ToString(), StringToEnum(lobjBatchDataReader("ContentStorageType").ToString(), GetType(Core.Content.StorageTypeEnum)), lobjBatchDataReader("DeclareAsRecordOnImport").ToString(), DeserializeString(lobjBatchDataReader("DeclareRecordConfiguration").ToString(), GetType(DeclareRecordConfiguration), New DeclareRecordConfiguration()), DeserializeString(lobjBatchDataReader("Transformations").ToString(), GetType(Transformations.TransformationCollection), New Transformations.TransformationCollection), StringToEnum(lobjBatchDataReader("DocumentFilingMode").ToString(), GetType(Core.FilingMode)), lobjBatchDataReader("LeadingDelimiter").ToString(), StringToEnum(lobjBatchDataReader("BasePathLocation").ToString(), GetType(Migrations.ePathLocation)), lobjBatchDataReader("FolderDelimiter").ToString())
              Dim lobjBatch As Batch = GetBatchFromDataReader(lpJob, lobjBatchDataReader)

              lobjBatches.Add(lobjBatch)

            End While

            If (lobjBatchDataReader IsNot Nothing) Then
              lobjBatchDataReader.Close()
            End If

          End If

        End While

      End If

      If (lobjDataReader IsNot Nothing) Then
        lobjDataReader.Close()
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw

    Finally

      If (lobjDataReader IsNot Nothing) Then
        lobjDataReader.Close()
      End If

      If (lobjConnection.State = ConnectionState.Open) Then
        lobjConnection.Close()
      End If

    End Try

    Return lobjBatches

  End Function

#End Region

#Region "Save Methods"

  Private Sub SaveProjectToDB(ByVal lpProject As Project)

    Try

      'Check if database exists, if not, create it
      If (Not DoesDBExist(mstrFullDatabasePath)) Then
        CreateDatabaseAndTables(mstrFullDatabasePath)
      End If

      'Save Project Related Info
      UtilSaveProject(lpProject)

      'Save Job and Batch Related Info
      SaveJobsAndBatchesToDB(lpProject.Jobs)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Sub

  Private Sub UtilSaveProject(ByVal lpProject As Project)

    Dim lobjConnection As OleDbConnection = Nothing

    Try

      lobjConnection = New OleDbConnection(Me.ItemsLocation.Location)

      Dim lstrSQL As String = String.Format("SELECT ProjectId from tblProject where ProjectId = '{0}'", lpProject.Id)
      Dim lstrUpdateSQL As String = String.Empty

      Dim cmdSelect As New OleDbCommand(lstrSQL, lobjConnection)
      Helper.HandleConnection(lobjConnection)

      'Check if project already exists in db, if it does, do an update, else do an insert
      Dim lstrProjectId As String = cmdSelect.ExecuteScalar()

      If (lstrProjectId = Nothing OrElse lstrProjectId = String.Empty OrElse IsDBNull(lstrProjectId)) Then
        lstrUpdateSQL = String.Format("INSERT Into tblProject(ProjectId,ProjectName,Description,BatchSize,ItemsLocation) VALUES('{0}','{1}','{2}',{3},'{4}')", lpProject.Id, lpProject.Name.Replace("'", "''"), lpProject.Description.Replace("'", "''"), lpProject.BatchSize, SerializeString(lpProject.ItemsLocation).Replace("'", "''"))

      Else
        lstrUpdateSQL = String.Format("UPDATE tblProject set ProjectName = '{1}', Description = '{2}', BatchSize = {3}, ItemsLocation = '{4}' WHERE ProjectId = '{0}'", lpProject.Id, lpProject.Name.Replace("'", "''"), lpProject.Description.Replace("'", "''"), lpProject.BatchSize, SerializeString(lpProject.ItemsLocation).Replace("'", "''"))
      End If

      Dim cmdUpdate As New OleDbCommand(lstrUpdateSQL, lobjConnection)
      Helper.HandleConnection(lobjConnection)

      Dim lintNewRecordAff As Integer = cmdUpdate.ExecuteNonQuery()

      If (lintNewRecordAff = 0) Then
        Throw New Exception(String.Format("{0}: Failed to insert/update item tblProject. Sql: {1}", Reflection.MethodBase.GetCurrentMethod, lstrUpdateSQL))
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw

    Finally

      If (lobjConnection.State = ConnectionState.Open) Then
        lobjConnection.Close()
      End If

    End Try

  End Sub

  Private Sub SaveJobsAndBatchesToDB(ByVal lpJobs As Jobs)

    Dim lobjConnection As OleDbConnection = Nothing

    Try
      lobjConnection = New OleDbConnection(Me.ItemsLocation.Location)

      For Each lobjJob As Job In lpJobs

        'Insert/Update Job tables
        SaveJobToDB(lobjJob)

        'Insert/Update Batch tables
        SaveBatchesToDB(lobjJob.Batches)

      Next

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw

    Finally

      If (lobjConnection.State = ConnectionState.Open) Then
        lobjConnection.Close()
      End If

    End Try

  End Sub

  Private Sub SaveBatchesToDB(ByVal lpBatches As Batches)

    Dim lobjConnection As OleDbConnection = Nothing

    Try

      lobjConnection = New OleDbConnection(Me.ItemsLocation.Location)

      For Each lobjBatch As Batch In lpBatches

        Dim lstrJobSQL As String = String.Format("SELECT BatchId from tblBatch where BatchId = '{0}'", lobjBatch.Id)
        Dim lstrJobUpdateSQL As String = String.Empty

        Dim cmdJobSelect As New OleDbCommand(lstrJobSQL, lobjConnection)
        Helper.HandleConnection(lobjConnection)

        'Check if job already exists in db, if it does, do an update, else do an insert
        Dim lstrBatchId As String = cmdJobSelect.ExecuteScalar()

        If (lstrBatchId = Nothing OrElse lstrBatchId = String.Empty OrElse IsDBNull(lstrBatchId)) Then
          lstrJobUpdateSQL = String.Format("INSERT Into tblBatch(BatchId,BatchName,Description,ItemsLocation,AssignedTo,ExportPath,Transformations,Operation,DestinationConnectionString,SourceConnectionString,ContentStorageType,DeclareAsRecordOnImport,DeclareRecordConfiguration,DocumentFilingMode,LeadingDelimiter,BasePathLocation,FolderDelimiter) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}')", lobjBatch.Id, lobjBatch.Name.Replace("'", "''"), lobjBatch.Description.Replace("'", "''"), SerializeString(lobjBatch.ItemsLocation).Replace("'", "''"), lobjBatch.AssignedTo, lobjBatch.ExportPath, SerializeString(lobjBatch.Transformations).Replace("'", "''"), lobjBatch.Operation, lobjBatch.DestinationConnectionString.Replace("'", "''"), lobjBatch.SourceConnectionString.Replace("'", "''"), lobjBatch.ContentStorageType, lobjBatch.DeclareAsRecordOnImport, SerializeString(lobjBatch.DeclareRecordConfiguration).Replace("'", "''"), lobjBatch.DocumentFilingMode, lobjBatch.LeadingDelimiter, lobjBatch.BasePathLocation, lobjBatch.FolderDelimiter)

        Else
          lstrJobUpdateSQL = String.Format("UPDATE tblBatch set BatchName = '{1}', Description = '{2}', ItemsLocation = '{3}', AssignedTo = '{4}', ExportPath = '{5}', Transformations = '{6}' ,Operation = '{7}', DestinationConnectionString = '{8}',SourceConnectionString = '{9}', ContentStorageType = '{10}', DeclareAsRecordOnImport = '{11}',DeclareRecordConfiguration = '{12}',  DocumentFilingMode = '{13}', LeadingDelimiter = '{14}', BasePathLocation = '{15}', FolderDelimiter = '{16}' WHERE BatchId = '{0}'", lobjBatch.Id, lobjBatch.Name.Replace("'", "''"), lobjBatch.Description.Replace("'", "''"), SerializeString(lobjBatch.ItemsLocation).Replace("'", "''"), lobjBatch.AssignedTo, lobjBatch.ExportPath, SerializeString(lobjBatch.Transformations).Replace("'", "''"), lobjBatch.Operation, lobjBatch.DestinationConnectionString.Replace("'", "''"), lobjBatch.SourceConnectionString.Replace("'", "''"), lobjBatch.ContentStorageType, lobjBatch.DeclareAsRecordOnImport, SerializeString(lobjBatch.DeclareRecordConfiguration).Replace("'", "''"), lobjBatch.DocumentFilingMode, lobjBatch.LeadingDelimiter, lobjBatch.BasePathLocation, lobjBatch.FolderDelimiter)
        End If

        Dim cmdJobUpdate As New OleDbCommand(lstrJobUpdateSQL, lobjConnection)
        Helper.HandleConnection(lobjConnection)

        Dim lintJobNewRecordAff As Integer = cmdJobUpdate.ExecuteNonQuery()

        If (lintJobNewRecordAff = 0) Then
          Throw New Exception(String.Format("{0}: Failed to insert/update item tblJob. Sql: {1}", Reflection.MethodBase.GetCurrentMethod, lstrJobUpdateSQL))
        End If

        ''''JOB BATCH RELATIONSHIP''''
        Dim lstrPJSQL As String = String.Format("SELECT BatchId from tblJobBatchRel where BatchId = '{0}' and JobId = '{1}'", lobjBatch.Id, lobjBatch.Job.Id)
        Dim lstrPJUpdateSQL As String = String.Empty

        Dim cmdPJSelect As New OleDbCommand(lstrPJSQL, lobjConnection)
        Helper.HandleConnection(lobjConnection)

        'Check if relationship already exists in db, if it does, do nothing, else do an insert
        lstrBatchId = cmdPJSelect.ExecuteScalar()

        If (lstrBatchId = Nothing OrElse lstrBatchId = String.Empty OrElse IsDBNull(lstrBatchId)) Then
          lstrPJUpdateSQL = String.Format("INSERT Into tblJobBatchRel(BatchId,JobId) VALUES('{0}','{1}')", lobjBatch.Id, lobjBatch.Job.Id)

          Dim cmdPJUpdate As New OleDbCommand(lstrPJUpdateSQL, lobjConnection)
          Helper.HandleConnection(lobjConnection)

          Dim lintPJNewRecordAff As Integer = cmdPJUpdate.ExecuteNonQuery()

          If (lintPJNewRecordAff = 0) Then
            Throw New Exception(String.Format("{0}: Failed to insert item tblJobBatchRel. Sql: {1}", Reflection.MethodBase.GetCurrentMethod, lstrPJUpdateSQL))
          End If

        End If

      Next

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw

    Finally

      If (lobjConnection.State = ConnectionState.Open) Then
        lobjConnection.Close()
      End If

    End Try

  End Sub

  Private Sub SaveJobToDB(ByVal lpJob As Job)

    Dim lobjConnection As OleDbConnection = Nothing

    Try

      lobjConnection = New OleDbConnection(Me.ItemsLocation.Location)

      Dim lstrJobSQL As String = String.Format("SELECT JobId from tblJob where JobId = '{0}'", lpJob.Id)
      Dim lstrJobUpdateSQL As String = String.Empty

      Dim cmdJobSelect As New OleDbCommand(lstrJobSQL, lobjConnection)
      Helper.HandleConnection(lobjConnection)

      'Check if job already exists in db, if it does, do an update, else do an insert
      Dim lstrJobId As String = cmdJobSelect.ExecuteScalar()

      If (lstrJobId = Nothing OrElse lstrJobId = String.Empty OrElse IsDBNull(lstrJobId)) Then
        lstrJobUpdateSQL = String.Format("INSERT Into tblJob(JobId,JobName,Description,JobSource,BatchSize,ItemsLocation,Operation,DestinationConnectionString,ContentStorageType,DeclareAsRecordOnImport,DeclareRecordConfiguration,Transformations,DocumentFilingMode,LeadingDelimiter,BasePathLocation,FolderDelimiter) VALUES('{0}','{1}','{2}','{3}',{4},'{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}')", lpJob.Id, lpJob.Name.Replace("'", "''"), lpJob.Description.Replace("'", "''"), SerializeString(lpJob.Source).Replace("'", "''"), lpJob.BatchSize, SerializeString(lpJob.ItemsLocation).Replace("'", "''"), lpJob.Operation, lpJob.DestinationConnectionString.Replace("'", "''"), lpJob.ContentStorageType, lpJob.DeclareAsRecordOnImport, SerializeString(lpJob.DeclareRecordConfiguration).Replace("'", "''"), SerializeString(lpJob.Transformations).Replace("'", "''"), lpJob.DocumentFilingMode, lpJob.LeadingDelimiter, lpJob.BasePathLocation, lpJob.FolderDelimiter)

      Else
        lstrJobUpdateSQL = String.Format("UPDATE tblJob set JobName = '{1}', Description = '{2}', JobSource = '{3}', BatchSize = {4}, ItemsLocation = '{5}', Operation = '{6}' , DestinationConnectionString = '{7}', ContentStorageType = '{8}', DeclareAsRecordOnImport = '{9}',DeclareRecordConfiguration = '{10}', Transformations = '{11}', DocumentFilingMode = '{12}', LeadingDelimiter = '{13}', BasePathLocation = '{14}', FolderDelimiter = '{15}' WHERE JobId = '{0}'", lpJob.Id, lpJob.Name.Replace("'", "''"), lpJob.Description.Replace("'", "''"), SerializeString(lpJob.Source).Replace("'", "''"), lpJob.BatchSize, SerializeString(lpJob.ItemsLocation).Replace("'", "''"), lpJob.Operation, lpJob.DestinationConnectionString.Replace("'", "''"), lpJob.ContentStorageType, lpJob.DeclareAsRecordOnImport, SerializeString(lpJob.DeclareRecordConfiguration).Replace("'", "''"), SerializeString(lpJob.Transformations).Replace("'", "''"), lpJob.DocumentFilingMode, lpJob.LeadingDelimiter, lpJob.BasePathLocation, lpJob.FolderDelimiter)
      End If

      Dim cmdJobUpdate As New OleDbCommand(lstrJobUpdateSQL, lobjConnection)
      Helper.HandleConnection(lobjConnection)

      Dim lintJobNewRecordAff As Integer = cmdJobUpdate.ExecuteNonQuery()

      If (lintJobNewRecordAff = 0) Then
        Throw New Exception(String.Format("{0}: Failed to insert/update item tblJob. Sql: {1}", Reflection.MethodBase.GetCurrentMethod, lstrJobUpdateSQL))
      End If

      ''''PROJECT JOB RELATIONSHIP''''
      Dim lstrPJSQL As String = String.Format("SELECT JobId from tblProjectJobRel where JobId = '{0}' and ProjectId = '{1}'", lpJob.Id, lpJob.Project.Id)
      Dim lstrPJUpdateSQL As String = String.Empty

      Dim cmdPJSelect As New OleDbCommand(lstrPJSQL, lobjConnection)
      Helper.HandleConnection(lobjConnection)

      'Check if relationship already exists in db, if it does, do nothing, else do an insert
      lstrJobId = cmdPJSelect.ExecuteScalar()

      If (lstrJobId = Nothing OrElse lstrJobId = String.Empty OrElse IsDBNull(lstrJobId)) Then
        lstrPJUpdateSQL = String.Format("INSERT Into tblProjectJobRel(JobId,ProjectId) VALUES('{0}','{1}')", lpJob.Id, lpJob.Project.Id)

        Dim cmdPJUpdate As New OleDbCommand(lstrPJUpdateSQL, lobjConnection)
        Helper.HandleConnection(lobjConnection)

        Dim lintPJNewRecordAff As Integer = cmdPJUpdate.ExecuteNonQuery()

        If (lintPJNewRecordAff = 0) Then
          Throw New Exception(String.Format("{0}: Failed to insert item tblProjectJobRel. Sql: {1}", Reflection.MethodBase.GetCurrentMethod, lstrPJUpdateSQL))
        End If

      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw

    Finally

      If (lobjConnection.State = ConnectionState.Open) Then
        lobjConnection.Close()
      End If

    End Try

  End Sub

#End Region

  ''' <summary>
  ''' If the batch is NOT locked and there is at least one item 'NotProcessed' return true
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Private Function IsAvailableForProcessingDB(ByVal lpBatchId As String) As Boolean

    Dim lobjConnection As OleDbConnection = Nothing
    Dim lobjDataReader As OleDbDataReader = Nothing

    Try

      'Check if already locked, if so then throw exception
      Dim lstrSQL As String = String.Format("SELECT ID from {0} WHERE BatchID = '{1}' AND IsLocked = '1'", TABLE_BATCHLOCK_NAME, lpBatchId)

      lobjConnection = New OleDbConnection(Me.ItemsLocation.Location)

      Dim lobjCommand As New OleDbCommand(lstrSQL, lobjConnection)
      Helper.HandleConnection(lobjConnection)

      Dim lstrId As String = lobjCommand.ExecuteScalar()

      If (lstrId <> String.Empty) Then
        Return False
      End If

      lstrSQL = String.Format("SELECT TOP 1 ID from {0} where ProcessedStatus = '{1}' and BatchId = '{2}'", TABLE_BATCH_ITEM_NAME, ProcessedStatus.NotProcessed.ToString, lpBatchId)
      lobjCommand = New OleDbCommand(lstrSQL, lobjConnection)
      Helper.HandleConnection(lobjConnection)
      lobjDataReader = lobjCommand.ExecuteReader

      Return lobjDataReader.HasRows

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      Return False

    Finally

      If (lobjDataReader IsNot Nothing) Then
        lobjDataReader.Close()
      End If

      If (lobjConnection.State = ConnectionState.Open) Then
        lobjConnection.Close()
      End If

    End Try

  End Function

  Private Sub AddItemToDB(ByVal lpBatchItem As BatchItem)

    Try

      Dim lstrSql As String = String.Format("INSERT INTO {5}(BatchId,Title,SourceDocId,ProcessedStatus,Operation)" & "Values('{0}','{1}','{2}','{3}','{4}')", lpBatchItem.BatchId, lpBatchItem.Title.Replace("'", "''"), lpBatchItem.SourceDocId, ProcessedStatus.NotProcessed.ToString, lpBatchItem.Operation.ToString, TABLE_BATCH_ITEM_NAME)

      If (mobjConnection Is Nothing) Then
        mobjConnection = New OleDbConnection(Me.ItemsLocation.Location)
      End If

      Dim cmdAdd As New OleDbCommand(lstrSql, mobjConnection)
      Helper.HandleConnection(mobjConnection)

      Dim lintNewRecordAff As Integer = cmdAdd.ExecuteNonQuery()

      If (lintNewRecordAff = 0) Then
        Throw New Exception(String.Format("{0}: Failed to add item to the batch tblBatchItems. Sql: {1}", Reflection.MethodBase.GetCurrentMethod, lstrSql))
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw

    Finally
      'mobjConnection.Close()
    End Try

  End Sub

  Private Function GetItemsByIdDB(ByVal lpBatch As Batch, _
                                  ByVal lpIdArrayList As ArrayList) As BatchItems

    Dim lobjBatches As New BatchItems()
    Dim lobjDataReader As OleDbDataReader = Nothing
    Dim lobjConnection As OleDbConnection = Nothing

    Try

      If (lpIdArrayList.Count = 0) Then
        Return Nothing
      End If

      Dim lstrInList As String = String.Empty

      For i As Integer = 0 To lpIdArrayList.Count - 1
        lstrInList &= lpIdArrayList(i).ToString & ","
      Next

      If (lstrInList.EndsWith(",")) Then
        lstrInList = lstrInList.TrimEnd(",")
      End If

      Dim lstrSql As String = String.Format("SELECT ID,BatchID,Title ,SourceDocID,Operation FROM {0} WHERE Id in ({1}) order by Id", TABLE_BATCH_ITEM_NAME, lstrInList)

      lobjConnection = New OleDbConnection(Me.ItemsLocation.Location)

      Dim lobjCommand As New OleDbCommand(lstrSql, lobjConnection)
      Helper.HandleConnection(lobjConnection)

      lobjDataReader = lobjCommand.ExecuteReader

      If (lobjDataReader.HasRows = True) Then

        Dim lobjBatchItem As BatchItem = Nothing
        'Dim lenuOperationType As OperationType
        'Dim lstrOperationType As String

        While lobjDataReader.Read

          'If we have an issue with one particular item, log it and move on to the next one
          Try

            ''lenuOperationType = StringToEnum(lobjDataReader("Operation").ToString, GetType(OperationType))
            'lstrOperationType = lobjDataReader("Operation").ToString

            ''lobjBatchItem = BatchItem.CreateBatchItem(lenuOperationType, lobjDataReader("SourceDocId").ToString, lobjDataReader("Title").ToString, lpBatch)
            'lobjBatchItem = BatchItem.CreateBatchItem(lstrOperationType, lobjDataReader("SourceDocId").ToString, lobjDataReader("Title").ToString, lpBatch)

            'lobjBatchItem.BatchId = lobjDataReader("BatchID").ToString
            'lobjBatchItem.Id = lobjDataReader("ID").ToString
            ''lobjBatchItem.Operation = lenuOperationType
            'lobjBatchItem.Operation = lstrOperationType

            lobjBatchItem = GetBatchItemFromDataReader(lobjDataReader, lpBatch, False)
            lobjBatches.Add(lobjBatchItem)

          Catch ex As Exception
            ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
          End Try

        End While

      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)

    Finally

      If (lobjDataReader IsNot Nothing) Then
        lobjDataReader.Close()
      End If

      If (lobjConnection.State = ConnectionState.Open) Then
        lobjConnection.Close()
      End If

    End Try

    Return lobjBatches

  End Function

  Private Function GetAllUnprocessedItemsFromDB(ByVal lpBatch As Batch) As BatchItems

    Dim lobjBatches As New BatchItems()
    Dim lobjDataReader As OleDbDataReader = Nothing
    Dim lobjConnection As OleDbConnection = Nothing

    Try

      Dim lstrSql As String = String.Format("SELECT ID,BatchID,Title ,SourceDocID,Operation FROM {0} WHERE ProcessedStatus = '{1}' AND BatchId = '{2}' order by Title", TABLE_BATCH_ITEM_NAME, ProcessedStatus.NotProcessed, lpBatch.Id)

      lobjConnection = New OleDbConnection(Me.ItemsLocation.Location)

      Dim lobjCommand As New OleDbCommand(lstrSql, lobjConnection)
      Helper.HandleConnection(lobjConnection)

      lobjDataReader = lobjCommand.ExecuteReader

      If (lobjDataReader.HasRows = True) Then

        Dim lobjBatchItem As BatchItem = Nothing
        'Dim lenuOperationType As OperationType

        While lobjDataReader.Read

          'If we have an issue with one particular item, log it and move on to the next one
          Try

            'lenuOperationType = StringToEnum(lobjDataReader("Operation").ToString, GetType(OperationType))

            'lobjBatchItem = BatchItem.CreateBatchItem(lenuOperationType, lobjDataReader("SourceDocId").ToString, lobjDataReader("Title").ToString, lpBatch)

            'lobjBatchItem.BatchId = lobjDataReader("BatchID").ToString
            'lobjBatchItem.Id = lobjDataReader("ID").ToString
            'lobjBatchItem.Operation = lenuOperationType

            lobjBatchItem = GetBatchItemFromDataReader(lobjDataReader, lpBatch, False)
            lobjBatches.Add(lobjBatchItem)

          Catch ex As Exception
            ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
          End Try

        End While

      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)

    Finally

      If (lobjDataReader IsNot Nothing) Then
        lobjDataReader.Close()
      End If

      If (lobjConnection.State = ConnectionState.Open) Then
        lobjConnection.Close()
      End If

    End Try

    Return lobjBatches

  End Function

  Private Function GetAllItemsFromDB(ByVal lpBatch As Batch) As BatchItems

    Dim lobjBatches As New BatchItems()
    Dim lobjDataReader As OleDbDataReader = Nothing
    Dim lobjConnection As OleDbConnection = Nothing

    Try

      Dim lstrSql As String = String.Format("SELECT * FROM {0} WHERE BatchId = '{1}' order by Title", TABLE_BATCH_ITEM_NAME, lpBatch.Id)

      lobjConnection = New OleDbConnection(Me.ItemsLocation.Location)

      Dim lobjCommand As New OleDbCommand(lstrSql, lobjConnection)
      Helper.HandleConnection(lobjConnection)

      lobjDataReader = lobjCommand.ExecuteReader

      If (lobjDataReader.HasRows = True) Then

        Dim lobjBatchItem As BatchItem = Nothing
        'Dim lenuOperationType As OperationType

        While lobjDataReader.Read

          'If we have an issue with one particular item, log it and move on to the next one
          Try

            'lenuOperationType = StringToEnum(lobjDataReader("Operation").ToString, GetType(OperationType))

            'lobjBatchItem = BatchItem.CreateBatchItem(lenuOperationType, lobjDataReader("SourceDocId").ToString, lobjDataReader("Title").ToString, lpBatch)

            'lobjBatchItem.BatchId = lobjDataReader("BatchID").ToString
            'lobjBatchItem.Id = lobjDataReader("ID").ToString
            'lobjBatchItem.Operation = lenuOperationType

            lobjBatchItem = GetBatchItemFromDataReader(lobjDataReader, lpBatch, True)
            lobjBatches.Add(lobjBatchItem)

          Catch ex As Exception
            ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
          End Try

        End While

      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)

    Finally

      If (lobjDataReader IsNot Nothing) Then
        lobjDataReader.Close()
      End If

      If (lobjConnection.State = ConnectionState.Open) Then
        lobjConnection.Close()
      End If

    End Try

    Return lobjBatches

  End Function

  'Private Function GetBatchItemFromDataReader(ByRef lpDataReader As DbDataReader, ByRef lpBatch As Batch) As IBatchItem
  '  Try

  '    Dim lobjBatchItem As BatchItem = Nothing
  '    Dim lstrOperationType As String

  '    'lenuOperationType = StringToEnum(lobjDataReader("Operation").ToString, GetType(OperationType))
  '    lstrOperationType = lpDataReader("Operation").ToString

  '    'lobjBatchItem = BatchItem.CreateBatchItem(lenuOperationType, lobjDataReader("SourceDocId").ToString, lobjDataReader("Title").ToString, lpBatch)
  '    lobjBatchItem = BatchItem.CreateBatchItem(lstrOperationType, lpDataReader("SourceDocId").ToString, lpDataReader("Title").ToString, lpBatch)

  '    With lobjBatchItem
  '      .BatchId = lpDataReader("BatchID").ToString
  '      .Id = lpDataReader("ID").ToString
  '      .Operation = lstrOperationType
  '    End With

  '    Return lobjBatchItem

  '  Catch ex As Exception
  '    ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '    ' Re-throw the exception to the caller
  '    Throw
  '  End Try
  'End Function

  Private Function GetAllItemsFromDBToDataTable(ByVal lpObject As Object, _
                                                ByVal lpProcessedStatusFilter As String) As DataTable

    Dim lobjDataTable As DataTable = New DataTable()
    Dim lobjConnection As OleDbConnection = Nothing

    Try

      Dim lstrSql As String = String.Empty

      If TypeOf lpObject Is Batch Then

        Dim lobjBatch As Batch = lpObject

        If (lpProcessedStatusFilter Is Nothing OrElse lpProcessedStatusFilter = String.Empty) Then
          lstrSql = String.Format("SELECT * FROM {0} (nolock) WHERE BatchId = '{1}'", TABLE_BATCH_ITEM_NAME, lobjBatch.Id)

        Else
          lstrSql = String.Format("SELECT * FROM {0} (nolock) WHERE BatchId = '{1}' and ProcessedStatus = '{2}' ", TABLE_BATCH_ITEM_NAME, lobjBatch.Id, lpProcessedStatusFilter)
        End If

      ElseIf TypeOf lpObject Is Job Then

        Dim lobjJob As Job = lpObject

        If (lpProcessedStatusFilter Is Nothing OrElse lpProcessedStatusFilter = String.Empty) Then
          lstrSql = String.Format("SELECT * FROM {0} WHERE BatchId in (select batchid from tblJobBatchRel where JobId = '{1}') order by Title", TABLE_BATCH_ITEM_NAME, lobjJob.Id)

        Else
          lstrSql = String.Format("SELECT * FROM {0} WHERE BatchId in (select batchid from tblJobBatchRel where JobId = '{1}') and ProcessedStatus = '{2}' order by Title", TABLE_BATCH_ITEM_NAME, lobjJob.Id, lpProcessedStatusFilter)
        End If

      Else
        Throw New Exception("Can't get batch items, type not supported.")
      End If

      lobjConnection = New OleDbConnection(Me.ItemsLocation.Location)

      Dim lobjDA As New OleDbDataAdapter()
      lobjDA.SelectCommand = New OleDbCommand(lstrSql, lobjConnection)
      Helper.HandleConnection(lobjConnection)
      lobjDA.Fill(lobjDataTable)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)

    Finally

      If (lobjConnection.State = ConnectionState.Open) Then
        lobjConnection.Close()
      End If

    End Try

    Return lobjDataTable

  End Function

  Private Function ExecuteNonQuery(ByVal lpSQL As String) As Integer

    Dim lintNewRecordAff As Integer = 0
    Dim lobjConnection As OleDbConnection = Nothing
    Dim cmdAdd As OleDbCommand = Nothing

    Try
      lobjConnection = New OleDbConnection(Me.ItemsLocation.Location)

      cmdAdd = New OleDbCommand(lpSQL, lobjConnection)
      Helper.HandleConnection(lobjConnection)
      lintNewRecordAff = cmdAdd.ExecuteNonQuery()

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      Throw ex

    Finally

      If (lobjConnection.State = ConnectionState.Open) Then
        lobjConnection.Close()
      End If

    End Try

    Return lintNewRecordAff

  End Function

  Private Sub BeginItemProcessDB(ByVal e As BatchItemProcessEventArgs)

    Dim lintNewRecordAff As Integer = 0
    Dim lstrSql As String = String.Empty

    Try

      lstrSql = String.Format("UPDATE {0} set ProcessedStatus = '{1}'," & "ProcessStartTime = '{2}', ProcessedBy = '{5}' " & "WHERE ID = {3} AND BatchId = '{4}'", TABLE_BATCH_ITEM_NAME, ProcessedStatus.Processing, e.StartTime.ToString, e.ItemId, e.BatchId, e.ProcessedBy)

      lintNewRecordAff = ExecuteNonQuery(lstrSql)

      If (lintNewRecordAff = 0) Then
        Throw New Exception(String.Format("{0}: Failed to update item in tblBatchItems. Sql: {1}", Reflection.MethodBase.GetCurrentMethod, lstrSql))
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)

      If (ex.Message.StartsWith("Could not update; currently locked.")) Then

        'Retry
        Try
          ApplicationLogging.WriteLogEntry("BEGIN: Trying to update record after lock error. ItemId=" & e.ItemId, TraceEventType.Information)
          Threading.Thread.Sleep(1000) 'Hey, wait a sec
          lintNewRecordAff = ExecuteNonQuery(lstrSql)

        Catch exx As Exception
          ApplicationLogging.LogException(exx, MethodBase.GetCurrentMethod)
          Throw
        End Try

      End If

      ' Re-throw the exception to the caller
      Throw
    End Try

  End Sub

  Private Sub EndItemProcessDB(ByVal e As BatchItemProcessEventArgs)

    Dim lintNewRecordAff As Integer = 0
    Dim lstrSql As String = String.Empty

    Try

      lstrSql = String.Format("UPDATE {0} set ProcessedStatus = '{1}', DestDocId = '{2}'," & "ProcessedMessage = '{3}', ProcessFinishTime = '{4}', TotalProcessingTime = {7} " & "WHERE ID = {5} AND BatchId = '{6}'", TABLE_BATCH_ITEM_NAME, e.ProcessedStatus.ToString, e.DestDocId.Replace("'", "''"), e.ProcessedMessage.Replace("'", "''"), e.EndTime.ToString, e.ItemId, e.BatchId, e.TotalProcessingTime)

      lintNewRecordAff = ExecuteNonQuery(lstrSql)

      If (lintNewRecordAff = 0) Then
        Throw New Exception(String.Format("{0}: Failed to update item in tblBatchItems. Sql: {1}", Reflection.MethodBase.GetCurrentMethod, lstrSql))
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)

      If (ex.Message.StartsWith("Could not update; currently locked.")) Then

        'Retry
        Try
          ApplicationLogging.WriteLogEntry("END:Trying to update record after lock error. ItemID=" & e.ItemId, TraceEventType.Information)
          Threading.Thread.Sleep(1000) 'Hey, wait a sec
          lintNewRecordAff = ExecuteNonQuery(lstrSql)

        Catch exx As Exception
          ApplicationLogging.LogException(exx, MethodBase.GetCurrentMethod)
          Throw
        End Try

      End If

      ' Re-throw the exception to the caller
      Throw
    End Try

  End Sub

#Region "Lock/UnLock"

  ''' <summary>
  ''' Add a lock record
  ''' </summary>
  ''' <param name="lpBatchId"></param>
  ''' <param name="lpLockedBy"></param>
  ''' <remarks></remarks>
  Private Sub LockBatchDB(ByVal lpBatchId As String, _
                          ByVal lpLockedBy As String)

    Dim lobjConnection As OleDbConnection = Nothing

    Try

      'Check if already locked, if so then throw exception
      Dim lstrSQL As String = String.Format("SELECT LockedBy from {0} WHERE BatchID = '{1}' AND IsLocked = '1'", TABLE_BATCHLOCK_NAME, lpBatchId)

      lobjConnection = New OleDbConnection(Me.ItemsLocation.Location)

      Dim lobjCommand As New OleDbCommand(lstrSQL, lobjConnection)
      Helper.HandleConnection(lobjConnection)

      Dim lstrCurrentlyLockedBy As String = lobjCommand.ExecuteScalar()

      If (lstrCurrentlyLockedBy <> String.Empty) Then
        Throw New Exception(String.Format("This batch is locked by another system - {0}.  Cannot process a batch that is currently locked.", lstrCurrentlyLockedBy))
      End If

      'Lock the batch
      lstrSQL = String.Format("Insert into {0}(BatchId,IsLocked,LockDate,LockedBy) VALUES('{1}','1','{2}','{3}')", TABLE_BATCHLOCK_NAME, lpBatchId, Now.ToString, lpLockedBy)
      lobjCommand = New OleDbCommand(lstrSQL, lobjConnection)
      Helper.HandleConnection(lobjConnection)

      Dim lintRowsAffected As Integer = lobjCommand.ExecuteNonQuery()

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      Throw ex

      If (lobjConnection.State = ConnectionState.Open) Then
        lobjConnection.Close()
      End If

    End Try

  End Sub

  ''' <summary>
  ''' Removes a lock record
  ''' </summary>
  ''' <param name="lpBatchId"></param>
  ''' <param name="lpUnLockedBy"></param>
  ''' <remarks></remarks>
  Private Sub UnLockBatchDB(ByVal lpBatchId As String, _
                            ByVal lpUnLockedBy As String)

    Dim lobjConnection As OleDbConnection = Nothing

    Try

      'UnLock the batch
      lobjConnection = New OleDbConnection(Me.ItemsLocation.Location)

      Dim lstrSQL As String = String.Format("Delete FROM {0} WHERE BatchId = '{1}' and IsLocked = Yes", TABLE_BATCHLOCK_NAME, lpBatchId)
      Dim lobjCommand As New OleDbCommand(lstrSQL, lobjConnection)
      Helper.HandleConnection(lobjConnection)

      Dim lintRowsAffected As Integer = lobjCommand.ExecuteNonQuery()

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      Throw ex

      If (lobjConnection.State = ConnectionState.Open) Then
        lobjConnection.Close()
      End If

    End Try

  End Sub

#End Region

  Private Function GetBatchSummaryCountsDB(ByVal lpBatch As Batch) As WorkSummary

    Dim lobjDataReader As OleDbDataReader = Nothing
    Dim lobjConnection As OleDbConnection = Nothing
    Dim lobjBatchSummary As WorkSummary = Nothing

    Try

      Dim lstrSql As String = String.Format("SELECT  SuccessCount,FailedCount,NotProcessedCount,ProcessingCount,TotalItemCount,AvgProcessingTime from" & "(select count(*) as SuccessCount FROM {0} where processedstatus = 'Success' and batchid = '{1}') a," & "(select count(*) as FailedCount FROM {0} where processedstatus = 'Failed' and batchid = '{1}') b, " & "(select count(*) as NotProcessedCount FROM {0} where processedstatus = 'NotProcessed' and batchid = '{1}') c, " & "(select count(*) as ProcessingCount FROM {0} where processedstatus = 'Processing' and batchid = '{1}') d, " & "(select count(*) as TotalItemCount FROM {0} where  batchid = '{1}') e," & "(select Sum(TotalProcessingTime) /count(*) as AvgProcessingTime  FROM {0} where processedstatus = 'Success' and  batchid = '{1}') f", TABLE_BATCH_ITEM_NAME, lpBatch.Id)

      lobjConnection = New OleDbConnection(Me.ItemsLocation.Location)

      Dim lobjCommand As New OleDbCommand(lstrSql, lobjConnection)
      Helper.HandleConnection(lobjConnection)
      lobjDataReader = lobjCommand.ExecuteReader

      If (lobjDataReader.HasRows = True) Then

        While lobjDataReader.Read
          'Should only be 1 row

          Dim ldblAvgProcessingTime As Double = 0

          If (IsDBNull(lobjDataReader("AvgProcessingTime")) = False) Then
            ldblAvgProcessingTime = Convert.ToDouble(lobjDataReader("AvgProcessingTime"))
          End If

          lobjBatchSummary = New WorkSummary(lpBatch, Convert.ToInt32(lobjDataReader("NotProcessedCount")), Convert.ToInt32(lobjDataReader("SuccessCount")), Convert.ToInt32(lobjDataReader("FailedCount")), Convert.ToInt32(lobjDataReader("ProcessingCount")), Convert.ToInt32(lobjDataReader("TotalItemCount")), ldblAvgProcessingTime)

        End While

        lobjDataReader.Close()

      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)

    Finally

      If (lobjConnection.State = ConnectionState.Open) Then
        lobjConnection.Close()
      End If

    End Try

    Return lobjBatchSummary

  End Function

  'Private Function GetBatchSummaryCountsDB(ByVal lpJob As Job) As WorkSummary

  '  Dim lobjDataReader As OleDbDataReader = Nothing
  '  Dim lobjConnection As OleDbConnection = Nothing
  '  Dim lobjBatchSummary As WorkSummary = Nothing

  '  Try

  '    Dim lstrSql As String = String.Format("SELECT  SuccessCount,FailedCount,NotProcessedCount,ProcessingCount,TotalItemCount,AvgProcessingTime from" & "(select count(*) as SuccessCount FROM {0} where processedstatus = 'Success' and batchid in (select batchid from {1} where jobid = '{2}')) a," & "(select count(*) as FailedCount FROM {0} where processedstatus = 'Failed' and batchid in (select batchid from {1} where jobid = '{2}')) b, " & "(select count(*) as NotProcessedCount FROM {0} where processedstatus = 'NotProcessed' and batchid in (select batchid from {1} where jobid = '{2}')) c, " & "(select count(*) as ProcessingCount FROM {0} where processedstatus = 'Processing' and batchid in (select batchid from {1} where jobid = '{2}')) d, " & "(select count(*) as TotalItemCount FROM {0} where  batchid in (select batchid from {1} where jobid = '{2}')) e," & "(select Sum(TotalProcessingTime) /count(*) as AvgProcessingTime  FROM {0} where processedstatus = 'Success' and  batchid in (select batchid from {1} where jobid = '{2}')) f", TABLE_BATCH_ITEM_NAME, TABLE_JOBBATCHREL_NAME, lpJob.Id)

  '    lobjConnection = New OleDbConnection(Me.ItemsLocation.Location)

  '    Dim lobjCommand As New OleDbCommand(lstrSql, lobjConnection)
  '    Helper.HandleConnection(lobjConnection)
  '    lobjDataReader = lobjCommand.ExecuteReader

  '    If (lobjDataReader.HasRows = True) Then

  '      While lobjDataReader.Read
  '        'Should only be 1 row

  '        Dim ldblAvgProcessingTime As Double = 0

  '        If (IsDBNull(lobjDataReader("AvgProcessingTime")) = False) Then
  '          ldblAvgProcessingTime = Convert.ToDouble(lobjDataReader("AvgProcessingTime"))
  '        End If

  '        lobjBatchSummary = New WorkSummary(lpJob, Convert.ToInt32(lobjDataReader("NotProcessedCount")), Convert.ToInt32(lobjDataReader("SuccessCount")), Convert.ToInt32(lobjDataReader("FailedCount")), Convert.ToInt32(lobjDataReader("ProcessingCount")), Convert.ToInt32(lobjDataReader("TotalItemCount")), ldblAvgProcessingTime)

  '      End While

  '      lobjDataReader.Close()

  '    End If

  '  Catch ex As Exception
  '    ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)

  '  Finally

  '    If (lobjConnection.State = ConnectionState.Open) Then
  '      lobjConnection.Close()
  '    End If

  '  End Try

  '  Return lobjBatchSummary
  'End Function

  Private Function DBNullToDouble(ByVal lpDoubleValue As Object) As Double

    Dim ldblDouble As Double = 0

    Try

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)

    End Try

  End Function

  ''' <summary>
  ''' Delete most all items.  Leave tblaudit and tblversion along
  ''' </summary>
  ''' <remarks></remarks>
  Private Sub CleanEntireDB()

    Dim lobjConnection As OleDbConnection = Nothing

    Try
      lobjConnection = New OleDbConnection(Me.ItemsLocation.Location)

      Dim lstrSQL As String = String.Format("Delete FROM {0}", TABLE_BATCH_ITEM_NAME)
      Dim cmdDelete As New OleDbCommand(lstrSQL, lobjConnection)
      Helper.HandleConnection(lobjConnection)

      Dim lintNewRecordAff As Integer = cmdDelete.ExecuteNonQuery()

      lstrSQL = String.Format("Delete FROM {0}", TABLE_BATCH_NAME)
      cmdDelete = New OleDbCommand(lstrSQL, lobjConnection)
      Helper.HandleConnection(lobjConnection)
      lintNewRecordAff = cmdDelete.ExecuteNonQuery()

      lstrSQL = String.Format("Delete FROM {0}", TABLE_JOB_NAME)
      cmdDelete = New OleDbCommand(lstrSQL, lobjConnection)
      Helper.HandleConnection(lobjConnection)
      lintNewRecordAff = cmdDelete.ExecuteNonQuery()

      lstrSQL = String.Format("Delete FROM {0}", TABLE_PROJECTJOBREL_NAME)
      cmdDelete = New OleDbCommand(lstrSQL, lobjConnection)
      Helper.HandleConnection(lobjConnection)
      lintNewRecordAff = cmdDelete.ExecuteNonQuery()

      lstrSQL = String.Format("Delete FROM {0}", TABLE_JOBBATCHREL_NAME)
      cmdDelete = New OleDbCommand(lstrSQL, lobjConnection)
      Helper.HandleConnection(lobjConnection)
      lintNewRecordAff = cmdDelete.ExecuteNonQuery()

      lstrSQL = String.Format("Delete FROM {0}", TABLE_PROJECT_NAME)
      cmdDelete = New OleDbCommand(lstrSQL, lobjConnection)
      Helper.HandleConnection(lobjConnection)
      lintNewRecordAff = cmdDelete.ExecuteNonQuery()

      lstrSQL = String.Format("Delete FROM {0}", TABLE_BATCHLOCK_NAME)
      cmdDelete = New OleDbCommand(lstrSQL, lobjConnection)
      Helper.HandleConnection(lobjConnection)
      lintNewRecordAff = cmdDelete.ExecuteNonQuery()

      'Insert an audit record - TODO:make it's own method and add to all delete calls
      lstrSQL = String.Format("INSERT INTO {0}(AuditAction,Comments) VALUES('Database Clean','Database Clean')", TABLE_AUDIT_NAME)
      cmdDelete = New OleDbCommand(lstrSQL, lobjConnection)
      Helper.HandleConnection(lobjConnection)
      lintNewRecordAff = cmdDelete.ExecuteNonQuery()

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)

    Finally

      If (lobjConnection.State = ConnectionState.Open) Then
        lobjConnection.Close()
      End If

    End Try

  End Sub

  ''' <summary>
  ''' Removes items from tblBatch, tblJobBatchRel, tblBatchLock and tblBatchItems
  ''' </summary>
  ''' <param name="lpBatches"></param>
  ''' <remarks></remarks>
  Private Sub DeleteBatchesDB(ByVal lpBatches As Batches)

    Dim lobjConnection As OleDbConnection = Nothing

    Try
      lobjConnection = New OleDbConnection(Me.ItemsLocation.Location)

      Dim lstrSQL As String = String.Empty
      Dim cmdDelete As OleDbCommand
      Dim lintNewRecordAff As Integer

      For Each lobjBatch As Batch In lpBatches

        lstrSQL = String.Format("Delete FROM {0} WHERE BatchId = '{1}'", TABLE_BATCH_ITEM_NAME, lobjBatch.Id)
        cmdDelete = New OleDbCommand(lstrSQL, lobjConnection)
        Helper.HandleConnection(lobjConnection)
        lintNewRecordAff = cmdDelete.ExecuteNonQuery()

        lstrSQL = String.Format("Delete FROM {0} WHERE JobId = '{1}' and BatchId = '{2}'", TABLE_JOBBATCHREL_NAME, lpBatches.Job.Id, lobjBatch.Id)
        cmdDelete = New OleDbCommand(lstrSQL, lobjConnection)
        Helper.HandleConnection(lobjConnection)
        lintNewRecordAff = cmdDelete.ExecuteNonQuery()

        lstrSQL = String.Format("Delete FROM {0} WHERE BatchId = '{1}'", TABLE_BATCH_NAME, lobjBatch.Id)
        cmdDelete = New OleDbCommand(lstrSQL, lobjConnection)
        Helper.HandleConnection(lobjConnection)
        lintNewRecordAff = cmdDelete.ExecuteNonQuery()

        lstrSQL = String.Format("Delete FROM {0} WHERE BatchId = '{1}'", TABLE_BATCHLOCK_NAME, lobjBatch.Id)
        cmdDelete = New OleDbCommand(lstrSQL, lobjConnection)
        Helper.HandleConnection(lobjConnection)
        lintNewRecordAff = cmdDelete.ExecuteNonQuery()

      Next

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)

    Finally

      If (lobjConnection.State = ConnectionState.Open) Then
        lobjConnection.Close()
      End If

    End Try

  End Sub

  ''' <summary>
  ''' Removes a Job including all of it's batches
  ''' </summary>
  ''' <param name="lpJob"></param>
  ''' <remarks></remarks>
  Private Sub DeleteJobDB(ByVal lpJob As Job)

    Dim lobjConnection As OleDbConnection = Nothing

    Try

      'Delete all batches related to this job
      DeleteBatchesDB(lpJob.Batches)

      lobjConnection = New OleDbConnection(Me.ItemsLocation.Location)

      Dim lstrSQL As String = String.Empty
      Dim cmdDelete As OleDbCommand
      Dim lintNewRecordAff As Integer

      lstrSQL = String.Format("Delete FROM {0} WHERE JobId = '{1}'", TABLE_PROJECTJOBREL_NAME, lpJob.Id)
      cmdDelete = New OleDbCommand(lstrSQL, lobjConnection)
      Helper.HandleConnection(lobjConnection)
      lintNewRecordAff = cmdDelete.ExecuteNonQuery()

      lstrSQL = String.Format("Delete FROM {0} WHERE JobId = '{1}'", TABLE_JOB_NAME, lpJob.Id)
      cmdDelete = New OleDbCommand(lstrSQL, lobjConnection)
      Helper.HandleConnection(lobjConnection)
      lintNewRecordAff = cmdDelete.ExecuteNonQuery()

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)

    Finally

      If (lobjConnection.State = ConnectionState.Open) Then
        lobjConnection.Close()
      End If

    End Try

  End Sub

  Private Sub DeleteProjectDB(ByVal lpProject As Project)

    Dim lobjConnection As OleDbConnection = Nothing

    Try

      For Each lobjJob As Job In lpProject.Jobs

        'Delete all batches related to this job
        DeleteBatchesDB(lobjJob.Batches)

        'Delete the Job related entries
        DeleteJobDB(lobjJob)

      Next

      lobjConnection = New OleDbConnection(Me.ItemsLocation.Location)

      Dim lstrSQL As String = String.Empty
      Dim cmdDelete As OleDbCommand
      Dim lintNewRecordAff As Integer

      lstrSQL = String.Format("Delete FROM {0} WHERE ProjectId = '{1}'", TABLE_PROJECTJOBREL_NAME, lpProject.Id)
      cmdDelete = New OleDbCommand(lstrSQL, lobjConnection)
      Helper.HandleConnection(lobjConnection)
      lintNewRecordAff = cmdDelete.ExecuteNonQuery()

      lstrSQL = String.Format("Delete FROM {0} WHERE ProjectId = '{1}'", TABLE_PROJECT_NAME, lpProject.Id)
      cmdDelete = New OleDbCommand(lstrSQL, lobjConnection)
      Helper.HandleConnection(lobjConnection)
      lintNewRecordAff = cmdDelete.ExecuteNonQuery()

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)

    Finally

      If (lobjConnection.State = ConnectionState.Open) Then
        lobjConnection.Close()
      End If

    End Try

  End Sub

  Private Function DoesDBExist(ByVal lpDatabasePath As String) As Boolean

    Try
      Return IO.File.Exists(lpDatabasePath)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      Throw ex
    End Try

  End Function

  Private Sub CreateDatabaseAndTables(ByVal lpDatabasePath As String)

    Try

      Dim objStream As Stream = Nothing
      Dim objFileStream As FileStream = Nothing

      Dim abytResource As Byte()
      Dim objAssembly As System.Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly()
      'Dim lstrStrings As String() = objAssembly.GetManifestResourceNames()
      objStream = objAssembly.GetManifestResourceStream("Ecmg.Cts.Projects.MigrationJobTemplate.accdb")
      abytResource = New [Byte](objStream.Length - 1) {}
      objStream.Read(abytResource, 0, objStream.Length)
      objFileStream = New FileStream(lpDatabasePath, FileMode.Create)
      objFileStream.Write(abytResource, 0, objStream.Length)
      objFileStream.Close()

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      Throw ex
    End Try

  End Sub

  ' ''' <summary>
  ' ''' Provides a safe way to deserialize in case the string is empty or can't deserialize for some reason
  ' ''' returns an empty object reference
  ' ''' </summary>
  ' ''' <param name="lpString"></param>
  ' ''' <param name="lpType"></param>
  ' ''' <param name="lpEmptyObject"></param>
  ' ''' <returns></returns>
  ' ''' <remarks></remarks>
  'Private Function DeserializeString(ByVal lpString As String, _
  '                                   ByVal lpType As Type, _
  '                                   ByVal lpEmptyObject As Object) As Object

  '  Try
  '    Return Serializer.Deserialize.XmlString(lpString, lpType)

  '  Catch ex As Exception
  '    ApplicationLogging.WriteLogEntry(ex.Message, TraceEventType.Information)
  '  End Try

  '  Return lpEmptyObject

  'End Function

  'Public Function SerializeString(ByVal lpObject As Object) As String

  '  Try
  '    Return Serializer.Serialize.XmlString(lpObject)

  '  Catch ex As Exception
  '    ApplicationLogging.WriteLogEntry(ex.Message, TraceEventType.Information)
  '  End Try

  '  Return String.Empty
  'End Function

#End Region

#Region "Batch Update To Not Processed"

  Public Sub ResetItemsToNotProcessedDB(ByVal lpIdArrayList As ArrayList)

    Try

      If (lpIdArrayList.Count = 0) Then
        Exit Sub
      End If

      'Dim lobjDataTable As New DataTable
      'lobjDataTable.Columns.Add("Id", GetType(Integer))

      'For i As Integer = 0 To lpIdArrayList.Count - 1
      '  Dim lobjNewRow As DataRow = lobjDataTable.NewRow()
      '  lobjNewRow("Id") = lpIdArrayList(i)
      '  lobjDataTable.Rows.Add(lobjNewRow)
      'Next

      Using lobjConnection As New OleDbConnection(Me.ItemsLocation.Location)

        Dim lstrSQL = String.Empty
        Dim lintRowsAffected As Integer = 0
        Dim lobjCommand As OleDbCommand

        For i As Integer = 0 To lpIdArrayList.Count - 1
          lstrSQL = "Update " & TABLE_BATCH_ITEM_NAME & " SET DestDocID = null, ProcessedStatus = '" & ProcessedStatus.NotProcessed.ToString & "', ProcessedMessage=null, ProcessStartTime = null,ProcessFinishTime = null, TotalProcessingTime=null,ProcessedBy=null WHERE Id = " & lpIdArrayList(i)
          lobjCommand = New OleDbCommand(lstrSQL, lobjConnection)
          Helper.HandleConnection(lobjConnection)
          lintRowsAffected = lobjCommand.ExecuteNonQuery()
        Next

        'Dim lobjAdapter As New SqlDataAdapter()
        'lobjAdapter.UpdateCommand = New SqlCommand( _
        '  "Update " & TABLE_BATCH_ITEM_NAME & " SET DestDocID = null, ProcessedStatus = '" & ProcessedStatus.NotProcessed.ToString & "', ProcessedMessage=null, ProcessStartTime = null,ProcessFinishTime = null, TotalProcessingTime=null,ProcessedBy=null WHERE Id = @Id", lobjConnection)
        'lobjAdapter.UpdateCommand.Parameters.Add("@Id", SqlDbType.Int, 4, "Id")
        'lobjAdapter.UpdateCommand.UpdatedRowSource = UpdateRowSource.None

        '' Set the batch size.
        'lobjAdapter.UpdateBatchSize = 500 '0 = Let sql decide the max batchsize it will take

        '' Execute the Updates.
        'lobjAdapter.Update(lobjDataTable)

      End Using

    Catch ex As Exception
      Throw ex
    End Try

  End Sub

  ''' <summary>
  ''' Reset all the items in a batch from 'Failed' to 'Not Processed'
  ''' </summary>
  ''' <param name="lpBatch"></param>
  ''' <remarks></remarks>
  Public Sub ResetFailedItemsToNotProcessedDB(ByVal lpBatch As Batch)

    Try

      Using lobjConnection As New OleDbConnection(Me.ItemsLocation.Location)

        Dim lstrSQL = String.Empty
        Dim lintRowsAffected As Integer = 0
        Dim lobjCommand As OleDbCommand

        lstrSQL = "Update " & TABLE_BATCH_ITEM_NAME & " SET DestDocID = null, ProcessedStatus = '" & ProcessedStatus.NotProcessed.ToString & "', ProcessedMessage=null, ProcessStartTime = null,ProcessFinishTime = null, TotalProcessingTime=null,ProcessedBy=null WHERE BatchId = '" & lpBatch.Id & "' AND ProcessedStatus = '" & ProcessedStatus.Failed.ToString & "'"
        lobjCommand = New OleDbCommand(lstrSQL, lobjConnection)
        Helper.HandleConnection(lobjConnection)
        lintRowsAffected = lobjCommand.ExecuteNonQuery()

      End Using

    Catch ex As Exception
      Throw ex
    End Try

  End Sub

#End Region

#Region "Repository Access Methods"

  Public Overloads Overrides Function DeleteRepository(lpRepository As Core.Repository) As Boolean

    Try
      Return False

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  Public Overloads Overrides Function DeleteRepository(lpRepositoryName As String) As Boolean

    Try
      Return False

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  Public Overrides Function GetRepositoryByName(lpRepositoryName As String) As Core.Repository

    Try
      Return Nothing

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  Public Overrides Function GetRepositoryByConnectionString(lpConnectionString As String) As Core.Repository
    Try
      Return Nothing

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetRepositories(lpJob As Job) As Core.Repositories

    Try
      Return Nothing

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  Public Overrides Function GetRepositories(lpProject As Project) As Core.Repositories

    Try
      Return Nothing

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  Public Overrides Function SaveRepository(lpRepository As Core.Repository, _
                      ByVal lpJob As Job, ByVal lpScope As ExportScope) As Boolean

    Try
      Return False

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  Public Overrides Sub UpdateRepositories(lpJob As Job)
    Try

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overrides Sub UpdateRepositories(lpProject As Project)
    Try

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub
  
  Public Overrides Function GetProcessResultSummary(lpBatch As Batch) As IProcessResultSummary
    Try
      Throw New NotImplementedException
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetProcessResultSummary(lpJob As Job) As IProcessResultSummary
    Try
      Throw New NotImplementedException
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Sub SaveProcessResultSummary(lpBatch As Batch)
    Try
      Throw New NotImplementedException
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overrides Sub SaveProcessResultSummary(lpJob As Job)
    Try
      Throw New NotImplementedException
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

#Region "IDisposible"

  Protected Overrides Sub Dispose(ByVal disposing As Boolean)
    MyBase.Dispose(disposing)

    If (mobjConnection IsNot Nothing) Then

      If (mobjConnection.State <> ConnectionState.Closed Or mobjConnection.State <> ConnectionState.Broken) Then
        mobjConnection.Close()
      End If

    End If

  End Sub

#End Region

End Class
