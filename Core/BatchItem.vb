'---------------------------------------------------------------------------------
' <copyright company="ECMG">
'     Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'     Copying or reuse without permission is strictly forbidden.
' </copyright>
'---------------------------------------------------------------------------------

#Region "Imports"

Imports System.Reflection
Imports System.Runtime.Serialization
Imports System.Text
Imports System.Xml.Serialization
Imports Documents
Imports Documents.Core
Imports Documents.Providers
Imports Documents.SerializationUtilities
Imports Documents.Utilities
Imports Operations
Imports Projects.Operations

#End Region

''' <summary>
''' Base Class for BatchItem
''' </summary>
''' <remarks></remarks>
<DataContract()> <DebuggerDisplay("{DebuggerIdentifier(),nq}")> Public Class BatchItem
  Implements IBatchItem
  Implements ICloneable

#Region "Class Variables"

  Private ReadOnly mdatCreateDate As DateTime = DateTime.MinValue
  Private mstrId As String = String.Empty
  Private mstrBatchId As String = String.Empty
  Private mobjTag As Object = Nothing
  Private mstrTitle As String = String.Empty
  Private mstrSourceDocId As String = String.Empty
  Private mstrDestDocId As String = String.Empty
  Private menuProcessedStatus As ProcessedStatus
  Private mdteStartTime As DateTime
  Private mdteFinishTime As DateTime
  Private mobjTotalProcessingTime As TimeSpan
  Private mstrProcessedBy As String = String.Empty
  Private mstrProcessedMessage As String = String.Empty
  Private mobjItemsLocation As ItemsLocation
  Private mstrOperationType As String = OperationType.Export
  Private WithEvents MobjBatch As Batch
  Private mobjBatchItemProcessEventArgs As BatchItemProcessEventArgs
  Private WithEvents MobjProcess As IOperable = Nothing
  Private mobjProcessResults As IProcessResult = Nothing
  Private mobjDocument As Core.Document = Nothing
  Private mobjFolder As Folder = Nothing
  Private mobjCustomObject As CustomObject = Nothing
  Private ReadOnly mintCurrentExecutedItems As Integer

#End Region

#Region "Public Properties"

  Public ReadOnly Property CreateDate As DateTime Implements IWorkItem.CreateDate
    Get
      Return mdatCreateDate
    End Get
  End Property

  <XmlAttribute()>
  Public Property Id() As String Implements IBatchItem.Id
    Get
      Return mstrId
    End Get
    Set(ByVal value As String)
      mstrId = value
    End Set
  End Property

  Public Property Tag() As Object Implements IBatchItem.Tag
    Get
      Return mobjTag
    End Get
    Set(ByVal value As Object)
      mobjTag = value
    End Set
  End Property

  Public Property Title() As String Implements IBatchItem.Title
    Get
      Return mstrTitle
    End Get
    Set(ByVal value As String)
      mstrTitle = value
    End Set
  End Property

  Public Property BatchId() As String Implements IBatchItem.BatchId
    Get
      Return mstrBatchId
    End Get
    Set(ByVal value As String)
      mstrBatchId = value
    End Set
  End Property

  Public Property SourceDocId() As String Implements IBatchItem.SourceDocId
    Get
      Return mstrSourceDocId
    End Get
    Set(ByVal value As String)
      mstrSourceDocId = value
    End Set
  End Property

  Public Property DestinationDocId() As String Implements IBatchItem.DestinationDocId
    Get
      Return mstrDestDocId
    End Get
    Set(ByVal value As String)
      mstrDestDocId = value
    End Set
  End Property

  Public Property ProcessedStatus() As ProcessedStatus Implements IBatchItem.ProcessedStatus
    Get
      Return menuProcessedStatus
    End Get
    Set(ByVal value As ProcessedStatus)
      menuProcessedStatus = value
    End Set
  End Property

  Public Property StartTime() As DateTime Implements IBatchItem.StartTime
    Get
      Return mdteStartTime
    End Get
    Set(ByVal value As DateTime)
      mdteStartTime = value
    End Set
  End Property

  Public Property FinishTime() As DateTime Implements IBatchItem.FinishTime
    Get
      Return mdteFinishTime
    End Get
    Set(ByVal value As DateTime)
      mdteFinishTime = value
    End Set
  End Property

  Public Property TotalProcessingTime() As TimeSpan Implements IBatchItem.TotalProcessingTime
    Get
      Return mobjTotalProcessingTime
    End Get
    Set(ByVal value As TimeSpan)
      mobjTotalProcessingTime = value
    End Set
  End Property

  Public Property ProcessedBy() As String Implements IBatchItem.ProcessedBy
    Get
      Return mstrProcessedBy
    End Get
    Set(ByVal value As String)
      mstrProcessedBy = value
    End Set
  End Property

  Public Property ProcessedMessage() As String Implements IBatchItem.ProcessedMessage
    Get
      Return mstrProcessedMessage
    End Get
    Set(ByVal value As String)
      mstrProcessedMessage = value
    End Set
  End Property

  Public Property ItemsLocation() As ItemsLocation
    Get
      Return mobjItemsLocation
    End Get
    Set(ByVal value As ItemsLocation)
      mobjItemsLocation = value
    End Set
  End Property

  Public Overridable ReadOnly Property Operation() As String Implements IBatchItem.Operation
    Get
      Return mstrOperationType
    End Get
  End Property

  Public Property Process As IOperable Implements IBatchItem.Process
    Get
      Return MobjProcess
    End Get
    Set(value As IOperable)
      MobjProcess = value
    End Set
  End Property

  Public Property ProcessResult As IProcessResult Implements IBatchItem.ProcessResult
    Get
      Return mobjProcessResults
    End Get
    Set(ByVal value As IProcessResult)
      mobjProcessResults = value
    End Set
  End Property

  Public Property Document As Core.Document Implements IBatchItem.Document
    Get
      Return mobjDocument
    End Get
    Set(value As Core.Document)
      mobjDocument = value
    End Set
  End Property

  Public Property [Object] As CustomObject Implements IWorkItem.Object
    Get
      Return mobjCustomObject
    End Get
    Set(value As CustomObject)
      mobjCustomObject = value
    End Set
  End Property

  Public Property Folder As Core.Folder Implements IBatchItem.Folder
    Get
      Return mobjFolder
    End Get
    Set(value As Core.Folder)
      mobjFolder = value
    End Set
  End Property

  Public ReadOnly Property Batch() As Batch
    Get
      Return MobjBatch
    End Get
  End Property

  Public Property Parent As IItemParent Implements IBatchItem.Parent
    Get
      Return MobjBatch
    End Get
    Set(value As IItemParent)
      MobjBatch = value
    End Set
  End Property

  Public ReadOnly Property ProcessEventArgs() As BatchItemProcessEventArgs
    Get
      Return mobjBatchItemProcessEventArgs
    End Get
  End Property

#End Region

#Region "Constructors"

  Public Sub New()
    mdatCreateDate = Now
  End Sub

  Public Sub New(ByVal lpSourceDocId As String,
                ByVal lpTitle As String,
                ByVal lpBatch As Batch)
    Me.New(lpSourceDocId, String.Empty, lpTitle, lpBatch.Operation, lpBatch)
  End Sub

  Public Sub New(ByVal lpSourceDocId As String,
              ByVal lpTitle As String,
              ByVal lpOperation As String,
              ByVal lpBatch As Batch)
    Me.New(lpSourceDocId, String.Empty, lpTitle, lpOperation, lpBatch)
  End Sub

  Public Sub New(ByVal lpSourceDocId As String,
                 ByVal lpDestinationDocId As String,
                 ByVal lpTitle As String,
                 ByVal lpOperation As String,
                 ByVal lpBatch As Batch)

    Try
      mstrSourceDocId = lpSourceDocId
      mstrDestDocId = lpDestinationDocId
      mstrTitle = lpTitle
      mstrBatchId = lpBatch.Id
      mstrOperationType = lpOperation

      'Instead of pointing to existing batch create a new instance
      MobjBatch = lpBatch
      mdatCreateDate = Now

      'Trying to make it thread safe
      'mobjBatch = lpBatch.Clone
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Sub

  Public Sub New(ByVal lpSourceDocId As String,
             ByVal lpDestinationDocId As String,
             ByVal lpTitle As String,
             ByVal lpBatch As Batch,
             ByVal lpProcessedStatus As String,
             ByVal lpProcessedMessage As String,
             ByVal lpProcessStartTime As DateTime,
             ByVal lpProcessFinishTime As DateTime,
             ByVal lpTotalProcessingTime As Single,
             ByVal lpProcessedBy As String,
             ByVal lpCreateDate As DateTime)
    Me.New(lpSourceDocId, lpDestinationDocId, lpTitle, lpBatch,
           lpProcessedStatus, lpProcessedMessage, lpProcessStartTime,
           lpProcessFinishTime, lpTotalProcessingTime, lpProcessedBy,
           lpCreateDate, String.Empty)
  End Sub

  Public Sub New(ByVal lpSourceDocId As String,
               ByVal lpDestinationDocId As String,
               ByVal lpTitle As String,
               ByVal lpBatch As Batch,
               ByVal lpProcessedStatus As String,
               ByVal lpProcessedMessage As String,
               ByVal lpProcessStartTime As DateTime?,
               ByVal lpProcessFinishTime As DateTime?,
               ByVal lpTotalProcessingTime As Single?,
               ByVal lpProcessedBy As String,
               ByVal lpCreateDate As DateTime?,
               ByVal lpProcessResult As String)

    Try
      mstrSourceDocId = lpSourceDocId
      mstrDestDocId = lpDestinationDocId
      mstrTitle = lpTitle
      mstrBatchId = lpBatch.Id
      mstrOperationType = lpBatch.Operation
      'Instead of pointing to existing batch create a new instance
      MobjBatch = lpBatch
      '	mdatCreateDate = Now

      If Not String.IsNullOrEmpty(lpProcessedStatus) Then
        menuProcessedStatus = [Enum].Parse(GetType(ProcessedStatus), lpProcessedStatus)
      End If

      mstrProcessedMessage = lpProcessedMessage

      If lpProcessStartTime IsNot Nothing AndAlso lpProcessStartTime.HasValue Then
        mdteStartTime = lpProcessStartTime.Value
      End If

      If lpProcessFinishTime IsNot Nothing AndAlso lpProcessFinishTime.HasValue Then
        mdteFinishTime = lpProcessFinishTime.Value
      End If

      If lpTotalProcessingTime IsNot Nothing AndAlso lpTotalProcessingTime.HasValue Then
        mobjTotalProcessingTime = TimeSpan.FromSeconds(lpTotalProcessingTime)
      End If

      mstrProcessedBy = lpProcessedBy

      If lpCreateDate IsNot Nothing AndAlso lpCreateDate.HasValue Then
        mdatCreateDate = lpCreateDate.Value
      End If

      If Not String.IsNullOrEmpty(lpProcessResult) Then
        mobjProcessResults = Serializer.Deserialize.XmlString(lpProcessResult, GetType(ProcessResult))
      End If

      'Trying to make it thread safe
      'mobjBatch = lpBatch.Clone
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Sub

  Protected Sub New(ByVal lpBatch As Batch)

    Try
      MobjBatch = lpBatch
      mdatCreateDate = Now
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Sub

#End Region

#Region "Public Methods"

  Public Overridable Function Execute(lpProcess As IOperable) As Boolean Implements IBatchItem.Execute

    Dim lblnReturn As Boolean

    Try

      If Me.ProcessedStatus <> OperationEnumerations.ProcessedStatus.NotProcessed Then
        Return False
      End If

      BeginProcessItem()

      ' Return False

      ' Call the associated process
      'If Me.Process IsNot Nothing Then
      If lpProcess IsNot Nothing Then
        'Dim lobjResult As Result = Me.Process.Execute(Me)
        Dim lobjResult As OperationEnumerations.Result = lpProcess.Execute(Me)
        If lobjResult = OperationEnumerations.Result.Success Then
          lblnReturn = True
          TransactionCounter.Instance.Increment()
          Me.Batch.CurrentItemsProcessed += 1
          If lpProcess.LogResult = True Then
            If String.IsNullOrEmpty(ProcessedMessage) Then
              EndProcessItem(ProcessedStatus.Success, String.Empty, Me.DestinationDocId)
            Else
              EndProcessItem(ProcessedStatus.Success, ProcessedMessage, Me.DestinationDocId)
            End If
            'EndProcessItem(ProcessedStatus.Success, String.Empty, Me.DestinationDocId)
          End If
        ElseIf lobjResult = OperationEnumerations.Result.Failed Then
          lblnReturn = False
          If lpProcess.LogResult = True Then
            EndProcessItem(ProcessedStatus.Failed, ProcessedMessage, Me.DestinationDocId)
          End If
        End If
      Else

      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      EndProcessItem(Migrations.ProcessedStatus.Failed, ex.Message, Me.DestinationDocId)
    Finally
      If Me.Document IsNot Nothing AndAlso Me.Document.IsDisposed = False Then
        Me.Document.Dispose()
      End If
      ' Me.Process.Dispose()
    End Try

    Return lblnReturn

  End Function

  Public Sub BeginProcessItem() Implements IBatchItem.BeginProcessItem

    Try
      mobjBatchItemProcessEventArgs = BatchItemProcessEventArgs.InitializeEvent(Me.Id,
                                                                  Me.BatchId,
                                                                  Me.Title,
                                                                  Me.Operation,
                                                                  OperationEnumerations.ProcessedStatus.Processing,
                                                                  Me.Batch.MachineName)
      Me.Batch.BatchContainer.BeginProcessItem(mobjBatchItemProcessEventArgs)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Sub

  Public Sub EndProcessItem(ByVal lpProcessedStatus As ProcessedStatus,
                            ByVal lpProcessedMessage As String,
                            ByVal lpDestDocId As String) Implements IBatchItem.EndProcessItem

    Try
      EndProcessItem(lpProcessedStatus, lpProcessedMessage, lpDestDocId, Now)

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Sub

  Public Sub EndProcessItem(ByVal lpProcessedStatus As ProcessedStatus,
                            ByVal lpProcessedMessage As String,
                            ByVal lpDestDocId As String,
                            ByVal lpEndTime As DateTime) Implements IBatchItem.EndProcessItem

    Try
      With mobjBatchItemProcessEventArgs
        .ProcessedStatus = lpProcessedStatus
        .ProcessedMessage = lpProcessedMessage
        .DestDocId = lpDestDocId
        .EndTime = lpEndTime
        .Process = Me.Process
      End With

      Me.ProcessedStatus = lpProcessedStatus

      Me.Batch.BatchContainer.EndProcessItem(mobjBatchItemProcessEventArgs)

      mobjBatchItemProcessEventArgs = Nothing
      Me.Process.Reset()

      ' Clear out the memory used by the document, we no longer need it.
      If Me.Document IsNot Nothing Then
        Me.Document.Dispose()
        Me.Document = Nothing
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Sub

  ''' <summary>
  ''' 
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function Clone() As Object _
         Implements System.ICloneable.Clone

    Dim lobjBatchItem As BatchItem = Nothing

    Try

      ' lobjBatch.Name = Me.Batch.Name
      Dim lobjBatch As New Batch(Me.Batch.ItemsLocation) With {
        .AssignedTo = Me.Batch.AssignedTo,
        .BasePathLocation = Me.Batch.BasePathLocation,
        .ContentStorageType = Me.Batch.ContentStorageType,
        .DeclareAsRecordOnImport = Me.Batch.DeclareAsRecordOnImport,
        .DeclareRecordConfiguration = Me.Batch.DeclareRecordConfiguration,
        .Description = Me.Batch.Description,
        .DestinationConnectionString = Me.Batch.DestinationConnectionString,
        .DocumentFilingMode = Me.Batch.DocumentFilingMode,
        .ExportPath = Me.Batch.ExportPath,
        .FolderDelimiter = Me.Batch.FolderDelimiter,
        .Id = Me.Batch.Id,
        .LeadingDelimiter = Me.Batch.LeadingDelimiter,
        .Number = Me.Batch.Number,
        .Operation = Me.Batch.Operation,
        .SourceConnectionString = Me.Batch.SourceConnectionString,
        .Transformations = Me.Batch.Transformations
      }

      lobjBatchItem = New BatchItem(lobjBatch)

      With lobjBatchItem
        .DestinationDocId = Me.DestinationDocId
        '.Batch = Me.Batch
        .BatchId = Me.BatchId
        .Id = Me.Id
        .ItemsLocation = Me.ItemsLocation
        mstrOperationType = Me.Operation
        .ProcessedBy = Me.ProcessedBy
        .ProcessedMessage = Me.ProcessedMessage
        .ProcessedStatus = Me.ProcessedStatus
        '.ProcessEventArgs = Me.ProcessEventArgs
        .FinishTime = Me.FinishTime
        .StartTime = Me.StartTime
        .SourceDocId = Me.SourceDocId
        .Title = Me.Title
      End With

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      Return Nothing
    End Try

    Return lobjBatchItem

  End Function

  Public Overridable Function ToJsonString(ByVal lpIncludeProcessResult As Boolean) As String Implements IWorkItem.ToJsonString
    Try
      Return WorkItem.ToJsonString(Me, lpIncludeProcessResult)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  'Public Shared Function CreateBatchItem(ByVal lpOperationType As String, _
  '                                       ByVal lpDocId As String, _
  '                                       ByVal lpDocTitle As String, _
  '                                       ByVal lpBatch As Batch) As BatchItem

  '  Dim lobjBatchItem As BatchItem = Nothing

  '  Try

  '    Select Case lpOperationType

  '      Case OperationType.Export
  '        lobjBatchItem = New ExportBatchItem(lpDocId, lpDocTitle, lpBatch)

  '      Case OperationType.Migrate
  '        lobjBatchItem = New MigrateBatchItem(lpDocId, lpDocTitle, lpBatch)

  '      Case OperationType.Delete
  '        lobjBatchItem = New DeleteBatchItem(lpDocId, lpDocTitle, lpBatch)

  '      Case OperationType.CancelCheckOut
  '        lobjBatchItem = New CancelCheckoutBatchItem(lpDocId, lpDocTitle, lpBatch)

  '      Case OperationType.Replace
  '        lobjBatchItem = New ReplaceBatchItem(lpDocId, lpDocTitle, lpBatch)

  '        'Case OperationType.MigratePhysicalRecord
  '        '  lobjBatchItem = New MigratePhysicalRecord(lpDocId, lpDocTitle, lpBatch)

  '        'Case OperationType.ReplacePhysicalRecord
  '        '  lobjBatchItem = New ReplacePhysicalRecord(lpDocId, lpDocTitle, lpBatch)

  '      Case OperationType.UnFile
  '        lobjBatchItem = New UnfileBatchItem(lpDocId, lpDocTitle, lpBatch)

  '      Case Else
  '        Throw New Exception(String.Format("Unsupported operation type '{0}' in CreateBatchItem", lpOperationType))
  '    End Select

  '  Catch ex As Exception
  '    ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod)
  '  End Try

  '  Return lobjBatchItem

  'End Function

  Public Overrides Function ToString() As String

    Try
      Return DebuggerIdentifier()

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

#End Region

#Region "Friend Methods"

  Friend Sub SetBatch(lpBatch As Batch)
    Try
      MobjBatch = lpBatch
    Catch ex As Exception
      ApplicationLogging.LogException(ex, MethodBase.GetCurrentMethod())
      '  Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

#Region "Protected Methods"

  Protected Friend Overridable Function DebuggerIdentifier() As String

    Dim lobjIdentifierBuilder As New StringBuilder

    Try

      If Not String.IsNullOrEmpty(Id) Then
        lobjIdentifierBuilder.AppendFormat("{0}: ", Id)

      Else
        lobjIdentifierBuilder.Append("Id not set: ")
      End If

      lobjIdentifierBuilder.AppendFormat("{0}", Operation.ToString)

      If Not String.IsNullOrEmpty(Title) Then
        lobjIdentifierBuilder.AppendFormat(";Title=({0})", Title)
      End If

      If Not String.IsNullOrEmpty(SourceDocId) Then
        lobjIdentifierBuilder.AppendFormat(";SourceDocId=({0})", SourceDocId)
      End If

      If Not String.IsNullOrEmpty(DestinationDocId) Then
        lobjIdentifierBuilder.AppendFormat(";DestDocId=({0})", DestinationDocId)
      End If

      lobjIdentifierBuilder.AppendFormat(";ProcessedStatus={0}", ProcessedStatus)

      If ProcessedStatus = ProcessedStatus.Failed AndAlso Not String.IsNullOrEmpty(ProcessedMessage) Then
        lobjIdentifierBuilder.AppendFormat("<{0}>", ProcessedMessage)
      End If

      Return lobjIdentifierBuilder.ToString

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      Return lobjIdentifierBuilder.ToString
    End Try

  End Function

  ' <Added by: Ernie Bahr at: 9/29/2011-10:17:03 AM on machine: ERNIE-M4400>
  ''' <summary>
  ''' Initializes the classification related properties for 
  ''' both the source and destination providers if applicable.
  ''' </summary>
  ''' <remarks>
  ''' This is to resolve the issue of the first document in the run from always 
  ''' taking substantially longer to complete than the subsequent documents.
  ''' </remarks>
  Protected Friend Overridable Sub InitializeClassificationProperties(ByVal lpScope As ExportScope)
    Try
      ' This is to resolve the issue of the first document in the run from always taking 
      ' substantially longer to complete than the subsequent documents.

      Select Case lpScope
        Case ExportScope.Source
          ' Initialize the classification properties of the source provider (if applicable)
          If Me.Batch.SourceConnectionString IsNot Nothing Then
            Me.Batch.SourceContentSource.Provider?.InitializeClassificationProperties()
          End If
        Case ExportScope.Destination
          ' Initialize the classification properties of the destination provider (if applicable)
          Me.Batch.DestinationContentSource?.Provider?.InitializeClassificationProperties()
        Case ExportScope.Both
          ' Initialize the classification properties of the source provider (if applicable)
          If Me.Batch.SourceConnectionString IsNot Nothing Then
            Me.Batch.SourceContentSource.Provider?.InitializeClassificationProperties()
          End If

          ' Initialize the classification properties of the destination provider (if applicable)
          Me.Batch.DestinationContentSource?.Provider?.InitializeClassificationProperties()
      End Select

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub
  ' </Added by: Ernie Bahr at: 9/29/2011-10:17:03 AM on machine: ERNIE-M4400>

#End Region

  Private Sub MobjProcess_OperatingError(sender As Object, e As OperableErrorEventArgs) Handles MobjProcess.OperatingError
    Try

      If e.WorkItem.Parent.SourceConnection.State <> ProviderConnectionState.Connected Then
        Batch.Job.CancelJob(String.Format("The source system '{0}' is no longer available.", Batch.SourceContentSource.Name))
      End If

      If Me.Document IsNot Nothing AndAlso Me.Document.IsDisposed = False Then
        Me.Document.Dispose()
      End If


    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

End Class
