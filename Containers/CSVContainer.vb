'---------------------------------------------------------------------------------
' <copyright company="ECMG">
'     Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'     Copying or reuse without permission is strictly forbidden.
' </copyright>
'---------------------------------------------------------------------------------

#Region "Imports"

Imports Documents.Utilities
Imports System.Reflection
Imports Operations.OperationEnumerations
Imports Operations
Imports System.Data
Imports Documents.Core

#End Region

Public Class CSVContainer
  Inherits Container

#Region "Constructors"

  Public Sub New()
  End Sub

  Public Sub New(ByVal lpItemsLocation As ItemsLocation)
    Me.ItemsLocation = lpItemsLocation
  End Sub

#End Region

#Region "Public Methods"

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

  Public Overrides Function GetItemCount(ByVal lpBatch As Batch) As Integer
    Return 0
  End Function

  Public Overrides Function GetItemCount(ByVal lpJob As Job) As Long
    Return 0
  End Function

  Public Overrides Function GetItemCount(ByVal lpProject As Project) As Long
    Return 0
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

  Public Overrides Function GetWorkSummaryCounts(ByVal lpProject As Project) As WorkSummary

    Try
      Return Nothing

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
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

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      Throw
    End Try
  End Sub

  Public Overrides Function GetUpdateTable(lpJob As Job,
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

    Catch ex As Exception
      Throw
    End Try
  End Sub

  Public Overrides Function IsAvailableForProcessing(ByVal lpBatchId As String) As Boolean

    Try
      Return False

    Catch ex As Exception
      Throw
    End Try
  End Function

  Public Overrides Function GetItemsById(ByVal lpBatch As Batch,
                                         ByVal lpIdArrayList As ArrayList,
                                         ByVal lpIncludeProcessResults As Boolean) As BatchItems

    Try
      Return Nothing

    Catch ex As Exception
      Throw
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
    Throw New NotImplementedException()
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
    Throw New NotImplementedException()
  End Sub

  Friend Overrides Sub ResetItemsToNotProcessed(lpBatch As Batch, lpCurrentStatus As ProcessedStatus)
    Throw New NotImplementedException()
  End Sub

  Friend Overrides Sub ResetItemsToNotProcessed(lpJob As Job, lpCurrentStatus As ProcessedStatus)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub LockBatch(ByVal lpBatchId As String,
                                 ByVal lpLockedBy As String)

    Try

    Catch ex As Exception
      Throw
    End Try
  End Sub

  Public Overrides Sub UnLockBatch(ByVal lpBatchId As String,
                                   ByVal lpUnLockedBy As String)

    Try

    Catch ex As Exception
      Throw
    End Try
  End Sub

  Public Overrides Sub DeleteAll()

    Try

    Catch ex As Exception
      Throw
    End Try
  End Sub

  Public Overrides Sub DeleteBatches(ByVal lpBatches As Batches)

    Try

    Catch ex As Exception
      Throw
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

    Catch ex As Exception
      Throw
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

    Catch ex As Exception
      Throw
    End Try
  End Sub

  Public Overrides Function AddItem(ByVal lpBatchItem As BatchItem) As Boolean

    Try

      Dim lblnReturn As Boolean = False

      Return lblnReturn

    Catch ex As Exception
      Throw
    End Try
  End Function

  Friend Overrides Sub ClearWorkSummary(lpWorkParent As Object)
    Try
      Throw New NotImplementedException
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      '   Re-throw the exception to the caller
      Throw
    End Try
  End Sub

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
      Throw New NotImplementedException
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      '   Re-throw the exception to the caller
      Throw
    End Try
  End Sub


  Public Overloads Overrides Sub SaveJob(ByVal lpProjectId As String,
                                         lpJobConfiguration As Configuration.JobConfiguration)
    Try
      Throw New NotImplementedException
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      '   Re-throw the exception to the caller
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

  Public Overrides Sub SaveProject(ByVal lpProject As Project)
    Try
      Throw New NotImplementedException
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      '   Re-throw the exception to the caller
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
      Return Nothing

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
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

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  ''' <summary>
  ''' Called when a batch item has been migrated,exported,etc
  ''' </summary>
  ''' <param name="lpProcessedItemEventArgs"></param>
  ''' <remarks></remarks>
  Public Overrides Sub EndProcessItem(ByVal lpProcessedItemEventArgs As BatchItemProcessEventArgs)

    Try

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Overrides Function GetItems(ByVal lpBatch As Batch,
                                     ByVal lpStart As Integer,
                                     ByVal lpItemsToGet As Integer,
                                     ByVal lpSortColumn As String,
                                     ByVal lpAscending As Boolean,
                                     ByVal lpProcessedStatusFilter As String) As BatchItems

    Dim lobjBatchItems As BatchItems = Nothing

    Try

      Return lobjBatchItems

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetAllItems(ByVal lpBatch As Batch,
                                        ByVal lpProcessedStatusFilter As String) As BatchItems

    Dim lobjBatchItems As BatchItems = Nothing

    Try

      Return lobjBatchItems

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetItemsToDataTable(ByVal lpBatch As Batch,
                                                ByVal lpStart As Integer,
                                                ByVal lpItemsToGet As Integer,
                                                ByVal lpSortColumn As String,
                                                ByVal lpAscending As Boolean,
                                                ByVal lpProcessedStatusFilter As String) As System.Data.DataTable

    Dim lobjDataTable As DataTable = Nothing

    Try

      Return lobjDataTable

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

  Public Overrides Function GetAllItemsToDataTable(ByVal lpObject As Object,
                                                   ByVal lpProcessedStatusFilter As String) As DataTable

    Dim lobjDataTable As DataTable = Nothing

    Try

      Return lobjDataTable

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

    Dim lobjBatchItems As BatchItems = Nothing

    Try

      Return lobjBatchItems

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetWorkSummaryCounts(ByVal lpBatch As Batch) As WorkSummary

    Try
      Return Nothing

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetWorkSummaryCounts(ByVal lpJob As Job) As WorkSummary

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

  Public Overrides Function GetFailureItemIdsByProcessedMessage(lpJob As Job, lpProcessedMessage As String) _
    As IList(Of String)

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

  'Public Overrides Function GetItem(ByVal lpId As String, ByVal lpBatch As Batch) As BatchItem

  '  Dim lobjBatchItem As BatchItem = Nothing

  '  Try

  '    Return lobjBatchItem
  '  Catch ex As Exception
  '    ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
  '    ' Re-throw the exception to the caller
  '    Throw
  '  End Try

  'End Function

  'Public Overrides Function AddBatchItems(ByVal lpBatch As Batch) As Boolean
  '  Return False
  'End Function

#Region "Repository Access Methods"

  Public Overloads Overrides Function DeleteRepository(lpRepository As Repository) As Boolean

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

  Public Overrides Function GetRepositoryByName(lpRepositoryName As String) As Repository

    Try
      Return Nothing

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetRepositoryByConnectionString(lpConnectionString As String) As Repository
    Try
      Return Nothing

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetRepositories(lpJob As Job) As Repositories

    Try
      Return Nothing

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function GetRepositories(lpProject As Project) As Repositories

    Try
      Return Nothing

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function SaveRepository(lpRepository As Repository,
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

#End Region
End Class
