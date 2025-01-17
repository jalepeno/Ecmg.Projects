'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  JobInfo.vb
'   Description :  [type_description_here]
'   Created     :  12/31/2013 2:21:42 PM
'   <copyright company="ECMG">
'       Copyright (c) Enterprise Content Management Group, LLC. All rights reserved.
'       Copying or reuse without permission is strictly forbidden.
'   </copyright>
'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------

#Region "Imports"

Imports System.Text
Imports Documents.Utilities
Imports Newtonsoft.Json
Imports Operations
Imports Projects.Converters

#End Region

<DebuggerDisplay("{DebuggerIdentifier(),nq}")> _
Public Class JobInfo
  Implements IJobInfo
  'Implements IFirebasePusher
  Implements IComparable

#Region "Class Variables"

  Protected mintBatchSize As Integer
  Protected mstrCancellationReason As String = String.Empty
  Protected mstrDescription As String = String.Empty
  Protected mstrDisplayName As String = String.Empty
  Protected mstrId As String = String.Empty
  Protected mblnIsCancelled As Boolean
  Protected mblnIsCompleted As Boolean
  Protected mblnIsInitialized As Boolean
  Protected mblnIsRunning As Boolean
  Protected mintBatchThreadsRunning As Integer
  Protected mlngItemsProcessed As Long
  Protected mstrName As String = String.Empty
  Protected mstrProjectName As String = String.Empty
  Protected mstrOperation As String = String.Empty
  Protected mdatCreateDate As DateTime
  Protected mobjProcess As IProcess = Nothing
  Protected mobjWorkSummary As IWorkSummary = Nothing

#End Region

#Region "IJobInfo Implementation"

  <JsonProperty("id")> _
  Public ReadOnly Property Id As String Implements IJobInfo.Id
    Get
      Try
        Return mstrId
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  <JsonProperty("name")> _
  Public ReadOnly Property Name As String Implements IJobInfo.Name
    Get
      Try
        Return mstrName
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  <JsonProperty("projectName")> _
  Public ReadOnly Property ProjectName As String Implements IJobInfo.ProjectName
    Get
      Try
        Return mstrProjectName
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  <JsonProperty("displayName")> _
  Public ReadOnly Property DisplayName As String Implements IJobInfo.DisplayName
    Get
      Try
        Return mstrDisplayName
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  <JsonProperty("description")> _
  Public ReadOnly Property Description As String Implements IJobInfo.Description
    Get
      Try
        Return mstrDescription
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  <JsonProperty("batchSize")> _
  Public ReadOnly Property BatchSize As Integer Implements IJobInfo.BatchSize
    Get
      Try
        Return mintBatchSize
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  <JsonProperty("createDate")> _
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

  <JsonProperty("isInitialized")> _
  Public ReadOnly Property IsInitialized As Boolean Implements IJobInfo.IsInitialized
    Get
      Try
        If WorkSummary IsNot Nothing Then
          mblnIsInitialized = WorkSummary.IsInitialized
        End If
        Return mblnIsInitialized
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  <JsonProperty("isCompleted")> _
  Public ReadOnly Property IsCompleted As Boolean Implements IJobInfo.IsCompleted
    Get
      Try
        If WorkSummary IsNot Nothing Then
          mblnIsCompleted = WorkSummary.IsCompleted
        End If
        Return mblnIsCompleted
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  <JsonProperty("isRunning")> _
  Public ReadOnly Property IsRunning As Boolean Implements IJobInfo.IsRunning
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

  <JsonProperty("batchThreadsRunning")> _
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

  <JsonProperty("isCancelled")> _
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

  <JsonProperty("cancellationReason")> _
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

  <JsonProperty("itemsProcessed")> _
  Public ReadOnly Property ItemsProcessed As Long Implements IJobInfo.ItemsProcessed
    Get
      Try
        If mlngItemsProcessed = 0 AndAlso WorkSummary IsNot Nothing Then
          mlngItemsProcessed = WorkSummary.ProcessedCount
        End If
        Return mlngItemsProcessed
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  <JsonProperty("operation")> _
  Public ReadOnly Property Operation As String Implements IJobInfo.Operation
    Get
      Try
        Return mstrOperation
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  <JsonProperty("workSummary")> _
  Public ReadOnly Property WorkSummary As IWorkSummary Implements IJobInfo.WorkSummary
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

  <JsonProperty("process")> _
  Public ReadOnly Property Process As IProcess Implements IJobInfo.Process
    Get
      Try
        Return mobjProcess
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public Shared Function FromJson(lpJsonString As String) As IJobInfo
    Try
      Return JsonConvert.DeserializeObject(lpJsonString, GetType(JobInfo), New JobInfoConverter())
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

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

  '#Region "IFirebasePusher Implementation"

  '  Friend Sub UpdateFirebase(lpRootPath As String) Implements IFirebasePusher.UpdateFirebase
  '    Try
  '      UpdateFirebase(lpRootPath, Me.ToFireBaseJson())
  '    Catch ex As Exception
  '      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '      ' Re-throw the exception to the caller
  '      Throw
  '    End Try
  '  End Sub

  '  Friend Sub UpdateFirebase(lpRootPath As String, lpValue As String) Implements IFirebasePusher.UpdateFirebase
  '    Try
  '      If Not ConnectionSettings.Instance.DisableNotifications Then
  '        Using lobjFirebase As New FirebaseApplication(lpRootPath)
  '          If lobjFirebase.Available Then
  '            lobjFirebase.Put(String.Empty, lpValue)
  '          End If
  '        End Using
  '      End If
  '    Catch ex As Exception
  '      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod, Me.Name)
  '      ' Re-throw the exception to the caller
  '      Throw
  '    End Try
  '  End Sub

  '  Friend Function ToFireBaseJson() As String Implements IFirebasePusher.ToFireBaseJson
  '    Try
  '      Return JsonConvert.SerializeObject(Me, Formatting.None, New JobInfoFirebaseConverter())
  '    Catch ex As Exception
  '      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '      ' Re-throw the exception to the caller
  '      Throw
  '    End Try
  '  End Function

  '#End Region

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

#Region "Constructors"

  Public Sub New()

  End Sub

  Public Sub New(lpId As String, _
               lpName As String, _
               lpProjectName As String, _
               lpItemsProcessed As Long, _
               lpCreateDate As DateTime, _
               lpWorkSummary As IWorkSummary)

    Me.New(lpId, lpName, lpName, lpProjectName, String.Empty, _
           0, False, False, False, False, 0, _
           String.Empty, lpItemsProcessed, Nothing, lpCreateDate, Nothing, lpWorkSummary)

  End Sub

  Public Sub New(lpId As String, _
             lpName As String, _
             lpProjectName As String, _
             lpItemsProcessed As Long, _
             lpOperation As String, _
             lpCreateDate As DateTime, _
             lpWorkSummary As IWorkSummary)

    Me.New(lpId, lpName, lpName, lpProjectName, String.Empty, _
           0, False, False, False, False, 0, _
           String.Empty, lpItemsProcessed, lpOperation, lpCreateDate, Nothing, lpWorkSummary)
  End Sub

  Public Sub New(lpId As String, _
                 lpName As String, _
                 lpDisplayName As String, _
                 lpProjectName As String, _
                 lpDescription As String, _
                 lpBatchSize As Integer, _
                 lpItemsProcessed As Long, _
                 lpOperation As String, _
                 lpCreateDate As DateTime, _
                 lpProcess As IProcess, _
                 lpWorkSummary As IWorkSummary)

    Me.New(lpId, lpName, lpDisplayName, lpProjectName, lpDescription, _
           lpBatchSize, False, False, False, False, 0, _
           String.Empty, lpItemsProcessed, lpOperation, lpCreateDate, lpProcess, lpWorkSummary)

  End Sub

  Public Sub New(lpJob As Job)
    Me.New(lpJob.Id, _
           lpJob.Name, _
           lpJob.DisplayName, _
           lpJob.ProjectName, _
           lpJob.Description, _
           lpJob.BatchSize, _
           lpJob.IsCancelled, _
           lpJob.IsCompleted, _
           lpJob.IsInitialized, _
           lpJob.IsRunning, _
           lpJob.BatchThreadsRunning, _
           lpJob.CancellationReason, _
           lpJob.ItemsProcessed, _
           lpJob.Operation, _
           lpJob.CreateDate,
           lpJob.Process, _
           lpJob.WorkSummary)
  End Sub

  Public Sub New(lpId As String, _
               lpName As String, _
               lpDisplayName As String, _
               lpProjectName As String, _
               lpDescription As String, _
               lpBatchSize As Integer, _
               lpIsCancelled As Boolean, _
               lpIsCompleted As Boolean, _
               lpIsInitialized As Boolean, _
               lpIsRunning As Boolean, _
               lpBatchThreadsRunning As Integer, _
               lpCancellationReason As String, _
               lpItemsProcessed As Long, _
               lpOperation As String, _
               lpCreateDate As DateTime, _
               lpProcess As IProcess, _
               lpWorkSummary As IWorkSummary)
    Try
      mstrId = lpId
      mstrName = lpName
      mstrDisplayName = lpDisplayName
      mstrProjectName = lpProjectName
      mstrDescription = lpDescription
      mintBatchSize = lpBatchSize
      mblnIsCancelled = lpIsCancelled
      mblnIsCompleted = lpIsCompleted
      mblnIsInitialized = lpIsInitialized
      mblnIsRunning = lpIsRunning
      mintBatchThreadsRunning = lpBatchThreadsRunning
      mstrCancellationReason = lpCancellationReason
      mlngItemsProcessed = lpItemsProcessed
      mstrOperation = lpOperation
      mdatCreateDate = lpCreateDate
      mobjProcess = lpProcess
      mobjWorkSummary = lpWorkSummary

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

#Region "Public Methods"

#Region "ShouldSerialize Methods"

  Public Function ShouldSerializeBatchSize() As Boolean
    Try
      Return BatchSize > 0
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Function ShouldSerializeDescription() As Boolean
    Try
      Return Not String.IsNullOrEmpty(Description)
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Function ShouldSerializeDisplayName() As Boolean
    Try
      If String.IsNullOrEmpty(DisplayName) Then
        Return False
      Else
        If String.Equals(Name, DisplayName) Then
          Return False
        Else
          Return True
        End If
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Function ShouldSerializeProcess() As Boolean
    Try
      Return Process IsNot Nothing
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

#End Region

#End Region

#Region "Protected Methods"

  Protected Friend Overridable Function DebuggerIdentifier() As String

    Dim lobjIdentifierBuilder As New StringBuilder

    Try

      If WorkSummary IsNot Nothing Then
        If String.IsNullOrEmpty(WorkSummary.Name) Then
          If Not String.IsNullOrEmpty(Name) Then
            lobjIdentifierBuilder.AppendFormat("{0}: ", Name)
          Else
            lobjIdentifierBuilder.Append("Name not set: ")
          End If
        End If
        lobjIdentifierBuilder.Append(WorkSummary.ToString)
      Else
        lobjIdentifierBuilder.Append("<not initialized>")
      End If

      Return lobjIdentifierBuilder.ToString

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      Return lobjIdentifierBuilder.ToString
    End Try

  End Function

#End Region

End Class
