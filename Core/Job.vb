'---------------------------------------------------------------------------------
' <copyright company="ECMG">
'     Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'     Copying or reuse without permission is strictly forbidden.
' </copyright>
'---------------------------------------------------------------------------------

#Region "Imports"

Imports System.ComponentModel
Imports System.Data
Imports System.IO
Imports System.Reflection
Imports System.Text
Imports System.Threading
Imports System.Xml.Serialization
Imports Documents
Imports Documents.Configuration
Imports Documents.Core
Imports Documents.Exceptions
Imports Documents.Providers
Imports Documents.Search
Imports Documents.SerializationUtilities
Imports Documents.Transformations
Imports Documents.Utilities
Imports Newtonsoft.Json
Imports Operations
Imports Projects.Configuration

#End Region

<Serializable()> <Xml.Serialization.XmlInclude(GetType(StoredSearch))>
<DebuggerDisplay("{DebuggerIdentifier(),nq}")>
Public Class Job
  Implements IDescription
  Implements INotifyPropertyChanged
  Implements IJobInfo
  Implements IDisposable

#Region "Class Constants"

  Public Const PROCESS_STATUS_ALL As String = "-- All --"
  Private Const JOB_MANAGER_WINDOW As String = "JobManagerWindow"

  ' Evaluation Support
  Private Const MAX_EVAL_CONCURRENT_TRANSACTIONS As Integer = 10
  Public Const MAX_EVAL_BATCH_SIZE As Integer = 100
  Public Const MAX_EVAL_JOB_SIZE As Integer = 200
  Public Const MAX_EVAL_TRANSACTIONS As Integer = 2000
  Public Const MAX_EVAL_TRANSACTIONS_EXCEEDED_MESSAGE As String = "The maximum allowed evaluation transactions of 2000 has been reached."
  Private Const MAX_CONCURRENT_BATCHES As Integer = 2

#End Region

#Region "Class Variables"

  Private Const DEFAULT_BATCH_SIZE As Integer = 1
  Private mstrId As String = String.Empty
  Private mintIndex As Integer
  Private mobjBatches As New Batches(Me)
  ' Private mobjBatchLocks As New BatchLocks
  Private mstrDescription As String = String.Empty
  Private mstrName As String = String.Empty
  Private WithEvents MobjJobSource As New JobSource()
  Private mintCurrentWorkingBatch As Integer = 0
  Private mintCurrentBatchCount As Integer = 0
  Private mintTotalBatchCount As Integer = 0
  Private mintRunningCount As Integer = 0
  Private mstrCurrentSourceConnectionString As String = String.Empty
  Private mstrPreviousSourceConnectionString As String = String.Empty
  Private mobjProject As Project 'This is a reference to this Job's parent project
  Private mobjRelationships As New JobRelationships
  Private mobjContainer As Container
  Private mobjSourceRepository As Repository
  Private mobjDestinationRepository As Repository
  Private mblnIsCancelled As Boolean = False
  Private mblnIsCompleted As Boolean = False
  'Private mblnIsRunning As Boolean = False
  Private mstrCancellationReason As String = String.Empty
  Private mobjTag As Object = Nothing
  Private mintBatchThreadsRunning As Integer = 0
  Private mintMaxBatchThreads As Integer = 1
  Private mobjResetEvent As New ManualResetEvent(False)
  Private WithEvents mobjConfiguration As New JobConfiguration
  Private mintRunBeforeJobBeginCount As Integer
  Private mintCurrentItemsProcessed As Int64
  Private mlngLastItemsProcessed As Long
  Private mobjLastWorkSummary As IWorkSummary = Nothing
  Private mdatCreateDate As DateTime
  ' For automatically updating the status of this job
  Private mobjAutoEvent As AutoResetEvent
  'Private WithEvents mobjStatusChecker As NodeStatusChecker
  Private mobjTimerCallback As TimerCallback
  Private stateTimer As Timer

  'Private mobjLogSession As Gurock.SmartInspect.Session

#End Region

#Region "Event Delegates"

  Public Delegate Sub BatchCreatedEventHandler(ByVal sender As Object, ByRef e As WorkEventArgs)
  Public Delegate Sub BatchCreationUpdateEventHandler(ByVal sender As Object, ByRef e As BatchItemCreatedEventArgs)
  ''' <summary>Delegate event handler for the JobCancelled event.</summary>
  Public Delegate Sub JobCancelledEventHandler(ByVal sender As Object, ByRef e As WorkCancelledEventArgs)
  Public Delegate Sub JobEventHandler(ByVal sender As Object, ByRef e As WorkEventArgs)
  Public Delegate Sub JobErrorEventHandler(ByVal sender As Object, ByRef e As WorkErrorEventArgs)
  Public Delegate Sub JobInvokedEventHandler(ByVal sender As Object, ByRef e As WorkInvokedEventArgs)
  Public Delegate Sub WorkSummaryUpdatedEventHandler(ByVal sender As Object, ByRef e As WorkSummaryEventArgs)

#End Region

#Region "Public Events"

  Public Event BatchCreated As BatchCreatedEventHandler
  Public Event BatchCreationUpdate As BatchCreationUpdateEventHandler
  'Public Event BatchItemsCreated(ByVal lpJobName As String, ByVal lpCurrentCount As Integer, ByVal lpTotalCount As Integer)
  Public Event BatchItemsCreated As BatchCreationUpdateEventHandler
  Public Event BatchItemsCompleted(ByVal lpJobName)
  Public Event JobCancelled As JobCancelledEventHandler

  Public Event BeforeJobBegin As JobEventHandler
  Public Event AfterJobComplete As JobEventHandler
  Public Event JobError As JobErrorEventHandler
  Public Event JobInvoked As JobInvokedEventHandler
  Public Event WorkSummaryUpdated As WorkSummaryUpdatedEventHandler

#End Region

#Region "Public Properties"

  <XmlAttribute()>
  Public Property Id() As String
    Get
      Return mstrId
    End Get
    Set(ByVal value As String)

      If (Helper.IsDeserializationBasedCall = True) Then
        mstrId = value
      ElseIf Helper.CallStackContainsMethodName("SaveJob") Then
        mstrId = value
      Else
        Throw New InvalidOperationException("Although Job Id is a public property, set operations are not allowed.  Treat property as read-only.")
      End If

    End Set
  End Property

  <XmlIgnore(), JsonIgnore()>
  Public Property Index As Integer
    Get
      Try
        Return mintIndex
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Friend Set(value As Integer)
      Try
        mintIndex = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public ReadOnly Property BatchLocks As IBatchLocks
    Get
      Try
        Return GetBatchLocks()
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        If Me.Project IsNot Nothing Then
          Return New BatchLocks(Me.Project)
        Else
          Return New BatchLocks
        End If
      End Try
    End Get
  End Property

  Public ReadOnly Property BatchThreadsRunning As Integer Implements IJobInfo.BatchThreadsRunning
    Get
      Try
        Return mintBatchThreadsRunning
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  <XmlAttribute()>
  Public Property Description() As String _
                          Implements IDescription.Description
    Get
      ' Return mstrDescription
      Return Configuration.Description
    End Get
    Set(ByVal value As String)
      ' mstrDescription = value
      Configuration.Description = value
    End Set
  End Property

  <XmlAttribute()>
  Public Property Name() As String _
                          Implements IDescription.Name, INamedItem.Name
    Get
      ' Return mstrName
      Return Configuration.Name
    End Get
    Set(ByVal value As String)
      ' mstrName = value
      Configuration.Name = value
    End Set
  End Property

  Public Property DisplayName As String
    Get
      Try
        Return Configuration.DisplayName
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As String)
      Try
        Configuration.DisplayName = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public ReadOnly Property Configuration As JobConfiguration
    Get
      Try
        'If mobjConfiguration Is Nothing Then
        '  ' mobjConfiguration = GetConfiguration()
        'End If
        'If mobjConfiguration IsNot Nothing Then
        '  mobjConfiguration.NotificationConfiguration.Parent = Me
        'End If
        Return mobjConfiguration
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public Property Batches() As Batches
    Get
      Return mobjBatches
    End Get
    Set(ByVal value As Batches)

      If (Helper.IsDeserializationBasedCall = True) Then
        mobjBatches = value

      Else
        Throw New InvalidOperationException("Although MigrationJob Batches() is a public property, set operations are not allowed.  Treat property as read-only. Call CreateBatches() method to set Batches.")
      End If

    End Set
  End Property

  <XmlIgnore()>
  Public Property RunBeforeJobBeginCount As Integer
    Get
      Try
        Return mintRunBeforeJobBeginCount
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As Integer)
      Try
        mintRunBeforeJobBeginCount = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public Property Source() As JobSource
    Get
      Return Configuration.Source
    End Get
    Set(ByVal value As JobSource)
      Try
        Configuration.Source = value
        OnPropertyChanged("Source")
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public Property ItemsLocation() As ItemsLocation
    Get
      Return Configuration.ItemsLocation
    End Get
    Set(ByVal value As ItemsLocation)

      Try
        Configuration.ItemsLocation = value

        mobjContainer = Container.CreateContainer(Configuration.ItemsLocation)

      Catch ex As Exception
        ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try

    End Set
  End Property

  Public Property BatchSize() As Integer
    Get
      Try
        Return Configuration.BatchSize
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(ByVal value As Integer)
      Try
        'If LicenseRegister.IsEvaluation Then
        '  ' If this is an evaluation then only allow the batch size to be set up to the evaluation limit.
        '  If value < Job.MAX_EVAL_BATCH_SIZE Then
        '    Configuration.BatchSize = value
        '  Else
        '    Configuration.BatchSize = Job.MAX_EVAL_BATCH_SIZE
        '  End If
        'Else
        '  Configuration.BatchSize = value
        'End If

        Configuration.BatchSize = value

      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public Property Operation() As String
    Get
      Try
        Return Configuration.OperationName
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(ByVal value As String)
      Try
        Configuration.OperationName = value
        OnPropertyChanged("Operation")
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public ReadOnly Property OperationInfo As String Implements IJobInfo.Operation
    Get
      Try
        Return Operation
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property
  Public Property Process As IProcess
    Get
      Try
        Return Configuration.Process
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As IProcess)
      Try
        Configuration.Process = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public Property DestinationConnectionString() As String
    Get
      Try
        Return Configuration.DestinationConnectionString
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(ByVal value As String)
      Try
        Configuration.DestinationConnectionString = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public ReadOnly Property SourceConnectionString As String
    Get
      Try
        If String.IsNullOrEmpty(mstrCurrentSourceConnectionString) Then
          If Configuration IsNot Nothing AndAlso Configuration.Source IsNot Nothing Then
            mstrCurrentSourceConnectionString = Configuration.Source.SourceConnectionString
          End If
        End If
        Return mstrCurrentSourceConnectionString
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property TotalBatchCount As Integer
    Get
      Try
        Return mintTotalBatchCount
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public Property ContentStorageType() As Content.StorageTypeEnum
    Get
      Try
        Return Configuration.ContentStorageType
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(ByVal value As Content.StorageTypeEnum)
      Try
        Configuration.ContentStorageType = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public ReadOnly Property BatchCount As Integer
    Get
      Try
        Return Batches.Count
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public Property DeclareAsRecordOnImport() As Boolean
    Get
      Try
        Return Configuration.DeclareAsRecordOnImport
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(ByVal value As Boolean)
      Try
        Configuration.DeclareAsRecordOnImport = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public Property DeclareRecordConfiguration() As DeclareRecordConfiguration
    Get
      Try
        Return Configuration.DeclareRecordConfiguration
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(ByVal value As DeclareRecordConfiguration)
      Try
        Configuration.DeclareRecordConfiguration = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public Property Transformations() As TransformationCollection
    Get
      Try
        Return Configuration.Transformations
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(ByVal value As TransformationCollection)
      Try
        Configuration.Transformations = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public Property TransformationSourcePath As String
    Get
      Try
        Return Configuration.TransformationSourcePath
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(ByVal value As String)
      Try
        Configuration.TransformationSourcePath = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public Property DocumentFilingMode() As FilingMode
    Get
      Try
        Return Configuration.DocumentFilingMode
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(ByVal value As FilingMode)
      Try
        Configuration.DocumentFilingMode = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public Property LeadingDelimiter() As Boolean
    Get
      Try
        Return Configuration.LeadingDelimiter
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(ByVal value As Boolean)
      Try
        Configuration.LeadingDelimiter = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public Property BasePathLocation() As Migrations.ePathLocation
    Get
      Try
        Return Configuration.BasePathLocation
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(ByVal value As Migrations.ePathLocation)
      Try
        Configuration.BasePathLocation = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public Property FolderDelimiter() As String
    Get
      Try
        Return Configuration.FolderDelimiter
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(ByVal value As String)
      Try
        Configuration.FolderDelimiter = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public Property SourceRepository As Repository
    Get
      Try
        Return mobjSourceRepository
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Friend Set(ByVal value As Repository)
      Try
        mobjSourceRepository = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public ReadOnly Property DestinationRepository As Repository
    Get
      Try
        Return mobjDestinationRepository
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property IsCancelled As Boolean Implements IJobInfo.IsCancelled
    Get
      Try
        Return mblnIsCancelled
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property IsCompleted As Boolean Implements IJobInfo.IsCompleted
    Get
      Try
        Return mblnIsCompleted
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property IsRunning As Boolean Implements IJobInfo.IsRunning
    Get
      Try
        'Return mblnIsRunning
        If mintBatchThreadsRunning > 0 Then
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

  Friend Sub ResetCompletionFlag()
    Try
      mblnIsCompleted = False
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  'Friend Sub ResetRunningFlag()
  '  Try
  '    mblnIsRunning = False
  '  Catch ex As Exception
  '    ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '    ' Re-throw the exception to the caller
  '    Throw
  '  End Try
  'End Sub

  Public ReadOnly Property CancellationReason As String Implements IJobInfo.CancellationReason
    Get
      Try
        Return mstrCancellationReason
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property Project() As Project
    Get
      Try
        Return mobjProject
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public Property Tag As Object
    Get
      Try
        Return mobjTag
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As Object)
      Try
        mobjTag = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  'Public Property NotificationConfiguration As Notifications.NotificationConfiguration
  '  Get
  '    Try
  '      Return Configuration.NotificationConfiguration
  '    Catch Ex As Exception
  '      ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
  '      ' Re-throw the exception to the caller
  '      Throw
  '    End Try
  '  End Get
  '  Set(value As Notifications.NotificationConfiguration)
  '    Try
  '      Configuration.NotificationConfiguration = value
  '    Catch Ex As Exception
  '      ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
  '      ' Re-throw the exception to the caller
  '      Throw
  '    End Try
  '  End Set
  'End Property

#End Region

#Region "IJobInfo Implementation"

  Public ReadOnly Property BatchSizeInfo As Integer Implements IJobInfo.BatchSize
    Get
      Try
        Return Me.BatchSize
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property CreateDate As DateTime Implements IJobInfo.CreateDate
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

  Public ReadOnly Property DescriptionInfo As String Implements IJobInfo.Description
    Get
      Try
        Return Me.Description
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property DisplayNameInfo As String Implements IJobInfo.DisplayName
    Get
      Try
        Return Me.DisplayName
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property IdInfo As String Implements IJobInfo.Id
    Get
      Try
        Return Me.Id
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property IsInitialized As Boolean Implements IJobInfo.IsInitialized
    Get
      Try
        If Batches.Count > 0 Then
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

  Public ReadOnly Property HasOperationsToRunBeforeJobBegin As Boolean
    Get
      Try
        Return GetHasOperationsToRunBeforeJobBegin()
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property NameInfo As String Implements IJobInfo.Name
    Get
      Try
        Return Me.Name
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property ProjectName As String Implements IJobInfo.ProjectName
    Get
      Try
        If Me.Project IsNot Nothing Then
          Return Me.Project.Name
        Else
          Return String.Empty
        End If
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property ProcessInfo As IProcess Implements IJobInfo.Process
    Get
      Try
        Return Me.Process
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property Relationships As JobRelationships
    Get
      Try
        Return mobjRelationships
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property WorkSummary As IWorkSummary Implements IJobInfo.WorkSummary
    Get
      Try
        Return mobjLastWorkSummary
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property ItemsProcessed As Long Implements IJobInfo.ItemsProcessed
    Get
      Try
        ' If this value is not yet initialized we will try to initialize it from the work summary, if present.
        If mlngLastItemsProcessed = 0 AndAlso WorkSummary IsNot Nothing Then
          mlngLastItemsProcessed = WorkSummary.ProcessedCount
        End If
        Return mlngLastItemsProcessed
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public Function ToJson() As String Implements IJobInfo.ToJson
    Try
      If Helper.IsRunningInstalled Then
        Return JsonConvert.SerializeObject(Me, Formatting.None)
      Else
        Return JsonConvert.SerializeObject(Me, Formatting.Indented)
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

#End Region

#Region "Friend Properties"

  Friend Property CurrentItemsProcessed As Int64
    Get
      Try
        Return mintCurrentItemsProcessed
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As Int64)
      Try
        mintCurrentItemsProcessed = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  'Friend ReadOnly Property LogSession As Gurock.SmartInspect.Session
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

#Region "Private Properties"

  Friend ReadOnly Property JobBatchContainer As Container
    Get

      Try

        If (mobjContainer Is Nothing) Then
          ' mobjContainer = Container.CreateContainer(mobjItemsLocation)
          mobjContainer = Container.CreateContainer(Me.ItemsLocation)
        End If

        Return mobjContainer

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

    Try
      mstrId = Guid.NewGuid.ToString
      'InitializeLogSession()

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Sub

  Public Sub New(ByVal lpName As String,
                 ByVal lpDescription As String,
                 ByVal lpOperationType As String)

    Try
      ' Id = Guid.NewGuid.ToString
      Name = lpName
      Description = lpDescription
      Operation = lpOperationType
      'InitializeLogSession()

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Sub

  Public Sub New(ByVal lpName As String,
                 ByVal lpDescription As String,
                 ByVal lpOperationType As String,
                 ByVal lpBatchSize As Integer)
    Me.New(lpName, lpDescription, lpOperationType)

    Try
      BatchSize = lpBatchSize

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Sub

  Public Sub New(lpConfiguration As JobConfiguration)
    ' Me.New(Guid.NewGuid.ToString, lpConfiguration)
    Me.New(String.Empty, lpConfiguration)
  End Sub

  Public Sub New(lpJobId As String, lpConfiguration As JobConfiguration)
    Try

#If NET8_0_OR_GREATER Then
      ArgumentNullException.ThrowIfNull(lpConfiguration)
#Else
      If lpConfiguration Is Nothing Then
        Throw New ArgumentNullException(NameOf(lpConfiguration))
      End If
#End If

      mstrId = lpJobId
      mobjConfiguration = lpConfiguration
      ' SyncronizeWithConfiguration()
      'InitializeLogSession()

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Sub New(lpJobId As String, lpCreateDate As DateTime, lpConfiguration As JobConfiguration, lpItemsProcessed As Long, lpWorkSummary As IWorkSummary)
    Try

#If NET8_0_OR_GREATER Then
      ArgumentNullException.ThrowIfNull(lpConfiguration)
#Else
      If lpConfiguration Is Nothing Then
        Throw New ArgumentNullException(NameOf(lpConfiguration))
      End If
#End If

      mstrId = lpJobId
      mdatCreateDate = lpCreateDate
      mobjConfiguration = lpConfiguration
      mlngLastItemsProcessed = lpItemsProcessed
      'InitializeLogSession()
      SetLastWorkSummary(lpWorkSummary)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  ''' <summary>
  ''' This is used when populating from the Container, similar to deserialize
  ''' </summary>
  ''' <param name="lpId"></param>
  ''' <param name="lpName"></param>
  ''' <param name="lpDescription"></param>
  ''' <param name="lpOperationType"></param>
  ''' <param name="lpBatchSize"></param>
  ''' <param name="lpJobSource"></param>
  ''' <param name="lpItemsLocation"></param>
  ''' <param name="lpDestinationConnectionString"></param>
  ''' <param name="lpContentStorageType"></param>
  ''' <param name="lpDeclareAsRecordOnImport"></param>
  ''' <param name="lpDeclareRecordConfiguration"></param>
  ''' <param name="lpTransformations"></param>
  ''' <param name="lpDocumentFilingMode"></param>
  ''' <param name="lpLeadingDelimiter"></param>
  ''' <param name="lpBasePathLocation"></param>
  ''' <param name="lpFolderDelimiter"></param>
  ''' <remarks></remarks>
  Public Sub New(ByVal lpId As String,
                 ByVal lpName As String,
                 ByVal lpDescription As String,
                 ByVal lpOperationType As String,
                 ByVal lpCreateDate As DateTime,
                 ByVal lpProcess As Process,
                 ByVal lpBatchSize As Integer,
                 ByVal lpJobSource As JobSource,
                 ByVal lpItemsLocation As ItemsLocation,
                 ByVal lpDestinationConnectionString As String,
                 ByVal lpContentStorageType As Content.StorageTypeEnum,
                 ByVal lpDeclareAsRecordOnImport As Boolean,
                 ByVal lpDeclareRecordConfiguration As DeclareRecordConfiguration,
                 ByVal lpTransformations As TransformationCollection,
                 ByVal lpDocumentFilingMode As FilingMode,
                 ByVal lpLeadingDelimiter As Boolean,
                 ByVal lpBasePathLocation As Migrations.ePathLocation,
                 ByVal lpFolderDelimiter As String,
                 ByVal lpTransformationSourcePath As String)

    Try
      mstrId = lpId
      Name = lpName
      Description = lpDescription
      Operation = lpOperationType
      mdatCreateDate = lpCreateDate
      Process = lpProcess
      BatchSize = lpBatchSize
      Source = lpJobSource
      ItemsLocation = lpItemsLocation
      DestinationConnectionString = lpDestinationConnectionString
      ContentStorageType = lpContentStorageType
      DeclareAsRecordOnImport = lpDeclareAsRecordOnImport
      DeclareRecordConfiguration = lpDeclareRecordConfiguration
      Transformations = lpTransformations
      DocumentFilingMode = lpDocumentFilingMode
      LeadingDelimiter = lpLeadingDelimiter
      BasePathLocation = lpBasePathLocation
      FolderDelimiter = lpFolderDelimiter
      TransformationSourcePath = lpTransformationSourcePath

      If (String.IsNullOrEmpty(SourceConnectionString)) AndAlso (Source IsNot Nothing) Then
        mstrCurrentSourceConnectionString = Source.SourceConnectionString
      End If
      'InitializeLogSession()

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Sub

  ' <Added by: Ernie at: 2/21/2013-1:03:25 PM on machine: ERNIE-THINK>
  ''' <summary>
  '''     Creates a new job using the specified parameters.  Originally defined for use by the CreateJobOperation
  ''' </summary>
  ''' <param name="lpProject" type="Ecmg.Cts.Projects.Project">
  '''     <para>
  '''         The project to which this job is associated.
  '''     </para>
  ''' </param>
  ''' <param name="lpName" type="String">
  '''     <para>
  '''         The name of the job.
  '''     </para>
  ''' </param>
  ''' <param name="lpDisplayName" type="String">
  '''     <para>
  '''         The display name of the job.
  '''     </para>
  ''' </param>
  ''' <param name="lpDescription" type="String">
  '''     <para>
  '''        The description of the job. 
  '''     </para>
  ''' </param>
  ''' <param name="lpSourceContentSourceName" type="String">
  '''     <para>
  '''         The name of the source content source.
  '''     </para>
  ''' </param>
  ''' <param name="lpDestinationContentSourceName" type="String">
  '''     <para>
  '''         The name of the destination content source.
  '''     </para>
  ''' </param>
  ''' <param name="lpProcess" type="Operations.IProcess">
  '''     <para>
  '''         The process for the job.
  '''     </para>
  ''' </param>
  Public Sub New(ByVal lpProject As Project,
                 ByVal lpName As String,
                 ByVal lpDisplayName As String,
                 ByVal lpDescription As String,
                 ByVal lpSourceContentSourceName As String,
                 ByVal lpDestinationContentSourceName As String,
                 ByVal lpProcess As IProcess)
    Try
      mstrId = Guid.NewGuid.ToString
      ItemsLocation = lpProject.ItemsLocation
      Name = lpName
      DisplayName = lpDisplayName
      Description = lpDescription
      Source.Type = enumSourceType.Empty
      'InitializeLogSession()

      Dim lstrSourceContentSourceConnectionString As String = ConnectionSettings.Instance.GetConnectionString(lpSourceContentSourceName)
      Source.SourceConnectionString = lstrSourceContentSourceConnectionString

      ' Add the destination content source if specified.
      If Not String.IsNullOrEmpty(lpDestinationContentSourceName) Then
        Dim lstrDestinationContentSourceConnectionString As String = ConnectionSettings.Instance.GetConnectionString(lpDestinationContentSourceName)
        DestinationConnectionString = lstrDestinationContentSourceConnectionString
      End If

      BatchSize = lpProject.BatchSize
      Operation = lpProcess.Name
      Configuration.Process = lpProcess
      mobjProject = lpProject

      Save()

      SaveRepositories()

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub
  ' </Added by: Ernie at: 2/21/2013-1:03:25 PM on machine: ERNIE-THINK>

#End Region

#Region "Public Methods"

  Public Function GetAllProcessedByNames(Optional ByVal lpIncludeBlankAsFirst As Boolean = False) As List(Of String)
    Try
      Dim lobjProcessedByNames As New List(Of String)

      If lpIncludeBlankAsFirst Then
        lobjProcessedByNames.Add(String.Empty)
      End If
      lobjProcessedByNames.AddRange(JobBatchContainer.GetAllProcessedByNodeNames(Me))

      Return lobjProcessedByNames

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Function GetFilteredItems(lpFilter As ItemFilter) As DataTable
    Try
      Return JobBatchContainer.GetFilteredItemsToDataTable(lpFilter)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Async Sub RefreshRepositoriesFromProjectAsync()
    Try

      ' This currently only gets the source repository.  We probably want to get the destination as well if available.
      Dim lstrRepositoryName As String = ContentSource.GetNameFromConnectionString(Source.SourceConnectionString)
      Dim lobjRepository As Repository
      Dim lobjTask As Task = Task.Factory.StartNew(
                  Sub()
                    If Project.Repositories.Count = 0 Then
                      Project.Repositories.AddRange(Project.Container.GetRepositories(Project))
                      lobjRepository = Project.Repositories.ItemByName(lstrRepositoryName)
                    Else
                      lobjRepository = Project.Repositories.ItemByName(lstrRepositoryName)
                    End If

                    If lobjRepository IsNot Nothing Then
                      SourceRepository = lobjRepository
                    End If
                  End Sub)

      Await lobjTask

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Sub SaveRepositories()
    Try

      ' We used to always save (BLOB) the repositories each time this method was called.
      ' It was becoming a performance issue so now we will only save them if they are not 
      ' already in the tblRepository table for the project.

      ' The risk is that they may become stale as changes are made to the repository.
      ' If that becomes a problem we will need to add a way for them to get refreshed.

      ' Ernie Bahr October 8th, 2015

      If Me.SourceRepository Is Nothing AndAlso Not String.IsNullOrEmpty(Source.SourceConnectionString) Then
        mobjSourceRepository = JobBatchContainer.GetRepositoryByConnectionString(Source.SourceConnectionString)
        If mobjSourceRepository Is Nothing Then
          Dim lobjSourceContentSource As New ContentSource(Source.SourceConnectionString)
          mobjSourceRepository = New Repository(lobjSourceContentSource)
          JobBatchContainer.SaveRepository(Me.SourceRepository, Me, ExportScope.Source)
        End If
      End If

      If Not String.IsNullOrEmpty(Me.DestinationConnectionString) Then
        If Me.DestinationRepository Is Nothing Then
          mobjDestinationRepository = JobBatchContainer.GetRepositoryByConnectionString(Me.DestinationConnectionString)
          If mobjDestinationRepository Is Nothing Then
            Dim lobjDestinationContentSource As New ContentSource(Me.DestinationConnectionString)
            mobjDestinationRepository = New Repository(lobjDestinationContentSource)
            JobBatchContainer.SaveRepository(Me.DestinationRepository, Me, ExportScope.Destination)
          End If
        End If
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  'Public Function GetConfiguration() As JobConfiguration
  '  Try
  '    Return New JobConfiguration(Me)
  '  Catch Ex As Exception
  '    ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
  '    ' Re-throw the exception to the caller
  '    Throw
  '  End Try
  'End Function

  Public Sub UnlockBatch(lpBatchId As String)
    Try
      SaveBatchItemsProcessed(lpBatchId)
      JobBatchContainer.UnLockBatch(lpBatchId, String.Empty)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Sub UnlockBatches()
    Try
      Dim lobjBatchLocks As IBatchLocks = Me.BatchLocks
      For Each lobjBatchLock As IBatchLock In lobjBatchLocks
        If lobjBatchLock.JobId = Me.Id Then
          JobBatchContainer.UnLockBatch(lobjBatchLock.BatchId, Environment.MachineName)
        End If
      Next
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Sub CancelJob(ByVal lpReason As String)
    Try
      'Dim lobjCancelEventArgs = New DoWorkEventArgs(Me)
      'lobjCancelEventArgs.Cancel = True

      'CancelJob(lpReason, lobjCancelEventArgs)
      CancelJob(lpReason, Nothing)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Sub CancelJob(ByVal lpReason As String, ByVal lpBackgroundWorkerEventArgs As DoWorkEventArgs)
    Try
      If lpBackgroundWorkerEventArgs IsNot Nothing Then
        CancelBatches(Me.Batches, lpBackgroundWorkerEventArgs)
      Else
        CancelBatches(New Batches, lpBackgroundWorkerEventArgs)
      End If

      mblnIsCancelled = True
      mstrCancellationReason = lpReason
      ApplicationLogging.WriteLogEntry(String.Format("Job '{0}' has been cancelled: {1}",
                                          Me.Name, lpReason), TraceEventType.Warning, 61239)

      RefreshStatisticsAndPushToFirebaseAsync()
      RaiseEvent JobCancelled(Me, New WorkCancelledEventArgs(Me))
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Sub ResetCancellation()
    Try
      ResetCancellation(Nothing)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Sub ResetCancellation(ByVal lpBackgroundWorkerEventArgs As DoWorkEventArgs)
    Try
      mblnIsCancelled = False
      If lpBackgroundWorkerEventArgs IsNot Nothing Then
        lpBackgroundWorkerEventArgs.Cancel = False
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Function ResetFailedItemsByProcessedMessage(lpProcessedMessage As String) As Integer
    Try
      'LogSession.EnterMethod(Helper.GetMethodIdentifier(MethodBase.GetCurrentMethod()))
      Return JobBatchContainer.ResetFailedItemsByProcessedMessage(Me, lpProcessedMessage)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    Finally
      'LogSession.LeaveMethod(Helper.GetMethodIdentifier(MethodBase.GetCurrentMethod()))
    End Try
  End Function

  Public Sub ResetItemsToNotProcessed(lpCurrentStatus As ProcessedStatus)
    Try
      ResetCompletionFlag()
      JobBatchContainer.ResetItemsToNotProcessed(Me, lpCurrentStatus)
      GetWorkSummaryCounts()
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  ''' <summary>
  ''' Creates a set of batches
  ''' </summary>
  ''' <remarks>
  ''' Batch creation is based on two factors:
  ''' 1) The batch size and total number of documents
  ''' 2) The source connection strings
  ''' The goal is to create a batch that contains all the same source connection strings and
  ''' at the same time doesn't get larger than the batch size.
  '''</remarks>
  Public Sub CreateBatches(Optional ByVal lpBackgroundWorker As BackgroundWorker = Nothing,
                           Optional ByVal lpBackgroundWorkerEventArgs As DoWorkEventArgs = Nothing)

    Try

      'LogSession.EnterMethod(Helper.GetMethodIdentifier(MethodBase.GetCurrentMethod()))

      Dim lstrName As String = String.Empty

      Dim lobjSourceData As DataTable = Nothing

      If (Source Is Nothing) Then
        Throw New Exception(String.Format("You must define a 'Source' for this job '{0}'.", mstrName))
      End If

      'Save off the Batches so we can delete these old ones at the end of this method
      Dim lobjOldSavedBatches As New Batches(Me)

      For Each lobjBatch As Batch In Batches
        lobjOldSavedBatches.Add(lobjBatch)
      Next

      'Reset incase we call multiple times
      If (Batches.Count > 0) Then
        Batches.Clear()
      End If

      mintCurrentWorkingBatch = 0
      mintCurrentBatchCount = 0
      mintTotalBatchCount = 0
      mintRunningCount = 0

      Select Case Source.Type

        'Case enumSourceType.Search
        '  'CreateSearchBatches(lobjOldSavedBatches, lpBackgroundWorker, lpBackgroundWorkerEventArgs)
        '  lobjSourceData = GetSearchBatchSource(lobjOldSavedBatches, lpBackgroundWorker, lpBackgroundWorkerEventArgs)

        Case enumSourceType.Folder
          CreateFolderBatches(lobjOldSavedBatches, lpBackgroundWorker, lpBackgroundWorkerEventArgs)

        Case enumSourceType.List
          'CreateListBatches(lobjOldSavedBatches, lpBackgroundWorker, lpBackgroundWorkerEventArgs)
          Select Case Source.ListType
            Case enumListType.TextFile
              CreateListBatches(lobjOldSavedBatches, lpBackgroundWorker, lpBackgroundWorkerEventArgs)
              'Commit the last batch
              GetCurrentWorkingBatch().CommitBatchItems()

              'If we created the Batches successfully then save out the 
              ' new batches and job/batch relationships to the container.
              Save()

              'Delete the old batches if there were ones
              If (lobjOldSavedBatches.Count > 0) Then
                JobBatchContainer.DeleteBatches(lobjOldSavedBatches)
              End If

              ' Clear out any existing work summary
              JobBatchContainer.ClearWorkSummary(Me)

              Exit Sub

            Case enumListType.ExcelFile
              lobjSourceData = GetListBatchSource(lobjOldSavedBatches, lpBackgroundWorker, lpBackgroundWorkerEventArgs)

            Case enumListType.DBLookup
              'Delete the old batches if there were ones
              If (lobjOldSavedBatches.Count > 0) Then
                JobBatchContainer.DeleteBatches(lobjOldSavedBatches)
              End If
              'CreateDBListBatches(Source.SourceDBListConnectionString, Source.SourceSQLStatement)
              Dim lobjArgs As IDBLookupSourceEventArgs
              If Source.Type = enumSourceType.OtherJob Then
                lobjArgs = New CreateBatchesFromOtherJobSourceEventArgs(Source, lobjOldSavedBatches, lpBackgroundWorker, lpBackgroundWorkerEventArgs)
              Else
                lobjArgs = New ListDBLookupSourceEventArgs(Me, Source.SourceSQLStatement, Source.SourceDBListConnectionString)
              End If
              JobBatchContainer.CreateListBatches(lobjArgs)

              ' Clear out any existing work summary
              JobBatchContainer.ClearWorkSummary(Me)

              Exit Sub

          End Select
        Case enumSourceType.OtherJob
          Dim lobjOtherJobArgs As New CreateBatchesFromOtherJobSourceEventArgs(Me,
                                                                               lobjOldSavedBatches,
                                                                               lpBackgroundWorker,
                                                                               lpBackgroundWorkerEventArgs)
          'Delete the old batches if there were ones
          If (lobjOldSavedBatches.Count > 0) Then
            JobBatchContainer.DeleteBatches(lobjOldSavedBatches)
          End If
          CreateOtherJobBatches(lobjOtherJobArgs)

        Case enumSourceType.Empty
          ' We are not going to create any batches in this case, just delete the old ones.

        Case Else
          Throw New Exception(String.Format("{0}: Unknown BatchSourceType '{1}'", Reflection.MethodBase.GetCurrentMethod, Source.Type))
      End Select

      If (lpBackgroundWorker IsNot Nothing) AndAlso (lpBackgroundWorker.CancellationPending) Then
        CancelBatches(lobjOldSavedBatches, lpBackgroundWorkerEventArgs)
        Exit Sub
      End If

      If (Not Source.Type = enumSourceType.Folder) AndAlso lobjSourceData IsNot Nothing Then
        CreateBatchesFromDataTable(lobjSourceData, lobjOldSavedBatches, lpBackgroundWorker, lpBackgroundWorkerEventArgs)
      End If

      'Commit the last batch
      GetCurrentWorkingBatch().CommitBatchItems()

      CurrentItemsProcessed = 0

      'If we created the Batches successfully then save out the 
      ' new batches and job/batch relationships to the container.
      Save()

      'Delete the old batches if there were ones
      If (lobjOldSavedBatches.Count > 0) Then
        JobBatchContainer.DeleteBatches(lobjOldSavedBatches)
      End If

      ' Clear out any existing work summary
      JobBatchContainer.ClearWorkSummary(Me)

      ' Update the work summary for the job and push the results
      RefreshStatisticsAndPushToFirebaseAsync()

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw

    Finally

      'If (mobjBatches.Count > 0) Then
      '  mobjBatches(mintCurrentWorkingBatch).Dispose()
      'End If

      RaiseEvent BatchItemsCompleted(Me.Name)
      'LogSession.LeaveMethod(Helper.GetMethodIdentifier(MethodBase.GetCurrentMethod()))
    End Try

  End Sub

  Private Sub CreateFolderBatches(ByRef lpOldSavedBatches As Batches,
                                  Optional ByRef lpBackgroundWorker As BackgroundWorker = Nothing,
                                  Optional ByRef lpBackgroundWorkerEventArgs As DoWorkEventArgs = Nothing)

    Try

      If (Source.FolderPath Is Nothing) Then
        Throw New Exception(String.Format("{0}: FolderPath cannot be nothing", Reflection.MethodBase.GetCurrentMethod))

      Else

        mstrCurrentSourceConnectionString = Source.SourceConnectionString

        Dim lobjFolder As IFolder = GetFolderByID(Source.FolderPath, Source.SourceConnectionString)

        If (lobjFolder Is Nothing OrElse (lobjFolder.Contents.Count = 0) AndAlso (lobjFolder.HasSubFolders = False)) Then
          Throw New Exception(String.Format("{0}: Folder ({1}) returned no results, no items to add to the batch.", Reflection.MethodBase.GetCurrentMethod, Source.FolderPath))
        End If

        Dim lblnCancel As Boolean = False
        CountFiles(lobjFolder, lpBackgroundWorker, lpBackgroundWorkerEventArgs, lblnCancel)

        If (lblnCancel) Then
          Exit Sub
        End If

        CreateBatches(lobjFolder, lpBackgroundWorker, lpBackgroundWorkerEventArgs, lpOldSavedBatches, lblnCancel)

        If (lblnCancel) Then
          Exit Sub
        End If

      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Sub

  Private Sub CreateOtherJobBatches(lpArgs As CreateBatchesFromOtherJobSourceEventArgs)
    Try
      ' CreateDBListBatches(Source.SourceDBListConnectionString, Source.SourceSQLStatement)
      JobBatchContainer.CreateListBatches(lpArgs)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Friend Sub CreateBatchesFromDataTable(ByVal lpSourceData As DataTable)
    Try
      CreateBatchesFromDataTable(lpSourceData, Nothing, Nothing, Nothing)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Friend Sub Cleanup()
    Try
      For Each lobjBatch As Batch In Me.Batches
        lobjBatch.CleanUp()
      Next
      Me.Process.Reset()
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Sub CreateBatchesFromDataTable(ByVal lpSourceData As DataTable,
                                         ByRef lpOldSavedBatches As Batches,
                                         Optional ByRef lpBackgroundWorker As BackgroundWorker = Nothing,
                                         Optional ByRef lpBackgroundWorkerEventArgs As DoWorkEventArgs = Nothing)

    Try
      'LogSession.EnterMethod(Helper.GetMethodIdentifier(Reflection.MethodBase.GetCurrentMethod))

      'LogSession.LogMessage("Beginning CreateBatchesFromDataTable for Job '{0}'", Me.Name)

#If NET8_0_OR_GREATER Then
      ArgumentNullException.ThrowIfNull(lpSourceData)
#Else
      If lpSourceData Is Nothing Then
        Throw New ArgumentNullException(NameOf(lpSourceData))
      End If
#End If

      If lpSourceData.Rows.Count = 0 Then
        'If Source IsNot Nothing AndAlso Source.Search IsNot Nothing Then
        '  Throw New Exception(String.Format("{0}: The collection of source ID's is empty, no items to add to the batch.", Source.Search.DisplayName))
        'Else
        Throw New Exception("The collection of source ID's is empty, no items to add to the batch.")
        'End If

      End If

      Dim lobjBatchItem As BatchItem = Nothing

      'Loop through source IDs and add to OLEDB table
      'For Each lstrID As String In lpCollection.Keys
      Dim llngTotalRowCount As Long = lpSourceData.Rows.Count

      Dim lobjAvailableRows As DataRowCollection = lpSourceData.Rows
      'If (LicenseRegister.IsEvaluation) AndAlso (llngTotalRowCount <= Job.MAX_EVAL_JOB_SIZE) Then
      '  ' Only create the maximum allowable number of items in the job.
      '  lobjAvailableRows.Clear()
      '  For lintItemCounter As Integer = 0 To Job.MAX_EVAL_JOB_SIZE - 1
      '    lobjAvailableRows.Add(lpSourceData.Rows(lintItemCounter))
      '  Next
      'Else
      '  lobjAvailableRows = lpSourceData.Rows
      'End If

      lobjAvailableRows = lpSourceData.Rows

      Dim lobjCurrentBatch As Batch

      'LogSession.LogMessage("CreateBatchesFromDataTable for Job '{0}' with {1} batch items", Me.Name, llngTotalRowCount)

      For Each lobjRow As DataRow In lpSourceData.Rows

        If (lpBackgroundWorker IsNot Nothing) AndAlso (lpBackgroundWorker.CancellationPending) Then
          CancelBatches(lpOldSavedBatches, lpBackgroundWorkerEventArgs)
          Exit Sub
        End If

        'If String.IsNullOrEmpty(lobjRow) Then
        '  Throw New InvalidOperationException(String.Format("{0}: Source list values must all contain valid IDs", _
        '                                                    Reflection.MethodBase.GetCurrentMethod))
        'End If

        lobjCurrentBatch = GetCurrentWorkingBatch()

        'Dim lobjBatchItem As New BatchItem(lstrID, lpCollection.Item(lstrID), lobjCurrentBatch)
        Select Case lpSourceData.Columns.Count

          Case 1
            lobjBatchItem = New BatchItem(lobjRow(0), lobjRow(0), lobjCurrentBatch)

          Case 2
            lobjBatchItem = New BatchItem(lobjRow(0), lobjRow(1), lobjCurrentBatch)

          Case 3
            lobjBatchItem = New BatchItem(lobjRow(0), lobjRow(1), lobjRow(2), lobjCurrentBatch)

          Case Else
            Throw New InvalidOperationException("There should be one or two rows in the source data table.")
        End Select

        lobjCurrentBatch.AddItem(lobjBatchItem)

        mintCurrentBatchCount += 1
        mintTotalBatchCount += 1
        mstrPreviousSourceConnectionString = mstrCurrentSourceConnectionString
        ' RaiseEvent BatchItemsCreated(Me.Name, mintTotalBatchCount, Source.SourceIDs.Count)
        'RaiseEvent BatchItemsCreated(Me.Name, mintTotalBatchCount, llngTotalRowCount)

        RaiseEvent BatchItemsCreated(Me, New BatchItemCreatedEventArgs(Me.Name, mintTotalBatchCount, llngTotalRowCount))

        'Debug.Print(String.Format("{0}:{1} {2} of {3} new batch items created.", _
        '                          Me.Name, lobjCurrentBatch.Name, mintTotalBatchCount, llngTotalRowCount))
      Next

      If Me.BatchCount > 1 Then
        'LogSession.LogMessage("Completing CreateBatchesFromDataTable for Job '{0}', {1} batches, {2} total items",
        'Me.Name, Me.BatchCount, llngTotalRowCount)
      Else
        'LogSession.LogMessage("Completing CreateBatchesFromDataTable for Job '{0}', {1} batch, {2} total items",
        'Me.Name, Me.BatchCount, llngTotalRowCount)
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    Finally
      'LogSession.LeaveMethod(Helper.GetMethodIdentifier(Reflection.MethodBase.GetCurrentMethod))
    End Try

  End Sub

  Public Shared Function CreateSourceDataTableFromFilePaths(lpFilePaths As IEnumerable(Of String)) As DataTable
    Try

      Dim lobjFileDataTable As DataTable = Job.CreateNewSourceDataTable
      Dim lobjNewRow As DataRow = Nothing
      For Each lstrFileName As String In lpFilePaths
        lobjNewRow = lobjFileDataTable.NewRow()
        lobjNewRow("Id") = lstrFileName
        lobjNewRow("Title") = IO.Path.GetFileName(lstrFileName)
        ' <Modified by: Ernie at 7/30/2013-11:42:06 AM on machine: ERNIE-THINK>
        ' Added a duplicate check
        If lobjFileDataTable.Rows.Contains(lstrFileName) = False Then
          lobjFileDataTable.Rows.Add(lobjNewRow)
        End If
        ' </Modified by: Ernie at 7/30/2013-11:42:06 AM on machine: ERNIE-THINK>
      Next

      Return lobjFileDataTable

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Shared Function CreateNewSourceDataTable() As DataTable

    Try

      Return CreateNewSourceDataTable(False)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  Public Shared Function CreateNewSourceDataTable(lpIncludeOperationColumn As Boolean) As DataTable

    Try

      Dim lobjDataTable As New DataTable("tmpSourceData")
      Dim lobjIdColumn As New DataColumn("Id", String.Empty.GetType())
      lobjDataTable.Columns.Add(lobjIdColumn)
      lobjDataTable.Columns.Add("Title", String.Empty.GetType())
      If lpIncludeOperationColumn = True Then
        lobjDataTable.Columns.Add("Operation", String.Empty.GetType())
      End If
      lobjDataTable.PrimaryKey = New DataColumn() {lobjIdColumn}

      Return lobjDataTable

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  Private Sub CreateListBatches(Optional ByRef lpOldSavedBatches As Batches = Nothing,
                                Optional ByRef lpBackgroundWorker As BackgroundWorker = Nothing,
                                Optional ByRef lpBackgroundWorkerEventArgs As DoWorkEventArgs = Nothing)

    Try

      Dim lobjDataTable As DataTable = CreateNewSourceDataTable()
      Dim lintItemCounter As Integer = 0

      If Source IsNot Nothing Then

        'Dim lblnIsEvaluation As Boolean = LicenseRegister.IsEvaluation
        Dim lstrFileName As String = Nothing

        For Each lstrIdFilePath As String In Source.SourceIdFileNames
          lstrFileName = IO.Path.GetFileName(lstrIdFilePath)
          If JobBatchContainer.FileExists(Me, lstrFileName) = False Then
            JobBatchContainer.StoreFile(Me, lstrIdFilePath)
            ' Make sure we were successfull
            If JobBatchContainer.FileExists(Me, lstrFileName) = False Then
              Throw New InvalidOperationException(String.Format("Could not access list file '{0}'.", lstrIdFilePath))
            End If
          End If

          Using lobjListStream As Stream = JobBatchContainer.RetrieveFile(Me, lstrFileName)

            Dim lobjNewRow As DataRow = Nothing

            Using lobjStreamReader As New StreamReader(lobjListStream)

              Dim lstrLine As String

              ' Read and display lines from the file until the end of
              ' the file is reached.
              Do
                lstrLine = lobjStreamReader.ReadLine
                lintItemCounter += 1

                If Not String.IsNullOrEmpty(lstrLine) Then

                  lobjNewRow = lobjDataTable.NewRow()
                  lobjNewRow(0) = lstrLine
                  lobjNewRow(1) = lstrLine

                  'If Not lblnIsEvaluation Then
                  '  Try
                  '    lobjDataTable.Rows.Add(lobjNewRow)
                  '  Catch ConstrEx As ConstraintException
                  '    Continue Do
                  '  End Try
                  'Else
                  '  ' Only build the table up to the maximum allowable evaluation job size.
                  '  If lobjDataTable.Rows.Count < Job.MAX_EVAL_JOB_SIZE Then
                  '    lobjDataTable.Rows.Add(lobjNewRow)
                  '  Else
                  '    Exit For
                  '  End If
                  'End If

                  Try
                    lobjDataTable.Rows.Add(lobjNewRow)
                  Catch ConstrEx As ConstraintException
                    Continue Do
                  End Try

                End If

                If lintItemCounter Mod BatchSize = 0 Then
                  CreateBatchesFromDataTable(lobjDataTable, lpOldSavedBatches, lpBackgroundWorker, lpBackgroundWorkerEventArgs)
                  lobjDataTable.Dispose()
                  lobjDataTable = CreateNewSourceDataTable()
                End If

              Loop Until lstrLine Is Nothing

              ' Get the remainders
              If lobjDataTable.Rows.Count > 0 Then
                CreateBatchesFromDataTable(lobjDataTable, lpOldSavedBatches, lpBackgroundWorker, lpBackgroundWorkerEventArgs)
              End If

            End Using
          End Using

        Next

      End If

      ' CreateBatchesFromDataTable(lobjDataTable, lpOldSavedBatches, lpBackgroundWorker, lpBackgroundWorkerEventArgs)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Sub

  Public Function GetListIdsAsString() As String
    Try

      Dim lstrIdsString As String = String.Empty

      If Source.Type <> enumSourceType.List Then
        Throw New InvalidOperationException("Expected Type List")
      End If

      If Source.ListType <> enumListType.TextFile Then
        Throw New InvalidOperationException("Expected ListType TextFile")
      End If

      If Source.SourceIdFileNames Is Nothing OrElse Source.SourceIdFileNames.Count = 0 Then
        Throw New InvalidOperationException("No SourceId Files For Job")
      End If

      Dim lstrSourceIdFilePath As String = Source.SourceIdFileNames.First()
      Dim lstrSourceIdFileName As String = Path.GetFileName(lstrSourceIdFilePath)

      If JobBatchContainer.FileExists(Me, lstrSourceIdFileName) = False Then
        Throw New InvalidOperationException(String.Format("SourceId File '{0}' not found in project database.", lstrSourceIdFileName))
      End If

      Using lobjListStream As Stream = JobBatchContainer.RetrieveFile(Me, lstrSourceIdFileName)
        lstrIdsString = Helper.CopyStreamToString(lobjListStream)
      End Using

      Return lstrIdsString

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      '  Re-throw the exception to the caller
      Throw
    End Try
  End Function

  'Private Sub CreateSearchBatches(Optional ByRef lpOldSavedBatches As Batches = Nothing,
  '                                Optional ByRef lpBackgroundWorker As BackgroundWorker = Nothing,
  '                                Optional ByRef lpBackgroundWorkerEventArgs As DoWorkEventArgs = Nothing)

  '  Dim lstrName As String = String.Empty

  '  Try

  '    Dim lobjSearchResultSet As Core.SearchResultSet = Source.Search.Execute()

  '    If (lobjSearchResultSet Is Nothing OrElse lobjSearchResultSet.Count = 0) Then
  '      Throw New InvalidOperationException(String.Format("{0}: Search ({1}) returned no results, no items to add to the batch.", Reflection.MethodBase.GetCurrentMethod, Source.Search.DisplayName))
  '    End If

  '    'Dim lobjSourceDictionary As New Dictionary(Of String, String)
  '    Dim lobjDataTable = CreateNewSourceDataTable()

  '    'Dim lblnIsEvaluation As Boolean = LicenseRegister.IsEvaluation

  '    'Loop through results and add to OLEDB table
  '    For Each lobjResult As Core.SearchResult In lobjSearchResultSet.Results

  '      If (lpOldSavedBatches IsNot Nothing) AndAlso (lpBackgroundWorker IsNot Nothing) AndAlso (lpBackgroundWorker.CancellationPending) Then
  '        CancelBatches(lpOldSavedBatches, lpBackgroundWorkerEventArgs)
  '        Exit Sub
  '      End If

  '      If (lobjResult.ID = String.Empty) Then
  '        Throw New Exception(String.Format("{0}: Search ({1}) results must contain a valid Id", Reflection.MethodBase.GetCurrentMethod, Source.Search.DisplayName))
  '      End If

  '      lstrName = String.Empty

  '      If (lobjResult.Values(0).Value IsNot Nothing) Then
  '        lstrName = lobjResult.Values(0).ToString
  '      End If

  '      'If (lobjResult.Values(Search.StoredSearch.SOURCE_CONNECTIONSTRING_FIELD_NAME) Is Nothing) Then
  '      '  mstrCurrentSourceConnectionString = Source.SourceConnectionString

  '      'Else
  '      '  mstrCurrentSourceConnectionString = lobjResult.Values(Search.StoredSearch.SOURCE_CONNECTIONSTRING_FIELD_NAME).ToString
  '      'End If

  '      'Dim lobjCurrentBatch As Batch = GetCurrentWorkingBatch()
  '      'Dim lobjBatchItem As New BatchItem(lobjResult.ID, lstrName, lobjCurrentBatch)

  '      'lobjCurrentBatch.AddItem(lobjBatchItem)
  '      'mintCurrentBatchCount += 1
  '      'mintTotalBatchCount += 1
  '      'mstrPreviousSourceConnectionString = mstrCurrentSourceConnectionString
  '      'RaiseEvent BatchItemsCreated(Me.Name, mintTotalBatchCount, lobjSearchResultSet.Results.Count)

  '      Dim lobjNewRow As DataRow = lobjDataTable.NewRow()
  '      lobjNewRow(0) = lobjResult.ID
  '      lobjNewRow(1) = lstrName

  '      'If Not lblnIsEvaluation Then
  '      '  lobjDataTable.Rows.Add(lobjNewRow)
  '      'Else
  '      '  ' Only build the table up to the maximum allowable evaluation job size.
  '      '  If lobjDataTable.Rows.Count < Job.MAX_EVAL_JOB_SIZE Then
  '      '    lobjDataTable.Rows.Add(lobjNewRow)
  '      '  Else
  '      '    Exit For
  '      '  End If
  '      'End If

  '      lobjDataTable.Rows.Add(lobjNewRow)

  '      'lobjSourceDictionary.Add(lobjResult.ID, lstrName)

  '      '#If DEBUG Then
  '      '          Console.WriteLine(String.Format("Processing : {0} of {1}", lintTotalBatchCount, lobjSearchResultSet.Results.Count))
  '      '#End If

  '    Next

  '    CreateBatchesFromDataTable(lobjDataTable, lpOldSavedBatches, lpBackgroundWorker, lpBackgroundWorkerEventArgs)

  '  Catch ex As Exception
  '    ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '    ' Re-throw the exception to the caller
  '    Throw
  '  End Try

  'End Sub

  Private Function GetListBatchSource(Optional ByRef lpOldSavedBatches As Batches = Nothing,
                                      Optional ByRef lpBackgroundWorker As BackgroundWorker = Nothing,
                                      Optional ByRef lpBackgroundWorkerEventArgs As DoWorkEventArgs = Nothing) As DataTable

    'Dim lstrName As String = String.Empty

    Try

      Dim lobjDataTable = CreateNewSourceDataTable()

      If Source IsNot Nothing Then

        'Dim lblnIsEvaluation As Boolean = LicenseRegister.IsEvaluation

        For Each lstrIdFileName As String In Source.SourceIdFileNames
          If JobBatchContainer.FileExists(Me, lstrIdFileName) = False Then
            JobBatchContainer.StoreFile(Me, lstrIdFileName)
            ' Make sure we were successfull
            If JobBatchContainer.FileExists(Me, lstrIdFileName) = False Then
              Throw New InvalidOperationException(String.Format("Could not access list file '{0}'.", lstrIdFileName))
            End If
          End If

          Dim lobjListStream As Stream = JobBatchContainer.RetrieveFile(Me, lstrIdFileName)
          Dim lobjNewRow As DataRow = Nothing

          Using lobjStreamReader As New StreamReader(lobjListStream)

            Dim lstrLine As String

            ' Read and display lines from the file until the end of
            ' the file is reached.
            Do
              lstrLine = lobjStreamReader.ReadLine

              If Not String.IsNullOrEmpty(lstrLine) Then

                lobjNewRow = lobjDataTable.NewRow()
                lobjNewRow(0) = lstrLine
                lobjNewRow(1) = lstrLine

                'If Not lblnIsEvaluation Then
                '  Try
                '    lobjDataTable.Rows.Add(lobjNewRow)
                '  Catch ConstrEx As ConstraintException
                '    Continue Do
                '  End Try
                'Else
                '  ' Only build the table up to the maximum allowable evaluation job size.
                '  If lobjDataTable.Rows.Count < Job.MAX_EVAL_JOB_SIZE Then
                '    lobjDataTable.Rows.Add(lobjNewRow)
                '  Else
                '    Exit For
                '  End If
                'End If

                Try
                  lobjDataTable.Rows.Add(lobjNewRow)
                Catch ConstrEx As ConstraintException
                  Continue Do
                End Try

              End If

            Loop Until lstrLine Is Nothing

          End Using

        Next

      End If

      Return lobjDataTable

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  'Private Function GetSearchBatchSource(Optional ByRef lpOldSavedBatches As Batches = Nothing,
  '                                      Optional ByRef lpBackgroundWorker As BackgroundWorker = Nothing,
  '                                      Optional ByRef lpBackgroundWorkerEventArgs As DoWorkEventArgs = Nothing) As DataTable

  '  Dim lstrName As String = String.Empty

  '  Try

  '    Dim lobjSearchResultSet As Core.SearchResultSet = Source.Search.Execute()
  '    Dim lobjDataTable = CreateNewSourceDataTable()

  '    If (lobjSearchResultSet.HasException) AndAlso (lobjSearchResultSet.Exception IsNot Nothing) Then
  '      Throw New InvalidOperationException(lobjSearchResultSet.Exception.Message, lobjSearchResultSet.Exception)
  '    End If

  '    If (lobjSearchResultSet Is Nothing OrElse lobjSearchResultSet.Count = 0) Then
  '      'Throw New InvalidOperationException(String.Format("{0}: Search ({1}) returned no results, no items to add to the batch.", _
  '      '                                  Reflection.MethodBase.GetCurrentMethod, Source.Search.DisplayName))
  '      Return lobjDataTable
  '    End If

  '    'Dim lblnIsEvaluation As Boolean = LicenseRegister.IsEvaluation

  '    'Loop through results and add to OLEDB table
  '    For Each lobjResult As Core.SearchResult In lobjSearchResultSet.Results

  '      If (lpOldSavedBatches IsNot Nothing) AndAlso (lpBackgroundWorker IsNot Nothing) AndAlso (lpBackgroundWorker.CancellationPending) Then
  '        Return lobjDataTable
  '      End If

  '      If (lobjResult.ID = String.Empty) Then
  '        Throw New Exception(String.Format("{0}: Search ({1}) results must contain a valid Id", Reflection.MethodBase.GetCurrentMethod, Source.Search.DisplayName))
  '      End If

  '      lstrName = String.Empty

  '      If (lobjResult.Values(0).Value IsNot Nothing) Then
  '        lstrName = lobjResult.Values(0).ToString
  '      End If

  '      'If (lobjResult.Values(Search.StoredSearch.SOURCE_CONNECTIONSTRING_FIELD_NAME) Is Nothing) Then
  '      '  mstrCurrentSourceConnectionString = Source.SourceConnectionString

  '      'Else
  '      '  mstrCurrentSourceConnectionString = lobjResult.Values(Search.StoredSearch.SOURCE_CONNECTIONSTRING_FIELD_NAME).ToString
  '      'End If

  '      Dim lobjNewRow As DataRow = lobjDataTable.NewRow()
  '      lobjNewRow(0) = lobjResult.ID
  '      lobjNewRow(1) = lstrName

  '      'If Not lblnIsEvaluation Then
  '      '  lobjDataTable.Rows.Add(lobjNewRow)
  '      'Else
  '      '  ' Only build the table up to the maximum allowable evaluation job size.
  '      '  If lobjDataTable.Rows.Count < Job.MAX_EVAL_JOB_SIZE Then
  '      '    lobjDataTable.Rows.Add(lobjNewRow)
  '      '  Else
  '      '    Exit For
  '      '  End If
  '      'End If

  '      lobjDataTable.Rows.Add(lobjNewRow)

  '    Next

  '    Return lobjDataTable

  '  Catch ex As Exception
  '    ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '    ' Re-throw the exception to the caller
  '    Throw
  '  End Try

  'End Function

  Private Sub CancelBatches(ByRef lpOldSavedBatches As Batches,
                            ByRef lpBackgroundWorkerEventArgs As DoWorkEventArgs)

    Try

      For Each lobjBatch As Batch In lpOldSavedBatches
        Batches.Add(lobjBatch)
      Next

      If lpBackgroundWorkerEventArgs IsNot Nothing Then
        lpBackgroundWorkerEventArgs.Cancel = True
      Else
        For Each lobjBatch As Batch In Me.Batches
          lobjBatch.Cancel()
          'If lobjBatch.Worker IsNot Nothing Then
          '  lobjBatch.Worker.CancelAsync()
          'End If
        Next
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Sub

  Public Function GetBatchLocks() As IBatchLocks
    Try
      Dim lobjBatchLocks As BatchLocks = JobBatchContainer.GetBatchLocks(Me.Name)
      If Me.Project IsNot Nothing Then
        lobjBatchLocks.Project = Me.Project
      End If
      Return lobjBatchLocks
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Sub SetToDeclareAsRecordOnImport(ByVal lpDeclareRecordPlugInConfigFile As String,
                                          ByVal lpExportRecordConnectionString As String)

    Try
      DeclareAsRecordOnImport = True
      DeclareRecordConfiguration = New DeclareRecordConfiguration()
      With DeclareRecordConfiguration
        .DeclareRecordPlugInConfigFile = lpDeclareRecordPlugInConfigFile
        .ExportRecordConnectionString = lpExportRecordConnectionString
      End With

      'Go through each batch and add
      For Each lobjBatch As Batch In Batches
        lobjBatch.DeclareAsRecordOnImport = DeclareAsRecordOnImport
        lobjBatch.DeclareRecordConfiguration = DeclareRecordConfiguration
      Next

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
    End Try

  End Sub

  Public Sub AddTransformation(ByVal lpTransformationFile As String)

    Try

      Dim lobjTransform As New Transformation(lpTransformationFile)

      ' Combine the transformation in the event that it is referencing other transformations
      lobjTransform = lobjTransform.Combine

      Transformations.Add(lobjTransform)

      'Go through each batch and add
      For Each lobjBatch As Batch In Batches
        lobjBatch.Transformations.Add(lobjTransform)
      Next

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Sub

  'Public Sub CreateDBListBatches(lpSourceConnectionString As String, lpSQL As String)

  '  Dim lstrDocumentTitle As String
  '  Dim lstrId As String

  '  Try

  '    Dim lobjItems As New Dictionary(Of String, String)
  '    Dim lintRowCounter As Integer
  '    Dim lintTotalRowCount As Integer
  '    Dim lstrErrorMessage As String = String.Empty
  '    Dim lstrAddResult As String
  '    Dim lobjBatchItemEventArgs As BatchItemCreatedEventArgs


  '    Using lobjConnection As New OleDbConnection(lpSourceConnectionString)
  '      lobjConnection.Open()

  '      ' We need to get the total number of expected records
  '      Dim lstrRowCountSQL As String = Helper.ConvertSelectSQLToSelectCount(lpSQL)

  '      If Not String.IsNullOrEmpty(lstrErrorMessage) Then
  '        Throw New Exception(String.Format("Unable to build total record count SQL for DB list: '{0}'", lstrErrorMessage))
  '      ElseIf String.IsNullOrEmpty(lstrRowCountSQL) Then
  '        Throw New Exception("Unable to build total record count SQL for DB list")
  '      End If

  '      Using lobjCommand As New OleDbCommand(lstrRowCountSQL, lobjConnection)
  '        lintTotalRowCount = lobjCommand.ExecuteScalar()
  '      End Using

  '      If lintTotalRowCount = 0 Then
  '        ApplicationLogging.WriteLogEntry(String.Format("No items found using query '{0}'.", lstrRowCountSQL), _
  '                                         MethodBase.GetCurrentMethod(), TraceEventType.Warning, 52404)
  '      End If

  '      lobjBatchItemEventArgs = New BatchItemCreatedEventArgs(Me.Name, 0, lintTotalRowCount)

  '      Using lobjCommand As New OleDbCommand(lpSQL, lobjConnection)
  '        Using lobjDataReader As OleDbDataReader = lobjCommand.ExecuteReader
  '          If lobjDataReader.HasRows Then
  '            Do Until lobjDataReader.Read() = False
  '              If ((lintRowCounter > 0) AndAlso (lintRowCounter Mod 10000 = 0)) Then
  '                ' Add the items
  '                ' Debug.WriteLine(String.Format("Writing up to {0}", lintRowCounter))
  '                'lobjDataReader.
  '                lobjBatchItemEventArgs.CurrentCount = lintRowCounter
  '                lobjBatchItemEventArgs.Message = String.Format("Writing up to {0}", lintRowCounter)
  '                RaiseEvent BatchCreationUpdate(Me, lobjBatchItemEventArgs)

  '                lstrAddResult = AddItems(lobjItems)
  '                Debug.WriteLine(lstrAddResult)
  '                lobjItems.Clear()
  '                GC.Collect()
  '              End If
  '              If Not IsDBNull(lobjDataReader(0)) AndAlso Not String.IsNullOrEmpty(lobjDataReader(0).ToString()) Then
  '                lstrDocumentTitle = lobjDataReader(0).ToString
  '                ' lobjId = lobjDataReader(1)
  '                lstrId = lstrDocumentTitle
  '                ' Debug.WriteLine(String.Format("Reading row {0}: {1}", lintRowCounter.ToString("#,###"), lstrDocumentTitle))
  '                lobjBatchItemEventArgs.CurrentCount = lintRowCounter
  '                lobjBatchItemEventArgs.Message = String.Format("Reading row {0}: {1}", lintRowCounter.ToString("#,###"), lstrDocumentTitle)
  '                RaiseEvent BatchCreationUpdate(Me, lobjBatchItemEventArgs)
  '                If (lobjItems.ContainsKey(lstrId)) Then
  '                  ApplicationLogging.WriteLogEntry(String.Format("Duplicate key found: {0}, skipping row.", lstrId), Reflection.MethodBase.GetCurrentMethod, TraceEventType.Warning, 88769)
  '                Else
  '                  lobjItems.Add(lstrId, lstrDocumentTitle)
  '                End If

  '                lintRowCounter += 1
  '              Else
  '                Debug.Print("Skipping Row")
  '              End If
  '            Loop
  '            ' Debug.WriteLine(String.Format("Writing last {0}", lobjItems.Count))
  '            lobjBatchItemEventArgs.CurrentCount = lintRowCounter
  '            lobjBatchItemEventArgs.Message = String.Format("Writing last {0}", lobjItems.Count)
  '            RaiseEvent BatchCreationUpdate(Me, lobjBatchItemEventArgs)

  '            If lobjItems.Count > 0 Then
  '              lstrAddResult = AddItems(lobjItems)
  '            End If
  '          Else
  '            If lobjItems.Count > 0 Then
  '              lstrAddResult = AddItems(lobjItems)
  '            End If
  '          End If
  '        End Using
  '      End Using
  '    End Using

  '    'RaiseEvent BatchItemsCreated(Me.Name, mintTotalBatchCount, lintRowCounter)
  '    RaiseEvent BatchItemsCreated(Me, New BatchItemCreatedEventArgs(Me.Name, mintTotalBatchCount, lintRowCounter))

  '  Catch ex As Exception
  '    ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '    ' Re-throw the exception to the caller
  '    Throw
  '  End Try
  'End Sub

#Region "Add Items"

#Region "Add Items"

#Region "Id Only"

  ''' <summary>Adds one item to a job with only the source doc id.</summary>
  ''' <returns>A string result containing any messages regarding the operation result.</returns>
  ''' <example>
  '''  <code title="Add a single item using only the Id." description="" groupname="Adding Items" lang="VB.NET">
  ''' Dim lobjProject As Project = Project.OpenProject("Type=SQLServer;Location=Data Source=localhost;Initial Catalog=jmOpDemo;Integrated Security=True;ServerName=localhost;DBName=jmTestProject;UserName=;Password=;TrustedConnection=True;DatabasePath=")
  ''' Dim lstrAddResult As String
  ''' Dim lobjJob As Job = lobjProject.Jobs("Import")
  ''' lstrAddResult = lobjJob.AddItem("100")</code>
  ''' </example>
  Public Function AddItem(lpId As String) As String
    Try
      Return AddItems(New String() {lpId})
    Catch Ex As Exception
      ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  ''' <summary>Adds one or more items to a job with only the source doc id(s).</summary>
  ''' <param name="lpIds">An array of source doc ids.</param>
  ''' <returns>A string result containing any messages regarding the operation result.</returns>
  ''' <example>
  '''  <code title="Adding Multiple Items using only Ids" description="" groupname="Adding Items" lang="VB.NET">
  ''' Dim lobjProject As Project = Project.OpenProject("Type=SQLServer;Location=Data Source=localhost;Initial Catalog=jmOpDemo;Integrated Security=True;ServerName=localhost;DBName=jmTestProject;UserName=;Password=;TrustedConnection=True;DatabasePath=")
  ''' Dim lstrAddResult As String
  ''' Dim lobjJob As Job = lobjProject.Jobs("Import")
  ''' Dim lobjNewItems As New List(Of String)
  ''' 
  ''' lobjNewItems.Add("123")
  ''' lobjNewItems.Add("124")
  ''' lobjNewItems.Add("125")
  ''' 
  ''' lstrAddResult = lobjJob.AddItems(lobjNewItems)</code>
  ''' </example>
  Public Function AddItems(lpIds As IEnumerable(Of String)) As String
    Try

      Dim lobjItemTable As DataTable = IdsToDataTable(lpIds)
      Dim lstrResult As String = String.Empty
      AddItemsFromDataTable(lobjItemTable, lstrResult)
      Return lstrResult
    Catch Ex As Exception
      ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

#End Region

#Region "Id and Title"

  ''' <summary>Adds one item to a job with the source doc id and title.</summary>
  ''' <returns>A string result containing any messages regarding the operation result.</returns>
  ''' <example>
  '''  <code title="Add a single item using the Id and Title." description="" groupname="Adding Items" lang="VB.NET">
  ''' Dim lobjProject As Project = Project.OpenProject("Type=SQLServer;Location=Data Source=localhost;Initial Catalog=jmOpDemo;Integrated Security=True;ServerName=localhost;DBName=jmTestProject;UserName=;Password=;TrustedConnection=True;DatabasePath=")
  ''' Dim lstrAddResult As String
  ''' Dim lobjJob As Job = lobjProject.Jobs("Import")
  ''' lstrAddResult = lobjJob.AddItem("100", "Test Document 0")</code>
  ''' </example>
  Public Function AddItem(lpId As String, ByRef lpTitle As String) As String
    Try
      Dim lobjDictionary As New Dictionary(Of String, String) From {
        {lpId, lpTitle}
      }
      Return AddItems(lobjDictionary)
    Catch Ex As Exception
      ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Function AddItem(lpId As String, lpTitle As String, lpOperation As String) As String
    Try
      ' Dim lobjDictionary As New Dictionary(Of String, String)
      Dim lobjItemTable As DataTable = CreateNewSourceDataTable(True)
      Dim lobjNewRow As DataRow
      'lobjDictionary.Add(lpId, lpTitle)
      lobjNewRow = lobjItemTable.NewRow()
      lobjNewRow(0) = lpId
      lobjNewRow(1) = lpTitle
      lobjNewRow(2) = lpOperation
      lobjItemTable.Rows.Add(lobjNewRow)
      Dim lstrResult As String = String.Empty
      AddItemsFromDataTable(lobjItemTable, lstrResult)
      Return lstrResult
    Catch Ex As Exception
      ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  ''' <summary>Adds one or more items to a job with a dictionary containing both Id(s) and Title(s).</summary>
  ''' <param name="lpItems">A dictionary of Id, Title pairs.</param>
  ''' <returns>A string result containing any messages regarding the operation result.</returns>
  ''' <example>
  '''  <code title="Adding Multiple Items using both Ids and Titles" description="" groupname="Adding Items" lang="VB.NET">
  ''' Dim lobjProject As Project = Project.OpenProject("Type=SQLServer;Location=Data Source=localhost;Initial Catalog=jmOpDemo;Integrated Security=True;ServerName=localhost;DBName=jmTestProject;UserName=;Password=;TrustedConnection=True;DatabasePath=")
  ''' Dim lstrAddResult As String
  ''' Dim lobjJob As Job = lobjProject.Jobs("Import")
  ''' Dim lobjItems As New Dictionary(Of String, String)
  ''' 
  ''' lobjItems.Add("101", "Test Document 1")
  ''' lobjItems.Add("102", "Test Document 2")
  ''' lobjItems.Add("103", "Test Document 3")
  ''' lobjItems.Add("104", "Test Document 4")
  ''' lobjItems.Add("105", "Test Document 5")
  ''' 
  ''' lstrAddResult = lobjJob.AddItems(lobjItems)</code>
  ''' </example>
  Public Function AddItems(lpItems As IDictionary(Of String, String)) As String
    Try
      Dim lobjItemTable As DataTable = DictionaryToDataTable(lpItems)
      Dim lstrResult As String = String.Empty
      AddItemsFromDataTable(lobjItemTable, lstrResult)
      Return lstrResult
    Catch Ex As Exception
      ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

#End Region

#Region "DataTableCreators"

  Private Function IdsToDataTable(lpItems As IEnumerable(Of String)) As DataTable
    Try
      Dim lobjItemTable As DataTable = CreateNewSourceDataTable()
      Dim lobjNewRow As DataRow

      For Each lstrItem As String In lpItems
        lobjNewRow = lobjItemTable.NewRow()
        lobjNewRow(0) = lstrItem
        lobjNewRow(1) = lstrItem
        lobjItemTable.Rows.Add(lobjNewRow)
      Next
      Return lobjItemTable
    Catch Ex As Exception
      ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function DictionaryToDataTable(lpItems As IDictionary(Of String, String)) As DataTable
    Try
      Dim lobjItemTable As DataTable = CreateNewSourceDataTable()
      Dim lobjNewRow As DataRow

      For Each lobjItem As KeyValuePair(Of String, String) In lpItems
        lobjNewRow = lobjItemTable.NewRow()
        lobjNewRow(0) = lobjItem.Key
        lobjNewRow(1) = lobjItem.Value
        lobjItemTable.Rows.Add(lobjNewRow)
      Next
      Return lobjItemTable
    Catch Ex As Exception
      ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

#End Region

#End Region

#End Region

  Public Sub AddItemsFromDataTable(lpDataTable As DataTable, ByRef lpResult As String)
    Try

      Dim lobjNewItems As DataTable = JobBatchContainer.GetUpdateTable(Me, lpDataTable)
      Dim lobjHost As Object = Nothing
      Dim lobjCurrentBatch As Batch = Nothing
      Dim lintNewItemCount As Integer = lobjNewItems.Rows.Count

      If lintNewItemCount > 0 Then
        CreateBatchesFromDataTable(lobjNewItems)
        lpResult = String.Format("Added {0} new batch items to job '{1}'.", lobjNewItems.Rows.Count, Me.Name)

        'Commit the last batch
        lobjCurrentBatch = GetCurrentWorkingBatch()
        lobjCurrentBatch.CommitBatchItems()

        'If we created the Batches successfully then save out the 
        ' new batches and job/batch relationships to the container.
        SaveBatch(lobjCurrentBatch)

        If ((Project.Host IsNot Nothing) AndAlso (String.Compare(Project.Host.GetType.Name, JOB_MANAGER_WINDOW) = 0)) Then
          lobjHost = Project.Host
        ElseIf ((Project.Host IsNot Nothing) AndAlso (String.Compare(Project.Host.GetType.Name, JOB_MANAGER_WINDOW) = 0)) Then
          lobjHost = Project.Host
        End If

        lobjHost?.ReloadProjectTree()

      Else
        lpResult = "No new items found."
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  ''' <summary>
  '''     Creates a duplicate of the current job using the new name.
  ''' </summary>
  ''' <param name="lpNewJobName" type="String">
  '''     <para>
  '''         
  '''     </para>
  ''' </param>
  Public Function Duplicate(lpNewJobName As String) As Job
    Try
      Dim lobjNewJob As Job = Nothing

      ' Ernie Bahr at: 9/12/2012-10:42:34 on machine: ERNIEBAHR-THINK
      ' Once the configuration property is completely implemented, use that here instead of all of these discreet properties.
      'lobjNewJob = New Job(Guid.NewGuid.ToString, lpNewJobName, Me.Description, Me.Operation, _
      '                     Me.Process, Me.BatchSize, Me.Source, Me.ItemsLocation, _
      '                     Me.DestinationConnectionString, Me.ContentStorageType, _
      '                     Me.DeclareAsRecordOnImport, Me.DeclareRecordConfiguration, _
      '                     Me.Transformations, Me.DocumentFilingMode, Me.LeadingDelimiter, _
      '                     Me.BasePathLocation, Me.FolderDelimiter, Me.TransformationSourcePath)


      Dim lobjConfiguration As JobConfiguration = Me.Configuration.Clone
      lobjConfiguration.Name = lpNewJobName

      lobjNewJob = New Job(lobjConfiguration)

      Me.Project.Jobs.Add(lobjNewJob)
      Me.Project.SaveJob(lobjNewJob)

      Return lobjNewJob

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  ''' <summary>
  '''     Renames the current job and saves it to the database.
  ''' </summary>
  ''' <param name="lpNewJobName" type="String">
  '''     <para>
  '''         
  '''     </para>
  ''' </param>
  Public Sub Rename(lpNewJobName As String)
    Try
      Me.Name = lpNewJobName
      Me.Save()
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Function GetCachedWorkSummaryCounts() As IWorkSummary
    Try
      Dim lobjCachedSummary As IWorkSummary = If(JobBatchContainer.GetCachedWorkSummaryCounts(Me), JobBatchContainer.GetWorkSummaryCounts(Me))

      mobjLastWorkSummary = lobjCachedSummary
      Return mobjLastWorkSummary
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Function GetWorkSummaryCounts() As WorkSummary

    Try
      Dim lobjWorkSummary As IWorkSummary

      Try
        lobjWorkSummary = JobBatchContainer.GetWorkSummaryCounts(Me)
      Catch ex As Exception
        ApplicationLogging.WriteLogEntry(String.Format("Unable to get work summary counts for job: {0}", ex.Message),
          Reflection.MethodBase.GetCurrentMethod(), TraceEventType.Warning, 61342)
        Return Nothing
      End Try

      If lobjWorkSummary Is Nothing Then
        If Me.IsRunning = False AndAlso lobjWorkSummary.ProcessingCount > 0 Then
          JobBatchContainer.ResetItemsToNotProcessed(Me, ProcessedStatus.Processing)
          Try
            lobjWorkSummary = JobBatchContainer.GetWorkSummaryCounts(Me)
          Catch ex As Exception
            ApplicationLogging.WriteLogEntry(String.Format("Unable to get work summary counts for job: {0}", ex.Message),
              Reflection.MethodBase.GetCurrentMethod(), TraceEventType.Warning, 61343)
            Return Nothing
          End Try
        End If
      End If

      If lobjWorkSummary IsNot Nothing AndAlso IsCancelled Then
        DirectCast(lobjWorkSummary, WorkSummary).ClearProcessingRate()
      End If

      SetLastWorkSummary(lobjWorkSummary)

      Return mobjLastWorkSummary

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  Public Function GetFailureSummaries() As FailureSummaries
    Try
      Return JobBatchContainer.GetFailureSummaries(Me)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Function GetAllItemsToDataTable(ByVal lpProcessedStatusFilter As String) As DataTable

    Try
      Return JobBatchContainer.GetAllItemsToDataTable(Me, lpProcessedStatusFilter)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  Public Function GetItemCount() As Long
    Try
      Return JobBatchContainer.GetItemCount(Me)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Function GetItemByDocId(ByVal lpDocId As String, ByVal lpScope As OperationScope) As IBatchItem
    Try
      Return JobBatchContainer.GetItemById(Me.Name, lpDocId, lpScope)
    Catch Ex As Exception
      ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  'Public Sub UpdateSearch(lpSearchXML As String)

  '  Try
  '    Me.Source.Search = Serializer.Deserialize.XmlString(lpSearchXML, GetType(Ecmg.Cts.Search.StoredSearch))
  '    'JobBatchContainer.UpdateJobSource(Me)
  '    JobBatchContainer.SaveJob(Me)

  '  Catch ex As Exception
  '    ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
  '    ' Re-throw the exception to the caller
  '    Throw
  '  End Try

  'End Sub

  Public Sub UpdateRepository(ByVal lpScope As ExportScope)
    Try

      Dim lobjRepository As Repository = Nothing

      Select Case lpScope
        Case ExportScope.Source
          lobjRepository = GetLiveRepository(Me.SourceConnectionString)
        Case ExportScope.Destination
          lobjRepository = GetLiveRepository(Me.DestinationConnectionString)
        Case ExportScope.Both
          UpdateRepository(ExportScope.Source)
          UpdateRepository(ExportScope.Destination)
      End Select

      If lobjRepository IsNot Nothing Then
        Me.JobBatchContainer.SaveRepository(lobjRepository, Me, lpScope)
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Sub UpdateIdListFile(ByVal lpList As String)
    Try
      If Source.Type <> enumSourceType.List Then
        Throw New InvalidOperationException("Expected Type List")
      End If

      If Source.ListType <> enumListType.TextFile Then
        Throw New InvalidOperationException("Expected ListType TextFile")
      End If

      If Source.SourceIdFileNames Is Nothing OrElse Source.SourceIdFileNames.Count = 0 Then
        Throw New InvalidOperationException("No SourceId Files For Job")
      End If

      Dim lstrSourceIdFilePath As String = Source.SourceIdFileNames.First()
      Dim lstrSourceIdFileName As String = Path.GetFileName(lstrSourceIdFilePath)

      If JobBatchContainer.FileExists(Me, lstrSourceIdFileName) = False Then
        Throw New InvalidOperationException(String.Format("SourceId File '{0}' not found in project database.", lstrSourceIdFileName))
      End If

      Using lobjListStream As MemoryStream = Helper.CopyStringToStream(lpList)
        JobBatchContainer.StoreFile(Me, lstrSourceIdFileName, lobjListStream)
      End Using

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      '  Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Sub UpdateConfiguration(ByVal lpXML As String)
    Try
      If (String.IsNullOrEmpty(lpXML) = False) Then
        Dim lobjNewConfiguration As JobConfiguration = Nothing

        Try
          lobjNewConfiguration = Serializer.Deserialize.XmlString(lpXML, GetType(JobConfiguration))

        Catch ex As Exception
          ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
          Throw New DeserializationException(String.Format("Unable to create JobConfiguration object from xml: {0}", ex.Message), ex)
        End Try

        mobjConfiguration = lobjNewConfiguration

        Me.Process = lobjNewConfiguration.Process

        For Each lobjBatch As Batch In Me.Batches
          lobjBatch.Process = lobjNewConfiguration.Process
        Next

        Me.Transformations = lobjNewConfiguration.Transformations

        JobBatchContainer.SaveJob(Me)

        ' Push the change down to the batches in memory in case the user wants 
        ' to execute the process before closing and re-opening the project.
        For Each lobjBatch As Batch In Me.Batches
          lobjBatch.UpdateTransformations(Me)
        Next

      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Sub UpdateProcess(ByVal lpXML As String)

    Try

      If (String.IsNullOrEmpty(lpXML) = False) Then

        Dim lobjNewProcess As Process = Nothing

        Try
          lobjNewProcess = Serializer.Deserialize.XmlString(lpXML, GetType(Process))

        Catch ex As Exception
          ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
          Throw New DeserializationException(String.Format("Unable to create Process object from xml: {0}", ex.Message), ex)
        End Try

        Me.Process = lobjNewProcess

        Me.Batches.UpdateProcess(lpXML)

      End If

      ' JobBatchContainer.UpdateProcess(Me)
      JobBatchContainer.SaveJob(Me)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Sub

  Public Sub UpdateTransformation(ByVal lpTransformation As ITransformation)
    Try

      Me.Transformations.Clear()

      If lpTransformation IsNot Nothing Then

        ' Make sure the transformation has a name
        If String.IsNullOrEmpty(lpTransformation.Name) Then
          lpTransformation.Name = String.Format("Transformation for job '{0}'", Me.Name)
        End If

        ' Check to see if the transformation name matches the one that might might be in the process
        ' They need to be in synch
        If Process IsNot Nothing Then
          For Each lobjOperation As IOperable In Process.Operations
            If TypeOf lobjOperation Is TransformOperation Then
              Dim lobjTransformOperation As TransformOperation = lobjOperation
              If (lobjTransformOperation.Parameters IsNot Nothing) AndAlso
                (lobjTransformOperation.Parameters.Count > 0) AndAlso
                (lobjTransformOperation.Parameters.Contains("RootTransformation")) Then
                Dim lobjRootTransformationParameter = lobjTransformOperation.Parameters.Item("RootTransformation")
                If lobjRootTransformationParameter IsNot Nothing Then
                  If String.Equals(lobjRootTransformationParameter.Value, lpTransformation.Name) = False Then
                    ' Syncronize the name
                    lobjRootTransformationParameter.Value = lpTransformation.Name
                    ' Push the change down to the batches in memory in case the user wants 
                    ' to execute the process before closing and re-opening th eproject.
                    For Each lobjBatch As Batch In Me.Batches
                      lobjBatch.UpdateProcess(Me)
                    Next
                  End If
                End If
                Exit For
              End If
            End If
          Next
        End If

        Me.Transformations.Add(lpTransformation)

      End If

      'JobBatchContainer.UpdateTransformations(Me)
      JobBatchContainer.SaveJob(Me)

      ' Push the change down to the batches in memory in case the user wants 
      ' to execute the process before closing and re-opening the project.
      For Each lobjBatch As Batch In Me.Batches
        lobjBatch.UpdateTransformations(Me)
      Next

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Sub UpdateTransformation(ByVal lpTransformationXML As String)

    Try

      If (String.IsNullOrEmpty(lpTransformationXML) = False) Then

        Dim lobjNewTransformation As Transformation = Nothing

        Try
          lobjNewTransformation = Serializer.Deserialize.XmlString(lpTransformationXML, GetType(Documents.Transformations.Transformation))

        Catch ex As Exception
          ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
          Throw New DeserializationException(String.Format("Unable to create Transformation object from xml: {0}", ex.Message), ex)
        End Try

        UpdateTransformation(lobjNewTransformation)

      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Sub

  Public Sub RefreshTransform()

    Try

      If (IO.File.Exists(Me.TransformationSourcePath)) Then
        Me.Transformations.Clear()
        Me.Transformations.Add(New Transformations.Transformation(Me.TransformationSourcePath))

        For Each lobjBatch In Me.Batches
          lobjBatch.Transformations.Clear()
          lobjBatch.Transformations.Add(New Transformations.Transformation(Me.TransformationSourcePath))
        Next

        'Save updates to the container
        JobBatchContainer.RefreshTransform(Me)

      Else
        Throw New Exception(String.Format("Job '{0}', cannot refresh transform, file does not exist: '{1}' ", Me.Name, Me.TransformationSourcePath))
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Sub

  ''' <summary>
  ''' Update this job and return the number of new items
  ''' </summary>
  ''' <remarks></remarks>
  Public Function Update(ByVal lpBackgroundWorker As BackgroundWorker,
                         ByVal lpBackgroundWorkerEventArgs As DoWorkEventArgs) As ProvisionStatus

    Try

      ' We want to recreate the batches with a twist.  
      ' For each existing batch item, we want to leave it in place.  
      ' For newly discovered items, we want to add them.

      ' Essentially we will just add additional batches for the newly discovered items only.

      'Dim lstrName As String = String.Empty
      Dim lobjOriginalItems As DataTable = Nothing
      Dim lobjNewItems As DataTable = Nothing

      'Dim llngNewItemCount As Long = 0
      Dim lobjProvisionStatus As New ProvisionStatus(Me.Name, 0)

      If (Source Is Nothing) Then
        Throw New Exception(String.Format("Unable to update batches, no 'Source' is defined for job '{0}'.", Me.Name))
      End If

      'Save off the Batches so we can delete these old ones at the end of this method
      Dim lobjOldSavedBatches As New Batches(Me)

      For Each lobjBatch As Batch In Batches
        lobjOldSavedBatches.Add(lobjBatch)
      Next

      Select Case Source.Type

        'Case enumSourceType.Search
        '  lobjOriginalItems = GetSearchBatchSource(lobjOldSavedBatches, lpBackgroundWorker, lpBackgroundWorkerEventArgs)

        Case enumSourceType.Folder

          'CreateFolderBatches(lobjOldSavedBatches, lpBackgroundWorker, lpBackgroundWorkerEventArgs)
        Case enumSourceType.List
          lobjOriginalItems = GetListBatchSource(lobjOldSavedBatches, lpBackgroundWorker, lpBackgroundWorkerEventArgs)

          'CreateListBatches(lobjOldSavedBatches, lpBackgroundWorker, lpBackgroundWorkerEventArgs)
        Case Else
          Throw New Exception(String.Format("{0}: Unknow BatchSourceType '{1}'", Reflection.MethodBase.GetCurrentMethod, Source.Type))
      End Select

      ''Reset incase we call multiple times
      'If (Batches.Count > 0) Then
      '  Batches.Clear()
      'End If

      ' <Modified by: Rick and Ernie at 9/19/2011-1:06:52 PM on machine: ERNIE-M4400>
      If (Batches.Count > 0) Then
        mintCurrentWorkingBatch = Batches.Count - 1
      End If

      ' </Modified by: Rick and Ernie at 9/19/2011-1:06:52 PM on machine: ERNIE-M4400>

      mstrPreviousSourceConnectionString = mstrCurrentSourceConnectionString

      If Batches.Count = 0 Then
        mintCurrentBatchCount = 0

      Else
        mintCurrentBatchCount = Batches.Last.GetTotalItemCount
      End If

      mintTotalBatchCount = mintCurrentBatchCount
      'mintRunningCount = 0

      'User Cancels Process
      If (lpBackgroundWorker IsNot Nothing) Then

        If (lpBackgroundWorker.CancellationPending) Then
          lpBackgroundWorkerEventArgs.Cancel = True
          Return lobjProvisionStatus
        End If

      End If

      If lobjOriginalItems IsNot Nothing Then
        lobjNewItems = mobjContainer.GetUpdateTable(Me, lobjOriginalItems)
        'llngNewItemCount = lobjNewItems.Rows.Count
        lobjProvisionStatus.ItemCount = lobjNewItems.Rows.Count

        If lobjProvisionStatus.ItemCount > 0 Then
          ' We have new items to add to the job
          CreateBatchesFromDataTable(lobjNewItems, lobjOldSavedBatches, lpBackgroundWorker, lpBackgroundWorkerEventArgs)

          'Commit the last batch
          GetCurrentWorkingBatch().CommitBatchItems()

          'If we created the Batches successfully then save out the 
          ' new batches and job/batch relationships to the container.
          Save()

        End If

      End If

      Return lobjProvisionStatus

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  ''' <summary>
  ''' Executes the job in a single thread.
  ''' </summary>
  ''' <remarks></remarks>
  Friend Sub Execute()
    Try
      'mblnIsRunning = True
      'For Each lobjBatch As Batch In Me.Batches
      '  If lobjBatch.IsAvailableForProcessing Then
      '    lobjBatch.Execute()
      '  End If
      'Next
      StartAllBatches(Me, 1)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    Finally
      'mblnIsRunning = False
    End Try
  End Sub

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
  ''' <remarks></remarks>
  Public Sub RunJob(lpNumberOfThreads As Integer)

    Try
      'LogSession.EnterMethod(Helper.GetMethodIdentifier(Reflection.MethodBase.GetCurrentMethod))

      'Based on the number of threads, process the batches with threading goodness.
      StartAllBatches(Me, lpNumberOfThreads)

      'Wait for all threads to finish
      mobjResetEvent.WaitOne()

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    Finally
      'LogSession.LeaveMethod(Helper.GetMethodIdentifier(Reflection.MethodBase.GetCurrentMethod))
    End Try

  End Sub

  Private Sub StartAllBatches(lpJob As Job,
                              lpMaxBatchThreads As Integer)

    Try

      'LogSession.EnterMethod(Helper.GetMethodIdentifier(Reflection.MethodBase.GetCurrentMethod))

      If (lpJob.IsCancelled) Then
        mobjResetEvent.Set()
        Exit Sub
      End If

      mintMaxBatchThreads = lpMaxBatchThreads

      If ((lpJob.Process IsNot Nothing) AndAlso (lpJob.Process.RunBeforeJobBegin IsNot Nothing)) Then
        Dim lobjRunBeforeJobBeginResult As OperationEnumerations.Result =
          lpJob.Process.RunBeforeJobBegin.Execute(New JobItemProxy(lpJob))
        If lobjRunBeforeJobBeginResult <> OperationEnumerations.Result.Success Then
          ApplicationLogging.WriteLogEntry(String.Format("RunBeforeJobBegin failed for job '{0}'.",
            lpJob.Name), Reflection.MethodBase.GetCurrentMethod, TraceEventType.Warning, 56391)
          Exit Sub
        End If
      End If

      For Each lobjBatch As Batch In lpJob.Batches

        ' <Added by: Ernie at: 8/11/2014-3:20:54 PM on machine: ERNIE-THINK>
        ' Sometimes the job reference is lost or has not been properly set, we will check it here.
        If lobjBatch.Job Is Nothing Then
          lobjBatch.SetJob(Me)
        End If
        ' </Added by: Ernie at: 8/11/2014-3:20:54 PM on machine: ERNIE-THINK>

        Dim lobjBatchWorker As New BatchWorker(lobjBatch, Nothing, Nothing)

        If (lobjBatchWorker.IsBusy = False AndAlso lobjBatchWorker.CancellationPending = False AndAlso lobjBatchWorker.Batch.IsAvailableForProcessing() AndAlso (mintBatchThreadsRunning < lpMaxBatchThreads)) Then

          AddHandler lobjBatchWorker.DoWork, AddressOf Me.DoWork
          AddHandler lobjBatchWorker.RunWorkerCompleted, AddressOf Me.RunWorkerCompleted
          AddHandler lobjBatchWorker.ProgressChanged, AddressOf Me.ProgressChanged
          AddHandler lobjBatch.ItemProcessed, AddressOf Me.Batch_ItemProcessed

          lobjBatchWorker.RunWorkerAsync()
          mintBatchThreadsRunning += 1

          If (mintBatchThreadsRunning = lpMaxBatchThreads) Then Exit Sub

        End If

      Next

      'If all threads are done running
      If mintBatchThreadsRunning = 0 Then
        mobjResetEvent.Set()
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      mobjResetEvent.Set()
      Throw
    Finally
      'LogSession.LeaveMethod(Helper.GetMethodIdentifier(Reflection.MethodBase.GetCurrentMethod))
    End Try

  End Sub

#Region "Execute Batch Background Worker Methods"

  ' <Modified by: Ernie Bahr at 10/2/2012-11:56:30 on machine: ERNIEBAHR-THINK>
  ' <Removed by: Ernie Bahr at: 10/2/2012-11:56:39 on machine: ERNIEBAHR-THINK>
  '   Private Sub Batch_ItemProcessed(ByVal lpBatchWorker As BatchWorker, _
  '                                   ByVal lpItemId As String, _
  '                                   ByVal lpTitle As String, _
  '                                   ByVal lpBatchSummary As WorkSummary)
  ' 
  '     Try
  '       ProgressChanged(lpBatchWorker, New ProgressChangedEventArgs((lpBatchSummary.NotProcessedCount + lpBatchSummary.SuccessCount + lpBatchSummary.FailedCount / lpBatchSummary.TotalItemsCount) * 100, lpBatchSummary))
  ' 
  '     Catch ex As Exception
  '       ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '     End Try
  ' 
  '   End Sub
  ' </Removed by: Ernie Bahr at: 10/2/2012-11:56:39 on machine: ERNIEBAHR-THINK>
  ' <Added by: Ernie Bahr at: 10/2/2012-11:56:51 on machine: ERNIEBAHR-THINK>
  Private Sub Batch_ItemProcessed(sender As Object, ByRef e As BatchItemProcessedEventArgs)
    Try
      ProgressChanged(e.BatchWorker, New ProgressChangedEventArgs(e.WorkSummary.ProgressPercentage * 100, e))
    Catch Ex As Exception
      ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
    End Try
  End Sub


  ' </Added by: Ernie Bahr at: 10/2/2012-11:56:51 on machine: ERNIEBAHR-THINK>
  ' </Modified by: Ernie Bahr at 10/2/2012-11:56:30 on machine: ERNIEBAHR-THINK>

  Private Sub DoWork(ByVal sender As Object,
                     ByVal e As DoWorkEventArgs)

    Try

      'LogSession.EnterMethod(Helper.GetMethodIdentifier(Reflection.MethodBase.GetCurrentMethod))

      Dim worker As BaseBatchWorker = CType(sender, BaseBatchWorker)
      worker.DoWorkEventArgs = e

      RaiseEvent BatchStarted(sender, worker.Batch)

      If ((worker.Batch.SelectedItemList IsNot Nothing) AndAlso (worker.Batch.SelectedItemList.Count > 0)) Then
        ' mblnIsRunning = True
        worker.Batch.Execute(worker, worker.Batch.SelectedItemList)
      Else
        ' mblnIsRunning = True
        worker.Batch.Execute(worker)
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
    Finally
      'LogSession.LeaveMethod(Helper.GetMethodIdentifier(Reflection.MethodBase.GetCurrentMethod))
    End Try

  End Sub

  Private Sub ProgressChanged(ByVal sender As Object,
                              ByVal e As ProgressChangedEventArgs)

    Try

      'Dim worker As BatchWorker = CType(sender, BatchWorker)
      'Dim lobjBatchSummary As WorkSummary = e.UserState

      'RaiseEvent BatchItemCompleted(sender, lobjBatchSummary)
      RaiseEvent BatchItemCompleted(sender, e.UserState)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
    End Try

  End Sub

  Private Sub RunWorkerCompleted(ByVal sender As Object,
                                 ByVal e As RunWorkerCompletedEventArgs)

    Try

      Dim worker As BatchWorker = CType(sender, BatchWorker)

      RaiseEvent BatchCompleted(sender, worker.Batch)

      'Does this in a thread safe manner - mintBatchThreadsRunning -= 1
      Interlocked.Decrement(mintBatchThreadsRunning)

      'Call start all batches again to pick up the next batch to run
      StartAllBatches(worker.Batch.Job, mintMaxBatchThreads)

      'If mintBatchThreadsRunning = 0 Then
      '  mintBatchThreadsRunning = False
      'End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
    End Try

  End Sub

#End Region

#End Region

  ''' <summary>
  ''' Delete this job
  ''' </summary>
  ''' <remarks></remarks>
  Friend Sub Delete()

    Try
      Project.Container.DeleteJob(Me)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Sub

  Public Overrides Function ToString() As String

    Try
      Return DebuggerIdentifier()

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  Public Sub ClearSelectedBatchItems()
    Try
      For Each lobjBatch In Batches
        lobjBatch.SelectedItems.Clear()
        lobjBatch.SelectedItemList.Clear()
      Next
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      '   Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Function GetUpdateTable(ByVal lpUpdateCandidateItems As DataTable) As DataTable
    Try
      If JobBatchContainer IsNot Nothing Then
        Return JobBatchContainer.GetUpdateTable(Me, lpUpdateCandidateItems)
      Else
        Return CreateNewSourceDataTable()
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Function GetSourceRepository() As Repository
    Try
      Dim lobjRepository As Repository = GetSourceRepositoryFromContainer()

      If lobjRepository Is Nothing AndAlso Not String.IsNullOrEmpty(Me.SourceConnectionString) Then
        UpdateRepository(ExportScope.Source)
        lobjRepository = GetSourceRepositoryFromContainer()
      End If

      Return lobjRepository

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Function GetDestinationRepository() As Repository
    Try
      Dim lobjRepository As Repository = GetDestinationRepositoryFromContainer()

      If lobjRepository Is Nothing AndAlso Not String.IsNullOrEmpty(Me.DestinationConnectionString) Then
        UpdateRepository(ExportScope.Destination)
        lobjRepository = GetDestinationRepositoryFromContainer()
      End If

      Return lobjRepository

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Function GetProcessResultsSummary() As IProcessResultSummary
    Try

      Return Project.Container.GetProcessResultSummary(Me)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Function GetAllRepositories() As Repositories
    Try
      Dim lobjRepositories As Repositories = GetAllJobRepositoriesFromContainer()

      If lobjRepositories Is Nothing AndAlso Not String.IsNullOrEmpty(Me.DestinationConnectionString) Then
        UpdateRepository(ExportScope.Both)
        lobjRepositories = GetAllJobRepositoriesFromContainer()
      End If

      Return lobjRepositories

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

#End Region

#Region "Friend Methods"

  Friend Sub SetProject(ByVal lpProject As Project)

    Try
      mobjProject = lpProject

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      '  Re-throw the exception to the caller
      Throw
    End Try

  End Sub

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

      lobjIdentifierBuilder.AppendFormat("{0}", Operation)

      If Source IsNot Nothing Then
        lobjIdentifierBuilder.AppendFormat(";Source=({0})", Source.ToString)
      End If

      If Not String.IsNullOrEmpty(DestinationConnectionString) Then
        lobjIdentifierBuilder.AppendFormat(";Destination={0}", ContentSource.GetNameFromConnectionString(DestinationConnectionString))
      End If

      lobjIdentifierBuilder.AppendFormat(";BatchSize={0}", BatchSize)

      lobjIdentifierBuilder.AppendFormat(";TransformCount={0}", Transformations.Count)

      Return lobjIdentifierBuilder.ToString

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      Return lobjIdentifierBuilder.ToString
    End Try

  End Function

#End Region

#Region "Private Methods"

  Private Sub CountFiles(ByVal lpFolder As IFolder,
                         ByVal lpBackgroundWorker As BackgroundWorker,
                         ByVal lpBackgroundWorkerEventArgs As DoWorkEventArgs,
                         ByVal lpCancel As Boolean)

    Try

      If (lpFolder IsNot Nothing) Then

        For Each lobjResult As Core.FolderContent In lpFolder.Contents

          If (lpBackgroundWorker IsNot Nothing) Then

            If (lpBackgroundWorker.CancellationPending) Then
              lpBackgroundWorkerEventArgs.Cancel = True
              lpCancel = True
              Exit Sub
            End If

          End If

          mintTotalBatchCount += 1

        Next

        If (lpFolder.SubFolders IsNot Nothing) Then

          For Each lobjSubFolder As IFolder In lpFolder.SubFolders
            CountFiles(lobjSubFolder, lpBackgroundWorker, lpBackgroundWorkerEventArgs, lpCancel)
          Next

        End If

      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
    End Try

  End Sub

  ''' <summary>
  ''' Creates batches for all the contents in a folder
  ''' </summary>
  ''' <param name="lpFolder"></param>
  ''' <remarks></remarks>
  Private Sub CreateBatches(ByVal lpFolder As IFolder, ByVal lpBackgroundWorker As BackgroundWorker, ByVal lpBackgroundWorkerEventArgs As DoWorkEventArgs,
                            ByVal lpOldSavedBatches As Batches, ByRef lpCancel As Boolean)

    Try

      'LogSession.EnterMethod(Helper.GetMethodIdentifier(MethodBase.GetCurrentMethod()))

      Dim lstrName As String = String.Empty

      If (lpFolder IsNot Nothing) Then

        For Each lobjResult As Core.FolderContent In lpFolder.Contents

          If (lpBackgroundWorker IsNot Nothing) Then

            If (lpBackgroundWorker.CancellationPending) Then

              For Each lobjBatch As Batch In lpOldSavedBatches
                Batches.Add(lobjBatch)
              Next

              lpBackgroundWorkerEventArgs.Cancel = True
              lpCancel = True
              Exit Sub
            End If

          End If

          If (lobjResult.ID = String.Empty) Then
            Throw New Exception(String.Format("{0}: Folder ({1}) results must contain a valid Id", Reflection.MethodBase.GetCurrentMethod, Source.FolderPath))
          End If

          lstrName = lobjResult.Name

          Dim lobjCurrentBatch As Batch = GetCurrentWorkingBatch()
          Dim lobjBatchItem As New BatchItem(lobjResult.ID, lstrName, lobjCurrentBatch)

          lobjCurrentBatch.AddItem(lobjBatchItem)
          mintCurrentBatchCount += 1
          mintRunningCount += 1
          mstrPreviousSourceConnectionString = mstrCurrentSourceConnectionString
          'RaiseEvent BatchItemsCreated(Me.Name, mintRunningCount, mintTotalBatchCount)
          RaiseEvent BatchItemsCreated(Me, New BatchItemCreatedEventArgs(Me.Name, mintRunningCount, mintTotalBatchCount))
        Next

        If (lpFolder.SubFolders IsNot Nothing) Then

          For Each lobjSubFolder As IFolder In lpFolder.SubFolders
            CreateBatches(lobjSubFolder, lpBackgroundWorker, lpBackgroundWorkerEventArgs, lpOldSavedBatches, lpCancel)
          Next

        End If

        CurrentItemsProcessed = 0

      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      'Throw
    Finally
      'LogSession.LeaveMethod(Helper.GetMethodIdentifier(MethodBase.GetCurrentMethod()))
    End Try

  End Sub

  'Private Sub InitializeLogSession()
  '  Try
  '    'mobjLogSession = SiAuto.Si.AddSession(String.Format("Batch: {0}", Id))
  '    mobjLogSession = SiAuto.Si.AddSession("Job")
  '    mobjLogSession.Color = System.Drawing.Color.LightSteelBlue
  '  Catch ex As Exception
  '    ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
  '    ' Re-throw the exception to the caller
  '    Throw
  '  End Try
  'End Sub

  Friend Sub InitializeTimedStatusPusher()
    Try

      If stateTimer Is Nothing Then

        ' For automatically updating the status of this job

        ' Create an event to signal the timeout count threshold in the 
        ' timer callback. 
        mobjAutoEvent = New AutoResetEvent(False)
        'CType(mobjInstance, ProjectCatalog).mobjStatusChecker = New NodeStatusChecker(mobjInstance.ThisNode, 10)

        ' Create an inferred delegate that invokes methods for the timer. 
        mobjTimerCallback = AddressOf RefreshStatisticsAndPushToFirebase

        ' Create a timer that signals the delegate to invoke 
        ' PushFirebaseUpdate after one second, and every minute 
        ' thereafter.
        stateTimer = New Timer(mobjTimerCallback, mobjAutoEvent, 1000, 1000 * ConnectionSettings.Instance.JobStatusRefreshInterval)

      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Friend Sub DisableStatusPusher()
    Try
      If stateTimer IsNot Nothing Then
        stateTimer = Nothing
        mobjTimerCallback = Nothing
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Friend Async Sub RefreshStatisticsAndPushToFirebaseAsync()
    Try
      Dim lobjTask As Task =
      Task.Factory.StartNew(Sub()
                              RefreshStatisticsAndPushToFirebase()
                            End Sub)
      Await lobjTask

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Friend Async Sub PushFirebaseUpdateAsync()
    Try
      Dim lobjTask As Task =
      Task.Factory.StartNew(Sub()
                              PushFirebaseUpdate()
                            End Sub)
      Await lobjTask

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Friend Sub RefreshStatisticsAndPushToFirebase()
    Try
      If Not IsDisposed OrElse Not String.IsNullOrEmpty(Me.Id) Then
        GetWorkSummaryCounts()
        PushFirebaseUpdate()
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
    End Try
  End Sub

  Friend Sub PushFirebaseUpdate()
    Try
      If Not ConnectionSettings.Instance.DisableNotifications Then
        Dim lstrCatalogId As String = ProjectCatalog.Instance.Id

        Dim lstrPutPath As String = String.Format("{0}catalogs/{1}/areas/{2}/{3}/jobs/{4}",
              FIREBASE_APP_URL, lstrCatalogId, Me.Project.AreaName, Me.Project.Name.Replace(".", "_"), Me.Name.Replace(".", "_"))

        Dim lobjCurrentJobInfo As IFirebasePusher
        lobjCurrentJobInfo = New JobInfo(Me)

        Static lstrLastProjectInfoJson As String
        Dim lstrCurrentProjectInfoJson As String

        ' We only want to push an update to Firebase if there is an actual change.

        If String.IsNullOrEmpty(lstrLastProjectInfoJson) Then
          ' This is the first time through, go ahead and push it.
          lstrLastProjectInfoJson = lobjCurrentJobInfo.ToFireBaseJson()
          'lobjFirebase.Put(lstrPutPath, lstrLastProjectInfoJson)
          lobjCurrentJobInfo.UpdateFirebase(lstrPutPath, lstrLastProjectInfoJson)
        Else
          ' Get the current information so we can compare it to last time
          lstrCurrentProjectInfoJson = lobjCurrentJobInfo.ToFireBaseJson()

          If Not lstrCurrentProjectInfoJson.Equals(lstrLastProjectInfoJson) Then
            ' The information has changed, lets push it.
            lstrLastProjectInfoJson = lstrCurrentProjectInfoJson
            'lobjFirebase.Put(lstrPutPath, lstrLastProjectInfoJson)
            lobjCurrentJobInfo.UpdateFirebase(lstrPutPath, lstrLastProjectInfoJson)
          End If
        End If
      End If

    Catch ex As Exception
      'ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Just log it and move on, we don't want this to cause a problem
    End Try
  End Sub

  Public Function GetBatch(ByVal lpId As String) As Batch

    Try
      Return Batches(lpId)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  Private Function GetHasOperationsToRunBeforeJobBegin() As Boolean
    Try
      If Process Is Nothing Then
        Return False
      End If

      If Process.RunBeforeJobBegin IsNot Nothing Then
        Return True
      Else
        Return False
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Sub SaveBatchItemsProcessed(lpBatchId As String)
    Try
      Dim lobjBatch As Batch = GetBatch(lpBatchId)
      If lobjBatch.CurrentItemsProcessed > 0 Then
        ' Dim llngCurrentItemsCount As Long = JobBatchContainer.GetProcessedItemsCount(Me.Project)
        ' JobBatchContainer.SetProcessedItemsCount(Me.Project, llngCurrentItemsCount + lobjBatch.CurrentItemsProcessed)
        Dim llngCurrentItemsCount As Long = JobBatchContainer.GetProcessedItemsCount(Me)
        JobBatchContainer.SetProcessedItemsCount(Me, lobjBatch.CurrentItemsProcessed)
        Me.CurrentItemsProcessed -= lobjBatch.CurrentItemsProcessed
        lobjBatch.CurrentItemsProcessed = 0
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Sub Save()
    Try
      ' If any batches were allocated but not provisioned, 
      ' we need to remove them before commiting the changes to the database.
      Dim lobjLastBatch As Batch = Batches.LastOrDefault
      If lobjLastBatch IsNot Nothing AndAlso lobjLastBatch.NewItemCount = 0 Then
        Batches.Remove(lobjLastBatch)
      End If

      ' Save the job to the database.
      JobBatchContainer.SaveJob(Me)

      If ((Not String.IsNullOrEmpty(Me.Configuration.JobId)) AndAlso (String.Equals(Me.Configuration.JobId, Me.Id) = False)) Then
        mstrId = Me.Configuration.JobId
      End If

      If Me.Project IsNot Nothing Then
        If Me.Project.Jobs.Contains(Me.Name) = False Then
          Me.Project.Jobs.Add(Me)
        End If
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Friend Sub SaveBatch(ByVal lpBatch As Batch)
    Try
      JobBatchContainer.SaveBatch(lpBatch)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      '   Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Friend Sub SaveBatches()
    Try
      ' If any batches were allocated but not provisioned, 
      ' we need to remove them before commiting the changes to the database.
      Dim lobjLastBatch As Batch = Batches.LastOrDefault
      If lobjLastBatch IsNot Nothing AndAlso lobjLastBatch.NewItemCount = 0 Then
        Batches.Remove(lobjLastBatch)
      End If

      '	Save the latest batches and batch items
      JobBatchContainer.SaveBatches(Me)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      '   Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Sub SaveJobRelationships()
    Try
      For Each lobjRelationship As JobRelationship In Relationships
        JobBatchContainer.SaveJobRelationship(lobjRelationship)
      Next
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Sub SetLastWorkSummary(lpWorkSummary As IWorkSummary)
    Try
      mobjLastWorkSummary = lpWorkSummary
      mblnIsCompleted = lpWorkSummary.IsCompleted
      RaiseEvent WorkSummaryUpdated(Me, New WorkSummaryEventArgs(mobjLastWorkSummary))
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Sub RefreshBatches()
    Try
      mobjBatches = JobBatchContainer.GetBatches(Me)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  ''' <summary>
  ''' Verifies the caached batch ids against the database to determine whether or not we have current batch information.
  ''' </summary>
  ''' <returns>True if all the batch ids we have in memory match those in the project database, otherwise false.</returns>
  ''' <remarks></remarks>
  Public Function VerifyBatchIds() As Boolean
    Try

      Dim lobjCurrentBatchIds As IList(Of String) = JobBatchContainer.GetBatchIds(Me.Id)

      For Each lobjBatch As Batch In Me.Batches
        If Not lobjCurrentBatchIds.Contains(lobjBatch.Id) Then
          Return False
        End If
      Next

      Return True

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Function CheckForCompletion(lpForceRefresh As Boolean)
    Try
      Dim lobjCurrentWorkSummary As IWorkSummary

      If lpForceRefresh Then
        lobjCurrentWorkSummary = GetWorkSummaryCounts()
      Else
        lobjCurrentWorkSummary = GetCachedWorkSummaryCounts()
      End If

      mblnIsCompleted = lobjCurrentWorkSummary.IsCompleted

      Return mblnIsCompleted

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Friend Function GetCurrentWorkingBatch() As Batch

    Try
      ' 'LogSession.EnterMethod(Helper.GetMethodIdentifier(Reflection.MethodBase.GetCurrentMethod))
      'Creates batches as needed
      If (mobjBatches.Count = 0) Then

        'Create a batch for this case
        ' lobjBatch.Name = "Batch " & (mintCurrentWorkingBatch + 1).ToString("000")
        Dim lobjBatch As New Batch(Me) With {
          .Number = mintCurrentWorkingBatch + 1
        }
        mobjBatches.Add(lobjBatch)
        mintCurrentBatchCount = 1
        mintCurrentBatchCount = 0

      Else
        If mintCurrentBatchCount = 0 Then
          mintCurrentBatchCount = mobjBatches.Last.GetTotalItemCount
        End If
        ' If mintCurrentBatchCount >= mintBatchSize Then
        If mintCurrentBatchCount >= BatchSize Then
          mobjBatches.Last.CommitBatchItems()
          'SaveBatches()
          'LogSession.LogMessage("Job({0}).GetCurrentWorkingBatch.SaveBatch({1})",
          'Me.Name, mobjBatches.Last.Name)
          SaveBatch(mobjBatches.Last)

          ' lobjBatch.Name = String.Format("Batch {0}", (mobjBatches.Count + 1).ToString("000"))
          Dim lobjBatch As New Batch(Me) With {
            .Number = mobjBatches.Count + 1
          }
          mobjBatches.Add(lobjBatch)
          mintCurrentWorkingBatch += 1
          mintCurrentBatchCount = 0
          RaiseEvent BatchCreated(Me, New WorkEventArgs(Me, lobjBatch))
          'End If
        End If
      End If

      Return Batches.Last

      '  'ElseIf (mintCurrentBatchCount >= mintBatchSize) Or (SourceConnectionString <> mstrPreviousSourceConnectionString) Then
      'ElseIf (mobjBatches.Last.GetTotalItemCount >= mintBatchSize) Or (SourceConnectionString <> mstrPreviousSourceConnectionString) Then
      '  mobjBatches(mintCurrentWorkingBatch).CommitBatchItems()
      '  ' mobjBatches(mintCurrentWorkingBatch).Dispose()

      '  Dim lobjBatch As New Batch(Me)
      '  lobjBatch.Name = "Batch " & (mintCurrentWorkingBatch + 2).ToString("000")
      '  mobjBatches.Add(lobjBatch)
      '  mintCurrentWorkingBatch += 1
      '  mintCurrentBatchCount = 0
      'End If

      'Return mobjBatches(mintCurrentWorkingBatch)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    Finally
      ' 'LogSession.LeaveMethod(Helper.GetMethodIdentifier(Reflection.MethodBase.GetCurrentMethod))
    End Try

  End Function

  Private Function GetFolderByID(ByVal lpId As String,
                                ByVal lpConnectionString As String) As IFolder

    Dim lobjExplorer As IExplorer
    Dim lobjFolder As IFolder = Nothing
    Dim lobjProvider As CProvider

    Try
      lobjProvider = GetProvider(lpConnectionString)

      If (lobjProvider IsNot Nothing) Then

        If lobjProvider.SupportsInterface(ProviderClass.Explorer) Then
          lobjExplorer = CType(lobjProvider, IExplorer)
          lobjFolder = lobjExplorer.GetFolderByID(lpId, 100, -1)

          If (lobjFolder IsNot Nothing) Then
            lobjFolder.Provider = lobjProvider
            lobjFolder.ContentSource = lobjProvider.ContentSource
          End If

          Return lobjFolder

        End If

      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
    End Try

    Return Nothing

  End Function

  Private Function GetProvider(ByVal lpConnectionString As String) As CProvider

    Try

      Dim mobjProvider As CProvider = Nothing
      Dim mobjContentSource As ContentSource
      mobjContentSource = New ContentSource(lpConnectionString)

      If (mobjContentSource IsNot Nothing) Then

        If (mobjContentSource.Provider(False) IsNot Nothing) Then
          mobjProvider = mobjContentSource.Provider
          mobjProvider.Connect(mobjContentSource)
          Return mobjProvider
        End If

      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
    End Try

    Return Nothing

  End Function

  ''' <summary>
  ''' Retrieves the repository from the live system.
  ''' </summary>
  ''' <param name="lpConnectionString">The content source 
  ''' connection used to connect to the system.</param>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Private Function GetLiveRepository(lpConnectionString As String)
    Try

      Dim lobjContentSource As ContentSource = Nothing
      Dim lobjRepository As Repository = Nothing

      If Not String.IsNullOrEmpty(lpConnectionString) Then
        lobjContentSource = New ContentSource(Me.SourceConnectionString)
        If lobjContentSource.Provider.SupportsInterface(ProviderClass.Classification) Then
          lobjRepository = New Repository(lobjContentSource)
        End If
      End If

      Return lobjRepository

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function GetSourceRepositoryFromContainer() As Repository
    Try
      If Me.JobBatchContainer IsNot Nothing Then
        Return Me.JobBatchContainer.GetSourceRepository(Me)
      Else
        Return Nothing
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function GetDestinationRepositoryFromContainer() As Repository
    Try
      If Me.JobBatchContainer IsNot Nothing Then
        Return Me.JobBatchContainer.GetDestinationRepository(Me)
      Else
        Return Nothing
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function GetAllJobRepositoriesFromContainer() As Repositories
    Try
      If Me.JobBatchContainer IsNot Nothing Then
        Return Me.JobBatchContainer.GetRepositories(Me)
      Else
        Return Nothing
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  'Private Sub SyncronizeWithConfiguration()
  '  Try
  '    SyncronizeWithConfiguration(Configuration)
  '  Catch ex As Exception
  '    ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '    ' Re-throw the exception to the caller
  '    Throw
  '  End Try
  'End Sub

  'Private Sub SyncronizeWithConfiguration(lpConfiguration As JobConfiguration)
  '  Try
  '    With lpConfiguration
  '      Name = .Name
  '      Description = .Description
  '      Source = .Source
  '      If (String.IsNullOrEmpty(SourceConnectionString)) AndAlso (Source IsNot Nothing) Then
  '        mstrCurrentSourceConnectionString = Source.SourceConnectionString
  '      End If
  '      ItemsLocation = .ItemsLocation
  '      BatchSize = .BatchSize
  '      Operation = .OperationName
  '      DestinationConnectionString = .DestinationConnectionString
  '      ContentStorageType = .ContentStorageType
  '      DeclareAsRecordOnImport = .DeclareAsRecordOnImport
  '      DeclareRecordConfiguration = .DeclareRecordConfiguration
  '      Transformations = .Transformations
  '      TransformationSourcePath = .TransformationSourcePath
  '      DocumentFilingMode = .DocumentFilingMode
  '      LeadingDelimiter = .LeadingDelimiter
  '      BasePathLocation = .BasePathLocation
  '      FolderDelimiter = .FolderDelimiter
  '      Process = .Process
  '    End With
  '  Catch ex As Exception
  '    ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '    ' Re-throw the exception to the caller
  '    Throw
  '  End Try
  'End Sub

#End Region

#Region "Event Handlers"

  Private Sub mobjConfiguration_PropertyChanged(sender As Object,
                                                e As PropertyChangedEventArgs) _
                                              Handles mobjConfiguration.PropertyChanged
    Try
      If TypeOf sender Is JobConfiguration Then
        If Not Helper.CallStackContainsMethodName("OpenProject", ".ctor") Then
          ' SyncronizeWithConfiguration(sender)
          Save()
        End If
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Sub mobjJobSource_PropertyChanged(sender As Object,
                                            e As System.ComponentModel.PropertyChangedEventArgs) _
          Handles MobjJobSource.PropertyChanged

    Try

      If e.PropertyName.Equals("SourceConnectionString") Then
        mstrPreviousSourceConnectionString = mstrCurrentSourceConnectionString
        mstrCurrentSourceConnectionString = Source.SourceConnectionString
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Sub

#End Region

#Region "Public Enums"

#End Region

#Region "NotifyObject Class"

  Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

  Friend Overridable Sub OnPropertyChanged(ByVal sProp As String)
    Try
      RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(sProp))
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Friend Overridable Sub OnJobBegin(args As WorkEventArgs)
    Try
      If IsRunning Then
        Exit Sub
      End If
      If args.OriginatingBatch IsNot Nothing Then
        RaiseEvent BeforeJobBegin(args.OriginatingBatch, args)
      Else
        RaiseEvent BeforeJobBegin(Me, args)
      End If
      'mblnIsRunning = True
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Friend Function AreAnyBatchesRunning() As Boolean
    Try
      For Each lobjBatch As Batch In Me.Batches
        If lobjBatch.IsRunning Then
          Return True
        End If
      Next

      Return False

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Friend Overridable Sub OnJobCancel(args As WorkCancelledEventArgs)
    Try
      'mblnIsRunning = False
      If args.OriginatingBatch IsNot Nothing Then
        ' RaiseEvent JobCancelled(args.OriginatingBatch, args)
        RaiseEvent JobCancelled(args.OriginatingBatch, args)
      Else
        RaiseEvent JobCancelled(Me, args)
      End If
      GetWorkSummaryCounts()
      Cleanup()
      DisableStatusPusher()
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Friend Sub OnBatchItemsCreated(sender As Object, args As BatchItemCreatedEventArgs)
    Try
      RaiseEvent BatchItemsCreated(sender, args)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Friend Sub OnBatchCreationUpdate(sender As Object, args As BatchItemCreatedEventArgs)
    Try
      RaiseEvent BatchCreationUpdate(sender, args)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Friend Overridable Sub OnAfterJobComplete(args As WorkEventArgs)
    Try
      If ((Me.Process IsNot Nothing) AndAlso (Me.Process.RunAfterJobComplete IsNot Nothing)) Then
        Dim lobjProxyItem As New JobItemProxy(Me)
        Me.Process.RunAfterJobComplete.Execute(lobjProxyItem)
        Me.Process.OnComplete(New OperableEventArgs(args.OriginatingBatch.Process, lobjProxyItem))
      End If
      RunBeforeJobBeginCount = 0

      mblnIsCompleted = True

      If args.OriginatingBatch IsNot Nothing Then
        RaiseEvent AfterJobComplete(args.OriginatingBatch, args)
      Else
        RaiseEvent AfterJobComplete(Me, args)
      End If
      GetWorkSummaryCounts()
      Cleanup()
      DisableStatusPusher()
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    Finally
      'mblnIsRunning = False
    End Try
  End Sub

  Friend Overridable Sub OnJobError(args As WorkErrorEventArgs)
    Try
      If args.OriginatingBatch IsNot Nothing Then
        RaiseEvent JobError(args.OriginatingBatch, args)
      Else
        RaiseEvent JobError(Me, args)
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Friend Overridable Sub OnJobInvoked(args As WorkInvokedEventArgs)
    Try
      If args.TargetHandler.Equals("JOB", StringComparison.CurrentCultureIgnoreCase) Then
        ' Run the job
        Execute()
      Else
        RaiseEvent JobInvoked(Me, args)
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

#Region "IDisposable Support"

  Private mblnIsDisposed As Boolean ' To detect redundant calls

  <XmlIgnore(), JsonIgnore()>
  Public ReadOnly Property IsDisposed As Boolean
    Get
      Try
        Return mblnIsDisposed
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  ' IDisposable
  Protected Overridable Sub Dispose(disposing As Boolean)
    Try
      If Not Me.mblnIsDisposed Then
        If disposing Then
          ' DISPOSETODO: dispose managed state (managed objects).
          mstrId = Nothing
          mstrDescription = Nothing
          mstrName = Nothing
          mstrCurrentSourceConnectionString = Nothing
          mstrPreviousSourceConnectionString = Nothing
          mobjProject = Nothing
          mobjContainer?.Dispose()
          mobjSourceRepository = Nothing
          mobjDestinationRepository = Nothing
          mblnIsCancelled = False
          mblnIsCompleted = False
          'mblnIsRunning = False
          mstrCancellationReason = Nothing
          mobjTag = Nothing
          If mobjResetEvent IsNot Nothing Then
            mobjResetEvent.Dispose()
            mobjResetEvent = Nothing
          End If
          mobjConfiguration = Nothing
          mobjLastWorkSummary = Nothing
          For Each lobjBatch In mobjBatches
            lobjBatch.Dispose()
          Next
          mobjBatches = Nothing
        End If

        ' DISPOSETODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
        ' DISPOSETODO: set large fields to null.
      End If
      Me.mblnIsDisposed = True
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  ' DISPOSETODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
  'Protected Overrides Sub Finalize()
  '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
  '    Dispose(False)
  '    MyBase.Finalize()
  'End Sub

  ' This code added by Visual Basic to correctly implement the disposable pattern.
  Public Sub Dispose() Implements IDisposable.Dispose
    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
    Dispose(True)
    GC.SuppressFinalize(Me)
  End Sub
#End Region

End Class
