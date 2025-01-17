'---------------------------------------------------------------------------------
' <copyright company="ECMG">
'     Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'     Copying or reuse without permission is strictly forbidden.
' </copyright>
'---------------------------------------------------------------------------------

#Region "Imports"

Imports System.Data
Imports System.Reflection
Imports System.Runtime.Serialization
Imports System.Text
Imports System.Threading
Imports System.Xml.Serialization
Imports Documents
Imports Documents.Core
Imports Documents.Providers
Imports Documents.Transformations
Imports Documents.Utilities
Imports Operations
Imports Operations.OperationEnumerations

#End Region

<Serializable()> <DataContract()> <DebuggerDisplay("{DebuggerIdentifier(),nq}")> Public Class Batch
  Implements Documents.Core.IDescription
  Implements IDisposable
  Implements ICloneable
  Implements IItemParent

#Region "Class Variables"

  Private mstrId As String = String.Empty
  Private mstrDescription As String = String.Empty
  Private mstrName As String = String.Empty
  Private mintNumber As Nullable(Of Integer)
  Private mobjItemsLocation As ItemsLocation

  'Assumption is batch will always contain the items with the same source and dest connection strings
  Private mstrDestinationConnectionString As String = String.Empty
  Private WithEvents MobjDestinationContentSource As Providers.ContentSource
  Private mstrSourceConnectionString As String = String.Empty
  Private WithEvents MobjSourceContentSource As Providers.ContentSource
  Private mobjTransformations As Transformations.TransformationCollection
  Private menuFilingMode As Core.FilingMode = Core.FilingMode.BaseFolderPathOnly
  Private mblnLeadingDelimiter As Boolean = True
  Private mstrFolderDelimiter As String = "/"
  Private menumBasePathLocation As Migrations.ePathLocation = Migrations.ePathLocation.Front
  Private mstrAssignedTo As String = String.Empty
  Private mstrExportPath As String = String.Empty
  Private WithEvents MobjBatchContainer As Container = Nothing
  Private mstrOperationType As String = String.Empty
  Private mobjProcess As IProcess = Nothing
  Private disposedValue As Boolean = False    ' To detect redundant calls
  Private menuStorageType As Core.Content.StorageTypeEnum = Core.Content.StorageTypeEnum.Reference
  Private mblnDeclareAsRecordOnImport As Boolean = False
  Private mobjDeclareRecordConfiguration As DeclareRecordConfiguration = Nothing
  Private lstrMachineName As String = String.Empty
  Private mblnIsCancelled As Boolean = False
  Private mblnIsRunning As Boolean = False
  Private mblnIsLocked As Boolean = False

  'Private mobjResetEvents() As ManualResetEvent
  Private mobjDoneEvent As ManualResetEvent
  Private mintNumBusy As Integer = 0
  Private mobjBatchWorker As BaseBatchWorker
  Private mobjIdArrayList As ArrayList = Nothing
  Private WithEvents MobjJob As Job 'This is a reference to this batch's parent job
  Private WithEvents MobjSelectedItems As New BatchItems
  Private mobjSelectedItemList As New ArrayList
  Private mintNewItemCount As Integer
  Private mblnJobCancelled As Boolean = False

  Private mintFailed As Integer = 0
  Private mintSuccess As Integer = 0
  Private mintNotProcessed As Integer = 0
  Private mdblAvgProcessingTime As Double = 0
  Private mdblLocalProcessingTime As Double = 0
  Private mdatCurrentStartTime As DateTime
  Private mdatLastItemFinishTime As DateTime
  Private mintCurrentItemsProcessed As Integer
  Private mobjCurrentWorkSummary As WorkSummary = Nothing

  'Private mobjLogSession As Gurock.SmartInspect.Session

  'Public Event ItemProcessed(ByVal lpBatchWorker As BatchWorker, ByVal lpItemId As String, ByVal lpTitle As String, ByVal lpItemCount As Integer, ByVal lpTotalItemCount As Integer)
  Public Event BeforeBatchBegin(ByVal lpBatchWorker As BatchWorker)
  ' Public Event ItemProcessed(ByVal lpBatchWorker As BatchWorker, ByVal lpItemId As String, ByVal lpTitle As String, ByVal lpBatchSummary As WorkSummary)
  Public Event ItemProcessed As BatchItemProcessedEventHandler
  Public Event BatchCompleted(ByVal lpBatchWorker As BatchWorker)
  Public Event BatchError(ByVal lpBatchWorker As BatchWorker)

#End Region

#Region "Event Delegates"

  ''' <summary>Delegate event handler for the BatchCancelled event.</summary>
  Public Delegate Sub JobCancelledEventHandler(ByVal sender As Object, ByRef e As WorkCancelledEventArgs)
  Public Delegate Sub BatchItemProcessedEventHandler(ByVal sender As Object, ByRef e As BatchItemProcessedEventArgs)

#End Region

#Region "Public Events"

  Public Event JobCancelled As JobCancelledEventHandler

#End Region

#Region "Public Properties"

  <XmlAttribute()>
  Public Property Id() As String Implements IItemParent.Id
    Get
      Return mstrId
    End Get
    Set(ByVal value As String)

      Try

        If (Helper.IsDeserializationBasedCall = True) Then
          mstrId = value

        Else
          Throw New InvalidOperationException("Although Id in the Batch Class is a public property, set operations are not allowed.  Treat property as read-only.")
        End If

      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try

    End Set
  End Property

  Public ReadOnly Property CurrentWorkSummary() As WorkSummary
    Get
      Try
        Return mobjCurrentWorkSummary
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  <XmlAttribute()>
  Public Property Description() As String _
                          Implements Core.IDescription.Description
    Get
      Return mstrDescription
    End Get
    Set(ByVal value As String)
      mstrDescription = value
    End Set
  End Property

  <XmlAttribute()>
  Public Property Name() As String _
                          Implements Core.IDescription.Name, Core.INamedItem.Name, IItemParent.Name
    Get
      Return mstrName
    End Get
    Set(ByVal value As String)
      Try
        mstrName = value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  <XmlIgnore()>
  Public Property Number As Nullable(Of Integer)
    Get
      Try
        Return mintNumber
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Friend Set(value As Nullable(Of Integer))
      Try
        mintNumber = value
        If value.HasValue Then
          ' mstrName = String.Format("Batch {0}", value.Value.ToString("000"))
          mstrName = String.Format("Batch {0}", value.Value.ToString())
        Else
          mstrName = "Batch 0"
        End If
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Public Property ItemsLocation() As ItemsLocation
    Get
      Return mobjItemsLocation
    End Get
    Set(ByVal value As ItemsLocation)
      mobjItemsLocation = value

      ' <Removed by: Ernie at: 10/17/2013-10:55:26 AM on machine: ERNIE-THINK>
      '       Try
      '         mobjBatchContainer = Container.CreateContainer(mobjItemsLocation)
      ' 
      '       Catch ex As Exception
      '         ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      '         ' Re-throw the exception to the caller
      '         Throw
      '       End Try
      ' </Removed by: Ernie at: 10/17/2013-10:55:26 AM on machine: ERNIE-THINK>

    End Set
  End Property

  Public ReadOnly Property NewItemCount As Integer
    Get
      Try
        Return mintNewItemCount
      Catch Ex As Exception
        ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public Property AssignedTo() As String
    Get
      Return mstrAssignedTo
    End Get
    Set(ByVal value As String)
      mstrAssignedTo = value
    End Set
  End Property

  Public Property ExportPath() As String Implements IItemParent.ExportPath
    Get
      Return mstrExportPath
    End Get
    Set(ByVal value As String)
      mstrExportPath = value
    End Set
  End Property

  Public Property Transformations() As TransformationCollection Implements IItemParent.Transformations
    Get
      Return mobjTransformations
    End Get
    Set(ByVal value As TransformationCollection)
      mobjTransformations = value
    End Set
  End Property

  Public Property Operation() As String
    Get
      Return mstrOperationType
    End Get
    Set(ByVal value As String)
      mstrOperationType = value
    End Set
  End Property

  Public Property Process As IProcess
    Get
      Return mobjProcess
    End Get
    Set(value As IProcess)
      mobjProcess = value
    End Set
  End Property

  Public Property DestinationConnectionString() As String
    Get
      Return mstrDestinationConnectionString
    End Get
    Set(ByVal value As String)
      mstrDestinationConnectionString = value
    End Set
  End Property

  Public Property SourceConnectionString() As String
    Get
      Return mstrSourceConnectionString
    End Get
    Set(ByVal value As String)
      mstrSourceConnectionString = value
    End Set
  End Property

  Public Property ContentStorageType() As Core.Content.StorageTypeEnum
    Get
      Return menuStorageType
    End Get
    Set(ByVal value As Core.Content.StorageTypeEnum)
      menuStorageType = value
    End Set
  End Property

  Public ReadOnly Property SourceContentSource() As Providers.ContentSource
    Get

      Try

        If (MobjSourceContentSource Is Nothing) Then

          If Not String.IsNullOrEmpty(SourceConnectionString) Then
            MobjSourceContentSource = New Providers.ContentSource(SourceConnectionString)
          End If

        End If

        Return MobjSourceContentSource

      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try

    End Get
  End Property

  Public ReadOnly Property DestinationContentSource() As Providers.ContentSource
    Get

      Try

        If (MobjDestinationContentSource Is Nothing) Then

          If Not String.IsNullOrEmpty(DestinationConnectionString) Then
            MobjDestinationContentSource = New Providers.ContentSource(DestinationConnectionString)
          End If

        End If

        Return MobjDestinationContentSource

      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try

    End Get
  End Property

  Public ReadOnly Property BatchContainer() As Container
    Get
      Try

        If MobjBatchContainer Is Nothing Then
          If Job IsNot Nothing AndAlso Job.Project IsNot Nothing Then
            MobjBatchContainer = Job.Project.Container
          Else
            Throw New InvalidOperationException("Unable to initialize batch container, there is no current job and project reference.")
          End If
        End If

        Return MobjBatchContainer

      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public Property DeclareAsRecordOnImport() As Boolean
    Get
      Return mblnDeclareAsRecordOnImport
    End Get
    Set(ByVal value As Boolean)
      mblnDeclareAsRecordOnImport = value
    End Set
  End Property

  Public Property DeclareRecordConfiguration() As DeclareRecordConfiguration
    Get
      Return mobjDeclareRecordConfiguration
    End Get
    Set(ByVal value As DeclareRecordConfiguration)
      mobjDeclareRecordConfiguration = value
    End Set
  End Property

  Public Property DocumentFilingMode() As Core.FilingMode
    Get
      Return menuFilingMode
    End Get
    Set(ByVal value As Core.FilingMode)
      menuFilingMode = value
    End Set
  End Property

  Public Property LeadingDelimiter() As Boolean
    Get
      Return mblnLeadingDelimiter
    End Get
    Set(ByVal value As Boolean)
      mblnLeadingDelimiter = value
    End Set
  End Property

  Public Property BasePathLocation() As Migrations.ePathLocation
    Get
      Return menumBasePathLocation
    End Get
    Set(ByVal value As Migrations.ePathLocation)
      menumBasePathLocation = value
    End Set
  End Property

  Public Property FolderDelimiter() As String
    Get
      Return mstrFolderDelimiter
    End Get
    Set(ByVal value As String)
      mstrFolderDelimiter = value
    End Set
  End Property

  Public ReadOnly Property Job() As Job
    Get
      Try
        Return MobjJob
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property IsCancelled As Boolean
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

  Public ReadOnly Property IsLocked As Boolean
    Get
      Try
        Return mblnIsLocked
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property IsRunning As Boolean
    Get
      Try
        Return mblnIsRunning
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property MachineName() As String
    Get

      Try

        If (lstrMachineName = String.Empty) Then
          lstrMachineName = System.Net.Dns.GetHostName()
        End If

        Return lstrMachineName

      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try

    End Get
  End Property

  Public ReadOnly Property SelectedItems As BatchItems
    Get
      Return MobjSelectedItems
    End Get
  End Property

  Public ReadOnly Property SelectedItemList As ArrayList
    Get
      Return mobjSelectedItemList
    End Get
  End Property

  Public ReadOnly Property FailureCount As Integer
    Get
      Try
        Return mintFailed
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property SuccessCount As Integer
    Get
      Try
        Return mintSuccess
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property NotProcessedCount As Integer
    Get
      Try
        Return mintNotProcessed
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property AvgProcessingTime As Double
    Get
      Try
        Return mdblAvgProcessingTime
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property CurrentStartTime As DateTime
    Get
      Try
        Return mdatCurrentStartTime
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property LastItemFinishTime As DateTime
    Get
      Try
        Return mdatLastItemFinishTime
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property CurrentProcessingTime As TimeSpan
    Get
      Try
        Return GetCurrentProcessingTime()
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public ReadOnly Property CurrentProcessingTimeString As String
    Get
      Try
        Return GetCurrentProcessingTimeString()
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

#End Region

#Region "Friend Properties"

  Friend Property CurrentItemsProcessed As Integer
    Get
      Try
        Return mintCurrentItemsProcessed
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As Integer)
      Try
        mintCurrentItemsProcessed = value
        Me.Job.CurrentItemsProcessed += value
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Set
  End Property

  Friend Property Locked As Boolean
    Get
      Try
        Return mblnIsLocked
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
    Set(value As Boolean)
      Try
        mblnIsLocked = value
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

  Friend ReadOnly Property Worker As BaseBatchWorker
    Get
      Try
        Return mobjBatchWorker
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
      lstrMachineName = System.Net.Dns.GetHostName()
      'InitializeLogSession()

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller      Throw
    End Try

  End Sub

  Public Sub New(ByVal lpName As String,
                 ByVal lpDescription As String)

    Try
      mstrId = Guid.NewGuid.ToString
      mstrName = lpName
      mstrDescription = lpDescription
      lstrMachineName = System.Net.Dns.GetHostName()
      'InitializeLogSession()

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller     Throw
    End Try

  End Sub

  Public Sub New(ByVal lpItemsLocation As ItemsLocation)

    Try
      mobjItemsLocation = lpItemsLocation
      MobjBatchContainer = Container.CreateContainer(mobjItemsLocation)
      lstrMachineName = System.Net.Dns.GetHostName()
      'InitializeLogSession()

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller      Throw
    End Try

  End Sub

  Public Sub New(ByVal lpJob As Job)

    Try
      mstrId = Guid.NewGuid.ToString
      lstrMachineName = System.Net.Dns.GetHostName()
      SetJob(lpJob)
      'InitializeLogSession()

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Sub

  Public Sub New(ByVal lpId As String, ByVal lpName As String, lpAssignedTo As String, lpJobConfiguration As Configuration.JobConfiguration)
    Try

      mstrId = lpId
      mstrName = lpName
      mstrAssignedTo = lpAssignedTo

      With lpJobConfiguration
        mstrDescription = .Description
        ItemsLocation = .ItemsLocation
        Transformations = .Transformations
        mstrOperationType = .OperationName
        If .Process IsNot Nothing Then
          mobjProcess = .Process.Clone
        End If
        mstrDestinationConnectionString = .DestinationConnectionString
        If ((.Source IsNot Nothing) AndAlso (.Source.SourceConnectionString IsNot Nothing)) Then
          mstrSourceConnectionString = .Source.SourceConnectionString
        End If
        menuStorageType = .ContentStorageType
        mblnDeclareAsRecordOnImport = .DeclareAsRecordOnImport
        mobjDeclareRecordConfiguration = .DeclareRecordConfiguration
        menuFilingMode = .DocumentFilingMode
        mblnLeadingDelimiter = .LeadingDelimiter
        menumBasePathLocation = .BasePathLocation
        mstrFolderDelimiter = .FolderDelimiter
      End With

      'InitializeLogSession()

    Catch Ex As Exception
      ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Sub New(ByVal lpId As String,
                 ByVal lpName As String,
                 ByVal lpDescription As String,
                 ByVal lpOperationType As String,
                 ByVal lpProcess As IProcess,
                 ByVal lpAssignedTo As String,
                 ByVal lpExportPath As String,
                 ByVal lpItemsLocation As ItemsLocation,
                 ByVal lpDestinationConnectionString As String,
                 ByVal lpSourceConnectionString As String,
                 ByVal lpContentStorageType As Core.Content.StorageTypeEnum,
                 ByVal lpDeclareAsRecordOnImport As Boolean,
                 ByVal lpDeclareRecordConfiguration As DeclareRecordConfiguration,
                 ByVal lpTransformations As TransformationCollection,
                 ByVal lpDocumentFilingMode As Core.FilingMode,
                 ByVal lpLeadingDelimiter As Boolean,
                 ByVal lpBasePathLocation As Migrations.ePathLocation,
                 ByVal lpFolderDelimiter As String)

    Try
      mstrId = lpId
      mstrName = lpName
      mstrDescription = lpDescription
      ItemsLocation = lpItemsLocation
      mstrAssignedTo = lpAssignedTo
      mstrExportPath = lpExportPath
      mobjTransformations = lpTransformations
      mstrOperationType = lpOperationType
      mobjProcess = lpProcess
      mstrDestinationConnectionString = lpDestinationConnectionString
      mstrSourceConnectionString = lpSourceConnectionString
      menuStorageType = lpContentStorageType
      mblnDeclareAsRecordOnImport = lpDeclareAsRecordOnImport
      mobjDeclareRecordConfiguration = lpDeclareRecordConfiguration
      menuFilingMode = lpDocumentFilingMode
      mblnLeadingDelimiter = lpLeadingDelimiter
      menumBasePathLocation = lpBasePathLocation
      mstrFolderDelimiter = lpFolderDelimiter

      'InitializeLogSession()

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller  
      Throw
    End Try

  End Sub

#End Region

#Region "Public Methods"

  ''' <summary>
  ''' If the batch is NOT locked and has at least one item to be processed, return true, else false
  ''' </summary>
  ''' <returns>true if available for processing, else false</returns>
  ''' <remarks></remarks>
  Public Function IsAvailableForProcessing() As Boolean

    Try
      If IsCancelled Then
        Return False
      Else
        Return BatchContainer.IsAvailableForProcessing(Me.Id)
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Return False
    End Try

  End Function

  Public Sub RefreshDestinationConnection() Implements IItemParent.RefreshDestinationConnection
    Try
      DestinationConnection = GetJobContentSource(OperationScope.Destination)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Sub RefreshSourceConnection() Implements IItemParent.RefreshSourceConnection
    Try
      SourceConnection = GetJobContentSource(OperationScope.Source)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Sub ResetItemsToNotProcessed(lpCurrentStatus As ProcessedStatus)
    Try
      Job.ResetCompletionFlag()
      BatchContainer.ResetItemsToNotProcessed(Me, lpCurrentStatus)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  ''' <summary>
  ''' Resets all the items in the array to NotProcessed
  ''' </summary>
  ''' <param name="lpIdArrayList"></param>
  ''' <remarks></remarks>
  Public Sub ResetItemsToNotProcessed(ByVal lpIdArrayList As ArrayList)

    Try
      Job.ResetCompletionFlag()
      BatchContainer.ResetItemsToNotProcessed(lpIdArrayList)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Sub

  Public Sub ResetFailedItemsToNotProcessed()

    Try
      Job.ResetCompletionFlag()
      BatchContainer.ResetFailedItemsToNotProcessed(Me)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Sub

  Public Function GetAllItems(ByVal lpProcessedStatusFilter As String) As BatchItems

    Try
      Return BatchContainer.GetAllItems(Me, lpProcessedStatusFilter)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  Public Function GetAllItemsToDataTable(ByVal lpProcessedStatusFilter As String) As DataTable

    Try
      Return BatchContainer.GetAllItemsToDataTable(Me, lpProcessedStatusFilter)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  Public Function GetItemsToDataTable(ByVal lpStart As Integer,
                                      ByVal lpItemsToGet As Integer,
                                      ByVal lpSortColumn As String,
                                      ByVal lpAscending As Boolean,
                                      ByVal lpProcessedStatusFilter As ProcessedStatus) As DataTable

    Try
      ' Return mobjBatchContainer.GetItemsToDataTable(Me, lpStart, lpItemsToGet, lpSortColumn, lpAscending, lpProcessedStatusFilter)
      Return BatchContainer.GetItemsToDataTable(Me, lpStart, lpItemsToGet, lpSortColumn, lpAscending, lpProcessedStatusFilter)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  Public Function AddItem(ByVal lpBatchItem As BatchItem) As Boolean

    Try

      ' Dim lbnlReturn As Boolean = mobjBatchContainer.AddItem(lpBatchItem)
      Dim lblnReturn As Boolean = BatchContainer.AddItem(lpBatchItem)
      If lblnReturn = True Then
        mintNewItemCount += 1
      End If
      Return lblnReturn

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  Public Sub CommitBatchItems()

    Try
      BatchContainer.CommitBatchItems()

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Sub

  Public Function Execute(ByVal lpBatchWorker As BaseBatchWorker) As Boolean

    Try
      'LogSession.EnterMethod(Helper.GetMethodIdentifier(Reflection.MethodBase.GetCurrentMethod))
      mobjBatchWorker = lpBatchWorker
      mobjIdArrayList = Nothing
      Return Execute()

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    Finally
      'LogSession.LeaveMethod(Helper.GetMethodIdentifier(Reflection.MethodBase.GetCurrentMethod))
    End Try

  End Function

  Public Function Execute(ByVal lpBatchWorker As BaseBatchWorker,
                          ByVal lpIdArrayList As ArrayList) As Boolean

    Try
      'LogSession.EnterMethod(Helper.GetMethodIdentifier(Reflection.MethodBase.GetCurrentMethod))
      mobjBatchWorker = lpBatchWorker
      mobjIdArrayList = lpIdArrayList
      Return Execute()

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    Finally
      'LogSession.LeaveMethod(Helper.GetMethodIdentifier(Reflection.MethodBase.GetCurrentMethod))
    End Try

  End Function

  ''' <summary>
  ''' Process all the unprocessed items or list of Ids in the batch
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function Execute() As Boolean

    Try

      'LogSession.EnterMethod(Helper.GetMethodIdentifier(Reflection.MethodBase.GetCurrentMethod))

      '	Make sure we have a valid process
      If ((Me.Process Is Nothing) OrElse (Me.Process.IsEmpty) OrElse (Me.Process.IsDisposed)) Then
        Me.Process = Me.Job.Process.Clone
        '	Double check
        If Me.Process.IsEmpty Then
          Throw New InvalidOperationException("The process is empty.")
        End If
      End If

      ' <Added by: Ernie at: 8/1/2014-8:10:50 AM on machine: ERNIE-THINK>
      ' Create a fresh but independent copy of the transformations
      ' We are doing this because we have been having issues with running multiple 
      ' batches with processes involving transformations.
      ' Testing indicated that we were somehow cross-referencing documents across different batches. 
      ' This was causing data from one document to get sent to another, most likely there was some 
      ' sharing of the Transformation.Document object reference.
      Dim lobjFreshTransformations As New TransformationCollection
      For Each lobjTransformation As Transformation In Me.Transformations
        lobjFreshTransformations.Add(lobjTransformation.Clone())
      Next
      Me.Transformations = lobjFreshTransformations
      ' </Added by: Ernie at: 8/1/2014-8:10:50 AM on machine: ERNIE-THINK>

      'Dim lblnIsEvaluation As Boolean = LicenseRegister.IsEvaluation

      'If ((lblnIsEvaluation = True) AndAlso (TransactionCounter.Instance.CurrentCount >= Job.MAX_EVAL_TRANSACTIONS)) Then
      '  Me.Job.CancelJob(Job.MAX_EVAL_TRANSACTIONS_EXCEEDED_MESSAGE)
      '  Return False
      'End If

      If BatchContainer.GetBatchLockCount(Me.Job.Name) = 0 AndAlso Me.Job.IsRunning = False Then
        Me.Job.OnJobBegin(New WorkEventArgs(Me))
      End If

      If ProjectCatalog.Instance IsNot Nothing AndAlso ProjectCatalog.Instance.ThisNode IsNot Nothing Then
        'LogSession.LogMessage("Starting Batch Execute for Batch {0} using thread ({3}) on node {1} - {2}.", _
        'Id, ProjectCatalog.Instance.ThisNode.Id, Environment.MachineName, Thread.CurrentThread.ManagedThreadId)
      Else
        'LogSession.LogMessage("Starting Batch Execute for Batch {0} on {1}.", Id, Environment.MachineName)
      End If

      'Add a lock record so no other machine can process this batch
      BatchContainer.LockBatch(Me.Id, Me.MachineName)
      mblnIsLocked = True

      RaiseEvent BeforeBatchBegin(mobjBatchWorker)

      Me.Process.RunBeforeParentBegin?.Execute(New BatchItemProxy(Me))

      Dim lobjBatchSummary As WorkSummary
      Dim lobjBatchItems As BatchItems = Nothing

      If ((mobjIdArrayList Is Nothing) OrElse (mobjIdArrayList.Count = 0)) Then
        lobjBatchItems = BatchContainer.GetAllUnprocessedItems(Me)

      Else
        lobjBatchItems = BatchContainer.GetItemsById(Me, mobjIdArrayList, True)
      End If

      Dim lintBatchCount As Integer = 0
      'Dim lintFailed As Integer = 0
      'Dim lintSuccess As Integer = 0
      'Dim lintNotProcessed As Integer = 0
      'Dim ldblAvgProcessingTime As Double = 0
      'Dim ldblLocalProcessingTime As Double = 0
      Dim lobjPreviouslyProcessedProcessResult As IProcessResult = Nothing

      'mintNumBusy = lobjBatchItems.Count
      'mobjDoneEvent = New ManualResetEvent(False)
      Dim lintCurrentCount As Integer = 1

      Me.Job.InitializeTimedStatusPusher()

      For Each lobjBatchItem As BatchItem In lobjBatchItems

        'Use threading
        'QueueItem(lobjBatchItem, lintCurrentCount)

        If (mobjBatchWorker IsNot Nothing) Then

          If (mobjBatchWorker.CancellationPending) Then
            mobjBatchWorker.DoWorkEventArgs.Cancel = True
            'If Me.Job.IsRunning Then
            '  Me.Job.OnJobCancel(New WorkCancelledEventArgs(Me))
            'End If
            If IsRunning Then
              mblnIsRunning = False
              mblnIsCancelled = True
            End If

            If Me.Job.AreAnyBatchesRunning = False Then
              Me.Job.OnJobCancel(New WorkCancelledEventArgs(Me))
            End If

            mblnJobCancelled = True
            Unlock()
            Me.Job.RefreshStatisticsAndPushToFirebaseAsync()
            Return False
            'Exit Function
          End If

        End If

        'If this is the first time through the loop, get the batch summary counts from the container
        If (lintCurrentCount = 1) Then

          lobjBatchSummary = Me.GetBatchSummaryCounts()
          mintNotProcessed = lobjBatchSummary.NotProcessedCount
          mintSuccess = lobjBatchSummary.SuccessCount
          mintFailed = lobjBatchSummary.FailedCount
          lintBatchCount = lobjBatchSummary.TotalItemsCount
          mdblAvgProcessingTime = lobjBatchSummary.AvgProcessingTime
          mdatCurrentStartTime = Now
          mblnIsRunning = True
          mblnIsCancelled = False
        End If

        If lobjBatchItem.ProcessedStatus <> ProcessedStatus.NotProcessed Then
          lobjPreviouslyProcessedProcessResult = lobjBatchItem.ProcessResult
          lobjPreviouslyProcessedProcessResult.Name = String.Format("{0}: {1}",
            lobjBatchItem.Id, lobjPreviouslyProcessedProcessResult.Name)
          Select Case lobjBatchItem.ProcessedStatus
            Case ProcessedStatus.Success
              lobjBatchItem.ProcessedStatus = ProcessedStatus.PreviouslySucceeded
              lobjPreviouslyProcessedProcessResult.Result = OperationEnumerations.Result.PreviouslySucceeded
            Case ProcessedStatus.Failed
              lobjBatchItem.ProcessedStatus = ProcessedStatus.PreviouslyFailed
              lobjPreviouslyProcessedProcessResult.Result = OperationEnumerations.Result.PreviouslyFailed
            Case ProcessedStatus.Processing
              '	Let's assume that this item is actually still in process and leave it as is.
          End Select
          mobjBatchWorker.ProcessResults.Add(lobjPreviouslyProcessedProcessResult)
          Continue For
        End If

        'Don't use threading
        Dim ldtBeginTime As DateTime = DateTime.Now
        Dim lblnReturn As Boolean
        'If Not lblnIsEvaluation Then
        '  lblnReturn = lobjBatchItem.Execute(Me.Process)
        'Else
        '  If TransactionCounter.Instance.CurrentCount <= Job.MAX_EVAL_TRANSACTIONS Then
        '    lblnReturn = lobjBatchItem.Execute(Me.Process)
        '  Else
        '    Me.Job.CancelJob(Job.MAX_EVAL_TRANSACTIONS_EXCEEDED_MESSAGE)
        '    Me.Job.RefreshStatisticsAndPushToFirebaseAsync()
        '    Exit For
        '  End If
        'End If

        lblnReturn = lobjBatchItem.Execute(Me.Process)

        Dim ldtEndTime As DateTime = DateTime.Now
        mdatLastItemFinishTime = ldtEndTime
        Dim ltpDifference As TimeSpan = ldtEndTime.Subtract(ldtBeginTime)
        mdblLocalProcessingTime += ltpDifference.TotalSeconds
        mdblAvgProcessingTime = mdblLocalProcessingTime / lintBatchCount

        mobjBatchWorker?.ProcessResults.Add(Me.Process.ResultDetail)

        If (lblnReturn) Then
          mintSuccess += 1

        Else
          mintFailed += 1
        End If

        mintNotProcessed -= 1

        ' RaiseEvent ItemProcessed(mobjBatchWorker, lobjBatchItem.Id, lobjBatchItem.Title, New WorkSummary(Me.Name, Me.Operation, lintNotProcessed, lintSuccess, lintFailed, 0, lintBatchCount, ldblAvgProcessingTime))
        mobjCurrentWorkSummary = New WorkSummary(Me.Name, Me.Operation, mintNotProcessed, mintSuccess, mintFailed, 0, lintBatchCount, mdblAvgProcessingTime, DateTime.MinValue, DateTime.MinValue, Now, -1, -1)
        RaiseEvent ItemProcessed(Me, New BatchItemProcessedEventArgs(lobjBatchItem, mobjBatchWorker, mobjCurrentWorkSummary))

        lintCurrentCount += 1

        'TEST
        'Threading.Thread.Sleep(5000)

        'TEST ONLY
        'If (lintCurrentCount > mintNumBusy) Then Exit For

        ' Dispose of any documents we no longer need
        If lobjBatchItem.Document IsNot Nothing AndAlso lobjBatchItem.Document.IsDisposed = False Then
          lobjBatchItem.Document.Dispose()
        End If

      Next

      Me.Process.RunAfterParentComplete?.Execute(New BatchItemProxy(Me))

      If ProjectCatalog.Instance IsNot Nothing AndAlso ProjectCatalog.Instance.ThisNode IsNot Nothing Then
        'LogSession.LogMessage("Completing Batch Execute for Batch {0} using thread ({3}) on node {1} - {2}.", _
        'Id, ProjectCatalog.Instance.ThisNode.Id, Environment.MachineName, Thread.CurrentThread.ManagedThreadId)
      Else
        'LogSession.LogMessage("Completing Batch Execute for Batch {0} on {1}.", Id, Environment.MachineName)
      End If

      RaiseEvent BatchCompleted(mobjBatchWorker)

      'Wait for all the batch items to execute
      'mobjDoneEvent.WaitOne()

      Unlock()

      Dim lobjJobSummary As WorkSummary = Me.Job.GetWorkSummaryCounts

      If lobjJobSummary IsNot Nothing Then
        If (BatchContainer.GetBatchLockCount(Me.Job.Name) = 0) Then
          If (lobjJobSummary.NotProcessedCount = 0) OrElse Me.mobjIdArrayList IsNot Nothing Then
            Me.Job.OnAfterJobComplete(New WorkEventArgs(Me))
          End If
        End If
      End If

      SaveBatchItemsProcessed()

      Me.Job.RefreshStatisticsAndPushToFirebaseAsync()

      CleanUp()

      Return True

    Catch exBatchLocked As BatchLockedException
      ApplicationLogging.LogException(exBatchLocked, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      Unlock()

      RaiseEvent BatchError(mobjBatchWorker)
      Me.Job?.OnJobError(New WorkErrorEventArgs(Me, ex))
      ' Re-throw the exception to the caller
      Throw
    Finally
      'LogSession.LeaveMethod(Helper.GetMethodIdentifier(Reflection.MethodBase.GetCurrentMethod))
    End Try

  End Function

  Friend Sub Cancel()
    Try
      mobjBatchWorker?.CancelAsync()
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Friend Sub CleanUp()
    Try

      ' Cleanup the process
      If Me.Process IsNot Nothing Then
        If Not Me.Process.IsDisposed Then
          Me.Process.Dispose()
        End If
      End If

      ' Cleanup the source content source
      CleanUpContentSource(MobjSourceContentSource)

      ' Cleanup the destination content source
      CleanUpContentSource(MobjDestinationContentSource)

      mblnIsRunning = False
      mblnIsCancelled = False

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Function GetJobContentSource(lpScope As OperationScope) As IRepositoryConnection
    Try
      If Me.Job Is Nothing Then
        Throw New InvalidOperationException("Unable to get job content source, the job reference is null.")
      End If

      Select Case lpScope
        Case OperationScope.Source
          If Not String.IsNullOrEmpty(Me.Job.SourceConnectionString) Then
            Return New Providers.ContentSource(Me.Job.SourceConnectionString)
          Else
            Throw New Exceptions.InvalidConnectionStringException(String.Format("Unable to get source content source from job '{0}', the source connection string is not set.", Me.Job.Name))
          End If

        Case OperationScope.Destination
          If Not String.IsNullOrEmpty(Me.Job.DestinationConnectionString) Then
            Return New Providers.ContentSource(Me.Job.DestinationConnectionString)
          Else
            Throw New Exceptions.InvalidConnectionStringException(String.Format("Unable to get destination content source from job '{0}', the source connection string is not set.", Me.Job.Name))
          End If

        Case Else
          Throw New ArgumentOutOfRangeException(NameOf(lpScope), String.Format("Invalid scope value: '{0}'.", lpScope))

      End Select
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Sub SaveBatchItemsProcessed()
    Try
      If CurrentItemsProcessed > 0 Then
        Dim llngCurrentItemsCount As Long = BatchContainer.GetProcessedItemsCount(Me.Job)
        BatchContainer.SetProcessedItemsCount(Me.Job, llngCurrentItemsCount + CurrentItemsProcessed)
        Me.Job.CurrentItemsProcessed -= CurrentItemsProcessed
        CurrentItemsProcessed = 0
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Shared Sub CleanUpContentSource(lpContentSource As ContentSource)
    Try
      If lpContentSource IsNot Nothing Then
        If lpContentSource.State = Providers.ProviderConnectionState.Connected Then
          lpContentSource.Disconnect()
        End If
        lpContentSource.Dispose()
        lpContentSource = Nothing
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Default ReadOnly Property Item(ByVal lpId As String) As BatchItem
    Get

      Try
        Return GetBatchItemById(lpId)

      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try

    End Get
  End Property

  Public Function GetItemCount() As Long
    Try
      Return BatchContainer.GetItemCount(Me)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Function GetTotalItemCount() As Long

    Try

      Dim lobjBatchSummary As WorkSummary = GetBatchSummaryCounts()

      If lobjBatchSummary IsNot Nothing Then
        Return lobjBatchSummary.TotalItemsCount

      Else
        Return 0
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  Public Function GetBatchSummaryCounts() As WorkSummary

    Try
      Return BatchContainer.GetWorkSummaryCounts(Me)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  Public Function GetFailureSummaries() As FailureSummaries
    Try
      Return BatchContainer.GetFailureSummaries(Me)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Function GetProcessResults() As IProcessResults
    Try
      Return BatchContainer.GetProcessResults(Me)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Function GetProcessResultsSummary() As IProcessResultSummary
    Try

      Return BatchContainer.GetProcessResultSummary(Me)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Sub UpdateProcessResultsSummary()
    Try
      BatchContainer.SaveProcessResultSummary(Me)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod())
      '  Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Sub Unlock()
    Try
      If Not IsLocked Then
        Throw New InvalidOperationException("Batch is not locked")
      End If

      BatchContainer.UnLockBatch(Me.Id, Me.MachineName)
      mblnIsLocked = False

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Sub UpdateProcess(lpJob As Job)
    Try
      Me.Process = lpJob.Process.Clone
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Sub UpdateTransformations(ByVal lpJob As Job)
    Try
      Me.Transformations = lpJob.Transformations.Clone
      '	BatchContainer.UpdateTransformations(Me)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Function Clone() As Object _
         Implements System.ICloneable.Clone

    Dim lobjBatch As Batch

    Try
      ' lobjBatch.Name = Me.Name
      lobjBatch = New Batch(Me.ItemsLocation) With {
        .AssignedTo = Me.AssignedTo,
        .BasePathLocation = Me.BasePathLocation,
        .ContentStorageType = Me.ContentStorageType,
        .DeclareAsRecordOnImport = Me.DeclareAsRecordOnImport,
        .DeclareRecordConfiguration = Me.DeclareRecordConfiguration,
        .Description = Me.Description,
        .DestinationConnectionString = Me.DestinationConnectionString,
        .DocumentFilingMode = Me.DocumentFilingMode,
        .ExportPath = Me.ExportPath,
        .FolderDelimiter = Me.FolderDelimiter,
        .mstrId = Me.Id,
        .LeadingDelimiter = Me.LeadingDelimiter,
        .Number = Me.Number,
        .Operation = Me.Operation,
        .SourceConnectionString = Me.SourceConnectionString,
        .Transformations = Me.Transformations
      }

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      Return Nothing
    End Try

    Return lobjBatch

  End Function

  'Public Sub ExportItems(ByVal lpItemsArray As ArrayList)

  '  Try

  '    Dim lobjExportBatchItem As ExportBatchItem = Nothing
  '    Dim lblnReturn As Boolean = False

  '    For Each lstrId As String In lpItemsArray
  '      lobjExportBatchItem = New ExportBatchItem(lstrId, lstrId, Me, False)
  '      lblnReturn = lobjExportBatchItem.Execute()
  '      If (lblnReturn = False) Then
  '        Throw New Exception("Failed to export one or more documents.  See the application log for more information.")
  '      End If
  '    Next

  '  Catch ex As Exception
  '    ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
  '    ' Re-throw the exception to the caller
  '    Throw
  '  End Try

  'End Sub

  Public Function ExportItems(ByVal lpBatchItems As IEnumerable(Of BatchItem),
                              ByVal lpExportScope As ExportScope) As List(Of String)

    Dim lobjListOfPaths As New List(Of String)
    'Dim lstrExportDocPath As String

    Try

      ' <Modified by: Ernie at 12/2/2011-4:12:10 PM on machine: ERNIE-M4400>
      ' We still need to refactor this method as a result of re-arranging the logic into the Operations dll
      'Throw New NotImplementedException


      ''Dim lobjExportBatchItem As ExportBatchItem = Nothing
      'Dim lobjExportOperation As ExportOperation = OperationFactory.Create("Export")

      'With lobjExportOperation
      '  .LogResult = False
      '  .Scope = lpExportScope
      'End With

      For Each lobjBatchItem As BatchItem In lpBatchItems
        ' lobjExportBatchItem = New ExportBatchItem(lobjBatchItem.SourceDocId, lobjBatchItem.Title, lobjBatchItem.Batch, False)
        ' lobjExportBatchItem.Id = lobjBatchItem.Id

        Select Case lpExportScope

          Case ExportScope.Source, ExportScope.Destination

            '' Dim lstrExportDocPath As String = lobjExportBatchItem.ExportSourceDocument()
            'Dim lenuResult As OperationEnumerations.Result = lobjExportOperation.Execute(lobjBatchItem)

            'If lenuResult = OperationEnumerations.Result.Success Then
            '  lstrExportDocPath = lobjExportOperation.WorkItem.DestinationDocId
            'ElseIf lenuResult = OperationEnumerations.Result.Failure Then
            '  Throw New InvalidOperationException(String.Format("Failed to export one or more documents: {0}", ExceptionTracker.LastException.Message), ExceptionTracker.LastException)
            'End If

            'lobjListOfPaths.Add(lobjExportOperation.WorkItem.DestinationDocId)

            lobjListOfPaths.Add(ExportItem(lobjBatchItem, lpExportScope))
          Case ExportScope.Both

            lobjListOfPaths.Add(ExportItem(lobjBatchItem, ExportScope.Source))
            lobjListOfPaths.Add(ExportItem(lobjBatchItem, ExportScope.Destination))

            ''Case ExportScope.Destination
            ''  ' lobjExportBatchItem.DestDocId = lobjBatchItem.DestDocId
            ''  ' lobjListOfPaths.Add(lobjExportBatchItem.ExportDestinationDocument())
            ''  lobjExportOperation.WorkItem.DestDocId = lobjBatchItem.DestDocId
            ''  lobjListOfPaths.Add(lobjExportBatchItem.ExportDestinationDocument())

            ''Case ExportScope.Both
            ''  lobjListOfPaths.Add(lobjExportBatchItem.ExportSourceDocument())
            ''  lobjExportBatchItem.DestDocId = lobjBatchItem.DestDocId
            ''  lobjListOfPaths.Add(lobjExportBatchItem.ExportDestinationDocument())
        End Select

      Next

      ' </Modified by: Ernie at 12/2/2011-4:12:10 PM on machine: ERNIE-M4400>

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

    Return lobjListOfPaths

  End Function

  Public Sub ResetCancelledFlag()
    Try
      mblnIsCancelled = False
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub
#End Region

#Region "Friend Methods"

  Friend Sub ResetRunningFlag()
    Try
      mblnIsRunning = False
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Friend Sub SetJob(ByVal lpJob As Job)

    Try
      MobjJob = lpJob

      With lpJob
        ItemsLocation = .ItemsLocation
        Operation = .Operation
        'Process = .Process '.Clone
        '	Process = lpJob.Process.Clone
        'If Process Is Nothing Then
        '  Process = .Process.Clone
        'End If
        DestinationConnectionString = .DestinationConnectionString
        SourceConnectionString = .SourceConnectionString
        ContentStorageType = .ContentStorageType
        DeclareAsRecordOnImport = .DeclareAsRecordOnImport
        DeclareRecordConfiguration = .DeclareRecordConfiguration
        Transformations = .Transformations

        DocumentFilingMode = .DocumentFilingMode
        LeadingDelimiter = .LeadingDelimiter
        FolderDelimiter = .FolderDelimiter
        BasePathLocation = .BasePathLocation
      End With

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

      If Not String.IsNullOrEmpty(Id) Then
        lobjIdentifierBuilder.AppendFormat("{0}: ", Id)

      Else
        lobjIdentifierBuilder.Append("ID not set: ")
      End If

      lobjIdentifierBuilder.AppendFormat("{0}", Operation)

      Return lobjIdentifierBuilder.ToString

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      Return lobjIdentifierBuilder.ToString
    End Try

  End Function

#End Region

#Region "Private Methods"

#Region "Threading Methods"

  ''' <summary>
  ''' Adds a batch item to the queue
  ''' </summary>
  ''' <param name="lpBatchItem"></param>
  ''' <param name="lpCurrentCount"></param>
  ''' <remarks></remarks>
  Private Sub QueueItem(ByVal lpBatchItem As BatchItem,
                        ByVal lpCurrentCount As Integer)

    Dim lobjThreadBatchItem As New ThreadBatchItem(lpBatchItem, lpCurrentCount)
    Threading.ThreadPool.QueueUserWorkItem(New Threading.WaitCallback(AddressOf ExecuteBatchItemByThread), lobjThreadBatchItem)
  End Sub

  ''' <summary>
  ''' Delegate for each batch execute
  ''' </summary>
  ''' <param name="lpThreadBatchItem"></param>
  ''' <remarks></remarks>
  Private Sub ExecuteBatchItemByThread(ByVal lpThreadBatchItem As Object)
    Try

      Dim lobjThreadBatchItem As ThreadBatchItem = lpThreadBatchItem

      Dim lobjBatchItem As BatchItem = lobjThreadBatchItem.BatchItem
      lobjBatchItem.Execute(Me.Process)
      Console.WriteLine("Processed by Thread: " & Thread.CurrentThread.GetHashCode.ToString)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)

    Finally

      If (Interlocked.Decrement(mintNumBusy) = 0) Then
        mobjDoneEvent.Set()
      End If

    End Try

  End Sub

#End Region

  'Private Sub InitializeLogSession()
  '  Try
  '    'mobjLogSession = SiAuto.Si.AddSession(String.Format("Batch: {0}", Id))
  '    mobjLogSession = SiAuto.Si.AddSession("Batch")
  '    mobjLogSession.Color = System.Drawing.Color.LightSkyBlue
  '  Catch ex As Exception
  '    ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
  '    ' Re-throw the exception to the caller
  '    Throw
  '  End Try
  'End Sub

  Private Function ExportItem(ByVal lpBatchItem As BatchItem,
                              ByVal lpExportScope As ExportScope) As String
    Try

      Dim lstrExportDocPath As String = String.Empty

      Dim lenuResult As OperationEnumerations.Result
      Dim lobjExportOperation As ExportOperation = OperationFactory.Create("Export")

      With lobjExportOperation
        .LogResult = False
        .Scope = lpExportScope
      End With

      lenuResult = lobjExportOperation.Execute(lpBatchItem)

      If lenuResult = OperationEnumerations.Result.Success Then
        lstrExportDocPath = lobjExportOperation.ExportedDocumentPath
      ElseIf lenuResult = OperationEnumerations.Result.Failed Then
        Throw New InvalidOperationException(String.Format("Failed to export one or more documents: {0}", ExceptionTracker.LastException.Message), ExceptionTracker.LastException)
      End If

      ' Dispose of any documents we no longer need
      'If lpBatchItem.Document IsNot Nothing Then
      '  lpBatchItem.Document.Dispose()
      'End If

      Return lstrExportDocPath

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Function GetItemByBatchItemId(ByVal lpId As String) As BatchItem
    Try
      Dim lobjBatchItem As BatchItem = BatchContainer.GetBatchItemById(lpId)

      If lobjBatchItem.Batch Is Nothing Then
        lobjBatchItem.SetBatch(Me)
      End If

      Return lobjBatchItem

    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod())
      '  Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function GetBatchItemById(ByVal lpId As String) As BatchItem

    Try

      Dim lobjBatchItems As BatchItems = Nothing
      Dim lobjBatchItem As BatchItem = Nothing

      'If (mobjIdArrayList Is Nothing) Then
      '  lobjBatchItems = mobjBatchContainer.GetAllItems(Me, Nothing)
      'Else
      '  lobjBatchItems = mobjBatchContainer.GetItemsById(Me, mobjIdArrayList)
      'End If

      lobjBatchItems = BatchContainer.GetAllItems(Me, Nothing)

      If lobjBatchItems IsNot Nothing Then
        lobjBatchItem = (From b In lobjBatchItems Where b.Id = lpId Select b).FirstOrDefault
      End If

      Return lobjBatchItem

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  'Private Sub CreateBatchContainter()

  '  Try
  '    Select Case mobjItemsLocation.Type
  '      Case ItemsLocation.enumType.CSV
  '        mobjBatchContainer = New BatchContainerCSV(mobjItemsLocation)
  '      Case ItemsLocation.enumType.OLEDB
  '        mobjBatchContainer = New BatchContainerOLEDB(mobjItemsLocation)
  '      Case Else
  '        Throw New Exception(String.Format("{1}: Uknown ItemsLocation '{0}'", mobjItemsLocation.Type, MethodBase.GetCurrentMethod.ToString))
  '    End Select

  '  Catch ex As Exception
  '    ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
  '    ' Re-throw the exception to the caller
  '    Throw
  '  End Try

  'End Sub

  Private Function GetCurrentProcessingTime() As TimeSpan
    Try
      Return mdatLastItemFinishTime.Subtract(mdatCurrentStartTime)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function GetCurrentProcessingTimeString() As String
    Try
      Dim lstructCurrentProcessingTime As TimeSpan = GetCurrentProcessingTime()
      Return Helper.FormatTimeSpan(lstructCurrentProcessingTime)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  'Private Sub PushFirebaseUpdate()
  '  Try
  '    If Not ConnectionSettings.Instance.DisableNotifications Then
  '      Dim lstrCatalogId As String = ProjectCatalog.Instance.Id
  '      Dim lstrProjectId As String = Me.Job.Project.Id
  '      Dim lstrJobId As String = Me.Job.Id
  '      Using lobjFirebase As New FirebaseApplication(FIREBASE_APP_URL)

  '        Dim lstrPutPath As String = String.Format("catalogs/{0}/projects/{1}/jobs/{2}", lstrCatalogId, lstrProjectId, Me.Job.Index)
  '        Dim lobjCurrentJobInfo As IJobInfo = ProjectCatalog.Instance(True).GetJobInfo(lstrProjectId, lstrJobId)

  '        Static lstrLastProjectInfoJson As String
  '        Dim lstrCurrentProjectInfoJson As String

  '        If String.IsNullOrEmpty(lstrLastProjectInfoJson) Then
  '          lstrLastProjectInfoJson = lobjCurrentJobInfo.ToJson()
  '          lobjFirebase.Put(lstrPutPath, lstrLastProjectInfoJson)
  '        Else
  '          lstrCurrentProjectInfoJson = lobjCurrentJobInfo.ToJson()
  '          If Not lstrCurrentProjectInfoJson.Equals(lstrLastProjectInfoJson) Then
  '            lstrLastProjectInfoJson = lstrCurrentProjectInfoJson
  '            lobjFirebase.Put(lstrPutPath, lstrLastProjectInfoJson)
  '          End If
  '        End If
  '      End Using
  '    End If
  '  Catch ex As Exception
  '    'ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '    ' Just log it and move on, we don't want this to cause a problem
  '  End Try
  'End Sub

  Private Sub MobjJob_AfterJobComplete(sender As Object, ByRef e As WorkEventArgs) Handles MobjJob.AfterJobComplete
    Try
      If String.Equals(e.OriginatingBatch.Id, Me.Id) Then
        ' We were the one to complete the job, we will process this call
        Me.Process.RunAfterJobComplete?.Execute(New JobItemProxy(Me.Job))
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Sub MobjJob_BeforeJobBegin(sender As Object, ByRef e As WorkEventArgs) Handles MobjJob.BeforeJobBegin
    Try
      If String.Equals(e.OriginatingBatch.Id, Me.Id) Then
        ' We were the one to start the job, we will process this call
        If ((Me.Process Is Nothing) AndAlso (Me.Job.Process IsNot Nothing)) Then
          Me.Process = Me.Job.Process.Clone
        End If

        If Me.Process.RunBeforeJobBegin IsNot Nothing AndAlso Me.Job.RunBeforeJobBeginCount < 1 Then
          Me.Process.RunBeforeJobBegin.Execute(New JobItemProxy(Me.Job))
        End If
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Sub MobjJob_JobError(sender As Object, ByRef e As WorkErrorEventArgs) Handles MobjJob.JobError
    Try
      If String.Equals(e.OriginatingBatch.Id, Me.Id) Then
        ' We were the one to invoke the failure, we will process this call
        Me.Process.RunOnJobFailure?.Execute(New JobItemProxy(Me.Job))
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

#Region "IDisposible Methods"

  ' IDisposable
  Protected Overridable Sub Dispose(ByVal disposing As Boolean)
    Try
      If Not Me.disposedValue Then

        If disposing Then
          ' DISPOSETODO: free other state (managed objects).
          'LogSession.LogMessage(Helper.GetMethodIdentifier(String.Format("Batch {0} disposing", Me.Id)))
          If MobjBatchContainer IsNot Nothing Then
            MobjBatchContainer.Dispose()
            MobjBatchContainer = Nothing
          End If

          mstrId = Nothing
          mstrDescription = Nothing
          mstrName = Nothing
          mobjItemsLocation = Nothing
          mstrDestinationConnectionString = Nothing
          MobjDestinationContentSource = Nothing
          mstrSourceConnectionString = Nothing
          MobjSourceContentSource = Nothing
          mobjTransformations = Nothing
          menuFilingMode = Nothing
          mblnLeadingDelimiter = Nothing
          mstrFolderDelimiter = Nothing
          menumBasePathLocation = Nothing
          mstrAssignedTo = Nothing
          mstrExportPath = Nothing

          mstrOperationType = Nothing
          If mobjProcess IsNot Nothing AndAlso mobjProcess.IsDisposed = False Then
            mobjProcess.Dispose()
          End If
          mobjProcess = Nothing
          menuStorageType = Nothing
          mblnDeclareAsRecordOnImport = Nothing
          mobjDeclareRecordConfiguration = Nothing
          lstrMachineName = Nothing
          mobjDoneEvent = Nothing
          mintNumBusy = Nothing
          mobjBatchWorker = Nothing
          mobjIdArrayList = Nothing
          MobjJob = Nothing
          MobjSelectedItems = Nothing
          mobjSelectedItemList = Nothing
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

  Private Sub MobjSelectedItems_CollectionChanged(ByVal sender As Object,
                                                  ByVal e As System.Collections.Specialized.NotifyCollectionChangedEventArgs) _
          Handles MobjSelectedItems.CollectionChanged

    Try

      Select Case e.Action

        Case Specialized.NotifyCollectionChangedAction.Add

          For Each lobjItem As BatchItem In e.NewItems

            If mobjSelectedItemList.Contains(lobjItem.Id) = False Then
              mobjSelectedItemList.Add(lobjItem.Id)
            End If

          Next

        Case Specialized.NotifyCollectionChangedAction.Remove

          For Each lobjItem As BatchItem In e.OldItems

            If mobjSelectedItemList.Contains(lobjItem.Id) Then
              mobjSelectedItemList.Remove(lobjItem.Id)
            End If

          Next

        Case Specialized.NotifyCollectionChangedAction.Replace

      End Select

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Sub

  Private Sub MobjDestinationContentSource_ConnectionStateChanged(sender As Object, ByRef e As Arguments.ConnectionStateChangedEventArgs) _
    Handles MobjDestinationContentSource.ConnectionStateChanged
    Try
      If Not Helper.CallStackContainsMethodName("Disconnect") Then
        If e.CurrentState = Providers.ProviderConnectionState.Disconnected OrElse
          e.CurrentState = Providers.ProviderConnectionState.Unavailable Then

          mobjBatchWorker?.CancelAsync()

          Job.CancelJob(String.Format("The destination system '{0}' is {1}",
            MobjDestinationContentSource.Name, e.CurrentState.ToString.ToLower))
        End If
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Private Sub MobjSourceContentSource_ConnectionStateChanged(sender As Object, ByRef e As Arguments.ConnectionStateChangedEventArgs) _
    Handles MobjSourceContentSource.ConnectionStateChanged
    Try
      If Not Helper.CallStackContainsMethodName("Disconnect") Then
        If e.CurrentState = Providers.ProviderConnectionState.Disconnected OrElse
          e.CurrentState = Providers.ProviderConnectionState.Unavailable Then
          mobjBatchWorker?.CancelAsync()
          Job.CancelJob(String.Format("The source system '{0}' is {1}",
            MobjSourceContentSource.Name, e.CurrentState.ToString.ToLower))
        End If
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Public Property DestinationConnection As Core.IRepositoryConnection Implements IItemParent.DestinationConnection
    Get
      Return DestinationContentSource
    End Get
    Set(value As Core.IRepositoryConnection)
      MobjDestinationContentSource = value
    End Set
  End Property

  Public Property SourceConnection As Core.IRepositoryConnection Implements IItemParent.SourceConnection
    Get
      Return SourceContentSource
    End Get
    Set(value As Core.IRepositoryConnection)
      MobjSourceContentSource = value
    End Set
  End Property

  Private Sub Batch_BatchCompleted(lpBatchWorker As BatchWorker) Handles Me.BatchCompleted
    Try

      ' Write the process results summary for the batch.
      UpdateProcessResultsSummary()

      Me.SourceConnection?.Disconnect()

      Me.DestinationConnection?.Disconnect()
      Try
        'Notifications.Notifier.SendNotification(lpBatchWorker, Notifications.EventBasis.BatchCompleted)
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      End Try

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  'Private Sub Batch_BeforeBatchBegin(lpBatchWorker As BatchWorker) Handles Me.BeforeBatchBegin
  '  Try
  '    'Notifications.Notifier.SendNotification(lpBatchWorker, Notifications.EventBasis.BatchStarted)
  '  Catch ex As Exception
  '    ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '  End Try
  'End Sub

End Class