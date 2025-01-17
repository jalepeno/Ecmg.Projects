'---------------------------------------------------------------------------------
' <copyright company="ECMG">
'     Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'     Copying or reuse without permission is strictly forbidden.
' </copyright>
'---------------------------------------------------------------------------------

#Region "Imports"


Imports System.Data
Imports System.Data.Common
Imports System.Reflection
Imports System.Runtime.Serialization
Imports Documents
Imports Documents.Core
Imports Documents.Exceptions
Imports Documents.Providers
Imports Documents.SerializationUtilities
Imports Documents.Transformations
Imports Documents.Utilities
Imports Operations
Imports Projects.Configuration

#End Region

<DataContract()>
Public MustInherit Class Container
  Implements IProjectContainer
  'Implements ILoggable
  Implements IDisposable

#Region "Class Constants"

  Protected Const PROJECT_NAME_COLUMN As String = "ProjectName"
  Protected Const PROJECT_DESCRIPTION_COLUMN As String = "Description"
  Protected Const PROJECT_BATCH_SIZE_COLUMN As String = "BatchSize"
  Protected Const PROJECT_ID_COLUMN As String = "ProjectId"
  Protected Const PROJECT_LOCATION_COLUMN As String = "ItemsLocation"
  Protected Const PROJECT_CREATE_DATE_COLUMN As String = "CreateDate"
  Protected Const PROJECT_ITEMS_PROCESSED_COLUMN As String = "ItemsProcessed"
  Protected Const PROJECT_WORK_SUMMARY_COLUMN As String = "WorkSummary"

  Protected Const CATALOG_PROJECT_LOCATION_COLUMN As String = "Location"

  Protected Const JOB_ID_COLUMN As String = "JobId"
  Protected Const JOB_NAME_COLUMN As String = "JobName"

  Protected Const JOB_RELATIONSHIP_ID_COLUMN As String = "JobRelationshipId"
  Protected Const JOB_RELATIONSHIP_NAME_COLUMN As String = "JobRelationshipName"
  Protected Const JOB_RELATIONSHIP_DESCRIPTION_COLUMN As String = "JobRelationshipDescription"

#End Region

#Region "Class Variables"

  Private disposedValue As Boolean = False    ' To detect redundant calls

  Private mobjItemsLocation As ItemsLocation
  Private ReadOnly mstrId As String = Guid.NewGuid.ToString

#End Region

#Region "Public MustOverride Methods"

  'Public MustOverride Function GetItem(ByVal lpId As String, ByVal lpBatch As Batch) As BatchItem Implements IBatchContainer.GetItem
  'Public MustOverride Function AddBatchItems(ByVal lpBatch As Batch) As Boolean Implements IBatchContainer.AddBatchItems
  Public MustOverride Function AddItem(ByVal lpBatchItem As BatchItem) As Boolean _
    Implements IProjectContainer.AddItem

  Public MustOverride Sub BeginProcessItem(ByVal e As BatchItemProcessEventArgs) _
    Implements IProjectContainer.BeginProcessItem

  Friend MustOverride Sub ClearWorkSummary(lpWorkParent As Object) _
    Implements IProjectContainer.ClearWorkSummary

  Public MustOverride Sub CommitBatchItems() _
    Implements IProjectContainer.CommitBatchItems

  Public MustOverride Sub CreateListBatches(lpArgs As IDBLookupSourceEventArgs) _
    Implements IProjectContainer.CreateListBatches

  Public MustOverride Sub DeleteAll() _
    Implements IProjectContainer.DeleteAll

  Public MustOverride Sub DeleteBatches(ByVal lpBatches As Batches) _
    Implements IProjectContainer.DeleteBatches

  Public MustOverride Sub DeleteOrphanBatches(lpProject As Project) _
    Implements IProjectContainer.DeleteOrphanBatches

  Public MustOverride Sub DeleteJob(ByVal lpJob As Job) _
    Implements IProjectContainer.DeleteJob

  Public MustOverride Function GetJobById(lpJobId As String) As Job Implements IProjectContainer.GetJobById

  Public MustOverride Function GetJobIdentifiers() As IJobIdentifiers Implements IProjectContainer.GetJobIdentifiers

  Public MustOverride Function GetJobConfigurationByName(lpJobName As String) As String _
    Implements IProjectContainer.GetJobConfigurationByName

  Public MustOverride Sub SaveJobConfiguration(lpJobConfiguration As JobConfiguration) _
    Implements IProjectContainer.SaveJobConfiguration

  Public MustOverride Sub DeleteProject(ByVal lpProject As Project) _
    Implements IProjectContainer.DeleteProject

  Public MustOverride Sub EndProcessItem(ByVal e As BatchItemProcessEventArgs) _
    Implements IProjectContainer.EndProcessItem

  Public MustOverride Function GetAllItems(ByVal lpBatch As Batch,
                                           ByVal lpProcessedStatusFilter As String) As BatchItems _
    Implements IProjectContainer.GetAllItems

  Public MustOverride Function GetAllItemsToDataTable(ByVal lpObject As Object,
                                                      ByVal lpProcessedStatusFilter As String) As DataTable _
    Implements IProjectContainer.GetAllItemsToDataTable


  Public MustOverride Function GetAllProcessedByNodeNames(lpJob As Job) As IList(Of String) _
    Implements IProjectContainer.GetAllProcessedByNodeNames

  'Public MustOverride Function GetNextUnprocessedItem(ByVal lpBatch As Batch) As BatchItem Implements IBatchContainer.GetNextUnprocessedItem
  Public MustOverride Function GetAllUnprocessedItems(ByVal lpBatch As Batch) As BatchItems _
    Implements IProjectContainer.GetAllUnprocessedItems

  Public MustOverride Function GetItemCount(ByVal lpBatch As Batch) As Integer Implements IProjectContainer.GetItemCount

  Public MustOverride Function GetItemCount(ByVal lpJob As Job) As Long Implements IProjectContainer.GetItemCount

  Public MustOverride Function GetItemCount(ByVal lpProject As Project) As Long _
    Implements IProjectContainer.GetItemCount

  Public MustOverride Function GetItems(ByVal lpBatch As Batch,
                                        ByVal lpStart As Integer,
                                        ByVal lpItemsToGet As Integer,
                                        ByVal lpSortColumn As String,
                                        ByVal lpAscending As Boolean,
                                        ByVal lpProcessedStatusFilter As String) As BatchItems _
    Implements IProjectContainer.GetItems

  Public MustOverride Function GetItemsById(ByVal lpBatch As Batch,
                                            ByVal lpIdArrayList As ArrayList,
                                            ByVal lpIncludeProcessResults As Boolean) As BatchItems _
    Implements IProjectContainer.GetItemsById

  Public MustOverride Function GetItemsById(ByVal lpJob As Job, ByVal lpIdTable As DataTable) As BatchItems _
    Implements IProjectContainer.GetItemsById

  Public MustOverride Function GetBatchItemById(lpId As String) As IBatchItem _
    Implements IProjectContainer.GetBatchItemById

  Public MustOverride Function GetItemById(ByVal lpJobName As String,
                                           ByVal lpId As String,
                                           ByVal lpScope As OperationScope) As IBatchItem _
    Implements IProjectContainer.GetItemById

  Public MustOverride Function GetItemsToDataTable(ByVal lpBatch As Batch,
                                                   ByVal lpStart As Integer,
                                                   ByVal lpItemsToGet As Integer,
                                                   ByVal lpSortColumn As String,
                                                   ByVal lpAscending As Boolean,
                                                   ByVal lpProcessedStatusFilter As String) As DataTable _
    Implements IProjectContainer.GetItemsToDataTable


  Public MustOverride Function GetFilteredItemsToDataTable(lpItemFilter As ItemFilter) As DataTable _
    Implements IProjectContainer.GetFilteredItemsToDataTable

  Public MustOverride Function GetProjectAvgProcessingTime(lpProject As Project) As Single _
    Implements IProjectContainer.GetProjectAvgProcessingTime

  Public MustOverride Function ProjectExists(lpProjectId As String) As Boolean _
    Implements IProjectContainer.ProjectExists

  Public MustOverride Function GetOrphanBatchCount() As Integer Implements IProjectContainer.GetOrphanBatchCount

  Public MustOverride Function GetOrphanBatchItemCount() As Integer Implements IProjectContainer.GetOrphanBatchItemCount

  Public MustOverride Function GetProjectInfo() As IProjectInfo Implements IProjectContainer.GetProjectInfo

  Public MustOverride Function GetBatchInfoCollection(lpJob As IJobInfo) As IBatchInfoCollection _
    Implements IProjectContainer.GetBatchInfoCollection

  Public MustOverride Function GetJobInfoCollection() As IJobInfoCollection _
    Implements IProjectContainer.GetJobInfoCollection

  Public MustOverride Function GetJobInfo(lpJobId As String) As IJobInfo Implements IProjectContainer.GetJobInfo

  Public MustOverride Function GetJobRelationships(lpJobId As String) As JobRelationships _
    Implements IProjectContainer.GetJobRelationships

  Public MustOverride Function GetDetailedJobInfo(lpJobId As String) As IDetailedJobInfo _
    Implements IProjectContainer.GetDetailedJobInfo

  Public MustOverride Function GetProjectSummaryCounts(ByVal lpProject As Project) As WorkSummaries _
    Implements IProjectContainer.GetProjectSummaryCounts

  Public MustOverride Function GetCachedWorkSummaryCounts(lpWorkParent As Object) As IWorkSummary _
    Implements IProjectContainer.GetCachedWorkSummaryCounts

  Public MustOverride Function GetUpdateTable(ByVal lpJob As Job,
                                              ByVal lpUpdateItems As DataTable) As DataTable _
    Implements IProjectContainer.GetUpdateTable

  Public MustOverride Function GetBatchLockCount(lpJobName As String) As Integer _
    Implements IProjectContainer.GetBatchLockCount

  Public MustOverride Function GetAllBatchLocks() As IBatchLocks Implements IProjectContainer.GetAllBatchLocks

  Public MustOverride Function GetBatches(lpJob As Job) As Batches Implements IProjectContainer.GetBatches

  Public MustOverride Function GetBatchIds(lpJobId As String) As IList(Of String) _
    Implements IProjectContainer.GetBatchIds

  Public MustOverride Function GetBatchLocks(lpJobName As String) As IBatchLocks _
    Implements IProjectContainer.GetBatchLocks

  Public MustOverride Function GetFailureSummaries(ByVal lpProject As Project) As FailureSummaries _
    Implements IProjectContainer.GetFailureSummaries

  Public MustOverride Function GetFailureSummaries(ByVal lpJob As Job) As FailureSummaries _
    Implements IProjectContainer.GetFailureSummaries

  Public MustOverride Function GetFailureItemIdsByProcessedMessage(lpJob As Job, lpProcessedMessage As String) _
    As IList(Of String) _
    Implements IProjectContainer.GetFailureItemIdsByProcessedMessage

  Public MustOverride Function GetFailureSummaries(ByVal lpBatch As Batch) As FailureSummaries _
    Implements IProjectContainer.GetFailureSummaries

  Public MustOverride Function GetProcessedItemsCount(ByVal lpProject As Project) As Long _
    Implements IProjectContainer.GetProcessedItemsCount

  Public MustOverride Function GetProcessedItemsCount(ByVal lpJob As Job) As Long _
    Implements IProjectContainer.GetProcessedItemsCount

  Friend MustOverride Sub SetProcessedItemsCount(ByVal lpProject As Project, ByVal lpProcessedCount As Long) _
    Implements IProjectContainer.SetProcessedItemsCount

  Friend MustOverride Sub SetProcessedItemsCount(ByVal lpJob As Job, ByVal lpProcessedCount As Long) _
    Implements IProjectContainer.SetProcessedItemsCount

  Friend MustOverride Function GetProcessResults(lpBatch As Batch) As IProcessResults _
    Implements IProjectContainer.GetProcessResults

  Friend MustOverride Function GetProcessResults(lpJob As Job) As IProcessResults _
    Implements IProjectContainer.GetProcessResults

  Public MustOverride Function GetWorkSummaryCounts(ByVal lpBatch As Batch) As WorkSummary _
    Implements IProjectContainer.GetWorkSummaryCounts

  Public MustOverride Function GetWorkSummaryCounts(ByVal lpJob As Job) As WorkSummary _
    Implements IProjectContainer.GetWorkSummaryCounts

  Public MustOverride Function GetWorkSummaryCounts(ByVal lpProject As Project) As WorkSummary _
    Implements IProjectContainer.GetWorkSummaryCounts

  Public MustOverride Function IsAvailableForProcessing(ByVal lpBatchId As String) As Boolean _
    Implements IProjectContainer.IsAvailableForProcessing

  Public MustOverride Sub LockBatch(ByVal lpBatchId As String,
                                    ByVal lpLockedBy As String) _
    Implements IProjectContainer.LockBatch

  Public MustOverride Sub InitializeCachedRepositories(lpProject As Project) _
    Implements IProjectContainer.InitializeCachedRepositories

  Public MustOverride Function OpenProject(ByRef lpErrorMessage As String) As Project _
    Implements IProjectContainer.OpenProject

  Public MustOverride Sub RefreshTransform(ByVal lpJob As Job) _
    Implements IProjectContainer.RefreshTransform

  Public MustOverride Sub ResetFailedItemsToNotProcessed(ByVal lpBatch As Batch) _
    Implements IProjectContainer.ResetFailedItemsToNotProcessed

  Public MustOverride Function ResetFailedItemsByProcessedMessage(lpJob As Job, lpProcessedMessage As String) As Integer _
    Implements IProjectContainer.ResetFailedItemsByProcessedMessage

  Friend MustOverride Sub ResetItemsToNotProcessed(lpBatch As Batch, lpCurrentStatus As ProcessedStatus) _
    Implements IProjectContainer.ResetItemsToNotProcessed

  Friend MustOverride Sub ResetItemsToNotProcessed(lpJob As Job, lpCurrentStatus As ProcessedStatus) _
    Implements IProjectContainer.ResetItemsToNotProcessed

  Public MustOverride Sub ResetItemsToNotProcessed(ByVal lpIdArrayList As ArrayList) _
    Implements IProjectContainer.ResetItemsToNotProcessed

  Public MustOverride Sub SaveBatch(lpBatch As Batch) _
    Implements IProjectContainer.SaveBatch

  Public MustOverride Sub SaveBatches(ByVal lpJob As Job) _
    Implements IProjectContainer.SaveBatches

  Public MustOverride Sub SaveJob(ByRef lpJob As Job) _
    Implements IProjectContainer.SaveJob

  Public MustOverride Sub SaveJob(ByVal lpProjectId As String, ByVal lpJobConfiguration As JobConfiguration) _
    Implements IProjectContainer.SaveJob

  Public MustOverride Sub SaveJobRelationship(ByVal lpJobRelationship As JobRelationship) _
    Implements IProjectContainer.SaveJobRelationship

  Public Overridable Sub SaveProject(ByVal lpProject As Project) _
    Implements IProjectContainer.SaveProject

    Try
      lpProject.SyncItemsLocation()

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
    End Try
  End Sub

  Public Overridable Function GetJobViewName(lpJobName As String) As String Implements IProjectContainer.GetJobViewName
    Try
      Return GenerateJobViewName(lpJobName)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public MustOverride Function FileExists(ByVal lpJob As Job, ByVal lpFileName As String) As Boolean _
    Implements IProjectContainer.FileExists

  Public MustOverride Function GetFileList(ByVal lpJob As Job) As IList(Of String) _
    Implements IProjectContainer.GetFileList

  Public MustOverride Function RetrieveFile(ByVal lpJob As Job, ByVal lpFileName As String) As IO.Stream _
    Implements IProjectContainer.RetrieveFile

  Public MustOverride Function RetrieveFile(ByVal lpFileId As String, ByVal lpFileName As String) As IO.Stream _
    Implements IProjectContainer.RetrieveFile

  Public MustOverride Sub StoreFile(ByVal lpJob As Job, ByVal lpFilePath As String) _
    Implements IProjectContainer.StoreFile

  Public MustOverride Sub StoreFile(ByVal lpJob As Job, ByVal lpFileName As String, ByVal lpFileData As IO.Stream) _
    Implements IProjectContainer.StoreFile

  Public MustOverride Sub UnLockBatch(ByVal lpBatchId As String,
                                      ByVal lpUnLockedBy As String) _
    Implements IProjectContainer.UnLockBatch

  Public MustOverride Sub UpdateJobSource(ByVal lpJob As Job) _
    Implements IProjectContainer.UpdateJobSource

  Public MustOverride Sub UpdateProcess(ByVal lpJob As Job) _
    Implements IProjectContainer.UpdateProcess

  Public MustOverride Sub UpdateTransformations(ByVal lpJob As Job) _
    Implements IProjectContainer.UpdateTransformations

  Public MustOverride Sub UpdateTransformations(ByVal lpBatch As Batch) _
    Implements IProjectContainer.UpdateTransformations

#Region "Repository Access Methods"

  Public MustOverride Function DeleteRepository(ByVal lpRepository As Repository) As Boolean _
    Implements IProjectContainer.DeleteRepository

  Public MustOverride Function DeleteRepository(ByVal lpRepositoryName As String) As Boolean _
    Implements IProjectContainer.DeleteRepository

  Public MustOverride Function GetRepositoryByConnectionString(ByVal lpConnectionString As String) As Repository _
    Implements IProjectContainer.GetRepositoryByConnectionString

  Public MustOverride Function GetRepositoryByName(ByVal lpRepositoryName As String) As Repository _
    Implements IProjectContainer.GetRepositoryByName

  Public Function GetSourceRepository(lpJob As Job) As Repository _
    Implements IProjectContainer.GetSourceRepository
    Try
      If Not String.IsNullOrEmpty(lpJob.SourceConnectionString) Then
        Return GetRepositoryByConnectionString(lpJob.SourceConnectionString)
      Else
        Return Nothing
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Function GetDestinationRepository(lpJob As Job) As Repository _
    Implements IProjectContainer.GetDestinationRepository
    Try
      If Not String.IsNullOrEmpty(lpJob.DestinationConnectionString) Then
        Return GetRepositoryByConnectionString(lpJob.DestinationConnectionString)
      Else
        Return Nothing
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public MustOverride Function GetRepositories(ByVal lpJob As Job) As Repositories _
    Implements IProjectContainer.GetRepositories

  Public MustOverride Function GetRepositories(ByVal lpProject As Project) As Repositories _
    Implements IProjectContainer.GetRepositories

  Public MustOverride Function SaveRepository(ByVal lpRepository As Repository,
                                              ByVal lpJob As Job, ByVal lpScope As ExportScope) As Boolean _
    Implements IProjectContainer.SaveRepository

  Public MustOverride Sub UpdateRepositories(ByVal lpJob As Job) _
    Implements IProjectContainer.UpdateRepositories

  Public MustOverride Sub UpdateRepositories(ByVal lpProject As Project) _
    Implements IProjectContainer.UpdateRepositories

#End Region

#Region "Process Result Summary Methods"

  Public MustOverride Function GetProcessResultSummary(lpBatch As Batch) As IProcessResultSummary Implements IProjectContainer.GetProcessResultSummary

  Public MustOverride Function GetProcessResultSummary(lpJob As Job) As IProcessResultSummary Implements IProjectContainer.GetProcessResultSummary

  Public MustOverride Sub SaveProcessResultSummary(lpBatch As Batch) Implements IProjectContainer.SaveProcessResultSummary

  Public MustOverride Sub SaveProcessResultSummary(lpJob As Job) Implements IProjectContainer.SaveProcessResultSummary

#End Region

#End Region

#Region "Public Shared Methods"

  Public Shared Function CreateContainer(ByVal lpItemsLocation As ItemsLocation) As Container

    Dim lobjBatchContainer As Container

    Try

      Select Case lpItemsLocation.Type

        'Case ContainerType.CSV
        '  lobjBatchContainer = New CSVContainer(lpItemsLocation)

        'Case ContainerType.OLEDB
        '  lobjBatchContainer = New OLEDBContainer(lpItemsLocation)

        Case ContainerType.SQLServer
          lobjBatchContainer = New SQLContainer(lpItemsLocation)

        Case Else
          Throw _
            New Exception(String.Format("{1}: Uknown ItemsLocation '{0}'", lpItemsLocation.Type,
                                        MethodBase.GetCurrentMethod.ToString))
      End Select

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

    Return lobjBatchContainer
  End Function

#End Region

#Region "Public Properties"

  Public ReadOnly Property Id() As String
    Get
      Return mstrId
    End Get
  End Property

  Public Property ItemsLocation() As ItemsLocation
    Get
      Return mobjItemsLocation
    End Get
    Set(ByVal value As ItemsLocation)
      mobjItemsLocation = value
    End Set
  End Property

#End Region

#Region "Protected Methods"

  ''' <summary>
  '''     Generates a standardized view name for a job based on the specified job name.
  ''' </summary>
  ''' <param name="lpJobName" type="String">
  '''     <para>
  '''         
  '''     </para>
  ''' </param>
  ''' <returns>
  '''     A job view name.
  ''' </returns>
  Protected Shared Function GenerateJobViewName(lpJobName As String) As String
    Try
      Return String.Format("jvw{0}", lpJobName.Replace(".", "_").Replace("'", "''").Replace(" ", String.Empty))
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Protected Overridable Function GetJobConfigurationFromDataReader(lpDataReader As DbDataReader) _
    As Configuration.JobConfiguration
    Try
      Dim lobjJobConfiguration As Configuration.JobConfiguration = Nothing
      Dim lobjJobConfig As Object = lpDataReader("Configuration")
      If Not IsDBNull(lobjJobConfig) Then
        lobjJobConfiguration = Configuration.JobConfiguration.FromXmlString(lobjJobConfig.ToString)
      Else
        ' This may be a legacy job created before we used the configuration object.  
        Return Nothing
      End If

      If lobjJobConfiguration Is Nothing Then
        Throw New ConfigurationException("Unable to get job configuration.")
      End If

      If lobjJobConfiguration.ItemsLocation.Equals(Me.ItemsLocation) = False Then
        lobjJobConfiguration.ItemsLocation = Me.ItemsLocation
      End If

      Return lobjJobConfiguration
    Catch Ex As Exception
      ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Protected Overridable Function GetOperationXmlFromDataReader(lpDataReader As DbDataReader) As String
    Try
      Dim lobjOperation As Object = lpDataReader("Process")
      Dim lstrOperation As String = String.Empty

      If IsDBNull(lobjOperation) Then
        Dim lobjJobConfiguration As Configuration.JobConfiguration = GetJobConfigurationFromDataReader(lpDataReader)
        Return lobjJobConfiguration.Process.Name
        'Dim lobjJobConfig As Object = lpDataReader("Configuration")
        'If Not IsDBNull(lobjJobConfig) Then
        '  Dim lobjJobConfiguration As Configuration.JobConfiguration = Configuration.JobConfiguration.FromXmlString(lobjJobConfig.ToString)
        '  lstrOperation = lobjJobConfiguration.Process.ToXmlString
        'End If
      Else
        lstrOperation = lobjOperation.ToString
      End If

      If String.IsNullOrEmpty(lstrOperation) Then
        lstrOperation = lpDataReader("Operation")
        If Not String.IsNullOrEmpty(lstrOperation) Then
          If OperationFactory.GetAvailableOperationTypes.Contains(lstrOperation) Then
            lstrOperation = ProcessFactory.CreateFromOperation(lstrOperation).ToXmlString
          Else
            lstrOperation = String.Empty
          End If
        End If
      End If

      Return lstrOperation

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Protected Overridable Function GetProcessFromDataReader(ByRef lpDataReader As DbDataReader) As IOperable
    Try

      Dim lstrOperation As String = GetOperationXmlFromDataReader(lpDataReader)

      If String.IsNullOrEmpty(lstrOperation) Then
        Return Nothing
      End If

      Dim lobjProcess As Process = Nothing

      Try
        lobjProcess = Serializer.Deserialize.XmlString(lstrOperation, GetType(Process))
      Catch DeSerEx As DeserializationException
        If DeSerEx.InnerException IsNot Nothing AndAlso TypeOf DeSerEx.InnerException Is InvalidOperationException Then
          If _
            DeSerEx.InnerException.InnerException IsNot Nothing AndAlso
            TypeOf DeSerEx.InnerException.InnerException Is UnknownOperationException Then
            Throw DeSerEx.InnerException.InnerException
          End If
        End If
      End Try

      Return lobjProcess

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  'Protected Overridable Function GetTransformationsFromDataReader(ByRef lpDataReader As DbDataReader) As Transformations.ITransformation
  '  Try

  '  Catch ex As Exception
  '    ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '    ' Re-throw the exception to the caller
  '    Throw
  '  End Try
  'End Function

  Protected Overridable Function GetBatchLockFromDataReader(ByRef lpDataReader As DbDataReader,
                                                            ByRef lpErrorMessage As String) As IBatchLock
    Try

      Return New BatchLock(lpDataReader("ID").ToString,
                           lpDataReader("BatchId").ToString,
                           lpDataReader("JobId").ToString,
                           lpDataReader("JobName").ToString,
                           lpDataReader("LockedBy").ToString,
                           lpDataReader("IsLocked"),
                           lpDataReader("LockDate"),
                           DateTime.MinValue)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Protected Overridable Function GetJobFromDataReader(ByRef lpDataReader As DbDataReader, ByRef lpProject As Project,
                                                      ByRef lpErrorMessage As String) As Job
    Try

      ' There was a change in the database schema related to how we store job configuration information starting with version 2.5.0.12
      ' Prior to that all information for the job was stored in separate columns in the job table.
      ' After that all the information for the job is stored in an xml serialized JobConfiguration object.
      ' This give us the benefit of not having to update the database schema each time we want to add a new job feature.
      ' Try to get the job configuration object first, if that fails then try using the earlier structure with discreet columns.

      Dim lstrName As String = Helper.GetValueFromDataRecord(lpDataReader, JOB_NAME_COLUMN, String.Empty)
      Dim llngItemsProcessed As Long = Helper.GetValueFromDataRecord(lpDataReader, PROJECT_ITEMS_PROCESSED_COLUMN, 0)
      Dim lstrWorkSummary As String = Helper.GetValueFromDataRecord(lpDataReader, PROJECT_WORK_SUMMARY_COLUMN,
                                                                    String.Empty)
      Dim lobjWorkSummary As IWorkSummary = New WorkSummary(lstrWorkSummary, lstrName)
      Dim lobjJobConfiguration As Configuration.JobConfiguration = GetJobConfigurationFromDataReader(lpDataReader)

      If lobjJobConfiguration IsNot Nothing Then
        ' We were able to read the job configuration from the DB, we will use it.
        Dim _
          lobjJob As _
            New Job(lpDataReader("JobId").ToString, lpDataReader("CreateDate"), lobjJobConfiguration, llngItemsProcessed,
                    lobjWorkSummary)
        'Return New Job(lpDataReader("JobId").ToString, lpDataReader("CreateDate"), lobjJobConfiguration, llngItemsProcessed, lobjWorkSummary)

        If lobjJob.Source IsNot Nothing AndAlso
           lpProject IsNot Nothing AndAlso
           lpProject.Repositories IsNot Nothing AndAlso
           Not String.IsNullOrEmpty(lobjJob.Source.SourceConnectionString) Then

          Dim lstrRepositoryName As String =
                ContentSource.GetNameFromConnectionString(lobjJob.Source.SourceConnectionString)
          Dim lobjRepository As Repository = lpProject.Repositories.ItemByName(lstrRepositoryName)
          If lobjRepository IsNot Nothing Then
            lobjJob.SourceRepository = lobjRepository
          End If

        End If

        lobjJob.Relationships.AddRange(GetJobRelationships(lobjJob.Id))

        Return lobjJob

      Else
        ' We were unable to read the job configuration from the DB, we will proceed based on the legacy schema.
        Dim lobjProcess As Process = Nothing
        Try
          lobjProcess = GetProcessFromDataReader(lpDataReader)
        Catch UnknownOpEx As UnknownOperationException
          lpErrorMessage = UnknownOpEx.Message
          lobjProcess = New Process
        End Try

        Return New Job(lpDataReader("JobId").ToString,
                       lpDataReader("JobName").ToString,
                       lpDataReader("Description").ToString(),
                       lpDataReader("Operation").ToString(),
                       lpDataReader("CreateDate").ToString(),
                       lobjProcess,
                       Convert.ToInt32(lpDataReader("BatchSize").ToString()),
                       DeserializeString(lpDataReader("JobSource").ToString(), GetType(JobSource), New JobSource()),
                       DeserializeString(lpDataReader("ItemsLocation").ToString(), ItemsLocation.GetType,
                                         Me.ItemsLocation),
                       lpDataReader("DestinationConnectionString").ToString(),
                       StringToEnum(lpDataReader("ContentStorageType").ToString(), GetType(Content.StorageTypeEnum)),
                       lpDataReader("DeclareAsRecordOnImport").ToString(),
                       DeserializeString(lpDataReader("DeclareRecordConfiguration").ToString(),
                                         GetType(DeclareRecordConfiguration), New DeclareRecordConfiguration()),
                       DeserializeString(lpDataReader("Transformations").ToString(),
                                         GetType(TransformationCollection),
                                         New TransformationCollection()),
                       StringToEnum(lpDataReader("DocumentFilingMode").ToString(), GetType(FilingMode)),
                       lpDataReader("LeadingDelimiter").ToString(),
                       StringToEnum(lpDataReader("BasePathLocation").ToString(), GetType(Migrations.ePathLocation)),
                       lpDataReader("FolderDelimiter").ToString(),
                       lpDataReader("TransformationSourcePath").ToString())

      End If


    Catch ex As Exception
      Dim lobjUnknownOperationExtension As UnknownOperationException = Helper.GetInnerException(ex,
                                                                                                GetType(
                                                                                                 UnknownOperationException))
      If lobjUnknownOperationExtension IsNot Nothing Then
        ApplicationLogging.LogException(lobjUnknownOperationExtension, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw lobjUnknownOperationExtension
      Else
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End If
    End Try
  End Function

  Protected Overridable Function GetJobRelationshipFromDataReader(ByRef lpDataReader As DbDataReader)
    Try
      Dim lstrId As String = Helper.GetValueFromDataRecord(lpDataReader, JOB_RELATIONSHIP_ID_COLUMN, String.Empty)
      Dim lstrName As String = Helper.GetValueFromDataRecord(lpDataReader, JOB_RELATIONSHIP_NAME_COLUMN, String.Empty)
      Dim lstrDescription As String = Helper.GetValueFromDataRecord(lpDataReader, JOB_RELATIONSHIP_DESCRIPTION_COLUMN,
                                                                    String.Empty)

      Dim lobjJobRelationship As New JobRelationship(lstrId, lstrName, lstrDescription)

      Return lobjJobRelationship

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Protected Overridable Function GetBatchFromDataReader(ByRef lpJob As Job, ByRef lpDataReader As DbDataReader) As Batch

    Try
      'Try

      ' lobjJobConfiguration = lpJob.GetConfiguration
      Dim lobjJobConfiguration As JobConfiguration = lpJob.Configuration
      Dim lobjReturnBatch As New Batch(lpDataReader("BatchId").ToString(),
                              lpDataReader("BatchName").ToString(),
                              lpDataReader("AssignedTo").ToString(),
                              lobjJobConfiguration)
      lobjReturnBatch.SetJob(lpJob)
      Return lobjReturnBatch
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try


    '  Try
    '    lobjProcess = GetProcessFromDataReader(lpDataReader)
    '  Catch UnknownOpEx As UnknownOperationException
    '    lobjProcess = New Process
    '  End Try

    '  Return New Batch(lpDataReader("BatchId").ToString, _
    '                              lpDataReader("BatchName").ToString, _
    '                              lpDataReader("Description").ToString(), _
    '                              lpDataReader("Operation").ToString(), _
    '                              lobjProcess, _
    '                              lpDataReader("AssignedTo").ToString(), _
    '                              lpDataReader("ExportPath").ToString(), _
    '                              DeserializeString(lpDataReader("ItemsLocation").ToString(), ItemsLocation.GetType, New ItemsLocation()), _
    '                              lpDataReader("DestinationConnectionString").ToString(), _
    '                              lpDataReader("SourceConnectionString").ToString(), _
    '                              StringToEnum(lpDataReader("ContentStorageType").ToString(), GetType(Core.Content.StorageTypeEnum)), _
    '                              lpDataReader("DeclareAsRecordOnImport").ToString(),
    '                              DeserializeString(lpDataReader("DeclareRecordConfiguration").ToString(), GetType(DeclareRecordConfiguration), New DeclareRecordConfiguration()), _
    '                              DeserializeString(lpDataReader("Transformations").ToString(), GetType(Transformations.TransformationCollection), New Transformations.TransformationCollection), _
    '                              StringToEnum(lpDataReader("DocumentFilingMode").ToString(), GetType(Core.FilingMode)), _
    '                              lpDataReader("LeadingDelimiter").ToString(), _
    '                              StringToEnum(lpDataReader("BasePathLocation").ToString(), GetType(Migrations.ePathLocation)), _
    '                              lpDataReader("FolderDelimiter").ToString())

    'Catch ex As Exception
    '  ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
    '  ' Re-throw the exception to the caller
    '  Throw
    'End Try
  End Function

  Protected Overridable Function GetBatchItemFromDataReader(ByRef lpDataReader As DbDataReader, ByRef lpBatch As Batch,
                                                            ByVal lpGetProcessResult As Boolean) As IBatchItem
    Try

      Dim lobjBatchItem As BatchItem = Nothing
      Dim lstrOperationType As String = Nothing
      Dim lstrProcessResult As String = Nothing

      'lenuOperationType = StringToEnum(lobjDataReader("Operation").ToString, GetType(OperationType))
      lstrOperationType = lpDataReader("Operation").ToString

      If lpGetProcessResult Then
        lstrProcessResult = lpDataReader("ProcessResult").ToString
      End If

      'lobjBatchItem = BatchItem.CreateBatchItem(lenuOperationType, lobjDataReader("SourceDocId").ToString, lobjDataReader("Title").ToString, lpBatch)
      'lobjBatchItem = BatchItem.CreateBatchItem(lstrOperationType, lpDataReader("SourceDocId").ToString, lpDataReader("Title").ToString, lpBatch)
      ''lobjBatchItem = New BatchItem(lpDataReader("SourceDocId").ToString, _
      ''                              lpDataReader("DestDocId").ToString, _
      ''                              lpDataReader("Title").ToString, _
      ''                              lpBatch)

      lobjBatchItem = New BatchItem(lpDataReader("SourceDocId").ToString,
                                    lpDataReader("DestDocId").ToString,
                                    lpDataReader("Title").ToString,
                                    lpBatch,
                                    lpDataReader("ProcessedStatus").ToString,
                                    lpDataReader("ProcessedMessage").ToString,
                                    GetDateFromDBValue(lpDataReader("ProcessStartTime")),
                                    GetDateFromDBValue(lpDataReader("ProcessFinishTime")),
                                    GetValueFromDBValue(lpDataReader("TotalProcessingTime")),
                                    GetValueFromDBValue(lpDataReader("ProcessedBy")),
                                    GetDateFromDBValue(lpDataReader("CreateDate")),
                                    lstrProcessResult)

      With lobjBatchItem
        .BatchId = lpDataReader("BatchID").ToString
        .Id = lpDataReader("ID").ToString
        '.Operation = lstrOperationType
        If lpBatch IsNot Nothing Then
          .Process = lpBatch.Process
        End If

        If lpGetProcessResult Then
          .ProcessedStatus = [Enum].Parse(GetType(ProcessedStatus), lpDataReader("ProcessedStatus").ToString)
          If Not String.IsNullOrEmpty(lstrProcessResult) Then
            .ProcessResult = Serializer.Deserialize.XmlString(lstrProcessResult, GetType(ProcessResult))
          End If
        End If

      End With

      Return lobjBatchItem

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Protected Shared Function GetDateFromDBValue(lpValue As Object) As DateTime?
    Try
      Dim ldatReturnValue As New DateTime?

      If IsDBNull(lpValue) Then
        '	ldatReturnValue = Nothing
      ElseIf lpValue Is Nothing Then
        '	ldatReturnValue = Nothing
      ElseIf IsDate(lpValue) Then
        ldatReturnValue = lpValue
      Else
        Throw New ArgumentException(String.Format("'{0}' is an invalid date value.", lpValue), NameOf(lpValue))
      End If

      Return ldatReturnValue

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Protected Shared Function GetValueFromDBValue(lpValue As Object) As Object
    Try
      If IsDBNull(lpValue) Then
        Return Nothing
      ElseIf lpValue Is Nothing Then
        Return Nothing
      Else
        Return lpValue
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  ''' <summary>
  ''' Provides a safe way to deserialize in case the string is empty or can't deserialize for some reason
  ''' returns an empty object reference
  ''' </summary>
  ''' <param name="lpString"></param>
  ''' <param name="lpType"></param>
  ''' <param name="lpEmptyObject"></param>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Protected Shared Function DeserializeString(ByVal lpString As String,
                                       ByVal lpType As Type,
                                       ByVal lpEmptyObject As Object) As Object

    Try

      If Not String.IsNullOrEmpty(lpString) Then
        Return Serializer.Deserialize.XmlString(lpString, lpType)
      Else
        Return lpEmptyObject
      End If

    Catch exx As DeserializationException
      Return lpEmptyObject

    Catch ex As Exception
      ApplicationLogging.WriteLogEntry(ex.Message, TraceEventType.Information)
    End Try

    Return lpEmptyObject
  End Function

  Protected Function SerializeString(ByVal lpObject As Object) As String

    Try

      Return SerializeString(lpObject, Nothing)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
    End Try

    Return String.Empty
  End Function

  Protected Shared Function SerializeString(ByVal lpObject As Object,
                                     ByVal lpEmptyObject As Object) As String

    Try

      If (lpObject IsNot Nothing) Then
        Return Serializer.Serialize.XmlString(lpObject)

      Else
        Return Serializer.Serialize.XmlString(lpEmptyObject)
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
    End Try

    Return String.Empty
  End Function

  Protected Shared Function StringToEnum(ByVal lpString As String,
                                  ByVal lpType As Type)

    Try
      Return Helper.StringToEnum(lpString, lpType)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

#End Region

#Region "IDisposable Methods"

  ' IDisposable
  Protected Overridable Sub Dispose(ByVal disposing As Boolean)

    If Not Me.disposedValue Then

      If disposing Then
        ' DISPOSETODO: free other state (managed objects).
        'FinalizeLogSession()
      End If

      ' DISPOSETODO: free your own state (unmanaged objects).
      ' DISPOSETODO: set large fields to null.
    End If

    Me.disposedValue = True
  End Sub

#Region " IDisposable Support "

  ' This code added by Visual Basic to correctly implement the disposable pattern.
  Public Sub Dispose() _
    Implements IDisposable.Dispose
    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    Dispose(True)
    GC.SuppressFinalize(Me)
  End Sub

#End Region

#End Region

#Region "ILoggable Implementation"

  'Private mobjLogSession As Gurock.SmartInspect.Session

  'Protected Overridable Sub FinalizeLogSession() Implements ILoggable.FinalizeLogSession
  '  Try
  '    ApplicationLogging.FinalizeLogSession(mobjLogSession)
  '  Catch ex As Exception
  '    ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
  '    ' Re-throw the exception to the caller
  '    Throw
  '  End Try
  'End Sub

  'Protected Overridable Sub InitializeLogSession() Implements ILoggable.InitializeLogSession
  '  Try
  '    mobjLogSession = ApplicationLogging.InitializeLogSession(Me.GetType.Name, System.Drawing.Color.MistyRose)
  '  Catch ex As Exception
  '    ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
  '    ' Re-throw the exception to the caller
  '    Throw
  '  End Try
  'End Sub

  Public MustOverride Function GetProjectDbFileInfo() As DbFilesInfo Implements IProjectContainer.GetProjectDbFileInfo

  'Protected Friend ReadOnly Property LogSession As Gurock.SmartInspect.Session Implements ILoggable.LogSession
  '  Get
  '    Try
  '      If mobjLogSession Is Nothing Then
  '        InitializeLogSession()
  '      End If
  '      Return mobjLogSession
  '    Catch ex As Exception
  '      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
  '      ' Re-throw the exception to the caller
  '      Throw
  '    End Try
  '  End Get
  'End Property

#End Region
End Class
