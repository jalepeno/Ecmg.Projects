'  ---------------------------------------------------------------------------------
'  ---------------------------------------------------------------------------------
'   Document    :  ProjectInfo.vb
'   Description :  [type_description_here]
'   Created     :  12/31/2013 2:49:40 PM
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
Imports Projects.Converters

#End Region

<DebuggerDisplay("{DebuggerIdentifier(),nq}")>
Public Class ProjectInfo
  Implements IProjectInfo
  'Implements IFirebasePusher
  Implements IComparable

#Region "Class Variables"

  Private ReadOnly mdatCreateDate As DateTime = DateTime.MinValue
  Private ReadOnly mstrDescription As String = String.Empty
  Private ReadOnly mstrId As String = String.Empty
  Private ReadOnly mstrLocation As String = String.Empty
  Private ReadOnly mlngItemsProcessed As Long
  Private ReadOnly mobjJobs As New JobInfoCollection
  Private ReadOnly mstrName As String = String.Empty
  Private ReadOnly mobjWorkSummary As IWorkSummary = Nothing
  Private ReadOnly mintOrphanBatchCount As Integer
  Private ReadOnly mintOrphanBatchItemCount As Integer

#End Region

#Region "IProjectInfo Implementation"

  <JsonProperty("id")>
  Public ReadOnly Property Id As String Implements IProjectInfo.Id
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

  <JsonProperty("name")>
  Public ReadOnly Property Name As String Implements IProjectInfo.Name
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

  <JsonProperty("createDate")>
  Public ReadOnly Property CreateDate As Date Implements IProjectInfo.CreateDate
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

  <JsonProperty("description")>
  Public ReadOnly Property Description As String Implements IProjectInfo.Description
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

  <JsonIgnore()>
  Public ReadOnly Property Location As String Implements IProjectInfo.Location
    Get
      Try
        Return mstrLocation
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  <JsonProperty("isInitialized")>
  Public ReadOnly Property IsInitialized As Boolean Implements IProjectInfo.IsInitialized
    Get
      Try
        If WorkSummary IsNot Nothing Then
          Return WorkSummary.IsInitialized
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

  <JsonProperty("isCompleted")>
  Public ReadOnly Property IsCompleted As Boolean Implements IProjectInfo.IsCompleted
    Get
      Try
        If WorkSummary IsNot Nothing Then
          Return WorkSummary.IsCompleted
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

  <JsonProperty("itemsProcessed")>
  Public ReadOnly Property ItemsProcessed As Long Implements IProjectInfo.ItemsProcessed
    Get
      Try
        Return mlngItemsProcessed
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  <JsonProperty("orphanBatchCount")>
  Public ReadOnly Property OrphanBatchCount As Integer Implements IProjectInfo.OrphanBatchCount
    Get
      Try
        Return mintOrphanBatchCount
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  <JsonProperty("orphanBatchItemCount")>
  Public ReadOnly Property OrphanBatchItemCount As Integer Implements IProjectInfo.OrphanBatchItemCount
    Get
      Try
        Return mintOrphanBatchItemCount
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod())
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  <JsonProperty("workSummary")>
  Public ReadOnly Property WorkSummary As IWorkSummary Implements IProjectInfo.WorkSummary
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

  <JsonIgnore()>
  Public ReadOnly Property DetailedWorkSummary As WorkSummaries Implements IProjectInfo.DetailedWorkSummary
    Get
      Try
        Return GetWorkSummaries(True)
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  <JsonProperty("jobs")>
  Public ReadOnly Property Jobs As IJobInfoCollection Implements IProjectInfo.Jobs
    Get
      Try
        Return mobjJobs
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public Function ToJson() As String Implements IProjectInfo.ToJson
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
  '        Dim lblnFirebaseIsAvailable As Boolean

  '        Using lobjFirebase As New FirebaseApplication(lpRootPath)
  '          lblnFirebaseIsAvailable = lobjFirebase.Available
  '          If lblnFirebaseIsAvailable Then
  '            Dim lstrProjectJson As String

  '            lstrProjectJson = JsonConvert.SerializeObject(Me, Formatting.None, New ProjectInfoFirebaseConverter())
  '            lobjFirebase.Put(String.Empty, lstrProjectJson)
  '          End If
  '        End Using

  '        If lblnFirebaseIsAvailable Then
  '          For Each lobjJobInfo As IJobInfo In Me.Jobs
  '            DirectCast(lobjJobInfo, IFirebasePusher).UpdateFirebase(String.Format("{0}/jobs/{1}",
  '                                                       lpRootPath, lobjJobInfo.Name.Replace(".", "_")))
  '          Next
  '        End If

  '      End If
  '    Catch ex As Exception
  '      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
  '      ' Re-throw the exception to the caller
  '      Throw
  '    End Try
  '  End Sub

  '  Friend Function ToFireBaseJson() As String Implements IFirebasePusher.ToFireBaseJson
  '    Try
  '      Return JsonConvert.SerializeObject(Me, Formatting.None, New ProjectInfoFirebaseConverter())
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

  Friend Sub New(lpId As String,
                 lpName As String,
                 lpDescription As String,
                 lpCreateDate As DateTime,
                 lpLocation As String,
                 lpItemsProcessed As Long,
                 lpWorkSummary As IWorkSummary)
    Try
      mstrId = lpId
      mstrName = lpName
      mstrDescription = lpDescription
      mdatCreateDate = lpCreateDate
      mstrLocation = lpLocation
      mlngItemsProcessed = lpItemsProcessed
      mobjWorkSummary = lpWorkSummary

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Friend Sub New(lpId As String,
               lpName As String,
               lpDescription As String,
               lpCreateDate As DateTime,
               lpLocation As String,
               lpItemsProcessed As Long,
               lpOrphanBatchCount As Integer,
               lpOrphanBatchItemCount As Integer,
               lpWorkSummary As IWorkSummary)
    Try
      mstrId = lpId
      mstrName = lpName
      mstrDescription = lpDescription
      mdatCreateDate = lpCreateDate
      mstrLocation = lpLocation
      mlngItemsProcessed = lpItemsProcessed
      mintOrphanBatchCount = lpOrphanBatchCount
      mintOrphanBatchItemCount = lpOrphanBatchItemCount
      mobjWorkSummary = lpWorkSummary

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

  Friend Sub New(lpId As String,
               lpName As String,
               lpDescription As String,
               lpCreateDate As DateTime,
               lpLocation As String,
               lpItemsProcessed As Long,
               lpWorkSummary As IWorkSummary,
               lpJobs As IJobInfoCollection)
    Try
      mstrId = lpId
      mstrName = lpName
      mstrDescription = lpDescription
      mdatCreateDate = lpCreateDate
      mstrLocation = lpLocation
      mlngItemsProcessed = lpItemsProcessed
      mobjWorkSummary = lpWorkSummary
      mobjJobs = lpJobs
      InitializeChildStatuses()
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

#Region "Public Methods"

  Public Shared Function FromJson(lpJsonString As String) As IProjectInfo
    Try
      Return JsonConvert.DeserializeObject(lpJsonString, GetType(ProjectInfo), New ProjectInfoConverter())
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

#Region "ShouldSerialize Methods"

  Public Function ShouldSerializeDescription() As Boolean
    Try
      Return Not String.IsNullOrEmpty(Description)
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

      If Not String.IsNullOrEmpty(Name) Then
        lobjIdentifierBuilder.AppendFormat("{0}: ", Name)

      Else
        lobjIdentifierBuilder.Append("Name not set: ")
      End If

      If WorkSummary IsNot Nothing Then

        If Not IsInitialized Then
          lobjIdentifierBuilder.Append(" <not initialized>")
        Else
          Select Case Jobs.Count
            Case 0
              lobjIdentifierBuilder.Append(" no jobs")

            Case 1
              lobjIdentifierBuilder.Append(" 1 job")

            Case Is > 1
              lobjIdentifierBuilder.AppendFormat("{0} jobs", Jobs.Count)

          End Select

          lobjIdentifierBuilder.AppendFormat(" <{0}% complete>", Format(WorkSummary.ProcessedPercentage * 100, "##.##"))
          lobjIdentifierBuilder.AppendFormat(" ({0} items processed)", ItemsProcessed)
        End If

      End If

      Return lobjIdentifierBuilder.ToString

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      Return lobjIdentifierBuilder.ToString
    End Try

  End Function

#End Region

#Region "Private Methods"

  Private Function GetWorkSummaries(lpIncludeTotalsRow As Boolean) As WorkSummaries
    Try
      Dim lobjWorkSummaries As New WorkSummaries

      For Each lobjJob As IJobInfo In Me.Jobs
        If ((lobjJob.WorkSummary IsNot Nothing) AndAlso (TypeOf lobjJob.WorkSummary Is WorkSummary)) Then
          lobjWorkSummaries.Add(CType(lobjJob.WorkSummary, WorkSummary))

        End If

      Next

      If lpIncludeTotalsRow Then
        lobjWorkSummaries.AddTotalsRow()
      End If

      Return lobjWorkSummaries

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Friend Sub InitializeChildStatuses()
    Try
      If mobjWorkSummary IsNot Nothing AndAlso Jobs IsNot Nothing AndAlso Jobs.Count > 0 Then
        If Jobs.Count > 0 Then
          If mobjWorkSummary.StatusEntries IsNot Nothing Then
            For Each lobjStatus As StatusEntry In mobjWorkSummary.StatusEntries
              If lobjStatus.Children IsNot Nothing Then
                lobjStatus.Children.Clear()
                For Each lobjJob As IJobInfo In Jobs
                  If lobjJob.WorkSummary IsNot Nothing AndAlso lobjJob.WorkSummary.StatusEntries IsNot Nothing Then
                    lobjStatus.Children.Add(DirectCast(lobjJob.WorkSummary.StatusEntries,
                                            StatusEntries)(lobjStatus.Status))
                  End If
                Next
              End If
            Next
          End If
        End If
      End If
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Sub

#End Region

End Class
