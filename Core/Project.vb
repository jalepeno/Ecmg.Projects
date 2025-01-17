'---------------------------------------------------------------------------------
' <copyright company="ECMG">
'     Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'     Copying or reuse without permission is strictly forbidden.
' </copyright>
'---------------------------------------------------------------------------------

#Region "Imports"

Imports System.ComponentModel
Imports System.IO
Imports System.Reflection
Imports System.Text
Imports System.Xml.Serialization
Imports Documents.Core
Imports Documents.Exceptions
Imports Documents.Providers
Imports Documents.SerializationUtilities
Imports Documents.Utilities
Imports Newtonsoft.Json
Imports OfficeOpenXml
Imports OfficeOpenXml.ConditionalFormatting.Contracts
Imports Projects.Configuration
Imports Projects.Converters

'Imports Gurock.SmartInspect

#End Region

<Serializable()> <DebuggerDisplay("{DebuggerIdentifier(),nq}")> Public Class Project
  Inherits NotifyObject
  Implements IDescription
  Implements IDisposable
  Implements IDataErrorInfo
  Implements IProjectDescription
  'Implements ILoggable

#Region "Class Variables"

  Private Const DEFAULT_BATCH_SIZE As Integer = 1000
  Private mstrName As String = String.Empty
  Private mstrDescription As String = String.Empty
  Private mintBatchSize As Integer = DEFAULT_BATCH_SIZE
  Private WithEvents mobjJobs As New Jobs(Me)
  Private mstrId As String = String.Empty
  Private mobjItemsLocation As ItemsLocation 'Tells where we are storing this project
  Private mobjContainer As Container
  Private mobjHost As Object = Nothing
  Private mobjJobIdentifiers As IJobIdentifiers = Nothing
  Private mobjProjectConnections As ProjectConnections = Nothing
  Private mobjArea As IArea = Nothing
  Private WithEvents mobjJob As Job
  Private mdatCreateDate As DateTime
  Private mlngItemsProcessed As Long
  Private mobjWorkSummary As IWorkSummary = Nothing
  Private mobjRepositories As New Repositories
  Private mintOrphanBatchCount As Integer
  Private mintOrphanBatchItemCount As Integer

  'Private mobjLogSession As Gurock.SmartInspect.Session

#End Region

#Region "Public Properties"

  <XmlAttribute()>
  Public Property Id() As String Implements IProjectDescription.Id
    Get
      Return mstrId
    End Get
    Set(ByVal value As String)

      If (Helper.IsDeserializationBasedCall = True) Then
        mstrId = value

      Else
        Throw New InvalidOperationException("Although Project Id is a public property, set operations are not allowed.  Treat property as read-only.")
      End If

    End Set
  End Property

  <XmlIgnore()>
  Public Property Area As IArea Implements IProjectDescription.Area
    Get
      Try
        Return mobjArea
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As IArea)
      Try
        If value Is Nothing Then
          Throw New ArgumentNullException
        End If
        Dim lobjOriginalArea As IArea = mobjArea

        mobjArea = value
        If Not mobjArea.Projects.Contains(Me) Then
          mobjArea.Projects.Add(Me)
        End If
        OnPropertyChanged("Area")
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  <JsonProperty("area")>
  ReadOnly Property AreaName As String Implements IProjectDescription.AreaName
    Get
      Try
        Return Area.Name
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  <Xml.Serialization.XmlAttribute()>
  Public Property Name() As String _
                                            Implements IDescription.Name, INamedItem.Name, IProjectDescription.Name
    Get
      Return mstrName
    End Get
    Set(ByVal value As String)
      Try
        mstrName = value
        OnPropertyChanged("Name")
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  <Xml.Serialization.XmlAttribute()>
  Public Property Description() As String _
                                            Implements IDescription.Description
    Get
      Return mstrDescription
    End Get
    Set(ByVal value As String)
      Try
        mstrDescription = value
        OnPropertyChanged("Description")
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public ReadOnly Property Repositories As Repositories
    Get
      Try
        Return mobjRepositories
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    'Friend Set(value As Repositories)
    '  Try
    '    mobjRepositories = value
    '  Catch ex As Exception
    '    ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
    '    ' Re-throw the exception to the caller
    '    Throw
    '  End Try
    'End Set
  End Property

  Public ReadOnly Property BatchLocks As IBatchLocks
    Get
      Try
        Return GetBatchLocks()
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        Return New BatchLocks(Me)
      End Try
    End Get
  End Property

  Public Property BatchSize() As Integer
    Get
      Return mintBatchSize
    End Get
    Set(ByVal value As Integer)
      Try
        mintBatchSize = value
        OnPropertyChanged("BatchSize")
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public ReadOnly Property CreateDate As DateTime Implements IProjectDescription.CreateDate
    Get
      Try
        Return mdatCreateDate
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public Property ItemsLocation() As IItemsLocation Implements IProjectDescription.Location
    Get
      Return mobjItemsLocation
    End Get
    Set(ByVal value As IItemsLocation)
      mobjItemsLocation = value
      OnPropertyChanged("ItemsLocation")

      Try
        mobjContainer = Container.CreateContainer(mobjItemsLocation)

      Catch ex As Exception
        ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try

    End Set
  End Property

  Public Property ItemsProcessed As Long Implements IProjectDescription.ItemsProcessed
    Get
      Try
        Return mlngItemsProcessed
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As Long)
      Try
        mlngItemsProcessed = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public Property OrphanBatchCount As Integer
    Get
      Try
        Return mintOrphanBatchCount
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Friend Set(value As Integer)
      Try
        mintOrphanBatchCount = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public Property OrphanBatchItemCount As Integer
    Get
      Try
        Return mintOrphanBatchItemCount
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Friend Set(value As Integer)
      Try
        mintOrphanBatchItemCount = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public ReadOnly Property WorkSummary As IWorkSummary Implements IProjectDescription.WorkSummary
    Get
      Try
        Return mobjWorkSummary
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public Property Jobs() As Jobs
    Get
      Return mobjJobs
    End Get
    Set(ByVal value As Jobs)
      Try
        mobjJobs = value
        OnPropertyChanged("Jobs")
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public ReadOnly Property IsRunningInProcess As Boolean
    Get
      Try
        If RunningJobs.Count > 0 Then
          Return True
        Else
          Return False
        End If
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property RunningJobs As Jobs
    Get
      Try
        Return GetRunningJobs()
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property JobIdentifiers As IJobIdentifiers
    Get
      Try
        If mobjJobIdentifiers Is Nothing Then
          If Container IsNot Nothing Then
            mobjJobIdentifiers = Container.GetJobIdentifiers()
          Else
            mobjJobIdentifiers = New JobIdentifiers
          End If
        End If

        Return mobjJobIdentifiers

      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property ProjectConnections As ProjectConnections
    Get
      Return mobjProjectConnections
    End Get
  End Property

  Public Property Host As Object
    Get

      Try
        Return mobjHost

      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try

    End Get
    Set(value As Object)

      Try
        mobjHost = value
        OnPropertyChanged("Host")
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try

    End Set
  End Property

#End Region

#Region "Friend Properties"

  Friend ReadOnly Property Container As Container
    Get

      Try
        Return mobjContainer

      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try

    End Get
  End Property

#End Region

#Region "Public Methods"

  Public Sub UnlockBatch(lpBatchId As String)
    Try
      Container.UnLockBatch(lpBatchId, String.Empty)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Sub UnlockBatches()
    Try
      For Each lobjJob As Job In Me.Jobs
        lobjJob.UnlockBatches()
      Next
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Sub DeleteJob(ByVal lpJob As Job)

    Try
      lpJob.Delete()
      ApplicationLogging.WriteLogEntry(String.Format("Deleted job '{0}' from project '{1}'.", lpJob.Name, Me.Name), TraceEventType.Information, 61322)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Sub

  ''' <summary>
  '''   Deletes all orphan batches and batch items from the project database.
  ''' </summary>
  ''' <remarks>
  '''   This method returns asyncronously so do not expect 
  '''   that the deletion is completed before the method returns.
  ''' </remarks>
  Public Sub DeleteOrphanBatches()
    Try
      Container.DeleteOrphanBatches(Me)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Function CreateJob(ByVal lpName As String,
                            ByVal lpDescription As String,
                            ByVal lpOperationType As String,
                            ByVal lpJobSource As JobSource,
                            ByVal lpDestConnectionString As String,
                            ByVal lpItemsLocation As ItemsLocation,
                            ByVal lpBatchSize As Integer) As Job

    Dim lobjJob As Job = Nothing

    Try
      lobjJob = New Job(lpName, lpDescription, lpOperationType, lpBatchSize)
      lobjJob.Source = lpJobSource
      lobjJob.DestinationConnectionString = lpDestConnectionString
      lobjJob.ItemsLocation = lpItemsLocation
      lobjJob.BatchSize = lpBatchSize

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

    Return lobjJob

  End Function

  Public Function CreateJob(ByVal lpName As String,
                            ByVal lpDescription As String,
                            ByVal lpOperationType As String,
                            ByVal lpJobSource As JobSource,
                            ByVal lpDestConnectionString As String,
                            ByVal lpItemsLocation As ItemsLocation) As Job

    Dim lobjJob As Job

    Try
      lobjJob = CreateJob(lpName, lpDescription, lpOperationType, lpJobSource, lpDestConnectionString, lpItemsLocation, mintBatchSize)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

    Return lobjJob

  End Function

  Public Function GetItemCount() As Long
    Try
      Return Container.GetItemCount(Me)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Function GetJobConfigurationString(lpJobId As String) As String
    Try
      Return Container.GetJobConfigurationByName(lpJobId)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Sub SaveJobConfiguration(lpJobConfiguration As JobConfiguration)
    Try
      Container.SaveJobConfiguration(lpJobConfiguration)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Sub SaveJob(ByVal lpJob As Job)

    Try
      lpJob.SetProject(Me)
      mobjContainer.SaveJob(lpJob)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Sub

  Public Sub AddJob(ByRef lpJob As Job)
    Try
      lpJob.SetProject(Me)
      If Jobs.Contains(lpJob.Name) Then
        Throw New ItemAlreadyExistsException(lpJob.Name,
          String.Format("A job by the name '{0}' already exists in project '{1}'.",
                        lpJob.Name, lpJob.ProjectName))
      End If
      Jobs.Add(lpJob)
      mobjContainer.SaveJob(lpJob)
      lpJob.PushFirebaseUpdateAsync()
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Sub AddJob(lpJobConfiguration As Configuration.JobConfiguration)
    Try
      mobjContainer.SaveJob(Me.Id, lpJobConfiguration)

      Dim lobjJob As Job = mobjContainer.GetJobById(lpJobConfiguration.Name)
      ' <Modified by: Ernie at 9/13/2014-5:53:33 PM on machine: ERNIE-THINK>
      ' lobjJob.SaveRepositories()
      If lobjJob IsNot Nothing Then
        Try
          lobjJob.SaveRepositories()
        Catch ex As Exception
          ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
          ' Just move on for now, we do not want this to cause a failure in adding a job.
        End Try
      End If
      ' </Modified by: Ernie at 9/13/2014-5:53:33 PM on machine: ERNIE-THINK>

    Catch Ex As Exception
      ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Sub AddJobs(lpJobConfigurations As Configuration.JobConfigurations)
    Try

      'If lpJobConfigurations.AreAllNamesTheSame AndAlso TypeOf lpJobConfigurations Is SplitJobConfigurations Then
      '  DirectCast(lpJobConfigurations, SplitJobConfigurations).RenameSplitItems()
      'End If

      For Each lobjConfiguration As JobConfiguration In lpJobConfigurations
        AddJob(lobjConfiguration)
      Next
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Sub SaveProject() Implements IProjectDescription.Save

    Try

      '' Determine if this is a new project be checking the catalog
      'Dim lblnIsNewProject As Boolean = Not ProjectCatalog.Instance.ProjectExists(Me.Id)

      If Area Is Nothing Then
        Area = ProjectCatalog.Instance.DefaultArea
      End If

      mobjContainer.SaveProject(Me)

      If TypeOf mobjContainer Is ICatalogContainer Then
        CType(mobjContainer, ICatalogContainer).SaveProject(Me)
      Else
        ApplicationLogging.WriteLogEntry("Unable to save project to catalog, the current container does not support catalogs.")
      End If

      'If lblnIsNewProject Then
      '  'Dim lstrPushPath As String = String.Format("{0}catalogs/{1}/areas/{2}/{3}", FIREBASE_APP_URL, ProjectCatalog.Instance.Id, Area.Name, Me.Name.Replace(".", "_"))
      '  'DirectCast(GetProjectInfo(), IFirebasePusher).UpdateFirebase(lstrPushPath)
      '  'Beep()
      '  PushFirebaseUpdateAsync()
      'End If

      'PushFirebaseUpdateAsync()

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Sub

  'Private Async Sub PushFirebaseUpdateAsync()
  '  Try
  '    Dim lobjTask As Task = _
  '    Task.Factory.StartNew(Sub()
  '                            Dim lstrPushPath As String = String.Format("{0}catalogs/{1}/areas/{2}/{3}", FIREBASE_APP_URL, ProjectCatalog.Instance.Id, Area.Name, Me.Name.Replace(".", "_"))
  '                            DirectCast(GetProjectInfo(), IFirebasePusher).UpdateFirebase(lstrPushPath)
  '                          End Sub)
  '    Await lobjTask

  '  Catch ex As Exception
  '    ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '    ' Re-throw the exception to the caller
  '    Throw
  '  End Try
  'End Sub

  Public Shared Function OpenProjectById(lpProjectId As String) As Project
    Try
      Dim lobjCatalog As ProjectCatalog = ProjectCatalog.Instance()
      Dim lobjProjectInfo As IProjectInfo = lobjCatalog.GetProjectInfo(lpProjectId)

      Return OpenProject(lobjProjectInfo.Location)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Shared Function OpenProject(lpConnectionString As String) As Project
    Dim lstrErrorMessage As String = String.Empty
    Try

      Dim lobjProject As Project = OpenProject(lpConnectionString, lstrErrorMessage)
      If String.IsNullOrEmpty(lstrErrorMessage) Then
        Return lobjProject
      Else
        Throw New ApplicationException(lstrErrorMessage)
      End If
    Catch Ex As Exception
      ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Shared Function OpenProject(lpConnectionString As String,
                                     ByRef lpErrorMessage As String) As Project
    Try
      Dim lobjItemsLocation As New ItemsLocation(lpConnectionString)
      Return OpenProject(lobjItemsLocation, lpErrorMessage)
    Catch Ex As Exception
      ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Shared Function OpenProject(ByVal lpItemsLocation As ItemsLocation,
                                     ByRef lpErrorMessage As String) As Project

    Try

      Dim lobjContainer As Container = Container.CreateContainer(lpItemsLocation)
      Dim lobjProject As Project = lobjContainer.OpenProject(lpErrorMessage)
      'lobjProject.Repositories = lobjContainer.GetRepositories(lobjProject)
      Return lobjProject

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  Public Async Sub InitializeCachedRepositoriesAsync()
    Try
      'Container.InitializeCachedRepositories(Me)
      Dim lobjTask As Task = Task.Factory.StartNew(
            Sub()
              If Repositories.Count = 0 Then
                Repositories.AddRange(Container.GetRepositories(Me))
              End If
            End Sub)

      Await lobjTask

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Function UpdateAllJobs() As ProvisionStatuses

    Try
      Return UpdateAllJobs(Nothing, Nothing)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  Public Function UpdateAllJobs(ByVal lpBackgroundWorker As BackgroundWorker,
                                ByVal lpBackgroundWorkerEventArgs As DoWorkEventArgs) As ProvisionStatuses

    Try

      Dim llngTotalNewItems As Long = 0
      Dim lobjProvisionStatus As ProvisionStatus
      Dim lobjProvisionStatuses As New ProvisionStatuses(Me)

      For Each lobjJob As Job In Me.Jobs

        If (lpBackgroundWorker IsNot Nothing) Then

          If (lpBackgroundWorker.CancellationPending) Then
            lpBackgroundWorkerEventArgs.Cancel = True
            Return lobjProvisionStatuses
          End If

        End If

        lpBackgroundWorker.ReportProgress(0, lobjJob)
        lobjProvisionStatus = lobjJob.Update(lpBackgroundWorker, lpBackgroundWorkerEventArgs)
        lpBackgroundWorker.ReportProgress(100, lobjProvisionStatus)
        lobjProvisionStatuses.Add(lobjProvisionStatus)

      Next

      Return lobjProvisionStatuses

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  Public Sub UpdateRepositories(ByVal lpScope As ExportScope)

    Try
      'For Each lobjJob In Me.Jobs
      '  lobjJob.UpdateRepository(lpScope)
      'Next
      mobjProjectConnections = New ProjectConnections(Me)

      mobjContainer.UpdateRepositories(Me)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Sub

  Public Function GetBatch(ByVal lpId As String) As Batch

    Try
      Return Jobs.GetBatch(lpId)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  ''' <summary>
  ''' Gets a consolidated list of all content source 
  ''' connection strings used in all jobs in the project
  ''' </summary>
  ''' <returns>An ordered list of connection strings</returns>
  ''' <remarks></remarks>
  Public Function GetAllConnectionStrings() As List(Of String)

    Try

      Dim lobjReturnList As New List(Of String)

      For Each lobjJob As Job In Me.Jobs

        ' First get the source connection string
        If lobjJob.Source IsNot Nothing AndAlso Not String.IsNullOrEmpty(lobjJob.Source.SourceConnectionString) Then

          If lobjReturnList.Contains(lobjJob.Source.SourceConnectionString) = False Then
            lobjReturnList.Add(lobjJob.Source.SourceConnectionString)
          End If

        End If

        ' Now try to get the destination connection string (if defined)
        If Not String.IsNullOrEmpty(lobjJob.DestinationConnectionString) Then

          If lobjReturnList.Contains(lobjJob.DestinationConnectionString) = False Then
            lobjReturnList.Add(lobjJob.DestinationConnectionString)
          End If

        End If

      Next

      lobjReturnList.Sort()

      Return lobjReturnList

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  ''' <summary>
  ''' Gets a unique list of all content source names 
  ''' used in all jobs in the project
  ''' </summary>
  ''' <returns>An ordered list of content source names</returns>
  ''' <remarks></remarks>
  Public Function GetAllContentSourceNames() As List(Of String)

    Try

      Dim lobjReturnList As New List(Of String)
      Dim lstrCandidateContentSourceName As String = String.Empty
      Dim lobjConnectionStrings As List(Of String) = GetAllConnectionStrings()

      For Each lstrConnectionString As String In lobjConnectionStrings
        lstrCandidateContentSourceName = ContentSource.GetNameFromConnectionString(lstrConnectionString)

        If lobjReturnList.Contains(lstrCandidateContentSourceName) = False Then
          lobjReturnList.Add(lstrCandidateContentSourceName)
        End If

      Next

      lobjReturnList.Sort()

      Return lobjReturnList

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  Public Function ToJsonDescription() As String Implements IProjectDescription.ToJson
    Try
      If Helper.IsRunningInstalled Then
        Return JsonConvert.SerializeObject(Me, Formatting.None, New ProjectDescriptionConverter())
      Else
        Return JsonConvert.SerializeObject(Me, Formatting.Indented, New ProjectDescriptionConverter())
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function ToString() As String

    Try
      Return DebuggerIdentifier()

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  Public Function GetAvgProcessingTime() As Single

    Try
      Return mobjContainer.GetProjectAvgProcessingTime(Me)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      '' Re-throw the exception to the caller
      'Throw
      Return 0
    End Try

  End Function

  Public Function GetBatchSummaryCounts() As WorkSummary

    Try

      If (Me.Jobs.Count > 0) Then
        Return Me.Jobs(0).JobBatchContainer.GetWorkSummaryCounts(Me)
      End If

      Return Nothing

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  Public Function GetFailureSummaries() As FailureSummaries

    Try
      Return mobjContainer.GetFailureSummaries(Me)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  Public Function GetProjectInfo() As IProjectInfo Implements IProjectDescription.GetProjectInfo
    Try
      Return mobjContainer.GetProjectInfo
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Function GetCachedWorkSummaryCounts() As IWorkSummary
    Try
      Return mobjContainer.GetCachedWorkSummaryCounts(Me.GetProjectInfo)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Function GetProjectSummaryCounts() As WorkSummaries

    Try

      If (Me.Jobs.Count > 0) Then
        ' Return Me.Jobs(0).JobBatchContainer.GetProjectSummaryCounts(Me)
        mobjWorkSummary = Me.Container.GetWorkSummaryCounts(Me)
        Return Me.Container.GetProjectSummaryCounts(Me)
      End If

      Return Nothing

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      '' Re-throw the exception to the caller
      'Throw
      Return New WorkSummaries
    Finally
      'PushFirebaseUpdateAsync()
    End Try

  End Function

  Public Function CreateSummarySpreadsheet() As Object
    Try
      Dim lobjProjectSummary As WorkSummaries = GetProjectSummaryCounts()
      Dim lobjFailureSummary As FailureSummaries = GetFailureSummaries()
      lobjProjectSummary.AddTotalsRow(GetAvgProcessingTime)
      Dim lobjProjectSummaryPackage As ExcelPackage = lobjProjectSummary.ToSpreadsheet
      'Dim lobjFailureSummaryPackage As ExcelPackage = lobjFailureSummary.ToSpreadsheet
      'If lobjFailureSummaryPackage IsNot Nothing AndAlso _
      '  lobjFailureSummaryPackage.Workbook IsNot Nothing AndAlso _
      '  lobjFailureSummaryPackage.Workbook.Worksheets IsNot Nothing AndAlso _
      '  lobjFailureSummaryPackage.Workbook.Worksheets.Count > 0 Then

      ' Format the project summary
      Dim lobjProjectSummaryWorksheet As ExcelWorksheet = lobjProjectSummaryPackage.Workbook.Worksheets.First
      lobjProjectSummaryWorksheet.Tables.First.TableStyle = Table.TableStyles.Light9
      Dim lintEndColumn As Integer = lobjProjectSummaryWorksheet.Dimension.End.Column
      Dim lintEndRow As Integer = lobjProjectSummaryWorksheet.Dimension.End.Row
      Dim lobjFailedColumn As New ExcelAddress(2, 5, lintEndRow, 5)

      Dim lcfrGreaterThanZero As IExcelConditionalFormattingGreaterThan = lobjProjectSummaryWorksheet.ConditionalFormatting.AddGreaterThan(lobjFailedColumn)
      lcfrGreaterThanZero.Style.Font.Color.Color = System.Drawing.Color.Red
      lcfrGreaterThanZero.Formula = 0

      Dim lobjAllData As New ExcelAddress(2, 1, lintEndRow, lintEndColumn)

      Dim lcfrFailedExpression As IExcelConditionalFormattingExpression = lobjProjectSummaryWorksheet.ConditionalFormatting.AddExpression(lobjAllData)
      lcfrFailedExpression.Style.Font.Color.Color = System.Drawing.Color.Red
      lcfrFailedExpression.Formula = "$E2>0"

      Dim lcfrSuccessExpression As IExcelConditionalFormattingExpression = lobjProjectSummaryWorksheet.ConditionalFormatting.AddExpression(lobjAllData)
      lcfrSuccessExpression.Style.Font.Color.Color = System.Drawing.Color.Green
      lcfrSuccessExpression.Formula = "AND($K2=1,$I2=0)"

      Dim lcfrNotStartedExpression As IExcelConditionalFormattingExpression = lobjProjectSummaryWorksheet.ConditionalFormatting.AddExpression(lobjAllData)
      lcfrNotStartedExpression.Style.Font.Color.Color = System.Drawing.Color.Gray
      lcfrNotStartedExpression.Formula = "$G2=0"

      ' lobjProjectSummaryWorksheet

      Dim lobjWorkSheet As ExcelWorksheet = lobjProjectSummaryPackage.Workbook.Worksheets.Add("Failure Summary")

      lobjWorkSheet.Cells("A1").LoadFromDataTable(lobjFailureSummary.ToDataTable, True, Table.TableStyles.Medium9)
      lobjWorkSheet.Cells(lobjWorkSheet.Dimension.Address).AutoFitColumns()
      lobjWorkSheet.Tables.First.TableStyle = Table.TableStyles.Light9
      'End If

      Return lobjProjectSummaryPackage

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      Return Nothing
    End Try
  End Function

  Public Function CreateNamedSummarySpreadsheetStream() As INamedStream
    Try
      Return New NamedStream(CreateSummarySpreadsheetStream(), SummarySpreadsheetFileName)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Function CreateSummarySpreadsheetStream() As Stream
    Try
      Dim lobjSpreadSheet As ExcelPackage = Me.CreateSummarySpreadsheet
      Dim lobjOutputStream As New MemoryStream
      lobjSpreadSheet.SaveAs(lobjOutputStream)
      If lobjOutputStream.CanSeek Then
        lobjOutputStream.Seek(0, 0)
      End If
      Return lobjOutputStream
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public ReadOnly Property SummarySpreadsheetFileName As String
    Get
      Try
        Return String.Format("{0}_ProjectSummary.xlsx", Me.Name)
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public Function WriteSummarySpreadsheet() As String
    Try
      Dim lstrOutputPath As String = Helper.CleanPath(String.Format("{0}\{1}",
                                               FileHelper.Instance.TempPath, Me.SummarySpreadsheetFileName))
      lstrOutputPath = Helper.CleanFile(lstrOutputPath, "_")

      If File.Exists(lstrOutputPath) Then
        If Helper.IsFileLocked(lstrOutputPath) = True Then
          Throw New ItemAlreadyExistsException(lstrOutputPath, "The summary spreadsheet already exists and is locked.")
        End If
      End If
      Dim lobjSpreadSheet As ExcelPackage = Me.CreateSummarySpreadsheet
      If lobjSpreadSheet IsNot Nothing Then
        lobjSpreadSheet.SaveAs(New FileInfo(lstrOutputPath))
        Return lstrOutputPath
      Else
        Return String.Empty
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Sub CancelAllJobs(lpReason As String)
    Try
      For Each lobjJob As Job In Me.Jobs
        lobjJob.CancelJob(lpReason)
      Next
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

#Region "Threading Methods"

#Region "Events"

  Public Delegate Sub BatchStartedEventHandler(ByVal sender As Object, ByRef e As Object)
  Public Delegate Sub BatchCompletedEventHandler(ByVal sender As Object, ByRef e As Object)
  Public Delegate Sub BatchItemCompletedEventHandler(ByVal sender As Object, ByRef e As Object)

  Public Event BatchStarted As BatchStartedEventHandler
  Public Event BatchCompleted As BatchCompletedEventHandler
  Public Event BatchItemCompleted As BatchItemCompletedEventHandler

#End Region

  ''' <summary>
  ''' Runs the specified job with threading.
  ''' Executes each batch on a separate thread.
  ''' </summary>
  ''' <param name="lpJobName"></param>
  ''' <param name="lpNumberOfThreads"></param>
  ''' <remarks></remarks>
  Public Sub RunJob(lpJobName As String,
                    lpNumberOfThreads As Integer)

    Try

      'LogSession.EnterMethod(Helper.GetMethodIdentifier(Reflection.MethodBase.GetCurrentMethod))

      'Make sure job exists, if not throw an exception
      mobjJob = Me.Jobs(lpJobName)

      If mobjJob Is Nothing Then
        Throw New KeyNotFoundException(String.Format("Cannot run job, job name: {0} does not exist in project {1}.", lpJobName, Me.Name))
      End If

      mobjJob.RunJob(lpNumberOfThreads)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    Finally
      'LogSession.LeaveMethod(Helper.GetMethodIdentifier(Reflection.MethodBase.GetCurrentMethod))
    End Try

  End Sub

#Region "Event Notifications"

  Private Sub mobjJob_BatchStarted(sender As Object,
                                       ByRef e As Object) _
          Handles mobjJob.BatchStarted

    Try
      RaiseEvent BatchStarted(sender, e)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
    End Try

  End Sub
  Private Sub mobjJob_BatchCompleted(sender As Object,
                                         ByRef e As Object) _
          Handles mobjJob.BatchCompleted

    Try
      RaiseEvent BatchCompleted(sender, e)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
    End Try

  End Sub


  Private Sub mobjJob_BatchItemCompleted(sender As Object,
                                             ByRef e As Object) _
          Handles mobjJob.BatchItemCompleted

    Try
      RaiseEvent BatchItemCompleted(sender, e)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
    End Try

  End Sub

#End Region

#End Region

#Region "Private Properties"

  Private ReadOnly Property IsDisposed() As Boolean
    Get
      Return disposedValue
    End Get
  End Property

#End Region

#Region "Protected Methods"

  Protected Friend Overridable Function DebuggerIdentifier() As String

    Dim lobjIdentifierBuilder As New StringBuilder

    Try

      If Not String.IsNullOrEmpty(Name) Then
        lobjIdentifierBuilder.AppendFormat("{0}: ", Name)

      Else
        lobjIdentifierBuilder.Append("Name not set: ")
      End If

      If ItemsLocation IsNot Nothing Then
        lobjIdentifierBuilder.AppendFormat("{0}", ItemsLocation.Type)
      End If

      If Jobs IsNot Nothing Then

        If Jobs.Count = 0 Then
          lobjIdentifierBuilder.Append(" (No Jobs)")

        ElseIf Jobs.Count = 1 Then
          lobjIdentifierBuilder.Append(" (1 Job)")

        ElseIf Jobs.Count > 1 Then
          lobjIdentifierBuilder.AppendFormat(" ({0} Jobs)", Jobs.Count)
        End If

      End If

      lobjIdentifierBuilder.AppendFormat(";BatchSize={0}", BatchSize)

      Return lobjIdentifierBuilder.ToString

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      Return lobjIdentifierBuilder.ToString
    End Try

  End Function

#End Region

#Region "Private Methods"

  Private Sub Project_BatchCompleted(sender As Object, ByRef e As Object) Handles Me.BatchCompleted
    Try
      mobjWorkSummary = Me.Container.GetWorkSummaryCounts(Me)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Function GetBatchLocks() As IBatchLocks
    Try
      Dim lobjBatchLocks As BatchLocks = Container.GetBatchLocks(String.Empty)
      lobjBatchLocks.Project = Me
      Return lobjBatchLocks
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Sub Serialize(ByVal lpFilePath As String)

    Try

      If IsDisposed Then
        Throw New ObjectDisposedException(Me.GetType.ToString)
      End If

      Serializer.Serialize.XmlFile(Me, lpFilePath)

    Catch ex As Exception
      'ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      '  Re-throw the exception to the caller
      Throw
    End Try

  End Sub

  Public Function Serialize() As System.Xml.XmlDocument

    Try

      If IsDisposed Then
        Throw New ObjectDisposedException(Me.GetType.ToString)
      End If

      Return Serializer.Serialize.Xml(Me)

    Catch ex As Exception
      'ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      '  Re-throw the exception to the caller
      Throw
    End Try

  End Function

  Public Function Deserialize(ByVal lpFilePath As String,
                              Optional ByRef lpErrorMessage As String = "") As Project

    Try

      If IsDisposed Then
        Throw New ObjectDisposedException(Me.GetType.ToString)
      End If

      Return Serializer.Deserialize.XmlFile(lpFilePath, Me.GetType)

    Catch ex As Exception
      'ApplicationLogging.LogException(ex, String.Format("{0}::Deserialize('{1}', '{2}')", Me.GetType.Name, lpFilePath, lpErrorMessage))
      lpErrorMessage = ex.Message
      Return Nothing
    End Try

  End Function

  Public Function DeSerialize(ByVal lpXML As System.Xml.XmlDocument) As Object

    Try

      If IsDisposed Then
        Throw New ObjectDisposedException(Me.GetType.ToString)
      End If

      Return Serializer.Deserialize.XmlString(lpXML.OuterXml, Me.GetType)

    Catch ex As Exception
      'ApplicationLogging.LogException(ex, String.Format("{0}::Deserialize(lpXML)", Me.GetType.Name))
      'Helper.DumpException(ex)
      '  Re-throw the exception to the caller
      Throw
    End Try

  End Function

  Private Function GetRunningJobs() As Jobs
    Try

      Dim lobjRunningJobs As New Jobs(Me)

      Dim list As Object = From lobjJob In Jobs Where
      ((Not lobjJob.IsCompleted) AndAlso (lobjJob.IsRunning)) Select lobjJob

      For Each lobjJob As Job In list
        lobjRunningJobs.Add(lobjJob)
      Next

      Return lobjRunningJobs

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  'Private Async Sub PushFirebaseUpdateAsync()
  '  Try
  '    Dim lobjTask As Task =
  '    Task.Factory.StartNew(Sub()
  '                            PushFirebaseUpdate()
  '                          End Sub)
  '    Await lobjTask

  '  Catch ex As Exception
  '    ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '    ' Re-throw the exception to the caller
  '    Throw
  '  End Try
  'End Sub

  'Private Sub PushFirebaseUpdate()
  '  Try
  '    If Not ConnectionSettings.Instance.DisableNotifications Then
  '      Dim lstrCatalogId As String = ProjectCatalog.Instance.Id
  '      Dim lstrPutPath As String = String.Format("{0}catalogs/{1}/areas/{2}/{3}", FIREBASE_APP_URL, lstrCatalogId, Me.Area.Name, Me.Name.Replace(".", "_"))
  '      Dim lobjFirebase As New FirebaseApplication(lstrPutPath)
  '      If lobjFirebase.Available Then
  '        Static lstrLastProjectInfoJson As String
  '        Dim lstrCurrentProjectInfoJson As String

  '        If String.IsNullOrEmpty(lstrLastProjectInfoJson) Then
  '          lstrLastProjectInfoJson = JsonConvert.SerializeObject(Me.WorkSummary, New WorkSummaryFirebaseConverter())
  '          lobjFirebase.Put("/workSummary", lstrLastProjectInfoJson)
  '        Else
  '          lstrCurrentProjectInfoJson = JsonConvert.SerializeObject(Me.WorkSummary, New WorkSummaryFirebaseConverter())
  '          If Not lstrCurrentProjectInfoJson.Equals(lstrLastProjectInfoJson) Then
  '            lstrLastProjectInfoJson = lstrCurrentProjectInfoJson
  '            lobjFirebase.Put("/workSummary", lstrLastProjectInfoJson)
  '          End If
  '        End If
  '      Else
  '        ' <Added by: Ernie at: 1/27/2015-9:46:23 AM on machine: ERNIE-THINK>
  '        ' Disable the notifications until they are manually reset.
  '        ConnectionSettings.Instance.DisableNotifications = True
  '        ConnectionSettings.Instance.Save()
  '        ConnectionSettings.Instance.Refresh()
  '        ' </Added by: Ernie at: 1/27/2015-9:46:23 AM on machine: ERNIE-THINK>
  '      End If

  '      'Dim lstrCurrentWorkSummary As String = JsonConvert.SerializeObject(Me.WorkSummary, New WorkSummaryFirebaseConverter())
  '      'lobjFirebase.Put("/workSummary", lstrCurrentWorkSummary)

  '    End If
  '  Catch ex As Exception
  '    ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '    ' Just log it and move on, we don't want this to cause a problem
  '  End Try
  'End Sub

  Friend Sub SyncItemsLocation()

    Try
      'Make sure the jobs and batches are in sync with this location object
      ItemsLocation = mobjItemsLocation

      For Each lobjJob In Me.Jobs
        lobjJob.ItemsLocation = mobjItemsLocation

        For Each lobjBatch As Batch In lobjJob.Batches
          lobjBatch.ItemsLocation = mobjItemsLocation
        Next

      Next

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
    End Try

  End Sub

#End Region

#Region "Constructors"

  Public Sub New()

    Try
      mstrId = Guid.NewGuid.ToString
      mdatCreateDate = Now
      'InitializeLogSession()
    Catch ex As Exception
      Throw
    End Try

  End Sub

  Public Sub New(ByVal lpName As String,
               ByVal lpDescription As String,
               ByVal lpItemsLocation As ItemsLocation,
               ByVal lpBatchSize As Integer,
               ByVal lpId As String,
               ByVal lpCreateDate As DateTime)
    Me.New(lpName, lpDescription, lpItemsLocation, lpBatchSize, lpId, lpCreateDate, 0, Nothing)
  End Sub

  ''' <summary>
  ''' This is used when populating from the Container, similar to deserialize
  ''' </summary>
  ''' <param name="lpName"></param>
  ''' <param name="lpDescription"></param>
  ''' <param name="lpItemsLocation"></param>
  ''' <param name="lpBatchSize"></param>
  ''' <param name="lpId"></param>
  ''' <remarks></remarks>
  Public Sub New(ByVal lpName As String,
                 ByVal lpDescription As String,
                 ByVal lpItemsLocation As ItemsLocation,
                 ByVal lpBatchSize As Integer,
                 ByVal lpId As String,
                 ByVal lpCreateDate As DateTime,
                 ByVal lpItemsProcessed As Long,
                 ByVal lpWorkSummary As IWorkSummary)

    Try
      'InitializeLogSession()
      mstrName = lpName
      mstrDescription = lpDescription
      mintBatchSize = lpBatchSize
      mstrId = lpId
      ItemsLocation = lpItemsLocation
      mdatCreateDate = lpCreateDate
      mlngItemsProcessed = lpItemsProcessed
      mobjWorkSummary = lpWorkSummary

      ' Make sure all the available project based operation extensions are registered.
      ProjectExtensionRegistrator.Instance.RegisterCurrentProjectExtensions()

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Sub

  Public Sub New(ByVal lpName As String,
                 ByVal lpDescription As String,
                 ByVal lpItemsLocation As ItemsLocation,
                 ByVal lpBatchSize As Integer)
    Me.New(lpName, lpDescription, lpItemsLocation, lpBatchSize, Guid.NewGuid.ToString, Now)
  End Sub

  Public Sub New(ByVal lpName As String,
                 ByVal lpDescription As String,
                 ByVal lpItemsLocation As ItemsLocation)
    Me.New(lpName, lpDescription, lpItemsLocation, DEFAULT_BATCH_SIZE, Guid.NewGuid.ToString, Now)
  End Sub

#End Region

#Region " IDisposable Support "

  Private disposedValue As Boolean     ' To detect redundant calls

  ' IDisposable
  Protected Overridable Sub Dispose(ByVal disposing As Boolean)

    Try
      If Not Me.disposedValue Then

        If disposing Then
          ' DISPOSETODO: free other state (managed objects).
          mobjHost = Nothing
          mstrName = Nothing
          mstrDescription = Nothing
          mstrId = Nothing
          mobjItemsLocation = Nothing
          If mobjContainer IsNot Nothing Then
            mobjContainer.Dispose()
          End If
          mobjJobIdentifiers = Nothing
          mobjProjectConnections = Nothing
          mobjArea = Nothing
          mobjWorkSummary = Nothing
          For Each lobjJob As Job In Me.Jobs
            lobjJob.Dispose()
          Next
          mobjJobs = Nothing
          mobjRepositories = Nothing
          'FinalizeLogSession()
        End If

        ' DISPOSETODO: free your own state (unmanaged objects).
        ' DISPOSETODO: set large fields to null.
      End If

      Me.disposedValue = True
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  ' This code added by Visual Basic to correctly implement the disposable pattern.
  Public Sub Dispose() _
         Implements IDisposable.Dispose
    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    Dispose(True)
    GC.SuppressFinalize(Me)
  End Sub

#End Region

#Region "IDataErrorInfo"

  Public ReadOnly Property [Error]() As String _
                  Implements System.ComponentModel.IDataErrorInfo.Error
    Get
      Return Nothing
    End Get
  End Property

  Default Public ReadOnly Property Item(ByVal columnName As String) As String _
                          Implements System.ComponentModel.IDataErrorInfo.Item
    Get

      Dim lobjResult As String = Nothing

      If columnName = "BatchSize" Then

        If (mintBatchSize <= 0) Then
          lobjResult = "Batch Size should not be less than or equal to zero"
        End If

      End If

      Return lobjResult
    End Get
  End Property

  'Public ReadOnly Property LogSession As Gurock.SmartInspect.Session Implements ILoggable.LogSession
  '  Get
  '    Try
  '      Return mobjLogSession
  '    Catch ex As Exception
  '      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
  '      ' Re-throw the exception to the caller
  '      Throw
  '    End Try
  '  End Get
  'End Property

#End Region

  Private Sub mobjJobs_CollectionChanged(sender As Object, e As Specialized.NotifyCollectionChangedEventArgs) Handles mobjJobs.CollectionChanged
    Try
      mobjJobIdentifiers = Nothing
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  'Public Sub InitializeLogSession() Implements ILoggable.InitializeLogSession
  '    Try
  '      mobjLogSession = ApplicationLogging.InitializeLogSession(Me.GetType.Name, System.Drawing.Color.Peru)
  '    Catch ex As Exception
  '      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
  '      ' Re-throw the exception to the caller
  '      Throw
  '    End Try
  'End Sub

  'Public Sub FinalizeLogSession() Implements ILoggable.FinalizeLogSession
  '  Try
  '    If mobjLogSession IsNot Nothing Then
  '      ApplicationLogging.FinalizeLogSession(mobjLogSession)
  '    End If
  '  Catch ex As Exception
  '    ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
  '    ' Re-throw the exception to the caller
  '    Throw
  '  End Try
  'End Sub

End Class
