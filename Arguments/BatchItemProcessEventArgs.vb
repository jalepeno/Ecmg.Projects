'---------------------------------------------------------------------------------
' <copyright company="ECMG">
'     Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'     Copying or reuse without permission is strictly forbidden.
' </copyright>
'---------------------------------------------------------------------------------

#Region "Imports"

Imports Documents
Imports Documents.Utilities
Imports Operations

#End Region

Public Class BatchItemProcessEventArgs

#Region "Class Variables"

  Private mstrItemId As String = String.Empty
  Private mstrBatchId As String = String.Empty
  Private mstrDestDocId As String = String.Empty
  Private mstrTitle As String = String.Empty
  Private mstrOperationType As String = Operations.OperationType.NotSet
  Private menuProcessedStatus As Migrations.ProcessedStatus
  Private mobjProcess As IOperable = Nothing
  Private mstrProcessedMessage As String = String.Empty
  Private mstrProcessedBy As String = String.Empty
  Private mdteStartTime As DateTime
  Private mdteEndTime As DateTime
  Private mobjTotalProcessedTime As TimeSpan

#End Region

#Region "Public Properties"

  Public Property ItemId() As String
    Get
      Return mstrItemId
    End Get
    Set(ByVal value As String)
      mstrItemId = value
    End Set
  End Property

  Public Property BatchId() As String
    Get
      Return mstrBatchId
    End Get
    Set(ByVal value As String)
      mstrBatchId = value
    End Set
  End Property

  Public Property DestDocId() As String
    Get
      Return mstrDestDocId
    End Get
    Set(ByVal value As String)
      mstrDestDocId = value
    End Set
  End Property

  Public Property Title() As String
    Get
      Return mstrTitle
    End Get
    Set(ByVal value As String)
      mstrTitle = value
    End Set
  End Property

  Public Property OperationType() As String
    Get
      Return mstrOperationType
    End Get
    Set(ByVal value As String)
      mstrOperationType = value
    End Set
  End Property

  Public Property Process As IOperable
    Get
      Return mobjProcess
    End Get
    Set(value As IOperable)
      mobjProcess = value
    End Set
  End Property

  Public Property ProcessedStatus() As Migrations.ProcessedStatus
    Get
      Return menuProcessedStatus
    End Get
    Set(ByVal value As Migrations.ProcessedStatus)
      menuProcessedStatus = value
    End Set
  End Property

  Public Property ProcessedMessage() As String
    Get
      Return mstrProcessedMessage
    End Get
    Set(ByVal value As String)
      mstrProcessedMessage = value
    End Set
  End Property

  Public Property ProcessedBy() As String
    Get
      Return mstrProcessedBy
    End Get
    Set(ByVal value As String)
      mstrProcessedBy = value
    End Set
  End Property

  Public Property StartTime() As DateTime
    Get
      Return mdteStartTime
    End Get
    Set(ByVal value As DateTime)
      mdteStartTime = value
    End Set
  End Property

  Public Property EndTime() As DateTime
    Get
      Return mdteEndTime
    End Get
    Set(ByVal value As DateTime)
      mdteEndTime = value
    End Set
  End Property

  Public ReadOnly Property TotalProcessingTime() As TimeSpan
    Get

      Try


        ' <Modified by: Ernie at 12/1/2011-11:50:40 AM on machine: ERNIE-M4400>
        ' Dim lobjSpan As TimeSpan = mdteEndTime.Subtract(mdteStartTime)
        ' mstrTotalProcessedTime = lobjSpan.TotalSeconds.ToString
        mobjTotalProcessedTime = mdteEndTime.Subtract(mdteStartTime)
        ' </Modified by: Ernie at 12/1/2011-11:50:40 AM on machine: ERNIE-M4400>

      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        mobjTotalProcessedTime = TimeSpan.Zero '  String.Empty
      End Try

      Return mobjTotalProcessedTime
    End Get
  End Property

#End Region

#Region "Constructors"

  Public Sub New()

  End Sub

#End Region

#Region "Public Methods"

  ''' <summary>
  ''' Called for Begin Processing
  ''' </summary>
  ''' <param name="lpItemId"></param>
  ''' <param name="lpBatchId"></param>
  ''' <param name="lpTitle"></param>
  ''' <param name="lpOperationType"></param>
  ''' <remarks></remarks>
  Public Shared Function InitializeEvent(ByVal lpItemId As String,
                                         ByVal lpBatchId As String,
                                         ByVal lpTitle As String,
                                         ByVal lpOperationType As String,
                                         ByVal lpProcessedStatus As ProcessedStatus,
                                         ByVal lpProcessedBy As String) As BatchItemProcessEventArgs

    Dim lobjBatchItemPEA As New BatchItemProcessEventArgs
    With lobjBatchItemPEA
      .ItemId = lpItemId
      .BatchId = lpBatchId
      .Title = lpTitle
      .OperationType = lpOperationType
      .ProcessedStatus = lpProcessedStatus
      .ProcessedBy = lpProcessedBy
      .StartTime = Now
    End With

    Return lobjBatchItemPEA

  End Function

#End Region

End Class
