'---------------------------------------------------------------------------------
' <copyright company="ECMG">
'     Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'     Copying or reuse without permission is strictly forbidden.
' </copyright>
'---------------------------------------------------------------------------------

#Region "Imports"

Imports System.Globalization
Imports System.Text
Imports System.Xml.Serialization
Imports Documents.Utilities
Imports Newtonsoft.Json
Imports Projects.Converters
Imports Projects.Operations

#End Region

<DebuggerDisplay("{DebuggerIdentifier(),nq}")> _
Public Class WorkSummary
  Implements IWorkSummary
  Implements IComparable

#Region "Class Variables"

  Private mlngNotProcessedCount As Long = 0
  Private mlngSuccessCount As Long = 0
  Private mlngFailedCount As Long = 0
  Private mintProcessingCount As Integer = 0
  Private mlngTotalItemsCount As Long = 0
  Private mdblAvgProcessingTime As Double = 0
  Private mstrName As String = String.Empty
  Private mstrOperationType As String = OperationType.NotSet
  Private mobjJob As Job = Nothing

  Private mdatStartTime As DateTime
  Private mdatFinishTime As DateTime
  Private mdatLastUpdateTime As DateTime
  Private mdblPeakProcessingRate As Double = 0
  Private mdblProcessingRate As Double
  Private mobjStatusEntries As StatusEntries

#End Region

#Region "Public Properties"

  '#Region "Status Colors"

  '  <JsonIgnore()> _
  '  Public ReadOnly Property SuccessColor As Brush
  '    Get

  '      Try

  '        If (SuccessCount = 0) Then
  '          Return New SolidColorBrush(Colors.Transparent)

  '        Else
  '          If ConnectionSettings.Instance.CurrentTheme <> METRO_DARK Then
  '            Return New SolidColorBrush(Colors.Green)
  '          Else
  '            Return New SolidColorBrush(Colors.LightGreen)
  '          End If
  '        End If

  '      Catch ex As Exception
  '        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '        ' Re-throw the exception to the caller
  '        Throw
  '      End Try

  '    End Get
  '  End Property

  '  <JsonIgnore()> _
  '  Public ReadOnly Property FailureColor As Brush
  '    Get

  '      Try

  '        If (FailedCount = 0) Then
  '          Return New SolidColorBrush(Colors.Transparent)

  '        Else
  '          If ConnectionSettings.Instance.CurrentTheme <> METRO_DARK Then
  '            Return New SolidColorBrush(Colors.DarkRed)
  '          Else
  '            Return New SolidColorBrush(Colors.Salmon)
  '          End If
  '        End If

  '      Catch ex As Exception
  '        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '        ' Re-throw the exception to the caller
  '        Throw
  '      End Try

  '    End Get
  '  End Property

  '  <JsonIgnore()> _
  '  Public ReadOnly Property CompletedColor As Brush
  '    Get

  '      Try

  '        If (NotProcessedCount = TotalItemsCount) Then
  '          Return New SolidColorBrush(Colors.Transparent)

  '        Else
  '          If ConnectionSettings.Instance.CurrentTheme <> METRO_DARK Then
  '            Return New SolidColorBrush(Colors.DarkBlue)
  '          Else
  '            Return New SolidColorBrush(Colors.SkyBlue)
  '          End If

  '        End If

  '      Catch ex As Exception
  '        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '        ' Re-throw the exception to the caller
  '        Throw
  '      End Try

  '    End Get
  '  End Property

  '  <JsonIgnore()> _
  '  Public ReadOnly Property NameColor As Brush
  '    Get

  '      Try

  '        If (FailedCount > 0) Then
  '          ' We had some failures
  '          Return FailureColor

  '        ElseIf ProcessedPercentage = 1 AndAlso SuccessPercentage = 1 Then
  '          ' We completed everything successfully
  '          Return SuccessColor

  '        Else
  '          If ConnectionSettings.Instance.CurrentTheme <> METRO_DARK Then
  '            Return New SolidColorBrush(Colors.DarkBlue)
  '          Else
  '            Return New SolidColorBrush(Colors.SkyBlue)
  '          End If
  '        End If

  '      Catch ex As Exception
  '        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '        ' Re-throw the exception to the caller
  '        Throw
  '      End Try

  '    End Get
  '  End Property

  '  <JsonIgnore()> _
  '  Public ReadOnly Property NotProcessedColor As Brush
  '    Get

  '      Try

  '        If NotProcessedCount > 0 Then
  '          If ConnectionSettings.Instance.CurrentTheme <> METRO_DARK Then
  '            Return New SolidColorBrush(Colors.DarkGray)
  '          Else
  '            Return New SolidColorBrush(Colors.LightGray)
  '          End If

  '        Else
  '          Return New SolidColorBrush(Colors.Transparent)
  '        End If

  '      Catch ex As Exception
  '        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '        ' Re-throw the exception to the caller
  '        Throw
  '      End Try

  '    End Get
  '  End Property

  '#End Region

  <JsonProperty("name")> _
  Public ReadOnly Property Name As String Implements IWorkSummary.Name
    Get
      Return mstrName
    End Get
  End Property

  <JsonProperty("operation")> _
  Public Property Operation As String
    Get
      Return mstrOperationType
    End Get
    Set(value As String)
      mstrOperationType = value
    End Set
  End Property

  <JsonProperty("isInitialized")> _
  Public ReadOnly Property IsInitialized As Boolean Implements IWorkSummary.IsInitialized
    Get
      Try
        If TotalItemsCount > 0 Then
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

  <JsonProperty("isCompleted")> _
  Public ReadOnly Property IsCompleted As Boolean Implements IWorkSummary.IsCompleted
    Get
      Try
        If ((TotalItemsCount > 0) AndAlso (ProcessedPercentage = 1)) Then
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

  <JsonProperty("notProcessedCount")> _
  Public ReadOnly Property NotProcessedCount() As Long Implements IWorkSummary.NotProcessedCount
    Get
      Return mlngNotProcessedCount
    End Get
  End Property

  <JsonIgnore()> _
  Public ReadOnly Property NotProcessedCountString() As String
    Get
      Return FormatString(mlngNotProcessedCount)
    End Get
  End Property

  <JsonProperty("processedCount")> _
  Public ReadOnly Property ProcessedCount As Long Implements IWorkSummary.ProcessedCount
    Get
      Return TotalItemsCount - NotProcessedCount
    End Get
  End Property

  <JsonIgnore(), XmlIgnore()> _
  Public ReadOnly Property StatusEntries As IList(Of StatusEntry) Implements IWorkSummary.StatusEntries
    Get
      Try
        Return mobjStatusEntries
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  <JsonProperty("successCount")> _
  Public ReadOnly Property SuccessCount() As Long Implements IWorkSummary.SuccessCount
    Get
      Return mlngSuccessCount
    End Get
  End Property

  <JsonProperty("failedCount")> _
  Public ReadOnly Property FailedCount() As Long Implements IWorkSummary.FailedCount
    Get
      Return mlngFailedCount
    End Get
  End Property

  <JsonProperty("processingCount")> _
  Public ReadOnly Property ProcessingCount() As Integer Implements IWorkSummary.ProcessingCount
    Get
      Return mintProcessingCount
    End Get
  End Property

  <JsonProperty("totalItemsCount")> _
  Public ReadOnly Property TotalItemsCount() As Long Implements IWorkSummary.TotalItemsCount
    Get
      Return mlngTotalItemsCount
    End Get
  End Property

  <JsonProperty("avgProcessingTime")> _
  Public ReadOnly Property AvgProcessingTime() As Double Implements IWorkSummary.AvgProcessingTime
    Get
      Return mdblAvgProcessingTime
    End Get
  End Property

  <JsonProperty("totalProcessingTime")> _
  Public ReadOnly Property TotalProcessingTime As Double Implements IWorkSummary.TotalProcessingTime
    Get
      Try
        GetTotalProcessingTime()
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  <JsonProperty("startTime")> _
  Public ReadOnly Property StartTime As Date Implements IWorkSummary.StartTime
    Get
      Try
        Return mdatStartTime
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  <JsonProperty("finishTime")> _
  Public ReadOnly Property FinishTime As Date Implements IWorkSummary.FinishTime
    Get
      Try
        If ProcessingRate = 0 Then
          Return mdatFinishTime
        Else
          Return ProjectedFinishTime
        End If
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  <JsonIgnore()> _
  Public ReadOnly Property FinishTimeString As String
    Get
      Try
        If Not FinishTime = DateTime.MinValue Then
          Return FinishTime.ToString("F")
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

  <JsonProperty("lastUpdateTime")> _
  Public ReadOnly Property LastUpdateTime As Date Implements IWorkSummary.LastUpdateTime
    Get
      Try
        Return mdatLastUpdateTime
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  <JsonIgnore()> _
  Public ReadOnly Property LastUpdateTimeString As DateTime
    Get
      Try
        Return LastUpdateTime.ToString("F")
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  <JsonIgnore()> _
  Public ReadOnly Property ProjectedFinishTime As DateTime
    Get
      Try
        If ProcessingRate = 0 Then
          Return DateTime.MaxValue
        Else
          Return LastUpdateTime.AddSeconds(NotProcessedCount / DocsPerSecond)
        End If
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  <JsonIgnore()> _
  Public ReadOnly Property ProjectedFinishTimeString As DateTime
    Get
      Try
        If ProcessingRate = 0 Then
          Return String.Empty
        Else
          Return ProjectedFinishTime.ToString("F")
        End If
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  <JsonProperty("processingRate")> _
  Public ReadOnly Property ProcessingRate As Double Implements IWorkSummary.ProcessingRate
    Get
      Try
        Return mdblProcessingRate
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  <JsonProperty("peakProcessingRate")> _
  Public ReadOnly Property PeakProcessingRate As Double Implements IWorkSummary.PeakProcessingRate
    Get
      Try
        Return mdblPeakProcessingRate
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  <JsonIgnore()> _
  Public ReadOnly Property DocsPerSecond As Double
    Get
      Try
        Return ProcessingRate
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  <JsonIgnore()> _
  Public ReadOnly Property DocsPerMinute As Double
    Get
      Try
        Return ProcessingRate * 60
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  <JsonIgnore()> _
  Public ReadOnly Property DocsPerHour As Double
    Get
      Try
        Return ProcessingRate * 3600
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  <JsonIgnore()> _
  Public ReadOnly Property PeakDocsPerSecond As Double
    Get
      Try
        Return PeakProcessingRate
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  <JsonIgnore()> _
  Public ReadOnly Property PeakDocsPerMinute As Double
    Get
      Try
        Return PeakProcessingRate * 60
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  <JsonIgnore()> _
  Public ReadOnly Property PeakDocsPerHour As Double
    Get
      Try
        Return PeakProcessingRate * 3600
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

#Region "FormattedRates"

  <JsonIgnore()> _
  Public ReadOnly Property DocsPerSecondString As String
    Get
      Try
        If DocsPerSecond > 0 Then
          Return String.Format("{0:N1} docs/sec", DocsPerSecond)
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

  <JsonIgnore()> _
  Public ReadOnly Property DocsPerMinuteString As String
    Get
      Try
        If DocsPerMinute > 0 Then
          Return String.Format("{0:N0} docs/min", DocsPerMinute)
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

  <JsonIgnore()> _
  Public ReadOnly Property DocsPerHourString As String
    Get
      Try
        If DocsPerHour > 0 Then
          Return String.Format("{0:N0} docs/hr", DocsPerHour)
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

  <JsonIgnore()> _
  Public ReadOnly Property PeakDocsPerSecondString As String
    Get
      Try
        If PeakDocsPerSecond > 0 Then
          Return String.Format("{0:N1} docs/sec", PeakDocsPerSecond)
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

  <JsonIgnore()> _
  Public ReadOnly Property PeakDocsPerMinuteString As String
    Get
      Try
        If PeakDocsPerMinute > 0 Then
          Return String.Format("{0:N0} docs/min", PeakDocsPerMinute)
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

  <JsonIgnore()> _
  Public ReadOnly Property PeakDocsPerHourString As String
    Get
      Try
        If PeakDocsPerHour > 0 Then
          Return String.Format("{0:N0} docs/hr", PeakDocsPerHour)
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

#End Region

  <JsonIgnore()> _
  Public ReadOnly Property OperationString As String Implements IWorkSummary.Operation
    Get

      Try

        If Operation <> OperationType.NotSet Then
          Return mstrOperationType

        Else
          Return "---------"
        End If

      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try

    End Get
  End Property

  <JsonIgnore()> _
  Public ReadOnly Property Job As Job
    Get
      Try
        Return mobjJob
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

#Region "Percentages"

  <JsonProperty("failurePercentage")> _
  Public ReadOnly Property FailurePercentage As Single Implements IWorkSummary.FailurePercentage
    Get

      Try
        Return GetFailurePercentage()

      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try

    End Get
  End Property

  <JsonIgnore()> _
  Public ReadOnly Property FailurePercentageString As String
    Get

      Try
        Return Helper.FormatPercentage(FailurePercentage)

      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try

    End Get
  End Property

  <JsonProperty("successPercentage")> _
  Public ReadOnly Property SuccessPercentage As Single Implements IWorkSummary.SuccessPercentage
    Get

      Try
        Return GetSuccessPercentage()

      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try

    End Get
  End Property

  <JsonIgnore()> _
  Public ReadOnly Property SuccessPercentageString As String

    Get

      Try
        Return Helper.FormatPercentage(SuccessPercentage)

      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try

    End Get
  End Property

  <JsonProperty("processedPercentage")> _
  Public ReadOnly Property ProcessedPercentage As Single Implements IWorkSummary.ProcessedPercentage
    Get

      Try
        Return GetProcessedPercentage()

      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try

    End Get
  End Property

  <JsonProperty("progressPercentage")> _
  Public ReadOnly Property ProgressPercentage As Single Implements IWorkSummary.ProgressPercentage
    Get

      Try
        Return GetProgressPercentage()

      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try

    End Get
  End Property

  <JsonIgnore()> _
  Public ReadOnly Property ProcessedPercentageString As String
    Get

      Try
        Return Helper.FormatPercentage(ProcessedPercentage)

      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try

    End Get
  End Property

  <JsonProperty("notProcessedPercentage")> _
  Public ReadOnly Property NotProcessedPercentage As Single Implements IWorkSummary.NotProcessedPercentage
    Get

      Try
        Return GetNotProcessedPercentage()

      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try

    End Get
  End Property

  <JsonIgnore()> _
  Public ReadOnly Property NotProcessedPercentageString As String
    Get

      Try
        Return Helper.FormatPercentage(NotProcessedPercentage)

      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try

    End Get
  End Property

#End Region

#End Region

#Region "Constructors"

  Public Sub New()

  End Sub

  Public Sub New(ByVal lpBatch As Batch, _
                 ByVal lpNotProcessedCount As Integer, _
                 ByVal lpSuccessCount As Integer, _
                 ByVal lpFailedCount As Integer, _
                 ByVal lpProcessingCount As Integer, _
                 ByVal lpTotalItemsInBatchCount As Integer, _
                 ByVal lpAvgProcessingTime As Double)

    Try
      mstrName = lpBatch.Name
      mstrOperationType = lpBatch.Operation
      mlngNotProcessedCount = lpNotProcessedCount
      mlngSuccessCount = lpSuccessCount
      mlngFailedCount = lpFailedCount
      mintProcessingCount = lpProcessingCount
      mlngTotalItemsCount = lpTotalItemsInBatchCount
      mdblAvgProcessingTime = lpAvgProcessingTime

      InitializeStatusEntries()

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Sub

  Public Sub New(ByVal lpJob As Job,
                 ByVal lpNotProcessedCount As Long,
                 ByVal lpSuccessCount As Long,
                 ByVal lpFailedCount As Long,
                 ByVal lpProcessingCount As Integer,
                 ByVal lpTotalItemsInBatchCount As Long,
                 ByVal lpAvgProcessingTime As Double,
                 ByVal lpStartTime As DateTime,
                 ByVal lpFinishTime As DateTime,
                 ByVal lpLastUpdateTime As DateTime,
                 ByVal lpProcessingRate As Double,
                 ByVal lpPeakProcessingRate As Double)

    Try
      mobjJob = lpJob
      mstrName = lpJob.Name
      mstrOperationType = lpJob.Operation
      mlngNotProcessedCount = lpNotProcessedCount
      mlngSuccessCount = lpSuccessCount
      mlngFailedCount = lpFailedCount
      mintProcessingCount = lpProcessingCount
      mlngTotalItemsCount = lpTotalItemsInBatchCount
      mdblAvgProcessingTime = lpAvgProcessingTime
      mdatStartTime = lpStartTime
      mdatFinishTime = lpFinishTime
      mdatLastUpdateTime = lpLastUpdateTime
      mdblProcessingRate = lpProcessingRate
      mdblPeakProcessingRate = lpPeakProcessingRate

      InitializeStatusEntries()

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Sub

  Public Sub New(ByVal lpName As String, _
                 ByVal lpOperationType As String, _
                 ByVal lpNotProcessedCount As Long, _
                 ByVal lpSuccessCount As Long, _
                 ByVal lpFailedCount As Long, _
                 ByVal lpProcessingCount As Integer, _
                 ByVal lpTotalItemsInBatchCount As Long, _
                 ByVal lpAvgProcessingTime As Double,
                 ByVal lpStartTime As DateTime, _
                 ByVal lpFinishTime As DateTime, _
                 ByVal lpLastUpdateTime As DateTime, _
                 ByVal lpProcessingRate As Double, _
                 ByVal lpPeakProcessingRate As Double)

    Try
      mstrName = lpName

      If Not String.IsNullOrEmpty(lpOperationType) Then
        mstrOperationType = lpOperationType

      Else
        mstrOperationType = OperationType.NotSet
      End If

      mlngNotProcessedCount = lpNotProcessedCount
      mlngSuccessCount = lpSuccessCount
      mlngFailedCount = lpFailedCount
      mintProcessingCount = lpProcessingCount
      mlngTotalItemsCount = lpTotalItemsInBatchCount
      mdblAvgProcessingTime = lpAvgProcessingTime
      mdatStartTime = lpStartTime
      mdatFinishTime = lpFinishTime
      mdatLastUpdateTime = lpLastUpdateTime
      mdblProcessingRate = lpProcessingRate
      mdblPeakProcessingRate = lpPeakProcessingRate

      InitializeStatusEntries()

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Sub

  Public Sub New(lpSQLAbbreviation As String)
    Me.New(lpSQLAbbreviation, "TOTAL")
  End Sub

  Public Sub New(lpSQLAbbreviation As String, lpName As String)
    Me.New(lpSQLAbbreviation, String.Empty, lpName)
  End Sub

  Public Sub New(lpSQLAbbreviation As String, lpOperation As String, lpName As String)
    Try

      If Not String.IsNullOrEmpty(lpName) Then
        mstrName = lpName
      End If

      If String.IsNullOrEmpty(lpSQLAbbreviation) Then
        ' If we were passed an empty string then don't try to initialize further.
        ' This could happen as a result of opening an older project that has never had this value set.
        Exit Sub
      End If

      mstrOperationType = lpOperation

      Dim lstrParts() As String = Split(lpSQLAbbreviation, ";")
      Dim lstrPartComponents() As String = Nothing

      For Each lstrPart As String In lstrParts
        If String.IsNullOrEmpty(lstrPart) Then
          Continue For
        End If
        lstrPartComponents = Split(lstrPart, "=")
        If lstrPartComponents.Count <> 2 Then
          Continue For
        End If
        Select Case lstrPartComponents(0).ToLower
          Case "successcount"
            mlngSuccessCount = lstrPartComponents(1)

          Case "failedcount"
            mlngFailedCount = lstrPartComponents(1)

          Case "notprocessedcount"
            mlngNotProcessedCount = lstrPartComponents(1)

          Case "processingcount"
            mintProcessingCount = lstrPartComponents(1)

          Case "totalitemcount"
            mlngTotalItemsCount = lstrPartComponents(1)

          Case "avgprocessingtime"
            mdblAvgProcessingTime = lstrPartComponents(1)

          Case "start"
            DateTime.TryParse(lstrPartComponents(1), mdatStartTime)

          Case "finish"
            DateTime.TryParse(lstrPartComponents(1), mdatFinishTime)

          Case "lastupdate"
            DateTime.TryParse(lstrPartComponents(1), mdatLastUpdateTime)

          Case "rate"
            mdblProcessingRate = lstrPartComponents(1)

          Case "peakrate"
            mdblPeakProcessingRate = lstrPartComponents(1)

        End Select
      Next

      InitializeStatusEntries()

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

#Region "Public Methods"

  Public Function FromJson(lpJsonString As String) As IWorkSummary
    Try
      Return JsonConvert.DeserializeObject(lpJsonString, GetType(WorkSummary), New WorkSummaryConverter())
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Function FormattedRateText As String
    Try
      If (ProcessingRate * 60) < 1 Then
        Return String.Format("{0:N1} docs/hr", ProcessingRate * 3600)
      ElseIf (ProcessingRate * 3600) < 1 Then
        Return String.Format("{0:N1} docs/min", ProcessingRate * 60)
      Else
        Return String.Format("{0:N1} docs/sec", ProcessingRate)
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

#Region "ShouldSerialize Methods"

  Public Function ShouldSerializeOperationString() As Boolean
    Try
      Return Operation <> OperationType.NotSet
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

#End Region

#End Region

#Region "Friend Methods"

  Friend Sub ClearProcessingRate()
    Try
      mdblProcessingRate = 0
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Friend Function ToSQLTotal() As String
    Try

      Dim lobjStringBuilder As New StringBuilder

      lobjStringBuilder.AppendFormat("SuccessCount={0};", SuccessCount)
      lobjStringBuilder.AppendFormat("FailedCount={0};", FailedCount)
      lobjStringBuilder.AppendFormat("NotProcessedCount={0};", NotProcessedCount)
      lobjStringBuilder.AppendFormat("ProcessingCount={0};", ProcessingCount)
      lobjStringBuilder.AppendFormat("TotalItemCount={0};", TotalItemsCount)
      lobjStringBuilder.AppendFormat("AvgProcessingTime={0};", AvgProcessingTime.ToString("0.00000", CultureInfo.InvariantCulture))

      Return lobjStringBuilder.ToString

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

#End Region

#Region "Protected Methods"

  Protected Friend Overridable Function DebuggerIdentifier() As String

    Dim lobjIdentifierBuilder As New StringBuilder

    Try

      If Not String.IsNullOrEmpty(Name) Then
        lobjIdentifierBuilder.AppendFormat("{0}: {1:N1} docs/sec ", Name, ProcessingRate)

      Else
        lobjIdentifierBuilder.Append("Name not set: ")
      End If

      lobjIdentifierBuilder.AppendFormat("{0} ", Operation.ToString)

      If SuccessPercentage = 1 Then
        lobjIdentifierBuilder.AppendFormat("({0} items, {1} Processed, {2} Success)", TotalItemsCount, ProcessedPercentageString.Replace(" ", ""), SuccessPercentageString.Replace(" ", ""))

      Else
        lobjIdentifierBuilder.AppendFormat("({0} items, {1} Processed, {2} Success, {3} Failed)", TotalItemsCount, ProcessedPercentageString.Replace(" ", ""), SuccessPercentageString.Replace(" ", ""), FailurePercentageString.Replace(" ", ""))
      End If

      Return lobjIdentifierBuilder.ToString

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      Return lobjIdentifierBuilder.ToString
    End Try

  End Function

#End Region

#Region "Private Methods"

  Private Function FormatString(ByVal lpNumber As Integer) As String

    If (lpNumber = 0) Then
      Return "-"

    Else
      Return lpNumber.ToString("0,0")
    End If

  End Function

  Private Function GetFailurePercentage() As Single

    Try

      If TotalItemsCount = 0 Then
        Return 0
      ElseIf ProcessedCount = 0 Then
        Return 0
      Else
        Return FailedCount / ProcessedCount
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  Private Function GetSuccessPercentage() As Single

    Try

      If TotalItemsCount = 0 Then
        Return 0
      ElseIf ProcessedCount = 0 Then
        Return 0
      Else
        Return SuccessCount / ProcessedCount
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  Private Function GetProgressPercentage() As Single
    Try
      If TotalItemsCount = 0 Then
        Return 0
      Else
        Return ((NotProcessedCount + SuccessCount + FailedCount) / TotalItemsCount)
      End If

    Catch Ex As Exception
      ApplicationLogging.LogException(Ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Function GetProcessedPercentage() As Single

    Try

      If TotalItemsCount = 0 Then
        Return 0
      Else
        Return (TotalItemsCount - NotProcessedCount) / TotalItemsCount
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  Private Function GetNotProcessedPercentage() As Single

    Try

      If TotalItemsCount = 0 Then
        Return 0

      Else
        Return NotProcessedCount / TotalItemsCount
      End If

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try

  End Function

  Private Function GetTotalProcessingTime() As Double
    Try
      Return AvgProcessingTime * (SuccessCount + FailedCount)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Private Sub InitializeStatusEntries()
    Try
      mobjStatusEntries = New StatusEntries()

      With mobjStatusEntries
        .Add(New StatusEntry(Me.Name, "Not Processed", NotProcessedCount))
        .Add(New StatusEntry(Me.Name, "Success", SuccessCount))
        .Add(New StatusEntry(Me.Name, "Failed", FailedCount))
      End With

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

#Region "IComparable Implementation"

  Public Function CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
    Try
      Return Name.CompareTo(obj.Name)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

#End Region

End Class
