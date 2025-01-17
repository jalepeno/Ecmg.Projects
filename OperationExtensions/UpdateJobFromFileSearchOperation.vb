' ---------------------------------------------------------------------------------
' ---------------------------------------------------------------------------------
'  Document    :  UpdateJobFromFileSearchOperation.vb
'  Description :  [type_description_here]
'  Created     :  8/13/2012 4:15:13 PM
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
Imports Documents.Exceptions
Imports Documents.Utilities
Imports Operations

#End Region

Public Class UpdateJobFromFileSearchOperation
  Inherits ProjectOperationExtension

#Region "Class Constants"

  Private Shadows ReadOnly OPERATION_NAME As String = "UpdateJobFromFileSearch"
  Private Shadows ReadOnly OPERATION_DESCRIPTION As String =
    "Updates the job specified in the 'JobName' parameter with the output of the specified file search."
  Friend Const PARAM_JOB_NAME As String = "JobName"
  Friend Const PARAM_SEARCH_ROOT As String = "SearchRoot"
  Friend Const PARAM_SEARCH_PATTERN As String = "SearchPattern"
  Friend Const PARAM_TOP_DIRECTORY_ONLY As String = "TopDirectoryOnly"

#End Region

#Region "Public Overriden Methods"

  Public Overrides ReadOnly Property Name As String
    Get
      Try
        Return OPERATION_NAME
      Catch ex As Exception
        ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
        ' Re-throw the exception to the caller
        Throw
      End Try
    End Get
  End Property

  Public Overrides ReadOnly Property CanRollback As Boolean
    Get
      Return False
    End Get
  End Property

  Protected Overrides Function OnExecute() As OperationEnumerations.Result
    Try
      Dim lstrJobName As String = MyBase.GetParameterValue(PARAM_JOB_NAME, String.Empty)
      Dim lstrSearchRoot As String = MyBase.GetParameterValue(PARAM_SEARCH_ROOT, String.Empty)
      Dim lstrSearchPattern As String = MyBase.GetParameterValue(PARAM_SEARCH_PATTERN, String.Empty)
      Dim lblnTopDirectoriesOnly As Boolean = MyBase.GetParameterValue(PARAM_TOP_DIRECTORY_ONLY, True)
      Dim lstrFoundFiles As String()
      Dim lobjHost As Object = Nothing

      If String.IsNullOrEmpty(lstrJobName) Then
        Throw New ParameterValueNotSetException(PARAM_JOB_NAME)
      End If
      If String.IsNullOrEmpty(lstrSearchRoot) Then
        Throw New ParameterValueNotSetException(PARAM_SEARCH_ROOT)
      End If

      ' Find all the files using the specified search and add the results to the target job.

      Dim lobjJob As Job = GetJobByName(lstrJobName)

      ' Make sure the search root points to a valid folder location.
      If Directory.Exists(lstrSearchRoot) = False Then
        Throw New InvalidPathException(String.Format(
          "Unable to update job from file search, the search root '{0}' is not valid.", lstrSearchRoot),
        lstrSearchRoot)
      End If

      ' Execute the search and add the results to the list of found files.
      If lblnTopDirectoriesOnly = True Then
        If Not String.IsNullOrEmpty(lstrSearchPattern) Then
          lstrFoundFiles = Directory.GetFiles(lstrSearchRoot, lstrSearchPattern, SearchOption.TopDirectoryOnly)
        Else
          lstrFoundFiles = Directory.GetFiles(lstrSearchRoot, "*.*", SearchOption.TopDirectoryOnly)
        End If
      Else
        If Not String.IsNullOrEmpty(lstrSearchPattern) Then
          lstrFoundFiles = Directory.GetFiles(lstrSearchRoot, lstrSearchPattern, SearchOption.AllDirectories)
        Else
          lstrFoundFiles = Directory.GetFiles(lstrSearchRoot, "*.*", SearchOption.AllDirectories)
        End If
      End If

      If lstrFoundFiles.Length > 0 Then
        Dim lobjFoundFilesDataTable As DataTable = Job.CreateNewSourceDataTable
        Dim lobjNewRow As DataRow = Nothing
        For Each lstrFileName As String In lstrFoundFiles
          If Helper.IsFileLocked(lstrFileName) = True Then
            ApplicationLogging.WriteLogEntry(
              String.Format("Unable to add file '{0}' to job '{1}', the file is currently locked.", lstrFileName, lstrJobName),
              TraceEventType.Warning, 62004)
            Continue For
          End If
          lobjNewRow = lobjFoundFilesDataTable.NewRow()
          lobjNewRow("Id") = lstrFileName
          lobjNewRow("Title") = IO.Path.GetFileName(lstrFileName)
          lobjFoundFilesDataTable.Rows.Add(lobjNewRow)
        Next

        Dim lobjNewItems As DataTable = lobjJob.JobBatchContainer.GetUpdateTable(lobjJob, lobjFoundFilesDataTable)

        If lobjNewItems.Rows.Count > 0 Then
          lobjJob.CreateBatchesFromDataTable(lobjNewItems)
          Me.ProcessedMessage = String.Format("Added {0} new batch items to job '{1}'.", lobjNewItems.Rows.Count, lobjJob.Name)

          'Commit the last batch
          lobjJob.GetCurrentWorkingBatch().CommitBatchItems()

          'If we created the Batches successfully then save out the 
          ' new batches and job/batch relationships to the container.
          lobjJob.Save()

          If ((Me.Host IsNot Nothing) AndAlso (String.Compare(Me.Host.GetType.Name, JOB_MANAGER_WINDOW) = 0)) Then
            lobjHost = Me.Host
          ElseIf ((lobjJob.Project.Host IsNot Nothing) AndAlso (String.Compare(lobjJob.Project.Host.GetType.Name, JOB_MANAGER_WINDOW) = 0)) Then
            lobjHost = lobjJob.Project.Host
          End If

          lobjHost?.ReloadProjectTree()

          menuResult = OperationEnumerations.Result.Success

        Else
          If lstrFoundFiles.Length > 0 Then
            Me.ProcessedMessage = String.Format("{0} files found, all were already defined for job '{1}'.", lstrFoundFiles.Length, lstrJobName)
            menuResult = OperationEnumerations.Result.Success
          Else
            Me.ProcessedMessage = "No new items found."
            menuResult = OperationEnumerations.Result.Success
          End If
        End If

      End If

      Return menuResult

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

  Public Overrides Function Rollback() As OperationEnumerations.Result
    Try
      Return OnRollback()
    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

#End Region

#Region "Protected Methods"

  Protected Overrides Function GetDefaultParameters() As IParameters
    Try
      Dim lobjParameters As IParameters = New Parameters

      If lobjParameters.Contains(PARAM_JOB_NAME) = False Then
        lobjParameters.Add(ParameterFactory.Create(PropertyType.ecmString, PARAM_JOB_NAME, "Job Name", "The name of the job to update."))
        lobjParameters.Add(ParameterFactory.Create(PropertyType.ecmString, PARAM_SEARCH_ROOT, "C:\Temp", "The starting folder in which to search for files."))
        lobjParameters.Add(ParameterFactory.Create(PropertyType.ecmString, PARAM_SEARCH_PATTERN, "*.*", "This is specified in the same way as a 'dir' command."))
        lobjParameters.Add(ParameterFactory.Create(PropertyType.ecmBoolean, PARAM_TOP_DIRECTORY_ONLY, True, "Specifies whether or not to recursely search subdirectories as well."))
      End If

      Return lobjParameters

    Catch ex As Exception
      ApplicationLogging.LogException(ex, Reflection.MethodBase.GetCurrentMethod)
      ' Re-throw the exception to the caller
      Throw
    End Try
  End Function

#End Region

End Class
