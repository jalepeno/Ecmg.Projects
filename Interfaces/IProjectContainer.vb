' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------
'  Document    :  IProjectContainer.vb
'  Description :  Interface for project containers
'  Created     :  10/1/2011 4:23:05 PM
'  <copyright company="ECMG">
'      Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'      Copying or reuse without permission is strictly forbidden.
'  </copyright>
' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------

#Region "Imports"

Imports System.Data
Imports System.IO
Imports Documents.Core
Imports Operations
Imports Operations.OperationEnumerations
Imports Projects.Configuration

#End Region

#Region "IProjectContainer Interface"

Public Interface IProjectContainer
  Inherits IDisposable

  'Function GetItem(ByVal lpId As String, ByVal lpBatch As Batch) As BatchItem
  'Function AddBatchItems(ByVal lpBatch As Batch) As Boolean
  Function AddItem(ByVal lpBatchItem As BatchItem) As Boolean

  Sub BeginProcessItem(ByVal e As BatchItemProcessEventArgs)

  Sub ClearWorkSummary(lpWorkParent As Object)

  Sub CommitBatchItems()

  Sub CreateListBatches(ByVal lpArgs As IDBLookupSourceEventArgs)

  Sub DeleteAll()

  Sub DeleteBatches(ByVal lpBatches As Batches)

  Sub DeleteJob(ByVal lpJob As Job)

  Function GetJobIdentifiers() As IJobIdentifiers

  Function GetJobById(lpJobId As String) As Job

  Function GetJobConfigurationByName(lpJobName As String) As String

  Sub SaveJobConfiguration(lpJobConfiguration As JobConfiguration)

  Sub DeleteOrphanBatches(ByVal lpProject As Project)

  Sub DeleteProject(ByVal lpProject As Project)

  Function DeleteRepository(ByVal lpRepository As Repository) As Boolean

  Function DeleteRepository(ByVal lpRepositoryName As String) As Boolean

  Sub EndProcessItem(ByVal e As BatchItemProcessEventArgs)

  Function GetAllItems(ByVal lpBatch As Batch, ByVal lpProcessedStatusFilter As String) As BatchItems

  Function GetAllItemsToDataTable(ByVal lpObject As Object, ByVal lpProcessedStatusFilter As String) As DataTable

  Function GetAllProcessedByNodeNames(ByVal lpJob As Job) As IList(Of String)

  Function GetFilteredItemsToDataTable(lpItemFilter As ItemFilter) As DataTable

  'Function GetNextUnprocessedItem(ByVal lpBatch As Batch) As BatchItem
  Function GetAllUnprocessedItems(ByVal lpBatch As Batch) As BatchItems

  Function GetBatchLockCount(lpJobName As String) As Integer

  Function GetAllBatchLocks() As IBatchLocks

  Function GetBatches(ByVal lpJob As Job) As Batches

  Function GetBatchIds(lpJobId As String) As IList(Of String)

  Function GetBatchLocks(lpJobName As String) As IBatchLocks

  Function GetProcessResultSummary(ByVal lpBatch As Batch) As IProcessResultSummary

  Function GetProcessResultSummary(ByVal lpJob As Job) As IProcessResultSummary

  Sub SaveProcessResultSummary(ByVal lpBatch As Batch)

  Sub SaveProcessResultSummary(ByVal lpJob As Job)

  Function GetWorkSummaryCounts(ByVal lpBatch As Batch) As WorkSummary

  Function GetWorkSummaryCounts(ByVal lpJob As Job) As WorkSummary

  Function GetWorkSummaryCounts(ByVal lpProject As Project) As WorkSummary

  Function GetCachedWorkSummaryCounts(lpWorkParent As Object) As IWorkSummary

  Function GetFailureItemIdsByProcessedMessage(ByVal lpJob As Job, ByVal lpProcessedMessage As String) As IList(Of String)

  Function ResetFailedItemsByProcessedMessage(ByVal lpJob As Job, ByVal lpProcessedMessage As String) As Integer

  Function GetFailureSummaries(ByVal lpBatch As Batch) As FailureSummaries

  Function GetFailureSummaries(ByVal lpJob As Job) As FailureSummaries

  Function GetFailureSummaries(ByVal lpProject As Project) As FailureSummaries

  Function GetProcessedItemsCount(ByVal lpProject As Project) As Long

  Function GetProcessedItemsCount(ByVal lpJob As Job) As Long

  Sub SetProcessedItemsCount(ByVal lpProject As Project, ByVal lpProcessedCount As Long)

  Sub SetProcessedItemsCount(ByVal lpJob As Job, ByVal lpProcessedCount As Long)

  Function GetJobViewName(lpJobName As String) As String

  Function GetItems(ByVal lpBatch As Batch, ByVal lpStart As Integer, ByVal lpItemsToGet As Integer, ByVal lpSortColumn As String, ByVal lpAscending As Boolean, ByVal lpProcessedStatusFilter As String) As BatchItems

  Function GetRepositories(ByVal lpProject As Project) As Repositories

  Function GetRepositories(ByVal lpJob As Job) As Repositories

  Function GetSourceRepository(lpJob As Job) As Repository

  Function GetDestinationRepository(lpJob As Job) As Repository

  'Function GetItemCount(ByVal lpBatch As Batch, ByVal lpProcessedStatusFilter As String) As Integer

  'Function GetItemCount(ByVal lpJob As Job, ByVal lpProcessedStatusFilter As String) As Long

  Function GetItemCount(ByVal lpBatch As Batch) As Integer

  Function GetItemCount(ByVal lpJob As Job) As Long

  Function GetItemCount(ByVal lpProject As Project) As Long

  Function GetItemsById(ByVal lpJob As Job, ByVal lpIdTable As DataTable) As BatchItems

  Function GetItemsById(ByVal lpBatch As Batch, ByVal lpIdArrayList As ArrayList, ByVal lpIncludeProcessResults As Boolean) As BatchItems

  Function GetItemById(ByVal lpJobName As String, ByVal lpId As String, ByVal lpScope As OperationScope) As IBatchItem

  Function GetBatchItemById(ByVal lpId As String) As IBatchItem

  Function GetItemsToDataTable(ByVal lpBatch As Batch, ByVal lpStart As Integer, ByVal lpItemsToGet As Integer, ByVal lpSortColumn As String, ByVal lpAscending As Boolean, ByVal lpProcessedStatusFilter As String) As DataTable

  Function GetProjectAvgProcessingTime(lpProject As Project) As Single

  Function ProjectExists(lpProjectId As String) As Boolean

  Function GetOrphanBatchCount() As Integer

  Function GetOrphanBatchItemCount() As Integer

  Function GetProjectInfo() As IProjectInfo

  Function GetJobInfo(lpJobId As String) As IJobInfo

  Function GetJobRelationships(lpJobId As String) As JobRelationships

  Function GetProjectDbFileInfo() As DbFilesInfo

  Function GetDetailedJobInfo(lpJobId As String) As IDetailedJobInfo

  Function GetBatchInfoCollection(lpJob As IJobInfo) As IBatchInfoCollection

  Function GetJobInfoCollection() As IJobInfoCollection

  Function GetProjectSummaryCounts(ByVal lpProject As Project) As WorkSummaries

  Function GetProcessResults(ByVal lpBatch As Batch) As IProcessResults

  Function GetProcessResults(ByVal lpJob As Job) As IProcessResults

  Function GetRepositoryByConnectionString(ByVal lpConnectionString As String) As Repository

  Function GetRepositoryByName(ByVal lpRepositoryName As String) As Repository

  Function GetUpdateTable(ByVal lpJob As Job, ByVal lpUpdateItems As DataTable) As DataTable

  Function IsAvailableForProcessing(ByVal lpBatchId As String) As Boolean

  Sub LockBatch(ByVal lpBatchId As String, ByVal lpLockedBy As String)

  Function OpenProject(ByRef lpErrorMessage As String) As Project

  Sub InitializeCachedRepositories(lpProject As Project)

  Sub RefreshTransform(ByVal lpJob As Job)

  Sub ResetFailedItemsToNotProcessed(ByVal lpBatch As Batch)

  Sub ResetItemsToNotProcessed(lpBatch As Batch, lpCurrentStatus As ProcessedStatus)

  Sub ResetItemsToNotProcessed(lpJob As Job, lpCurrentStatus As ProcessedStatus)

  Sub ResetItemsToNotProcessed(ByVal lpIdArrayList As ArrayList)

  Sub SaveBatch(ByVal lpBatch As Batch)

  Sub SaveBatches(ByVal lpJob As Job)

  Sub SaveJob(ByRef lpJob As Job)

  Sub SaveJob(ByVal lpProjectId As String, ByVal lpJobConfiguration As JobConfiguration)

  Sub SaveJobRelationship(ByVal lpJobRelationship As JobRelationship)

  Sub SaveProject(ByVal lpProject As Project)

  Function SaveRepository(ByVal lpRepository As Repository,
                          ByVal lpJob As Job, ByVal lpScope As ExportScope) As Boolean

  Sub UnLockBatch(ByVal lpBatchId As String, ByVal lpUnLockedBy As String)

  Sub UpdateRepositories(ByVal lpJob As Job)

  Sub UpdateRepositories(lpProject As Project)

  Sub UpdateJobSource(ByVal lpJob As Job)

  Sub UpdateProcess(ByVal lpJob As Job)

  Sub UpdateTransformations(ByVal lpJob As Job)

  Sub UpdateTransformations(ByVal lpBatch As Batch)

  Function FileExists(ByVal lpJob As Job, ByVal lpFileName As String) As Boolean

  Function GetFileList(ByVal lpJob As Job) As IList(Of String)

  Function RetrieveFile(ByVal lpJob As Job, ByVal lpFileName As String) As Stream

  Function RetrieveFile(ByVal lpFileId As String, ByVal lpFileName As String) As Stream

  Sub StoreFile(ByVal lpJob As Job, ByVal lpFilePath As String)

  Sub StoreFile(ByVal lpJob As Job, ByVal lpFileName As String, ByVal lpFileData As Stream)

End Interface

#End Region